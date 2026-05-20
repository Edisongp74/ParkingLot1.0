$(function() {
    if (typeof $.fn.owlCarousel !== 'undefined') {
        if ($('#carousel1').length) {
            $('#carousel1').owlCarousel({
                loop: true,
                autoplay: true,
                margin: 10,
                responsiveClass: true,
                dots: true,
                responsive: {
                    0: {
                        items: 1,
                        nav: false
                    }
                }
            });
        }
        if ($('#testi').length) {
            $('#testi').owlCarousel({
                loop: true,
                margin: 30,
                nav: false,
                dots: true,
                autoplay: true,
                responsiveClass: true,
                responsive: {
                    0: {
                        items: 1,
                        nav: false
                    }
                }
            });
        }
    }
    $('a.nav-link, .dm-btn').on('click', function(event) {
        var $anchor = $(this);
        var target = $($anchor.attr('href'));
        if (target.length) {
            $('html, body').stop().animate({
                scrollTop: target.offset().top - 10
            }, 1000);
            event.preventDefault();
        }
    });
});
