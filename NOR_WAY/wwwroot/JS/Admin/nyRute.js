$(function () {
	Hjelpemetoder.endreBakgrunn();
	new NyRuteEvents(
		"#linjekode",
		"#rutenavn",
		"#startpris",
		"#tilleggPerStopp",
		"#kapasitet"
	);
});

const urlParameter = window.location.search;
const id = urlParameter.substr(1);
const lagre = $("#lagre");

if (id != "") {
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
	$("#tittel").html("Ny Rute");
	lagre.val("Opprett ny rute");
	lagre.click(() => {
		nyRute();
	});
}

function fyllInputfelter(rute) {
	$("#linjekode").val(rute.linjekode);
	$("#rutenavn").val(rute.rutenavn);
	$("#startpris").val(rute.startpris);
	$("#tilleggPerStopp").val(rute.tilleggPerStopp);
	$("#kapasitet").val(rute.kapasitet);
}

function oppdaterRute() {
	if (validerRuteInput()) {
		const rute = lagRuteObjekt();
		$.post("../Ruter/OppdaterRute", rute, () => {
			location.replace("ruter.html?ruteOppdatert");
		});
	}
}

function nyRute() {
	if (validerRuteInput()) {
		const rute = lagRuteObjekt();
		$.post("../Ruter/NyRute", rute, () => {
			location.replace("ruter.html?nyRute");
		});
	}
}

function lagRuteObjekt() {
	return {
		Linjekode: $("#linjekode").val(),
		Rutenavn: $("#rutenavn").val(),
		Startpris: $("#startpris").val(),
		TilleggPerStopp: $("#tilleggPerStopp").val(),
		Kapasitet: $("#kapasitet").val(),
	};
}
