const initialPrice = document.getElementById("InitialPrice");
const discountAmount = document.getElementById("DiscountAmount");
const discountPercent = document.getElementById("DiscountPercent");
const finalPrice = document.getElementById("FinalPrice");

let updating = false;

function calculateFromAmount() {

    if (updating) return;
    updating = true;

    const price = parseFloat(initialPrice.value) || 0;
    let discount = parseFloat(discountAmount.value) || 0;

    if (discount < 0) discount = 0;
    if (discount > price) discount = price;

    discountAmount.value = discount.toFixed(0);

    const percent = price === 0 ? 0 : (discount / price) * 100;

    discountPercent.value = percent.toFixed(2);
    finalPrice.value = (price - discount).toFixed(0);

    updating = false;
}

function calculateFromPercent() {

    if (updating) return;
    updating = true;

    const price = parseFloat(initialPrice.value) || 0;
    let percent = parseFloat(discountPercent.value) || 0;

    if (percent < 0) percent = 0;
    if (percent > 100) percent = 100;

    discountPercent.value = percent.toFixed(2);

    const discount = price * percent / 100;

    discountAmount.value = discount.toFixed(0);
    finalPrice.value = (price - discount).toFixed(0);

    updating = false;
}

discountAmount.addEventListener("input", calculateFromAmount);
discountPercent.addEventListener("input", calculateFromPercent);

initialPrice.addEventListener("input", calculateFromAmount);

calculateFromAmount();