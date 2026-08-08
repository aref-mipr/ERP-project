document.addEventListener("DOMContentLoaded", () => {

    const chartCanvas = document.getElementById("capitalChart");

    const chartScroll = document.querySelector(".capital-chart-scroll");

    const capitalChart = new Chart(chartCanvas, {

        type: "line",

        data: {

            labels: weeks,

            datasets: [

                {

                    label: "سرمایه",

                    data: capitalData,

                    borderColor: "#2563eb",

                    backgroundColor: "rgba(37, 99, 235, 0.10)",

                    borderWidth: 2.5,

                    pointRadius: 4,

                    pointHoverRadius: 6,

                    pointBackgroundColor: "#ffffff",

                    pointBorderColor: "#2563eb",

                    pointBorderWidth: 2,

                    fill: true,

                    tension: 0.35

                }

            ]

        },

        options: {

            responsive: false,

            maintainAspectRatio: false,

            animation: {

                duration: 700

            },

            interaction: {

                intersect: false,

                mode: "index"

            },

            plugins: {

                legend: {

                    display: false

                },

                tooltip: {

                    rtl: true,

                    textDirection: "rtl",

                    padding: 30,

                    titleFont: {

                        size: 18,

                        weight: "600"

                    },

                    bodyFont: {

                        size: 16,

                        weight: "500"

                    },

                    displayColors: false,

                    callbacks: {

                        label: function (context) {

                            return "سرمایه: " +
                                context.parsed.y.toLocaleString("en-US") +
                                " تومان";

                        }

                    }

                }

            },

            scales: {

                x: {

                    grid: {

                        display: false

                    },

                    ticks: {

                        color: "#334155",

                        font: {

                            size: 18,

                            weight: "600",

                        },

                        maxRotation: 0,

                        minRotation: 0,

                        autoSkip: false,

                        padding: 20,

                    }

                },

                y: {

                    beginAtZero: false,

                    grid: {

                        color: "#eef2f7",

                        drawBorder: false

                    },

                    ticks: {

                        color: "transparent",

                        font: {

                            size: 18,

                            weight: "600"

                        },

                        padding: 10,

                        callback: function (value) {

                            return value.toLocaleString("en-US");

                        }

                    }

                }

            }

        }

    });

    setTimeout(() => {

        chartScroll.scrollLeft =
            chartScroll.scrollWidth -
            chartScroll.clientWidth;

    }, 100);

});