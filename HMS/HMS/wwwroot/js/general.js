function setCarousel() {

    if ($('.tm-article-carousel').hasClass('slick-initialized')) {
        $('.tm-article-carousel').slick('destroy');
    }

    if ($(window).width() < 438) {
        // Slick carousel
        $('.tm-article-carousel').slick({
            infinite: false,
            dots: true,
            slidesToShow: 1,
            slidesToScroll: 1
        });
    }
    else {
        $('.tm-article-carousel').slick({
            infinite: false,
            dots: true,
            slidesToShow: 2,
            slidesToScroll: 1
        });
    }
}

function setPageNav() {
    if ($(window).width() > 991) {
        $('#tm-top-bar').singlePageNav({
            currentClass: 'active',
            offset: 79
        });
    }
    else {
        $('#tm-top-bar').singlePageNav({
            currentClass: 'active',
            offset: 65
        });
    }
}

let pickerCheckIn, pickerCheckOut;
let curCheckIn, curCheckOut;

function ShowToast(msg, isSuccess) {
    $("#statusToast").toast('show');
    $('#toastMsg').text(msg);
    if (isSuccess === 'true') {
        $('#successToast').removeClass('d-none').addClass('d-block');
        $('#errorToast').removeClass('d-block').addClass('d-none');
    } else {
        $('#errorToast').removeClass('d-none').addClass('d-block');
        $('#successToast').removeClass('d-block').addClass('d-none');
    }
    setTimeout(function () {
        $("#statusToast").toast('hide');
    }, 4000);
}

$(document).ready(function () {
    var today = new Date();

    // Date Picker
    if ($('#inputCheckIn').length > 0) {
        pickerCheckIn = datepicker('#inputCheckIn', {
            formatter: (input, date, instance) => {
                const value = date.toLocaleDateString()
                input.value = value // => '1/1/2099'
            },
            onSelect: (instance) => {
                // Do stuff when a date is selected (or unselected) on the calendar.
                // You have access to the datepicker instance for convenience.
                curCheckIn = instance.dateSelected;
                console.log(curCheckIn);
            },
            //minDate: today,
        });

        //pickerCheckIn.setDate(today, true);
    }
    if ($('#inputCheckOut').length > 0) {
        pickerCheckOut = datepicker('#inputCheckOut', {
            formatter: (input, date, instance) => {
                const value = date.toLocaleDateString()
                input.value = value // => '1/1/2099'
            },
            onSelect: (instance) => {
                // Do stuff when a date is selected (or unselected) on the calendar.
                // You have access to the datepicker instance for convenience.
                curCheckOut = instance.dateSelected;
                console.log(curCheckOut);
            },
            //minDate: today,
        });
    }

    // Slick carousel
    setCarousel();

    // Update the current year in copyright
    $('.tm-current-year').text(new Date().getFullYear());
});