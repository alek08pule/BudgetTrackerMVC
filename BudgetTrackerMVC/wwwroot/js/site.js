$(function () {
    $('#transactionForm').off('submit').on('submit', function (event) {
        event.preventDefault();

        $('#add').prop('disabled', true);
        $.ajax({
            url: '/Transaction/AddTransaction',
            method: 'POST',
            data: $(this).serialize(),
            success: function (data) {
                $('#amount_earned').text(data.totalIncome.toFixed(2));
                $('#amount_available').text(data.availableMoney.toFixed(2));
                $('#amount_spent').text(data.totalExpense.toFixed(2));
                $('#transactionForm')[0].reset();
                $('#error').empty();
                $('#add').prop('disabled', false);
            },
            error: function (jqXHR) {
                var errors = jqXHR.responseJSON.errors;
                var errorMessages = [];

                for (var key in errors) {
                    if (errors.hasOwnProperty(key)) {
                        errorMessages.push(errors[key]);
                    }
                }

                $('#error').html(errorMessages.join('<br/>'));
            }
        });
    });
});
