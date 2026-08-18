
const dropZone = document.getElementById("dropZone");
const profileImage = document.getElementById("profileImage");
const profileImageName = document.getElementById("profileImageName");

const dropTitle = document.getElementById("dropTitle");
const dropDescription = document.getElementById("dropDescription");
const dropIcon = document.getElementById("dropIcon");


// انتخاب فایل با کلیک
profileImage.addEventListener("change", function () {

    if (this.files.length > 0) {

        const file = this.files[0];
        setImage(file);
    }

});

// Drag Over
dropZone.addEventListener("dragover", function (event) {

    event.preventDefault();
    dropZone.classList.add("border-primary");
    dropZone.classList.add("bg-primary-subtle");

});


// Drag Leave
dropZone.addEventListener("dragleave", function () {

    dropZone.classList.remove("border-primary");
    dropZone.classList.remove("bg-primary-subtle");

});


// Drop
dropZone.addEventListener("drop", function (event) {

    event.preventDefault();
    dropZone.classList.remove("border-primary");
    dropZone.classList.remove("bg-primary-subtle");

    if (event.dataTransfer.files.length > 0) {

        const file = event.dataTransfer.files[0];

        if (!file.type.startsWith("image/")) {

            alert("لطفاً یک فایل تصویری انتخاب کنید.");

            return;
        }

        profileImage.files = event.dataTransfer.files;

        setImage(file);
    }

});


function setImage(file) {

    // فقط نام و پسوند فایل
    profileImageName.value = file.name;
    dropIcon.innerHTML = "✅";  
    dropTitle.textContent = file.name;
    dropDescription.textContent =
    "تصویر با موفقیت انتخاب شد";

}
