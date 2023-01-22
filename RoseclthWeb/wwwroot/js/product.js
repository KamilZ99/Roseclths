document.addEventListener('DOMContentLoaded', function () {
    const dataTable = new DataTable('#dataTable', {
        responsive: true,
        "ajax": {
            "url": "/Admin/Product/GetAll"
        },
        "columns": [
            { "data": "name" },
            { "data": "brand" },
            { "data": "listPrice" },
            { "data": "type.name" },
            { "data": "material.name" },
            {
                "data": "null",
                "name": "name",
                "render": function (data, type, row) {
                    return `
                    <div class="btn-group" role="group">
                        <a class="btn btn-warning ms-1" title="Edit" href="/Admin/Product/Upsert?id=${row.id}"><i class="bi bi-pencil-square"></i></a>
                        <a class="btn btn-danger" data-bs-toggle="modal" data-bs-target="#exampleModal-${row.id}" title="Delete"><i class="bi bi-trash-fill"></i></a>
                    </div>

                    <div class="modal fade" id="exampleModal-${row.id}" tabindex="-1" aria-hidden="true">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title" id="exampleModalLabel">Delete type</h5>
                                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                </div>
                                <div class="modal-body text-start">
                                    Are you sure you want to remove ${row.name}?
                                </div>
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                                    <a onclick="deleteRequest('/Delete/${row.id}')" class="btn btn-danger">Delete</a>
                                </div>
                            </div>
                        </div>
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