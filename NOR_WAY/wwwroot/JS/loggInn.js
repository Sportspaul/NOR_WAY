
function loggInn() {
    console.log($("#brukernavn").val());
    const brukernavnOK = validerBrukernavn($("#brukernavn"), '#feil');
    const passordOK = validerPassord($("#passord"),'#feil');

    if (brukernavnOK && passordOK) {
        const bruker = {
            brukernavn: $("#brukernavn").val(),
            passord: $("#passord").val()
        }
        $.post("Brukere/LoggInn", bruker, function (OK) {
            if (OK) {
                window.location.href = 'index.html';
            }
            else {
                $("#feil").html("Feil brukernavn eller passord");
            }
        })
            .fail(function (feil) {
                $("#feil").html("Feil på server - prøv igjen senere: " + feil.responseText + " : " + feil.status + " : " + feil.statusText);
            });
    } else {
        $("#feil").html("Feil brukernavn eller passord");
    }
}