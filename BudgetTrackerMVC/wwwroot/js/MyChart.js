let myChart;
let ctx = document.getElementById("myChart");

function CreateChart(chartData) {
    if (myChart) {
        // If the chart already exists, update the data
        myChart.data.datasets[0].data = chartData;
        myChart.update();  // Update the chart with new data
    } else {
        // Create a new chart if it doesn't exist
        myChart = new Chart(ctx, {
            type: 'doughnut',
            data: {
                labels: ["Income", "Expense", "Available"],
                datasets: [{
                    data: chartData,
                    backgroundColor: ["green", "red", "pink"]
                }]
            },
            options: {
                responsive: true,
                plugins: {
                    legend: {
                        position: 'top',
                    }
                }
            }
        });
    }
}

$(function () {
    // Handling form submission
    $('#transactionForm').off('submit').on('submit', function (event) {
        event.preventDefault(); // Prevent the default form submission

        $.ajax({
            url: '/Transaction/AddTransaction',
            method: 'POST',
            data: $(this).serialize(), // Serialize the form data
            success: function (response) {
                // Update values in the DOM
                $("#amount_earned").text(response.TotalIncome);
                $("#amount_spent").text(response.TotalExpense);
                $("#amount_available").text(response.TotalIncome - response.TotalExpense);

                // Prepare the updated chart data
                const updatedChartData = [response.TotalIncome, response.TotalExpense, response.TotalIncome - response.TotalExpense];
                console.log("Updated Chart Data:", updatedChartData);

                // Update the chart with the latest data
                CreateChart(updatedChartData);

                // Reset the form and UI elements
                $('#transactionForm')[0].reset();
                $('#error').empty();
                $('#add').prop('disabled', false);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.error("Error updating balance data:", errorThrown);
            }
        });
    });
});

// Initial chart data fetching from the `chartData` script tag
document.addEventListener("DOMContentLoaded", function () {
    const chartDataScript = document.getElementById("chartData");
    if (chartDataScript) {
        try {
            const initialChartData = JSON.parse(chartDataScript.textContent);
            console.log("Initial Chart Data:", initialChartData);
            CreateChart(initialChartData); // Initialize chart with initial data
        } catch (error) {
            console.error("Error parsing chart data:", error);
        }
    }
});
