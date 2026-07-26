

document.addEventListener("DOMContentLoaded", function () {
    const selectors = document.querySelectorAll(".sub-selector");

    selectors.forEach(function (selector) {
        const parentId = selector.getAttribute("data-parent-id");

        if (!parentId) {
            selector.innerHTML = '<option value="">دسته‌بندی نامعتبر است</option>';
            return;
        }

        const url = `?handler=ProductsByCategoryId&categoryId=${encodeURIComponent(parentId)}`;

        fetch(url).then(response => {
            if (!response.ok)
                throw new Error('Network error');

            return response.json();

        }).then(data => {
            selector.innerHTML = '<option value="">-- انتخاب محصول --</option>';

            data.forEach(function (product) {
                const option = document.createElement("option");
                option.value = product.id;
                option.textContent = product.name;
                selector.appendChild(option);
            });
        }).catch(error => {
            console.error("Error fetching products:", error);
            selector.innerHTML = '<option value="">خطا در بارگذاری</option>';
        });
        selector.addEventListener("change", function () {
            const selectedId = this.value;
            if (selectedId && selectedId !== "")
                window.location.href = `/Product/Details?id=${selectedId}`;
        });
    });
});