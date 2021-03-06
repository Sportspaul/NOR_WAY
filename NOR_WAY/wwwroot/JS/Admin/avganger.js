$(function () {
	const link = `nyAvgang.html?${linjekode}`;
	$("#tittel").html(`Avganger for: ${linjekode}`);
	let res = $.get(
		`../Avgang/HentAvganger?linjekode=${linjekode}&sidenummer=${sidenummer}`,
		() => {
			lagAvgangOversikt(res, "CUD", link);
		}
	).fail(function () {
		lagNyknapp(link);
		$("#nesteSide").remove();
		$("#ingenAvganger").html("Ingen flere avganger å vise");
	});

	if (sidenummer <= 0) {
		$("#forrigeSide").remove();
	}
	Hjelpemetoder.endreBakgrunn();
});

let urlParam = window.location.href.split("=");
let sidenummer = urlParam[2];
let linjekode = urlParam[1].split("&")[0];

function slettRad(id) {
	$.get(`../Avgang/FjernAvgang?id=${id}`, () => {
		$(`#${id}`).remove();
	}).fail(function () {
		console.log("Avgangen ble IKKE fjernet");
	});
}

function forrigeSide() {
	sidenummer--;
	const url = `avganger.html?linjekode=${linjekode}&side=${sidenummer}`;
	location.replace(url);
}

function nesteSide() {
	sidenummer++;
	const url = `avganger.html?linjekode=${linjekode}&side=${sidenummer}`;
	location.replace(url);
}
