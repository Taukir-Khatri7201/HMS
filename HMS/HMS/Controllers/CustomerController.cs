using HMS.Data;
using HMS.Security;
using HMS.Utility;
using HMS.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Collections.Generic;
using HMS.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Text.Json;

namespace HMS.Controllers
{
    [Authorize(Roles="Customer")]
    public class CustomerController : BaseController
    {
        private ILogger<HomeController> _logger;
        private HMSDBContext context;
        private ICustomDataProtector protector;
        public JsonSerializerOptions serializerOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = false
        };

        public CustomerController(ILogger<HomeController> logger, HMSDBContext context, ICustomDataProtector protector,
                                IHttpContextAccessor contextAccessor, IEmailSender emailSender) : base(contextAccessor)
        {
            _logger = logger;
            this.context = context;
            this.protector = protector;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            CombinedViewModel model = new CombinedViewModel();
            model.bookingHistory = context.Reservations
                                    .Where(s => s.UserId == loggedUser.UserId && s.CheckOutDate >= DateTime.Now && s.Status != (int)Status.Cancelled)
                                    .OrderByDescending(s => s.BookingId)
                                    .Select(s => new BookingHistoryViewModel()
                                    {
                                        BookingId = s.BookingId,
                                        CheckIn = s.CheckInDate,
                                        CheckOut = s.CheckOutDate,
                                        hotel = (from h in context.Hotels where h.HotelId == s.HotelId select h).First(),
                                        RoomType = s.RoomType,
                                        Status = s.Status,
                                        TotalAmount = (int)s.TotalAmount,
                                        TotalGuests = (int)s.NumberOfGuests,
                                        TotalNights = (s.CheckOutDate.Date - s.CheckInDate.Date).Days,
                                    }).ToList();

            // var hotel = (from s in context.Hotels where s.HotelId == s.HotelId select s).First();

            model.bookingHistoryHelper = new List<BookingHistoryHelperViewModel>();
            foreach(var booking in model.bookingHistory)
            {
                var cur = new BookingHistoryHelperViewModel()
                {
                    HotelId = booking.hotel.HotelId,
                    HotelName = booking.hotel.HotelName,
                    BookingId = booking.BookingId,
                };
                model.bookingHistoryHelper.Add(cur);
            }
            return View(model);
        }

        public IActionResult FavoriteHotels()
        {
            CombinedViewModel model = new CombinedViewModel();
            model.hotels = new List<Hotel>();
            var available = context.FavoriteHotels.Where(u => u.UserId == loggedUser.UserId
                                                                && u.IsFavorite == true);
            if(available.Any())
            {
                var data = available.Select(s => s.HotelId).ToList();
                foreach (var hotel in data)
                {
                    model.hotels.Add(context.Hotels.Where(s => s.HotelId == hotel).First());
                }
            }
            return View(model);
        }

        public IActionResult BookingHistory()
        {
            CombinedViewModel model = new CombinedViewModel();
            model.bookingHistory = context.Reservations
                                    .Where(s => s.UserId == loggedUser.UserId && (s.Status == (int)Status.Cancelled || s.CheckOutDate < DateTime.Now))
                                    .OrderByDescending(s => s.BookingId)
                                    .Select(s => new BookingHistoryViewModel()
                                    {
                                        BookingId = s.BookingId,
                                        CheckIn = s.CheckInDate,
                                        CheckOut = s.CheckOutDate,
                                        hotel = context.Hotels.Where(x => x.HotelId == s.HotelId).First(),
                                        RoomType = s.RoomType,
                                        Status = s.Status,
                                        TotalAmount = (int)s.TotalAmount,
                                        TotalGuests = (int)s.NumberOfGuests,
                                        TotalNights = (s.CheckOutDate.Date - s.CheckInDate.Date).Days,
                                    }).ToList();
            model.bookingHistoryHelper = new List<BookingHistoryHelperViewModel>();
            foreach(var booking in model.bookingHistory)
            {
                var cur = new BookingHistoryHelperViewModel()
                {
                    BookingId = booking.BookingId,
                    HotelId = booking.hotel.HotelId,
                    HotelName = booking.hotel.HotelName,
                };
                model.bookingHistoryHelper.Add(cur);
            }
            return View(model);
        }

        public IActionResult RemoveFromFavorite(int id)
        {
            var available = context.FavoriteHotels.Where(s => s.UserId == loggedUser.UserId && s.HotelId == id);
            if(available.Any())
            {
                var data = available.First();
                data.IsFavorite = false;
            }
            context.SaveChanges();
            return RedirectToAction("FavoriteHotels");
        }

        [HttpPost]
        public IActionResult RateHotel(RatingViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.RatingDate = DateTime.Now;
                var available = context.HotelReviewAndRatings.Where(r => r.ReservationId == model.ReservationId);
                if (available.Any())
                {
                    var updateRating = available.First();
                    updateRating.Ratings = model.Rating;
                    updateRating.Feedback = model.Description;
                    updateRating.RatedOn = model.RatingDate;
                    context.Update(updateRating);
                    context.SaveChanges();
                }
                else
                {
                    var cur = new HotelReviewAndRating()
                    {
                        Feedback = model.Description,
                        HotelId = model.HotelId,
                        RatedOn = model.RatingDate,
                        Ratings = model.Rating,
                        UserId = model.UserId,
                        ReservationId = model.ReservationId,
                    };
                    context.HotelReviewAndRatings.Add(cur);
                    context.SaveChanges();
                }
                return Json(new { Status = true, Msg = "Hotel ratings updated successfully!" }, serializerOptions);
            }
            return Json(new { Status = false, Msg = "Something went wrong." }, serializerOptions);
        }

        [HttpPost]
        public IActionResult CancelBooking(CancelBookingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var available = context.Reservations.Where(r => r.BookingId == model.BookingId);
                if (available.Any())
                {
                    var updateBooking = available.First();
                    updateBooking.Status = (int)Status.Cancelled;
                    context.Update(updateBooking);
                    context.SaveChanges();
                    return Json(new { Status = true, Msg = "Hotel ratings updated successfully!" }, serializerOptions);
                }
            }
            return Json(new { Status = false, Msg = "Something went wrong." }, serializerOptions);
        }
    }

    public enum RoomType
    {
        SingleAC = 1,
        DoubleAC,
        TripleAC,
        SingleNonAC,
        DoubleNonAC,
        TripleNonAC,
        King,
        Queen,
    }

    public enum Status
    {
        CheckedOut = 1,
        NotCheckedIn,
        Cancelled,
    }
}
