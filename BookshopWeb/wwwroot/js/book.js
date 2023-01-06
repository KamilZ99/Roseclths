document.addEventListener('DOMContentLoaded', function () {
    const dataTable = new DataTable('#dataTable', {
        "ajax": {
            "url": "/Admin/Book/GetAll"
        },
        "columns": [
            { "data": "title" },
            { "data": "isbn" },
            { "data": "author" },
            { "data": "listPrice" }
        ]
    });
});