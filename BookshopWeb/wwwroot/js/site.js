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
        const alert = makeAlert('danger', `<strong>Error!</strong> An unexpected error occurred while deleting a category.`)
        const container = document.querySelector('.container');
        console.log(alert);
        container.prepend(alert);
    }
}

function makeAlert(style, alertText) {
    let alertDiv = document.createElement('div');
    alertDiv.classList.add('alert', `alert-${style}`, 'alert-dismissible', 'fade', 'show');
    alertDiv.setAttribute('role', 'alert');

    let alertCloseBtn = document.createElement('button');
    alertCloseBtn.type = 'button';
    alertCloseBtn.classList.add('btn-close');
    alertCloseBtn.setAttribute('data-bs-dismiss', 'alert');
    alertCloseBtn.setAttribute('aria-label', 'Close');

    let cross = document.createElement('span');
    cross.setAttribute('aria-hidden', 'true');

    alertDiv.innerHTML = alertText;
    alertCloseBtn.appendChild(cross);
    alertDiv.appendChild(alertCloseBtn);

    return alertDiv;
}