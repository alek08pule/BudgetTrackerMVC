$(document).ready(function () {
    $('#list-link').on('click', function (event) {
        event.preventDefault();
        console.log("List link clicked");
        getTransactions();
    });

    $(document).on('click', '.delete-btn', function () {
        const id = $(this).data('id');
        console.log("Delete button clicked, ID:", id);
        deleteTransaction(id);
    });

    function getTransactions() {
        $.ajax({
            url: '/Transaction/GetTransactions',
            method: 'GET',
            success: function (data) {
                console.log('Received data:', data);
                if (!data || !Array.isArray(data.incomeTransactions) || !Array.isArray(data.expenseTransactions)) {
                    console.error("Expected data to have incomeTransactions and expenseTransactions arrays");
                    window.location.href = '/Transaction/GetTransactions';
                    return;
                }

                loadIncomeTransactions(data.incomeTransactions);
                loadExpenseTransactions(data.expenseTransactions);
                updateTransactionsCount('income-transaction-count', data.incomeTransactions.length);
                updateTransactionsCount('expense-transaction-count', data.expenseTransactions.length);
            },
            error: function (jqXHR) {
                var errors = jqXHR.responseJSON?.errors || {};
                var errorMessages = [];

                for (var key in errors) {
                    if (errors.hasOwnProperty(key)) {
                        errorMessages.push(errors[key]);
                    }
                }

                $('#error').html(errorMessages.join('<br/>'));
                window.location.href = '/Transaction/GetTransactions';
            }
        });
    }

    function loadIncomeTransactions(transactions) {
        $('#item-income-list').html('');
        transactions.forEach((item) => {
            const div = document.createElement('div');
            div.classList.add('listOfIncomes');
            div.id = item.id;
            div.innerHTML = `
                <div class="description">
                    <h4>${item.description}</h4>
                </div>
                <div class="symbol-amount">
                    <span>$</span>
                    <span class="income_amount">${item.amount}</span>
                </div>
                <button class="delete-btn" data-id="${item.id}"><i class="fa fa-xmark"></i></button>`
            $('#item-income-list').append(div);
        });
    }

    function loadExpenseTransactions(transactions) {
        $('#item-expense-list').html('');
        transactions.forEach((item) => {
            const div = document.createElement('div');
            div.classList.add('listOfExpenses');
            div.id = item.id;
            div.innerHTML = `
                <div class="description">
                    <h4>${item.description}</h4>
                </div>
                <div class="symbol-amount">
                    <span>$</span>
                    <span class="expense_amount">${item.amount}</span>
                </div>
                <button class="delete-btn" data-id="${item.id}"><i class="fa fa-xmark"></i></button>`
            $('#item-expense-list').append(div);
        });
    }

    function deleteTransaction(id) {
        $.ajax({
            url: '/Transaction/DeleteTransaction',
            method: 'DELETE',
            contentType: 'application/json',
            data: JSON.stringify({ id: id }),
            success: function (response) {
                if (response.success) {
                    console.log("Transaction deleted, ID:", id);
                    getTransactions(); // Reload all transactions
                    updateBalance();
                } else {
                    console.error('Error deleting transaction:', response.message);
                }
            },
            error: function (jqXHR) {
                console.error('Error deleting transaction:', jqXHR.responseText);
            }
        });
    }

    function updateBalance() {
        $.ajax({
            url: '/UserBalance/Index',
            method: 'GET',
            success: function (response) {
                $('#total-income').text(response.totalIncome);
                $('#total-expense').text(response.totalExpense);
                $('#available-money').text(response.availableMoney);
            },
            error: function (jqXHR) {
                console.error('Error updating balance:', jqXHR.responseText);
            }
        });
    }

    function updateTransactionsCount(countId, count) {
        $('#' + countId).text(count);
    }
});
