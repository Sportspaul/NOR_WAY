$(function () {
	Hjelpemetoder.endreBakgrunn();
	new NyAvgangEvents("#linjekode", "#dato");
});

const urlParameter = window.location.search;
let id = urlParameter.substr(1);
id = id.split("?");

const lagre = $("#lagre");
$("#linjekode").prop("readonly", true);

if (id.length == 2) {
	$("#tittel").html(`Oppdater Avgang`);
	lagre.val("Oppdater Avgang");

	const url = `../Avgang/HentEnAvgang?id=${id[1]}`;
	$.get(url, (avgang) => {
		fyllInputfelter(avgang);
	});
	lagre.click(() => {
		oppdaterAvgang();
	});
} else {
	$("#tittel").html("Ny Avgang");
	$("#linjekode").val(id[0]);
	lagre.val("Opprett ny avgang");
	lagre.click(() => {
		nyAvgang();
	});
}

function fyllInputfelter(avgang) {
	$("#linjekode").val(avgang.linjekode);
	$("#dato").val(avgang.dato), $("#tidspunkt").val(avgang.tidspunkt);
}

function oppdaterAvgang() {
	if (validerAvgangInput()) {
		const avgang = lagAvgangObjekt();
		avgang.Id = id[1];
		$.post("../Avgang/OppdaterAvgang", avgang, () => {
			location.replace(`avganger.html?linjekode=${id[0]}&side=0`);
		});
	}
}

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

function lagAvgangObjekt() {
	return {
		Dato: $("#dato").val(),
		Tidspunkt: $("#tidspunkt").val(),
	};
}
