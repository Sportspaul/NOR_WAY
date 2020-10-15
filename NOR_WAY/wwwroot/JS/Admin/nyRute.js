
$(function () {
    Hjelpemetoder.endreBakgrunn();
});

const urlParameter = window.location.search;
const id = urlParameter.substr(1);


if(id != ""){
    $("#tittel").html(`Oppdater Rute: ${id}`);
    $("#lagre").val("Oppdater Rute")
    const url = `../Ruter/HentEnRute?linjekode=${id}`
    $.get(url, function (rute) {
        fyllInputfelter(rute);
        console.table(rute);
    })
} else {
    $("#tittel").html("Ny Rute");
    $("#lagre").val("Opprett ny rute")
} 

function fyllInputfelter(rute) {
    $("#linjekode").val(rute.linjekode);
    $("#rutenavn").val(rute.rutenavn);
    $("#startpris").val(rute.startpris);
    $("#tilleggPerStopp").val(rute.tilleggPerStopp);
    $("#kapasitet").val(rute.kapasitet);
}