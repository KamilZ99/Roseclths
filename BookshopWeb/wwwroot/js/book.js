document.addEventListener('DOMContentLoaded', function () {
    const dataTable = new DataTable('#dataTable', {
        responsive: true,
        "ajax": {
            "url": "/Admin/Book/GetAll"
        },
        "columns": [
            { "data": "title" },
            { "data": "isbn" },
            { "data": "author" },
            { "data": "listPrice" },
            { "data": "category.name" },
            { "data": "coverType.name" },
            {
                "data": "null",
                "name": "title",
                "render": function (data, type, row) {
                    return `
                    <div class="btn-group" role="group">
                        <a class="btn btn-warning ms-1" title="Edit" href="/Admin/Book/Upsert?id=${row.id}"><i class="bi bi-pencil-square"></i></a>
                        <a class="btn btn-danger" data-bs-toggle="modal" data-bs-target="#exampleModal-${row.id}" title="Delete"><i class="bi bi-trash-fill"></i></a>
                    </div>

                    <div class="modal fade" id="exampleModal-${row.id}" tabindex="-1" aria-hidden="true">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title" id="exampleModalLabel">Delete category</h5>
                                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                </div>
                                <div class="modal-body text-start">
                                    Are you sure you want to remove ${row.title}?
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






/*

<div class="btn-group" role="group">
    <a asp-controller="Category" asp-action="Edit" asp-route-id="@category.Id" class="btn btn-warning ms-1" title="Edit"><i class="bi bi-pencil-square"></i></a>
    <a class="btn btn-danger" data-bs-toggle="modal" data-bs-target="#exampleModal-@category.Id" title="Delete"><i class="bi bi-trash-fill"></i></a>
</div>

<div class="modal fade" id="exampleModal-${data}" tabindex="-1" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="exampleModalLabel">Delete category</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body text-start">
                Are you sure you want to remove ${data.Name}?
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                <a onclick="deleteRequest('/Delete/${data}')" class="btn btn-danger">Delete</a>
            </div>
        </div>
    </div>
</div>


*/