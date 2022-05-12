using HMS.Extensions;
using HMS.Security;
using HMS.Utility;
using HMS.ViewModels;
using HMS.Data;
using HMS.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace HMS.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HMSDBContext context;
        private readonly ICustomDataProtector protector;
        private readonly IEmailSender emailSender;
        public JsonSerializerOptions serializerOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = false
        };

        public HomeController(ILogger<HomeController> logger, HMSDBContext context, ICustomDataProtector protector, 
                                IHttpContextAccessor contextAccessor, IEmailSender emailSender) : base(contextAccessor)
        {
            _logger = logger;
            this.context = context;
            this.protector = protector;
            this.emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult faq()
        {
            return View();
        }
        public IActionResult about()
        {
            return View();
        }
        public IActionResult contact()
        {
            return View();
        }

        public IActionResult services()
        {
            return View();
        }

        public IActionResult login()
        {
            if (TempData.ContainsKey("status"))
            {
                ViewBag.Status = TempData["status"];
                ViewBag.IsSuccess = false;
                if(TempData["success"] != null && (bool)TempData["success"] == true)
                {
                    ViewBag.IsSuccess = true;
                }
            }
            return View();
        }

        public IActionResult register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> login(CombinedViewModel model, string returnUrl = "")
        {
            if (ModelState.IsValid)
            {
                if (EmailPresent(model.LoginModel.Email) == false)
                {
                    ModelState.AddModelError("Email", "Invalid Email Address");
                    TempData["status"] = "Invalid Email Address";
                    return RedirectToAction("login");
                }
                var user = context.Users.Where(s => s.Email == model.LoginModel.Email).First();
                string pass = protector.Decrypt(user.Password);
                if (pass != model.LoginModel.Password)
                {
                    TempData["status"] = "Invalid Credentials";
                    return RedirectToAction("login");
                }
                var username = user.FirstName.ToString() + " " + user.LastName.ToString();
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                };
                string userrole = "";
                if (user.IsActive == false)
                {
                    TempData["status"] = "Your account is not active at the moment!";
                    return RedirectToAction("login");
                }
                if (user.UserType == (int)Roles.Customer)
                {
                    userrole = "Customer";
                }

                if (model.LoginModel.RememberMe == true)
                {
                    CookieOptions cookieOptions = new CookieOptions()
                    {
                        Expires = DateTime.Now.AddDays(30),
                        MaxAge = TimeSpan.FromDays(30),
                        SameSite = SameSiteMode.Strict,
                    };

                    model.LoginModel.Password = protector.Encrypt(model.LoginModel.Password);
                    string serialized = JsonSerializer.Serialize(model.LoginModel);
                    Response.Cookies.Append("UserCredentials", serialized, cookieOptions);
                }

                claims.Add(new Claim(ClaimTypes.Role, userrole));
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);
                AuthenticationProperties authProperties = new AuthenticationProperties()
                {
                    IsPersistent = model.LoginModel.RememberMe,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
                    RedirectUri = Url.Action("Logout"),
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                                new ClaimsPrincipal(claimsIdentity), authProperties);
                HttpContext.Session.Set<User>("User", user);
                loggedUser = user;
                if (returnUrl.Length > 0)
                {
                    ViewBag.LoginRequired = "";
                    return LocalRedirect(returnUrl);
                }
                return RedirectToAction("Dashboard", "Customer");
            }
            TempData["status"] = "Something went wrong!";
            return RedirectToAction("login");
        }

        [HttpPost]
        public async Task<IActionResult> register(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (EmailPresent(model.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use");
                    return View();
                }

                User user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Mobile = model.Mobile,
                    Password = protector.Encrypt(model.Password),
                    UserType = 1,
                    RegisteredOn = DateTime.Now,
                    IsApproved = true,
                    IsActive = true,
                };
                context.Users.Add(user);
                context.SaveChanges();

                using (HMSDBContext db = new HMSDBContext())
                {
                    var tmp = db.Users.Where(s => s.Email == user.Email).First();
                    // tmp.ModifiedBy = user.UserId;
                    db.SaveChanges();
                };

                LoginViewModel loginModel = new LoginViewModel
                {
                    Email = model.Email,
                    Password = model.Password,
                    RememberMe = false,
                };
                CombinedViewModel finalModel = new CombinedViewModel
                {
                    LoginModel = loginModel,
                };
                await login(finalModel);
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult forgotPassword(CombinedViewModel model)
        {
            //TempData["status"] = "done";
            //TempData["success"] = true;

            if (ModelState.IsValid)
            {
                if (EmailPresent(model.forgotPasswordViewModel.Email) == false)
                {
                    ModelState.AddModelError("Email", "Email not found");
                    TempData["status"] = "Invalid Email Address!";
                    return RedirectToAction("login");
                }
                var resetObj = new ChangePasswordData { Email = model.forgotPasswordViewModel.Email, Hash = Guid.NewGuid().ToString() };
                var serializedResetObj = JsonSerializer.Serialize(resetObj);
                var protectedObj = protector.Encrypt(serializedResetObj);
                var resetUrl = Url.Action("ChangePassword", "Home", new { token = protectedObj }, Request.Scheme);
                var temp = JsonSerializer.Deserialize<ChangePasswordData>(serializedResetObj);
                var user = context.Users.Where(s => s.Email == model.forgotPasswordViewModel.Email).First();
                var subject = "Reset Password";
                var message = "<div>" +
                                    "<h3>Hello " + user.FirstName + ",</h3>" +
                                    "<div>" +
                                          "You have submitted a password change request. <br>" +
                                          "If it wasn't you please disregard this email and make sure you can still login to your account. " +
                                          "If it was you, then <a href='" + resetUrl + "' title='Reset Password'>Click Here</a> to change your password." +
                                    "</div><br>" +
                                    "<div>Regards,</div>" +
                                    "<div><h4>HMS Team</h4></div>" +
                               "</div>";
                try
                {
                    Message emailMesssage = new Message(new string[] { model.forgotPasswordViewModel.Email }, subject, message);
                    emailSender.SendEmail(emailMesssage);
                    TempData["status"] = "Password reset link has been sent to provided email address! Kindly follow the steps provided in email!";
                    TempData["success"] = true;
                    return RedirectToAction("login");
                }
                catch (Exception)
                {
                    TempData["status"] = "Something went wrong!";
                    return RedirectToAction("login");
                }
            }
            TempData["status"] = "Something went wrong!";
            return RedirectToAction("login");
        }

        public IActionResult ChangePassword(string token)
        {
            try
            {
                var exapndedObj = protector.Decrypt(token);
                var deserializedObj = JsonSerializer.Deserialize<ChangePasswordData>(exapndedObj);
                ViewBag.Email = deserializedObj.Email;
                return View();
            }
            catch (Exception)
            {
                TempData["status"] = "Something went wrong!";
                return RedirectToAction("login");
            }
        }

        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (EmailPresent(model.Email) == false)
                {
                    return RedirectToAction("Error");
                }
                var user = context.Users.Where(s => s.Email == model.Email).First();
                user.Password = protector.Encrypt(model.Password);
                context.SaveChanges();

                LoginViewModel loginModel = new LoginViewModel
                {
                    Email = model.Email,
                    Password = model.Password,
                    RememberMe = false,
                };

                CombinedViewModel finalModel = new CombinedViewModel
                {
                    LoginModel = loginModel,
                };

                TempData["status"] = "Password changed successfully!";
                TempData["success"] = true;
                return RedirectToAction("login");
            }
            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["SuccessPopUpStatus"] = "Logout";
            HttpContext.Session.Remove("User");
            loggedUser = new User();
            HttpContext.Response.Cookies.Delete(".AspNetCore.Cookies");
            return RedirectToAction("Index");
        }

        public IActionResult HotelList(CombinedViewModel model)
        {
            model.hotels = context.Hotels.Select(x => x).ToList();

            if (model.UserFilters.fivestar
                || model.UserFilters.fourstar
                || model.UserFilters.threestar
                || model.UserFilters.twostar
                || model.UserFilters.onestar)
            {
                List<Hotel> updated = new List<Hotel>();
                foreach (var hotel in model.hotels)
                {
                    switch (Math.Floor((decimal)hotel.HotelRating))
                    {
                        case 1:
                            if (model.UserFilters.onestar)
                            {
                                updated.Add(hotel);
                            }
                            break;
                        case 2:
                            if (model.UserFilters.twostar)
                            {
                                updated.Add(hotel);
                            }
                            break;
                        case 3:
                            if (model.UserFilters.threestar)
                            {
                                updated.Add(hotel);
                            }
                            break;
                        case 4:
                            if (model.UserFilters.fourstar)
                            {
                                updated.Add(hotel);
                            }
                            break;
                        case 5:
                            if (model.UserFilters.fivestar)
                            {
                                updated.Add(hotel);
                            }
                            break;
                    }
                }
                model.hotels = updated;
            }

            if (model.UserFilters.price1
                || model.UserFilters.price2
                || model.UserFilters.price3
                || model.UserFilters.price4
                || model.UserFilters.price5)
            {
                List<Hotel> updated = new List<Hotel>();
                foreach (var hotel in model.hotels)
                {
                    if (hotel.Price <= 1500 && model.UserFilters.price1)
                    {
                        updated.Add(hotel);
                    }
                    else if (hotel.Price > 1500 && hotel.Price <= 2500 && model.UserFilters.price2)
                    {
                        updated.Add(hotel);
                    }
                    else if (hotel.Price > 2500 && hotel.Price <= 3500 && model.UserFilters.price3)
                    {
                        updated.Add(hotel);
                    }
                    else if (hotel.Price > 3500 && hotel.Price <= 4500 && model.UserFilters.price4)
                    {
                        updated.Add(hotel);
                    }
                    else if (hotel.Price > 4500 && model.UserFilters.price5)
                    {
                        updated.Add(hotel);
                    }
                }
                model.hotels = updated;
            }

            for(int i=0;i<model.hotels.Count();i++)
            {
                model.hotels[i].ImgSrc = context.HotelImages.Where(h => h.HotelId == model.hotels[i].HotelId).FirstOrDefault().ImageSrc;
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult HotelDetails(int id)
        {
            CombinedViewModel combinedViewModel = new CombinedViewModel();
            combinedViewModel.hotelDetails = new HotelDetailsViewModel();
            combinedViewModel.UserFilters = new UserFiltersViewModel();
            combinedViewModel.UserFilters2 = new UserFiltersViewModel();
            combinedViewModel.favHotel = new FavHotelViewModel();
            combinedViewModel.hotelDetails.hotelDetails = context.Hotels.Where(x => x.HotelId == id).FirstOrDefault();
            combinedViewModel.hotelDetails.images = context.HotelImages.Where(x => x.HotelId == id).Select(x => x.ImageSrc).Take(4).ToList();
            combinedViewModel.favHotel.isFav = User.Identity.IsAuthenticated
                                                && context.FavoriteHotels.Where(x => x.HotelId == id
                                                                                     && x.UserId == loggedUser.UserId
                                                                                     && x.IsFavorite == true).Any();
            return View(combinedViewModel);
        }

        [HttpPost]
        public IActionResult HotelDetails(int id, CombinedViewModel model)
        {
            model.hotelDetails = new HotelDetailsViewModel();
            model.favHotel = new FavHotelViewModel();
            model.hotelDetails.hotelDetails = context.Hotels.Where(x => x.HotelId == id).FirstOrDefault();
            model.hotelDetails.images = context.HotelImages.Where(x => x.HotelId == id).Select(x => x.ImageSrc).Take(4).ToList();
            model.favHotel.isFav = User.Identity.IsAuthenticated 
                                    && context.FavoriteHotels.Where(x => x.HotelId == id
                                                                         && x.UserId == loggedUser.UserId
                                                                         && x.IsFavorite == true).Any();
            return View(model); 
        }

        [Authorize]
        public IActionResult reserve(int id)
        {
            CombinedViewModel model = new CombinedViewModel();
            model.hotelDetails = new HotelDetailsViewModel();
            model.UserFilters = new UserFiltersViewModel();
            model.hotelDetails.bannerImage = context.HotelImages.Where(x => x.HotelId == id).Select(x => x.ImageSrc).FirstOrDefault();
            model.hotelDetails.hotelDetails = context.Hotels.Where(x => x.HotelId == id).FirstOrDefault();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult reserve(int id, CombinedViewModel model)
        {
            model.hotelDetails = new HotelDetailsViewModel();
            model.hotelDetails.bannerImage = context.HotelImages.Where(x => x.HotelId == id).Select(x => x.ImageSrc).FirstOrDefault();
            model.hotelDetails.hotelDetails = context.Hotels.Where(x => x.HotelId == id).FirstOrDefault();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult reserveRoom(CombinedViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if enough rooms available or not
                //var totalConflictingRooms = context.RoomReservationTimings.
                //                                    Where(
                //                                        s => s.Booking.HotelId == model.reservation.HotelId
                //                                        && s.Booking.RoomType == model.reservation.typeOfRoom
                //                                        && (s.CheckInDate <= model.reservation.CheckIn && s.CheckOutDate > model.reservation.CheckIn)
                //                                        || (s.CheckOutDate >= model.reservation.CheckOut && s.CheckInDate < model.reservation.CheckOut)
                //                                    );
                //var cnt = totalConflictingRooms.Count();
                Reservation reservation = new Reservation()
                {
                    CheckInDate = model.reservation.CheckIn,
                    CheckOutDate = model.reservation.CheckOut,
                    CreatedOn = DateTime.Now,
                    HotelId = model.reservation.HotelId,
                    NumberOfGuests = model.reservation.Adults + model.reservation.Children,
                    NumberOfRooms = model.reservation.Rooms,
                    RoomType = model.reservation.typeOfRoom,
                    Status = 2,
                    UserId = loggedUser.UserId,
                    TotalAmount = model.reservation.FinalPrice,
                    UpdatedBy = loggedUser.UserId,
                    UpdatedOn = DateTime.Now,
                };

                context.Reservations.Add(reservation);
                context.SaveChanges();
                return RedirectToAction("dashboard", "customer");

            }
            return View(model);
        }

        //[Authorize]
        //[HttpPost]
        //public IActionResult reserve(int id, ReservationViewModel model)
        //{
        //    return RedirectToAction("dashboard", "customer");
        //}

        //[Authorize]
        //[HttpPost]
        //public IActionResult reserve(int id)
        //{
        //    return RedirectToAction("dashboard", "customer");
        //}

        [Authorize]
        public IActionResult FavHotel(FavHotelViewModel model)
        {
            var available = context.FavoriteHotels.Where(x => x.HotelId == model.HotelId && x.UserId == loggedUser.UserId);
            if(available.Any())
            {
                var data = available.First();
                data.IsFavorite = model.isFav;
                context.Update(data);
            }
            else
            {
                FavoriteHotel data = new FavoriteHotel()
                {
                    HotelId = model.HotelId,
                    IsFavorite = model.isFav,
                    UserId = loggedUser.UserId,
                };
                context.FavoriteHotels.Add(data);
            }
            context.SaveChanges();
            return Json(new { Status = true, Msg = "Done", AdditionalMsg = "" }, serializerOptions);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Utility Functions
        bool EmailPresent(string email)
        {
            return context.Users.Any(s => s.Email == email);
        }

        public class ChangePasswordData
        {
            public string Email { get; set; }
            public string Hash { get; set; }
        }

        public enum Roles
        {
            Customer = 1,
            Staff,
            Admin,
        }
    }
}
