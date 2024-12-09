document.addEventListener('DOMContentLoaded', function () {
    const menuIcon = document.querySelector('.menu-icon');
    const navLinks = document.querySelector('.nav-links');

    menuIcon.addEventListener('click', function () {
        navLinks.classList.toggle('active');
    });
});

$(document).ready(function () {
    $('#websiteCarousel').carousel();

    $('#websiteCarousel').on('slide.bs.carousel', function (e) {
        var $e = $(e.relatedTarget); var idx = $e.index(); var itemsPerSlide = 3; // Adjust based on how many items you want visible
        var totalItems = $('.carousel-item').length; if (idx >= totalItems - (itemsPerSlide - 1)) {
            var it = itemsPerSlide - (totalItems - idx); for (var i = 0; i < it; i++) { // append slides to end 
                if (e.direction == "left") { $('.carousel-item').eq(i).appendTo('.carousel-inner'); } else { $('.carousel-item').eq(0).appendTo('.carousel-inner'); }
            }
        }
    }); // Prevent event bubbling for iframes
    $('.carousel-item iframe').on('click', function (e) {
        e.stopPropagation();
    });
});
function confirmLogout() { return confirm("Are you sure you want to logout?"); }
