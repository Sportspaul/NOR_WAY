// Denne filen håndterer oppdater av Stopp
$(function () {
	Hjelpemetoder.endreBakgrunn(); // Tilpasser bakgrunnen
	new NyStoppEvents("#stoppnavn"); // Setter eventlistners
});

// Henter og formaterer verdier fra url
const urlParameter = window.location.search;
const id = urlParameter.substr(1);
const lagre = $("#lagre");

// Setter siden sin overskrift
$("#tittel").html(`Oppdater Stoppnavn`);
lagre.val("Oppdater Stoppnavn");

// Eventlistner på lagreknappen
lagre.click(() => {
	oppdaterStoppnavn();
});

// Henter stoppet som skal hentes fra DB
const url = `../Stopp/HentEtStopp?id=${id}`;
$.get(url, (stopp) => {
	fyllInputfelter(stopp); // Fyller inputfeltet med stoppnavn
});

// Fyller inputfeltene med verdier fra eksisterende stopp
function fyllInputfelter(stopp) {
	$("#stoppnavn").val(stopp.navn);
}

// Function som kaller oppdaterStoppnavn() på server
function oppdaterStoppnavn() {
	if (validerEtStoppnavn($("#stoppnavn"), "#feilStoppnavn")) {
		let stopp = lagStoppObjekt();
		console.log(stopp);
		stopp.id = id;
		$.post("../stopp/oppdaterstoppnavn", stopp, () => {
			location.replace("stopp.html?nystoppnavn");
		});
	}
}

// Returnerer verdiene i inputfeltene
function lagStoppObjekt() {
	return {
		Navn: $("#stoppnavn").val(),
	};
}
