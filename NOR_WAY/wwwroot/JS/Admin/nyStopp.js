
$(function () {
    Hjelpemetoder.endreBakgrunn();
    new NyStoppEvents('#stoppnavn');
});

const urlParameter = window.location.search;
const id = urlParameter.substr(1);
const lagre = $("#lagre");

$("#tittel").html(`Oppdater Stoppnavn`);
lagre.val("Oppdater Stoppnavn");
    
const url = `../Stopp/HentEtStopp?id=${id}`
$.get(url, function (stopp) {
    fyllInputfelter(stopp);
})
lagre.click(()=> {
    oppdaterStoppnavn();
})

function fyllInputfelter(stopp) {
    $("#stoppnavn").val(stopp.navn)
}

function oppdaterStoppnavn() {
    if(validerEtStoppnavn($("#stoppnavn"), "#feilStoppnavn")){ 
        let stopp = lagStoppObjekt();
        console.log(stopp);
        stopp.id = id;
        $.post("../stopp/oppdaterstoppnavn", stopp, function () {
            location.replace("stopp.html?nystoppnavn");
        })
    }
}

function lagStoppObjekt(){
    return {
        Navn: $("#stoppnavn").val()
    }
} 