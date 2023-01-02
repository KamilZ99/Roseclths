// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

async function deleteData(url = '') {
    const token = document.querySelector('input[name="__RequestVerificationToken"]');

    const response = await fetch(url, {
        method: 'DELETE',
        mode: 'cors',
        cache: 'no-cache',
        credentials: 'same-origin',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': token.value
        },
        redirect: 'follow',
        referrerPolicy: 'no-referrer',
    });

    if (response.ok) {
        document.location.reload(true);
    } else {
        //let alert = document.createElement('div');
        //alert.classList.add('alert', 'alert-warning', 'alert-dismissible', 'fade', 'show');
        //alert.role = 'alert';
        //let closeBtn = document.createElement('button');
        //closeBtn.type = 'button';
        //closeBtn.classList.add('btn-close');
        //closeBtn.areaLabel
;    }
}