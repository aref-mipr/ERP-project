const sidebar = document.getElementById("sidebar");
const overlay = document.getElementById("sidebarOverlay");
const btn = document.getElementById("sidebarToggle");
const closeBtn = document.getElementById("closeSidebar");
const icon = document.getElementById("menuIcon");

function openSidebar() {

    sidebar.classList.add("show");
    overlay.classList.add("show");

    icon.className = "bi bi-x-lg";

}

function closeSidebar() {

    sidebar.classList.remove("show");
    overlay.classList.remove("show");

    icon.className = "bi bi-list";

}

btn.addEventListener("click", () => {

    if (sidebar.classList.contains("show")) {

        closeSidebar();

    } else {

        openSidebar();

    }

});

overlay.addEventListener("click", closeSidebar);

closeBtn.addEventListener("click", closeSidebar);

window.addEventListener("resize", () => {

    if (window.innerWidth >= 992) {

        sidebar.classList.remove("show");
        overlay.classList.remove("show");

        icon.className = "bi bi-list";
    }

});