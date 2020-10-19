$(function () {
    harAdminTilgang();
});

//Hvis bruker ikke er admin, sendes man til startsiden
function harAdminTilgang() {
    $.get("../Brukere/AdminTilgang", function (innlogget) {
        if (!innlogget) {
            window.location.href = '../index.html';
        }
    });
}