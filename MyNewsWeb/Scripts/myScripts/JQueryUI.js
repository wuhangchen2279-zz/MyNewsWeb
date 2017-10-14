$(function () {
    //date picker ui
    $("#dobDatePicker").datepicker({ dateFormat: 'dd/mm/yy' });
    $("#dobDatePicker2").datepicker({ dateFormat: 'dd/mm/yy' });

    //password match check
    $("#passwordverify").keyup(validate);
});

function validate() {
    var password1 = $("#passwordin").val();
    var password2 = $("#passwordverify").val();

    if (password1 != password2) {
        $("#validate-status").show();
        $("#validate-status").text("Confirmed password must be same as password typed by user!");
    } else {
        $("#validate-status").hide();
    }
}