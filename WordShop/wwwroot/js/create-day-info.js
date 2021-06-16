jQuery(function($){

    $(".day-info-save-data").on("click", function(event){

        event.preventDefault();

        console.log("clicked");
        var dayName = $(".day-name").val();
        var dayPosition = $(".day-position").val();
        var dayBlocks = $(".day-info-block-item");

        var arr = [];
        if (dayBlocks.length > 0) {
            $(dayBlocks).each(function (item, value){
                var blockTitle = $($(value).find(".day-block-name")[0]).val();
                var seqData = $(value).find(".day-block-seq-name").map(function(){return $(this).val();}).get();
                var cleanedData = seqData.filter(function(e){return e});

                if(blockTitle.length > 0) {
                    arr.push({blockTitle : blockTitle, text : cleanedData})
                }
            })
        }

        var data = {
            dayName : dayName,
            dayPosition : parseInt(dayPosition),
            blockInfo : arr
        };

        $.ajax({
            type : "POST",
            url : "/api/v1/create-day-info/",
            dataType : "json",
            contentType : "application/json; charset=utf-8",
            data : JSON.stringify(data),
            success: function (r){
                if (r.success) {
                    alert('Сохранил.');
                    setTimeout(function (){
                        document.location.href = '/day-info'
                    }, 50)
                }
            },
            error: function (err) {
                alert('Some shit happens');
            }
        })
    })

})