(function ($) {
    "use strict";
    $(window).on('load', function () {
        $('.preloader').fadeOut(1000);
    });

    // lightcase 
    $('a[data-rel^=lightcase]').lightcase();

    $(document).ready(function () {

        /*==== header Section Start here =====*/
        $("ul>li>ul").parent("li").addClass("menu-item-has-children");
        // drop down menu width overflow problem fix
        $('ul').parent('li').on('hover', function () {
            var menu = $(this).find("ul");
            var menupos = $(menu).offset();
            if (menupos.left + menu.width() > $(window).width()) {
                var newpos = -$(menu).width();
                menu.css({
                    left: newpos
                });
            }
        });
        $('.mainmenu ul li a').on('click', function (e) {
            var element = $(this).parent('li');
            if (parseInt($(window).width()) < 992) {
                if (element.hasClass('open')) {
                    element.removeClass('open');
                    element.find('li').removeClass('open');
                    element.find('ul').slideUp(300, "swing");
                } else {
                    element.addClass('open');
                    element.children('ul').slideDown(300, "swing");
                    element.siblings('li').children('ul').slideUp(300, "swing");
                    element.siblings('li').removeClass('open');
                    element.siblings('li').find('li').removeClass('open');
                    element.siblings('li').find('ul').slideUp(300, "swing");
                }
            }
        })
        //Header
        var fixed_top = $("header");
        $(window).on('scroll', function () {
            if ($(this).scrollTop() > 200) {
                fixed_top.addClass("header-fixed animated fadeInDown");
            } else {
                fixed_top.removeClass("header-fixed animated fadeInDown");
            }
        });

        // scroll up start here
        $(function () {
            $(window).on('scroll', function () {
                if ($(this).scrollTop() > 300) {
                    $('.scrollToTop').css({
                        'bottom': '2%',
                        'opacity': '1',
                        'transition': 'all .5s ease'
                    });
                } else {
                    $('.scrollToTop').css({
                        'bottom': '-30%',
                        'opacity': '0',
                        'transition': 'all .5s ease'
                    })
                }
            });

            //Click event to scroll to top
            $('a.scrollToTop').on('click', function () {
                $('html, body').animate({
                    scrollTop: 0
                }, 500);
                return false;
            });
        });

        // button effict
        // document.querySelectorAll('.default-btn').forEach(button => button.innerHTML = '<div><span>' + button.textContent.trim().split('').join('</span><span>')  +  '</span></div>');



        //Member Filter Isotop
        // init Isotope
        var $grid = $('.member__grid').isotope({
            itemSelector: '.member__item',
            layoutMode: 'fitRows',
        });

        // filter functions
        var filterFns = {
            // show if name ends with -ium
            ium: function () {
                var name = $(this).find('.name').text();
                return name.match(/ium$/);
            }
        };
        // bind filter button click
        $('.member__buttongroup').on('click', '.filter-btn', function () {
            var filterValue = $(this).attr('data-filter');
            // use filterFn if matches value
            filterValue = filterFns[filterValue] || filterValue;
            $grid.isotope({
                filter: filterValue
            });
        });
        // change is-checked class on buttons
        $('.member__buttongroup').each(function (i, buttonGroup) {
            var $buttonGroup = $(buttonGroup);
            $buttonGroup.on('click', '.filter-btn', function () {
                $buttonGroup.find('.is-checked').removeClass('is-checked');
                $(this).addClass('is-checked');
            });
        });


        

    });
    
    //Banner slider
    var swiper = new Swiper('.banner__slider', {
        slidesPerView: 1,
        spaceBetween: 0,
        autoplay: {
            delay: 10000,
            disableOnInteraction: false,
        },
        loop: true,
    });

    //====ragi slider================
    var swiper = new Swiper(".ragi__slider", {
        slidesPerView: 2,
        spaceBetween: 20,
        loop: true,
        autoplay: {
            delay: 5000,
            disableOnInteraction: false,
        },
        navigation: {
            nextEl: ".ragi-next",
            prevEl: ".ragi-prev",
        },
        breakpoints: {
            767: {
                slidesPerView: 5,
                spaceBetween: 20,
            },
            1199: {
                slidesPerView: 4,
                spaceBetween: 20,
            },
            1439: {
                slidesPerView: 5,
                spaceBetween: 20,
            },
        },
    });

    
    //countdown 
    $(window).on('scroll', function () {
        $('.counter').data('countToOptions', {
            formatter: function (value, options) {
                return value.toFixed(options.decimals).replace(/\B(?=(?:\d{3})+(?!\d))/g, ',');
            }
        });
        // start all the timers
        $('.counter').each(count);  
        function count(options) {
            var $this = $(this);
            options = $.extend({}, options || {}, $this.data('countToOptions') || {});
            $this.countTo(options);
        }
    });


    // post thumb slider
    var swiper = new Swiper('.blog__slider', {
        slidesPerView: 1,
        autoplay: {
            delay: 5000,
            disableOnInteraction: false,
        },
        navigation: {
            nextEl: '.thumb-next',
            prevEl: '.thumb-prev',
        },
        loop: true,
    });


    // product view mode change js
    $(function() {
        $('.product-view-mode').on('click', 'a', function (e) {
            e.preventDefault();
            var shopProductWrap = $('.shop-product-wrap');
            var viewMode = $(this).data('target');
            $('.product-view-mode a').removeClass('active');
            $(this).addClass('active');
            shopProductWrap.removeClass('grid list').addClass(viewMode);
        });
    });

    // model option start here
    $(function() {
        $('.view-modal').on('click', function () {
            $('.modal').addClass('show');
        });
        $('.close').on('click', function () {
            $('.modal').removeClass('show');
        });
    });

    // shop cart + - start here
    var CartPlusMinus = $('.cart-plus-minus');
    $(".qtybutton").on("click", function() {
        var $button = $(this);
        var oldValue = $button.parent().find("input").val();
        if ($button.text() === "+") {
            var newVal = parseFloat(oldValue) + 1;
        } else {
            if (oldValue > 0) {
                var newVal = parseFloat(oldValue) - 1;
            } else {
                newVal = 1;
            }
        }
        $button.parent().find("input").val(newVal);
    });

    // shop sidebar menu
    $(".shop-menu>li>ul").parent("li").addClass("catmenu-item-has-children");
    $('.shop-menu li a').on('click', function (e) {
        var element = $(this).parent('li');
        if (element.hasClass('open')) {
            element.removeClass('open');
            element.find('li').removeClass('open');
            element.find('ul').slideUp(300, "swing");
        } else {
            element.addClass('open');
            element.children('ul').slideDown(300, "swing");
            element.siblings('li').children('ul').slideUp(300, "swing");
            element.siblings('li').removeClass('open');
            element.siblings('li').find('li').removeClass('open');
            element.siblings('li').find('ul').slideUp(300, "swing");
        }
    })

    // product single thumb slider
    $(function() {
        var galleryThumbs = new Swiper('.pro-single-thumbs', {
            spaceBetween: 10,
            slidesPerView: 3,
            loop: true,
            freeMode: true,
            loopedSlides: 1,
            watchSlidesVisibility: true,
            watchSlidesProgress: true,
            navigation: {
                nextEl: '.pro-single-next',
                prevEl: '.pro-single-prev',
            },
        });
        var galleryTop = new Swiper('.pro-single-top', {
            spaceBetween: 10,
            loop:true,
            loopedSlides: 1,
            thumbs: {
                swiper: galleryThumbs,
            },
        });
    });


    //Review Tabs
    $('ul.review-nav').on('click', 'li', function (e) {
        e.preventDefault();
        var reviewContent = $('.review-content');
        var viewRev = $(this).data('target');
        $('ul.review-nav li').removeClass('active');
        $(this).addClass('active');
        reviewContent.removeClass('review-content-show description-show').addClass(viewRev);
    });


    // countdown or date & time
    $('#countdown').countdown({
        date: '10/22/2023 17:00:00',
        offset: +2,
        day: 'Day',
        days: 'Days'
    });


    new WOW().init();
    


    //contact form js
    $(function () {
        var form = $('#contact-form');
        var formMessages = $('.form-message');
        $(form).submit(function (e) {
            e.preventDefault();
            var formData = $(form).serialize();
            $.ajax({
                type: 'POST',
                url: $(form).attr('action'),
                data: formData
            })
            .done(function (response) {
                $(formMessages).removeClass('error');
                $(formMessages).addClass('success');
                $(formMessages).text(response);
                $('#contact-form input, #contact-form textarea').val('');
            })
            .fail(function (data) {
                $(formMessages).removeClass('success');
                $(formMessages).addClass('error');
                if (data.responseText !== '') {
                    $(formMessages).text(data.responseText);
                } else {
                    $(formMessages).text('Oops! An error occured and your message could not be sent.');
                }
            });
        });
    });

}(jQuery));