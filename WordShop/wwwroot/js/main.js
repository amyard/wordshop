jQuery(function($){

    // sliders

    function initTariffSlider() {
        var tariffSlider = $('.js-tariff-slider');

        if($(window).outerWidth() < 1024){
            if(!tariffSlider.hasClass('lightSlider')) {
                tariffSlider.lightSlider({
                    item: 1.7,
                    gallery: false,
                    pager: false,
                    prevHtml: '<button type="button" class="prev-btn square-btn">Previous<svg class="icon-sprite svg-icon"><use xlink:href="img/symbol_sprite.svg#icon-arrow"></use></svg></button>',
                    nextHtml: '<button type="button" class="next-btn square-btn">Next<svg class="icon-sprite svg-icon"><use xlink:href="img/symbol_sprite.svg#icon-arrow"></use></svg></button>',
                    responsive : [
                        {
                            breakpoint:768,
                            settings: {
                                item: 1
                            }
                        }
                    ]
                });

                $(window).on('tsdestroy', function() {
                    tariffSlider.destroy();
                })
            }
        } else {
            $(window).trigger('tsdestroy');
        }
    }

    $('.js-slider-day').lightSlider({
        item: 2.5,
        gallery: false,
        pager: false,
        prevHtml: '<button type="button" class="prev-btn square-btn">Previous<svg class="icon-sprite svg-icon"><use xlink:href="img/symbol_sprite.svg#icon-arrow"></use></svg></button>',
        nextHtml: '<button type="button" class="next-btn square-btn">Next<svg class="icon-sprite svg-icon"><use xlink:href="img/symbol_sprite.svg#icon-arrow"></use></svg></button>',
        responsive : [
            {
                breakpoint: 1024,
                settings: {
                    item: 1.7
                }
            },
            {
                breakpoint: 768,
                settings: {
                    item: 1
                }
            }
        ]
    });

    initTariffSlider();

    $(window).on('resize', function() {
        initTariffSlider();
    });

    // anchors

    $('a[href^="#"]').on('click', function(){
        var el = $(this).attr('href');
        $('html, body').stop().animate({
            scrollTop: $(el).offset().top}, 500);
        return false;
    });

    // open modals

    $('[data-modal]').on('click', function() {
        var tariffTitle = $(this).closest('.tariffs-item').find('.tariffs-item-title').text();
        var scrollWidth = $(window).outerWidth() - $(window).width();
        var body = $('body');
        body.css({'padding-right': scrollWidth+'px'});
        body.addClass('open-modal');

        var modal = $('#'+$(this).attr('data-modal'));
        modal.find('.tariff-name').text(tariffTitle);
        modal.find('input#user-tariff').val(tariffTitle);
        modal.addClass('before-show');
        setTimeout(function() {
            modal.addClass('show');
        }, 50);
    });

    $('.js-close-btn').on('click', function() {
        var body = $('body');
        $('.modal').removeClass('before-show show');
        body.removeClass('open-modal');
        body.css({'padding-right': 0});
    });

    //form mask
    $('.phone-input').mask("+99 (999) 999 99 99");

    //submit form

    $("#tariff-form").validate({
        rules: {
            user_name: {
                required : true
            },
            user_email: {
                required : true,
                email : true,
                regex : /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/
            }
        },
        messages: {
            user_name: {
                required : "Заполните поле"
            },
            user_email: {
                required : "Заполните поле",
                email : "Невалидная электронная почта",
                regex : "Адрес должен быть вида name@domain.com"
            }
        },
        errorClass: "validate-error",
        submitHandler: function (form) {
            console.log("submitHandler");

            $("#tariff-form").closest('.modal.modal-form').removeClass('before-show show');
            $('.success-modal').addClass('before-show');
            setTimeout(function() {
                $('.success-modal').addClass('show');
            }, 50);

        }
    });
    
});