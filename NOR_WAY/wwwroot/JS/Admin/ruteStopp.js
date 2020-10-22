$(function () {
	Hjelpemetoder.endreBakgrunn();
	let url = window.location.href.split("=");
	linjekode = url[1].split("&");
	linjekode = linjekode[0];
	const link = `nyRuteStopp.html?${linjekode}`;

	let res = $.get(`../RuteStopp/HentRuteStopp?linjekode=${linjekode}`, () => {
		lagTabell(res, "CUD", link);
	}).fail(() => {
		lagNyknapp(link);
	});
});

function slettRad(id) {
	$.get(`../RuteStopp/FjernRuteStopp?id=${id}`, () => {
		$(`#${id}`).remove();
	}).fail(function () {
		console.log("Ruten ble IKKE fjernet");
	});
}
