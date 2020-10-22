// Denne filen håndterer både ny rute og oppdater rute
$(function () {
	Hjelpemetoder.endreBakgrunn(); // Tilpasser bakgrunnen

	// Setter eventlistners
	new NyRuteEvents(
		"#linjekode",
		"#rutenavn",
		"#startpris",
		"#tilleggPerStopp",
		"#kapasitet"
	);
});

// Henter og formaterer verdier fra url
const urlParameter = window.location.search;
const id = urlParameter.substr(1);
const lagre = $("#lagre");

if (id != "") {
	// Hvis bruker skal oppdatere en rute
	$("#tittel").html(`Oppdater Rute`);
	$("#linjekode").prop("readonly", true);
	lagre.val("Oppdater Rute");

	const url = `../Ruter/HentEnRute?linjekode=${id}`;
	$.get(url, (rute) => {
		fyllInputfelter(rute);
	});
	lagre.click(() => {
		oppdaterRute();
	});
} else {
	// Hvis bruker skal opprette en ny rute
	$("#tittel").html("Ny Rute");
	lagre.val("Opprett ny rute");
	lagre.click(() => {
		nyRute();
	});
}

// Fyller inputfeltene med verdier fra eksisterende rute
function fyllInputfelter(rute) {
	$("#linjekode").val(rute.linjekode);
	$("#rutenavn").val(rute.rutenavn);
	$("#startpris").val(rute.startpris);
	$("#tilleggPerStopp").val(rute.tilleggPerStopp);
	$("#kapasitet").val(rute.kapasitet);
}

// Function som kaller oppdaterRute() på server
function oppdaterRute() {
	if (validerRuteInput()) {
		const rute = lagRuteObjekt();
		$.post("../Ruter/OppdaterRute", rute, () => {
			location.replace("ruter.html?ruteOppdatert");
		});
	}
}

// Function som kaller nyRute() på server
function nyRute() {
	if (validerRuteInput()) {
		const rute = lagRuteObjekt();
		$.post("../Ruter/NyRute", rute, () => {
			location.replace("ruter.html?nyRute");
		});
	}
}

// Returnerer verdiene i inputfeltene
function lagRuteObjekt() {
	return {
		Linjekode: $("#linjekode").val(),
		Rutenavn: $("#rutenavn").val(),
		Startpris: $("#startpris").val(),
		TilleggPerStopp: $("#tilleggPerStopp").val(),
		Kapasitet: $("#kapasitet").val(),
	};
}
