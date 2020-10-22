// Denne filen håndterer både ny RuteStopp og oppdater TuteStopp
$(function () {
	Hjelpemetoder.endreBakgrunn(); // Tilpasser bakgrunnen

	// Setter eventlistners
	new NyRuteStoppEvents(
		"#linjekode",
		"#stoppnavn",
		"#stoppnummer",
		"#minutterTilNesteStopp"
	);
});

// Henter og formaterer verdier fra url
const urlParameter = window.location.search;
let id = urlParameter.substr(1);
id = id.split("?");

const lagre = $("#lagre");
$("#linjekode").prop("readonly", true);

if (id.length == 2) {
	// Hvis bruker skal oppdatere RuteStopp
	$("#tittel").html(`Oppdater Rutestopp`);
	lagre.val("Oppdater Rutestopp");

	const url = `../RuteStopp/HentEtRuteStopp?id=${id[1]}`;
	$.get(url, (ruteStopp) => {
		fyllInputfelter(ruteStopp);
	});
	lagre.click(() => {
		console.log(1);
		oppdaterRuteStopp();
	});
} else {
	// Hvis bruker skal opprette ny RuteStopp
	$("#tittel").html("Nytt Rutestopp");
	$("#linjekode").val(id[0]);
	lagre.val("Opprett nytt rutestopp");
	lagre.click(() => {
		nyttRuteStopp();
	});
}

// Fyller inputfeltene med verdier fra eksisterende RuteStopp
function fyllInputfelter(ruteStopp) {
	$("#linjekode").val(ruteStopp.linjekode);
	$("#stoppnavn").val(ruteStopp.stoppnavn);
	$("#stoppnummer").val(ruteStopp.stoppNummer);
	$("#minutterTilNesteStopp").val(ruteStopp.minutterTilNesteStopp);
}

// Function som kaller oppdaterRuteStopp() på server
function oppdaterRuteStopp() {
	if (validerRuteStoppInput()) {
		let ruteStopp = lagRuteStoppObjekt();
		ruteStopp.Id = id[1];
		$.post("../RuteStopp/OppdaterRuteStopp", ruteStopp, () => {
			location.replace(`ruteStopp.html?linjekode=${id[0]}`);
		});
	}
}

// Function som kaller nyRuteStopp() på server
function nyttRuteStopp() {
	if (validerRuteStoppInput()) {
		let ruteStopp = lagRuteStoppObjekt();
		$.post("../RuteStopp/NyRuteStopp", ruteStopp, () => {
			location.replace(`ruteStopp.html?linjekode=${id[0]}`);
		});
	}
}

// Returnerer verdiene i inputfeltene
function lagRuteStoppObjekt() {
	return {
		Stoppnavn: $("#stoppnavn").val(),
		StoppNummer: $("#stoppnummer").val(),
		MinutterTilNesteStopp: $("#minutterTilNesteStopp").val(),
		Linjekode: $("#linjekode").val(),
	};
}
