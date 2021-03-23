"use strict"

const postQueryType = 'POST';
const IncorrectValue = -1;
const FirstIndex = 0;

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
            setLanguage(cultures[index], actionUrl, location.pathname + location.search);
        }
        languageMenu.appendChild(language);
    }
}

function setLanguage(culture, actionUrl, returnUrl) {
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

function callAvatarQuey(url) {
    $.ajax({
        type: postQueryType,
        url: url,
        success: function (result) {
            let liItem = document.getElementById("avatar");
            let img = document.createElement("img");
            img.src = result;
            img.width = "40";
            img.height = "40";
            liItem.appendChild(img);
        }
    });
}

function downloadImage(imageId, loadImage, incorrectImageFormat) {
    let image = document.getElementById(imageId);

    image.addEventListener('change', (event) => {
        let file = event.target.files[FirstIndex];

        if (file.type && file.type.indexOf('image') === IncorrectValue) {
            alert(incorrectImageFormat)
            return;
        }
        else {
            document.getElementById(loadImage).click();
        }
    });

    image.click();
}

function loadData(sumbitId, confirmQuestion) {
    let isLoad = confirm(confirmQuestion);

    if (isLoad) {
        document.getElementById(sumbitId).click();
    }
}