$('#formForLogin').submit((event) => {
    var recaptcha = $("#g-recaptcha-response").val();
    if (recaptcha === "") {
        event.preventDefault();
        alert("Please verify you are not a robot");
    }
})