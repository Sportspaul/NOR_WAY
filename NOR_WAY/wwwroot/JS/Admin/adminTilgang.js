$(function () {
    harAdminTilgang();
    loggUt();
});

//Hvis bruker ikke er admin, sendes man til startsiden
function harAdminTilgang() {
    $.get("../Brukere/AdminTilgang", function (innlogget) {
        if (!innlogget) {
            window.location.href = '../index.html';
        }
    });
}

function loggUt() {
    $("#loggUt").click( () => {
        $.get("../Brukere/LoggUt", function (){
            location.replace("../index.html");
        });
    });
}