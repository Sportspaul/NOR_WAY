$(function () {
    Hjelpemetoder.endreBakgrunn();
    const res = $.post("../Ruter/HentAlleRuter");
    const link = "nyRute.html";
    lagRuteoversikt(res, link);
});

function slettRad(id) {
    $.get(`../Ruter/FjernRute?linjekode=${id}`, function () {
        $(`#${id}`).remove();
    })
        .fail(function () {
            console.log("Ruten ble IKKE fjernet");
        });;
}