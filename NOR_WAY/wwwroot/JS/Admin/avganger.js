$(function () {
    let url = window.location.href.split("=");
    sidenummer = url[2];
    linjekode = url[1].split("&")
    linjekode = linjekode[0];
    
    let res = $.get(`../Avgang/HentAvganger?linjekode=${linjekode}&sidenummer=${sidenummer}`);
    lagAvgangOversikt(res, "CUD", `nyAvgang.html?${linjekode}`);
});

function slettRad(id) {
    $.get(`../Avgang/FjernAvgang?id=${id}`, function () {
        $(`#${id}`).remove();
    })
    .fail(function () {
        console.log("Avgangen ble IKKE fjernet");
    });;
}
