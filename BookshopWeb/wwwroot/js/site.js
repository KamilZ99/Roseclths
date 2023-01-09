// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

async function deleteRequest(url = '') {
    const token = document.querySelector('input[name="__RequestVerificationToken"]');
    const fullUrl = window.location.href + url;

    const response = await fetch(fullUrl, {
        method: 'DELETE',
        mode: 'cors',
        cache: 'no-cache',
        credentials: 'same-origin',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': token.value
        },
        redirect: 'manual',
        referrerPolicy: 'no-referrer',
    }).then(function () {
        document.location.reload(true);
    })
}