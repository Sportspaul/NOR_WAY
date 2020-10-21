$(function () {
	Hjelpemetoder.endreBakgrunn();
	new NyBillettypeEvents("#billettype", "#rabattsats");
});

const urlParameter = window.location.search;
const id = urlParameter.substr(1);
const lagre = $("#lagre");

if (id != "") {
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
	$("#tittel").html("Ny Billettype");
	lagre.val("Opprett ny billettype");
	lagre.click(() => {
		nyBillettype();
	});
}

function fyllInputfelter(billettype) {
	$("#billettype").val(billettype.billettype),
		$("#rabattsats").val(billettype.rabattsats);
}

function oppdaterBillettype() {
	if (validerBillettypeInput()) {
		let billettype = lagBillettypeObjekt();
		billettype.Id = id;
		$.post("../Billettyper/OppdaterBillettype", billettype, () => {
			location.replace("billettyper.html?nyBillettype");
		});
	}
}

function nyBillettype() {
	if (validerBillettypeInput()) {
		const billettype = lagBillettypeObjekt();
		$.post("../Billettyper/NyBillettype", billettype, () => {
			location.replace("billettyper.html?billettypeOppdatert");
		});
	}
}

function lagBillettypeObjekt() {
	return {
		Billettype: $("#billettype").val(),
		Rabattsats: $("#rabattsats").val(),
	};
}
