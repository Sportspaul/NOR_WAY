$(function () {
    Hjelpemetoder.endreBakgrunn();
    new NyRuteStoppEvents('#linjekode', '#stoppnavn', '#stoppnummer', '#minutterTilNesteStopp');
});

const urlParameter = window.location.search;
let id = urlParameter.substr(1);
id = id.split("?");

const lagre = $("#lagre");
$("#linjekode").prop("readonly", true);

if (id.length == 2) {
    $("#tittel").html(`Oppdater Rutestopp`);
    lagre.val("Oppdater Rutestopp");

    const url = `../RuteStopp/HentEtRuteStopp?id=${id[1]}`
    $.get(url, (ruteStopp) => {
        fyllInputfelter(ruteStopp);
    })
    lagre.click(() => {
        console.log(1);
        oppdaterRuteStopp();
    })
} else {
    $("#tittel").html("Nytt Rutestopp");
    $("#linjekode").val(id[0]);
    lagre.val("Opprett nytt rutestopp")
    lagre.click(() => {
        nyttRuteStopp();
    })
}

function fyllInputfelter(ruteStopp) {
    $("#linjekode").val(ruteStopp.linjekode);
    $("#stoppnavn").val(ruteStopp.stoppnavn);
    $("#stoppnummer").val(ruteStopp.stoppNummer);
    $("#minutterTilNesteStopp").val(ruteStopp.minutterTilNesteStopp);
}

function oppdaterRuteStopp() {
    if (validerRuteStoppInput()) {
        let ruteStopp = lagRuteStoppObjekt();
        ruteStopp.Id = id[1];
        $.post("../RuteStopp/OppdaterRuteStopp", ruteStopp, () => {
            location.replace(`ruteStopp.html?linjekode=${id[0]}`);
        })
    }
}

function nyttRuteStopp() {
    if (validerRuteStoppInput()) {
        let ruteStopp = lagRuteStoppObjekt();
        $.post("../RuteStopp/NyRuteStopp", ruteStopp, () => {
            location.replace(`ruteStopp.html?linjekode=${id[0]}`);
        })
    }
}

function lagRuteStoppObjekt() {
    return {
        Stoppnavn: $("#stoppnavn").val(),
        StoppNummer: $("#stoppnummer").val(),
        MinutterTilNesteStopp: $("#minutterTilNesteStopp").val(),
        Linjekode: $("#linjekode").val()
    }
} 