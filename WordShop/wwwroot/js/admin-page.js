var dataTable;

$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/api/admin/get-customer-info",
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