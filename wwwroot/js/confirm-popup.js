$(document).ready(function () {
    $('#btnSubmit').on('click', function (e) {
        e.preventDefault();
        $('#confirmationModal').modal('show');
    });

    $('#btnConfirmYes').on('click', function () {
        $('#formSubmit').submit();
    });

    $('#btnConfirmNo').on('click', function () {
        $('#confirmationModal').modal('hide');
    });
});

