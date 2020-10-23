// Denne filen håndterer både ny avgang og oppdater avgang
$(function () {
	Hjelpemetoder.endreBakgrunn(); // Tilpasser bakgrunnen
	new NyAvgangEvents("#linjekode", "#dato"); // Setter eventlistners
});

// Henter og formaterer verdier fra url
const urlParameter = window.location.search;
let id = urlParameter.substr(1);
id = id.split("?");

const lagre = $("#lagre");
$("#linjekode").prop("readonly", true);

if (id.length == 2) {
	// Hvis bruker skal oppdatere avgang
	$("#tittel").html(`Oppdater Avgang`);
	lagre.val("Oppdater Avgang");

	const url = `../Avgang/HentEnAvgang?id=${id[1]}`;
	$.get(url, (avgang) => {
		console.log(avgang);
		console.log(avgang.linjekode);
		fyllInputfelter(avgang);
	});
	lagre.click(() => {
		oppdaterAvgang();
	});
} else {
	// Hvis bruker skal opprette ny avgang
	$("#tittel").html("Ny Avgang");
	$("#linjekode").val(id[0]);
	lagre.val("Opprett ny avgang");
	lagre.click(() => {
		nyAvgang();
	});
}

// Fyller inputfeltene med verdier fra eksisterende avgang
function fyllInputfelter(avgang) {
	console.log(avgang.linjekode);
	$("#linjekode").val(avgang.linjekode);
	$("#dato").val(avgang.dato), $("#tidspunkt").val(avgang.tidspunkt);
}

// Function som kaller oppdaterAvgang() på server
function oppdaterAvgang() {
	if (validerAvgangInput()) {
		const avgang = lagAvgangObjekt();
		avgang.Id = id[1];
		$.post("../Avgang/OppdaterAvgang", avgang, () => {
			location.replace(`avganger.html?linjekode=${id[0]}&side=0`);
		});
	}
}

// Function som kaller nyAvgang() på server
function nyAvgang() {
	if (validerAvgangInput()) {
		let avgang = lagAvgangObjekt();
		avgang.Linjekode = $("#linjekode").val();
		avgang.SolgteBilletter = 0;
		$.post("../Avgang/NyAvgang", avgang, () => {
			location.replace(`avganger.html?linjekode=${id[0]}&side=0`);
		});
	}
}

// Returnerer verdiene i inputfeltene
function lagAvgangObjekt() {
	return {
		Dato: $("#dato").val(),
		Tidspunkt: $("#tidspunkt").val(),
	};
}
