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
        var tariffId = $(this).closest('.tariffs-item').find('.tariffs-item-title').attr("data-id");
        
        var scrollWidth = $(window).outerWidth() - $(window).width();
        
        var body = $('body');
        body.css({'padding-right': scrollWidth+'px'});
        body.addClass('open-modal');

        var modal = $('#'+$(this).attr('data-modal'));
        modal.find('.tariff-name').text(tariffTitle);
        modal.find('input#user-tariff').val(tariffTitle);
        modal.attr("data-id", tariffId);
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
                required : true,
                minlength : 3
            },
            user_email: {
                required : true,
                email : true,
                minlength: 7,
                regex : /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/
            },
            user_phone: {
                required : true
            }
        },
        messages: {
            user_name: {
                required : "Заполните поле",
                minlength: "Имя должно быть более 3-х и меньше 80-ти символов"
            },
            user_email: {
                required : "Заполните поле",
                email : "Невалидная электронная почта",
                minlength: "Поле должно быть более 7-ми и меньше 120-ти символов",
                regex : "Адрес должен быть вида name@domain.com"
            },
            user_phone: {
                required : "Заполните поле"
            }
        },
        errorClass: "validate-error",
        submitHandler: function (form) {
            // remove old errors 
            if(!$(".modal-error").hasClass("d-none")) {
                $(".modal-error").addClass("d-none");
                $(".modal-error-footer").remove();
            };
            
            var data = {
                'fullName': $("#user_name").val(),
                'email': $("#user_email").val(),
                'phoneNumber': $("#user_phone").val(),
                "tariffId" : $("#tariff-modal").attr("data-id")
            };
            
            $.ajax({
                type : "POST",
                url : "save-customer-info",
                dataType : "json",
                contentType : "application/json; charset=utf-8",
                data : JSON.stringify(data),
                success: function (r){
                    if(r.success) {
                        // redirect to payment
                        window.location.href = r.redirectUrl;
                        
                        $("#tariff-form")[0].reset();
                    } else if (r.error == 1) {
                        $(".modal-error").append('<div class="modal-error-footer">'+r.message+'</div>');
                        $(".modal-error").removeClass("d-none");
                    } else if (r.error == 2) {
                        // TODO --> тут ошибки которые связаные с кодом курса и по exception
                        $(".modal-error").removeClass("d-none");
                    }
                },
                error: function (r) {
                    console.log("error ", r);
                },
                failure: function () {
                    console.log("failure")
                }
            });
        }
    });


    $(document).ready(function() {
        var url = window.location.href;
        var domain = window.location.origin;
        
        if(url.includes("?orderId=")) {
            history.pushState({}, null, domain);

            // Modal to display success message
            $("body").addClass("open-modal")
            $("#tariff-form").closest('.modal.modal-form').removeClass('before-show show');
            $('.success-modal').addClass('before-show show');
        }
    });
   
});