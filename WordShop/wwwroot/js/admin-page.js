function loadCustomerInfoDataTable() {
    var dataTable;
    
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/api/v1/get-customer-info",
            "type": "GET",
        },
        "filter": true,
        "order": [[ 0, "desc" ]],
        "columns": [
            { 'data': "id", "width": "10%" },
            { 'data': "fullName", "width": "10%" },
            { 'data': "email", "width": "10%" },
            { 'data': "phoneNumber", "width": "10%" },
            { 'data': "tariffName", "width": "10%" },
            { 'data': "tariffNewPrice", "width": "10%" },
            { 'data': "courses", "width": "10%" },
            { 'data': "createdDate", "width": "10%" },
            { 'data': "paymentStatus", "width": "10%" }
        ]
    });
}


function loadTariffBenefitDataTable() {
    var dataTable;

    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/api/v1/get-tariff-benefits",
            "type": "GET",
        },
        "filter": true,
        "order": [[ 0, "desc" ]],
        "columns": [
            { 'data': "id", "width": "10%" },
            { 'data': "benefit", "width": "70%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href="/tariff-benefit-action?id=${data}" class="btn btn-success text-white" style="cursor:pointer;">
                                Изменить запись
                            </a>
                        </div>
                        `
                }, "width": "20%"
            }
        ]
    });
}

