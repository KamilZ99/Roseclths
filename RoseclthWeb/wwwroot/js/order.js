document.addEventListener('DOMContentLoaded', function () {

    const url = window.location.search;
    let status = "/";
   
    const dataTable = new DataTable('#dataTable', {
        responsive: true,
        "ajax": {
            "url": `/Admin/Order/GetAll`
        },
        "columns": [
            { "data": "id" },
            { "data": "name" },
            { "data": "phoneNumber" },
            { "data": "applicationUser.email" },
            { "data": "orderStatus" },
            { "data": "totalPrice" },
            {
                "data": "null",
                "name": "name",
                "render": function (data, type, row) {
                    return `
                    <div class="btn-group" role="group">
                        <a class="btn btn-warning ms-1" title="Details" href="/Admin/Order/Details?orderId=${row.id}"><i class="bi bi-pencil-square"></i></a>
                    </div>
                    `
                }
            },
        ],
        columnDefs: [
            {
                targets: -1,
                className: 'dt-body-center'
            }
        ]
    });
});