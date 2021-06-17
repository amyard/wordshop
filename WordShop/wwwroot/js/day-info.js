jQuery(function($){
    
    $(".day-info-save-data").on("click", function(event){

        event.preventDefault();
        
        var dayName = $(".day-name").val();
        var dayPosition = $(".day-position").val();
        var dayBlocks = $(".day-info-block-item");
        var dayInfoId = $("#day-info-id").val() == undefined
            ? 0
            : $("#day-info-id").val();
        

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
            dayInfoId : parseInt(dayInfoId),
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
                } else {
                    alert('Some shit happens');
                }
            },
            error: function (err) {
                alert('Some shit happens');
            }
        })
    })

})


function removeSeqItem(event) {
    event.target.parentElement.remove();
}

function addSeqItem(event) {
    var elem = $(event.target.parentElement.parentElement).find(".seq-item-list")[0];

    $(elem).append('<div class="dd-remove-seq mb-5">' +
        '<input class="form-control day-block-seq-name" type="text" placeholder="Видео урок 1. Разберем самый ..." required/>' +
        '<a class="btn btn-danger remove-seq-item" onclick="removeSeqItem(event);">-</a>' +
        '</div>');
}

function addBlock(event) {
    $(".add-additional-block").before('<div class="day-info-block-item">' +
        '<div class="d-grid grid-80">' +
        '<div>' +
        '<label>Название блока</label>' +
        '</div>' +
        '<div>' +
        '<input class="form-control day-block-name" type="text" placeholder="Чтение / Слова / Грамматика" required/>' +
        '</div>' +
        '</div>' +
        '<div class="d-grid grid-20-70-10">' +
        '<div>' +
        '<label>Пункты блока</label>' +
        '</div>' +
        '<div class="seq-item-list">' +
        '<div class="dd-remove-seq mb-5">' +
        '<input class="form-control day-block-seq-name" type="text" placeholder="Видео урок 1. Разберем самый ..." required/>' +
        '<a class="btn btn-danger remove-seq-item" onclick="removeSeqItem(event);">-</a>' +
        '</div>' +
        '</div>' +
        '<div>' +
        '<a class="btn-success btn float-right" onclick="addSeqItem(event);">+</a>' +
        '</div>' +
        '</div>' +
        '</div>');
}