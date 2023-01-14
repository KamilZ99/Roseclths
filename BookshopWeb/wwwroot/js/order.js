document.addEventListener('DOMContentLoaded', function () {

    const url = window.location.search;
    let status = "/";

    if (url.includes("inprocess")) {
        status = "?status=inprocess"
    } else if (url.includes("pending")) {
        status = "?status=pending"
    } else if (url.includes("completed")) {
        status = "?status=completed"
    } else if (url.includes("all")) {
        status = "?status=approved"
    } else if (url.includes("approved")) {
        status = "?status=all"
    }

    const dataTable = new DataTable('#dataTable', {
        responsive: true,
        "ajax": {
            "url": `/Admin/Order/GetAll${status}`
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
                "name": "title",
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