// Denne filen håndterer både ny billettype og oppdater billettype
$(function () {
	Hjelpemetoder.endreBakgrunn(); // Tilpasser bakgrunnen
	new NyBillettypeEvents("#billettype", "#rabattsats"); // Setter eventlistners
});

// Henter og formaterer verdier fra url
const urlParameter = window.location.search;
const id = urlParameter.substr(1);
const lagre = $("#lagre");

if (id != "") {
	// Hvis bruker skal oppdatere billettype
	$("#tittel").html(`Oppdater Billettype`);
	lagre.val("Oppdater Billettype");

	const url = `../Billettyper/HentEnBillettype?id=${id}`;
	$.get(url, (billettype) => {
		fyllInputfelter(billettype);
	});
	lagre.click(() => {
		oppdaterBillettype();
	});
} else {
	// Hvis bruker skal opprette ny billettype
	$("#tittel").html("Ny Billettype");
	lagre.val("Opprett ny billettype");
	lagre.click(() => {
		nyBillettype();
	});
}

// Fyller inputfeltene med verdier fra eksisterende billettype
function fyllInputfelter(billettype) {
	$("#billettype").val(billettype.billettype),
		$("#rabattsats").val(billettype.rabattsats);
}

// Function som kaller oppdaterBillettype() på server
function oppdaterBillettype() {
	if (validerBillettypeInput()) {
		let billettype = lagBillettypeObjekt();
		billettype.Id = id;
		$.post("../Billettyper/OppdaterBillettype", billettype, () => {
			location.replace("billettyper.html?nyBillettype");
		}).fail(() => {
			$("#feilBillett").html(
				"Billettypen kunne ikke oppdateres med verdiene du har skrevet inn"
			);
		});
	}
}

// Function som kaller nyBillettype() på server
function nyBillettype() {
	if (validerBillettypeInput()) {
		const billettype = lagBillettypeObjekt();
		$.post("../Billettyper/NyBillettype", billettype, () => {
			location.replace("billettyper.html?billettypeOppdatert");
		}).fail(() => {
			$("#feilBillett").html(
				"Ny Billettype kunne ikke opprettes med verdiene du har skrevet inn"
			);
		});
	}
}

// Returnerer verdiene i inputfeltene
function lagBillettypeObjekt() {
	return {
		Billettype: $("#billettype").val(),
		Rabattsats: $("#rabattsats").val(),
	};
}
