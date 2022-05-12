create database HMSDB;

use HMSDB;
Go

create table Users(
	UserID int IDENTITY(1,1) PRIMARY KEY NOT NULL,
	FirstName nvarchar(20) NOT NULL,
	LastName nvarchar(20) NOT NULL,
	Email nvarchar(50) UNIQUE NOT NULL,
	Password text NOT NULL,
	Mobile nvarchar(10) NOT NULL,
	DOB datetime NULL,
	Age smallint NULL,
	UserType smallint NOT NULL,
	RegisteredOn datetime NOT NULL default CURRENT_TIMESTAMP,
	IsApproved bit NOT NULL,
	IsActive bit NOT NULL,
	UserProfile text NULL,
);

create table Hotel(
	HotelId int IDENTITY(1,1) PRIMARY KEY NOT NULL,
	HotelName nvarchar(30) NOT NULL,
	AreaName nvarchar(30) NOT NULL,
	ContactNumber nvarchar(10) NOT NULL,
	City nvarchar(30) NOT NULL,
	ZipCode nvarchar(10) NOT NULL,
	Password text NOT NULL,
	HotelRating float DEFAULT 0,
);

create table HotelImages(
	HotelId int foreign key (HotelId) references Hotel(HotelId),
	ImageSrc text NOT NULL,
);

create table HotelReviewAndRating(
	ReviewId int IDENTITY(1, 1) primary key NOT NULL,
	HotelId int foreign key (HotelId) references Hotel(HotelId),
	UserId int foreign key (UserId) references Users(UserID),
	Ratings float NOT NULL,
	RatedOn datetime NOT NULL default CURRENT_TIMESTAMP,
	Feedback text NULL,
);

create table HotelRooms(
	HotelId int foreign key (HotelId) references Hotel(HotelId),
	No_Total int NOT NULL DEFAULT 0,
	No_AC_Single int NOT NULL DEFAULT 0,
	No_AC_Double int NOT NULL DEFAULT 0,
	No_AC_Triple int NOT NULL DEFAULT 0,
	No_Non_AC_Single int NOT NULL DEFAULT 0,
	No_Non_AC_Double int NOT NULL DEFAULT 0,
	No_Non_AC_Triple int NOT NULL DEFAULT 0,
	No_King int NOT NULL DEFAULT 0,
	No_Queen int NOT NULL DEFAULT 0,
);

create table RoomDetails(
	RoomId int Identity(1, 1) primary key NOT NULL,
	HotelId int foreign key (HotelId) references Hotel(HotelId),
	RoomNumber nvarchar(20) NOT NULL,
	RoomType int NOT NULL,
);

create table Reservations(
	BookingId int Identity(1, 1) primary key NOT NULL,
	UserId int foreign key (UserId) references Users(UserID),
	HotelId int foreign key (HotelId) references Hotel(HotelId),
	CheckInDate datetime NOT NULL,
	CheckOutDate datetime NOT NULL,
	NumberOfGuests int NOT NULL default 1,
	NumberOfRooms int NOT NULL default 1,
	RoomType int NOT NULL,
	TotalAmount float NOT NULL,
	Status int NOT NULL,
	ReservedRoomId int foreign key references RoomDetails(RoomId),
	CreatedOn datetime NOT NULL Default CURRENT_TIMESTAMP,
	UpdatedOn datetime NOT NULL,
	UpdatedBy int foreign key (UpdatedBy) references Users(UserID),
);

create table ExtraServices(
	ExtraServiceId int IDENTITY(1, 1) primary key NOT NULL,
	BookingId int foreign key (BookingId) references Reservations(BookingId),
	Type int NOT NULL,
);

create table RoomReservationTimings(
	RoomId int foreign key (RoomId) references RoomDetails(RoomId),
	BookingId int foreign key (BookingId) references Reservations(BookingId),
	CheckInDate datetime NOT NULL,
	CheckOutDate datetime NOT NULL,
);

create table FavoriteHotel(
	UserId int foreign key (UserId) references Users(UserID),
	HotelId int foreign key (HotelId) references Hotel(HotelId),
	IsFavorite BIT NOT NULL,
);

create table StaffApplications(
	ApplicationId int IDENTITY(1, 1) primary key NOT NULL,
	UserId int foreign key (UserId) references Users(UserID),
	HotelId int foreign key (HotelId) references Hotel(HotelId),
	ApplicationDate datetime NOT NULL default CURRENT_TIMESTAMP,
	ApplicationText text NOT NULL,
	ApplicationType int NOT NULL,
	IsApproved BIT NOT NULL,
);

create table StaffReviewAndRating(
	ReviewId int IDENTITY(1, 1) primary key NOT NULL,
	FromUserId int foreign key (FromUserId) references Users(UserID),
	ToUserId int foreign key (ToUserId) references Users(UserID),
	Ratings float default 0,
	RatedOn datetime NOT NULL default CURRENT_TIMESTAMP,
	HotelId int foreign key (HotelId) references Hotel(HotelId),
	BookingId int foreign key (BookingId) references Reservations(BookingId),
);