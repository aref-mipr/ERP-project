

document.addEventListener("DOMContentLoaded", function () {
    const selectors = document.querySelectorAll(".sub-selector");

    selectors.forEach(function (selector) {
        const parentId = selector.getAttribute("data-parent-id");

        if (!parentId) {
            selector.innerHTML = '<option value="">محصول نامعتبر است</option>';
            return;
        }

        const url = `?handler=ItemsByOrderId&orderId=${encodeURIComponent(parentId)}`;

        fetch(url).then(response => {
            if (!response.ok)
                throw new Error('Network error');

            return response.json();

        }).then(data => {
            selector.innerHTML = '<option value="">-- انتخاب محصول --</option>';

            data.forEach(function (orderItem) {
                const option = document.createElement("option");
                option.value = orderItem.productItemId;
                option.textContent = orderItem.productItemCode;
                selector.appendChild(option);
            });
        }).catch(error => {
            console.error("Error fetching products:", error);
            selector.innerHTML = '<option value="">خطا در بارگذاری</option>';
        });
        selector.addEventListener("change", function () {
            const selectedId = this.value;
            if (selectedId && selectedId !== "")
                window.location.href = `/ProductItem/Details?id=${selectedId}`;
        });
    });
});