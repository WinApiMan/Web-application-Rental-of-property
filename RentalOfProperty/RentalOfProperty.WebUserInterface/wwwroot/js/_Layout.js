"use strict"

function createLanguagesList(languages, cultures, actionUrl) {
    const languageMenuId = "language-menu", languageClassName = "dropdown-item", buttonTag = "input";
    const buttonType = "button", itemType = "type";

    let languageMenu = document.getElementById(languageMenuId);

    for (let index = 0; index < languages.length; index++) {
        let language = document.createElement(buttonTag);
        language.setAttribute(itemType, buttonType);
        language.className = languageClassName;
        language.value = languages[index];
        language.onclick = function () {
            setLanguage(cultures[index], actionUrl, location.pathname);
        }
        languageMenu.appendChild(language);
    }
}

function setLanguage(culture, actionUrl, returnUrl) {
    const postQueryType = 'POST';

    $.ajax({
        type: postQueryType,
        url: actionUrl,
        data: { culture: culture, returnUrl: returnUrl },
        success: function (result) {
            window.location.href = result;
        },
        error: function () {
            alert('Error query');
        }
    });
}

function downloadImage(imageId) {
    const fileIndex = 0;
    let image = document.getElementById(imageId);

    image.addEventListener('change', (event) => {
        let file = event.target.files[fileIndex];

        let fileReader = new FileReader();

        fileReader.addEventListener('load', (event) => {
            let array = new Uint8Array(event.target.result);
        });

        fileReader.readAsArrayBuffer(file);
    });

    image.click();
}