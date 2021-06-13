jQuery(function($){
    
    $("#course-start-form").on("submit", function(event){
        event.preventDefault();

        console.log("preventDefault")

        var advantage = $(".advantage-data .tariff-item");
        var disadvantage = $(".disadvantage-data .tariff-item");

        var arr = [];
        var defaultTariffId = 1;
        var tariffId = parseInt($("#tariff-editor-id").val());

        if (advantage.length > 0) {
            for (var i = 0; i < advantage.length; i++) {
                var orderedId = $(advantage[i]).find(".item-tariff-id").val();
                arr.push({orderPosition: i+1, tariffBenefitId: orderedId, advantageTariffId: tariffId, disadvantageTariffId: defaultTariffId});
            }
        }

        if (disadvantage.length > 0) {
            for (var i = 0; i < disadvantage.length; i++) {
                var orderedId = $(disadvantage[i]).find(".item-tariff-id").val();
                arr.push({orderPosition: i+1, tariffBenefitId: orderedId, advantageTariffId: defaultTariffId, disadvantageTariffId: tariffId});
            }
        }

        $.ajax({
            type : "POST",
            url : "api/v1/save-tariff-benefit/" + tariffId,
            dataType : "json",
            contentType : "application/json; charset=utf-8",
            data : JSON.stringify(arr),
            success: function (r){
                if (r.success) {
                    event.currentTarget.submit();
                } else {
                    alert('Some shit happens');
                }
            },
            error: function (err) {
                alert('Some shit happens');
            }
        })
    });
    
})