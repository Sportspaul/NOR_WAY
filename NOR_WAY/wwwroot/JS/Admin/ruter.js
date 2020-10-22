$(function () {
	Hjelpemetoder.endreBakgrunn();
	const link = "nyRute.html";
	const res = $.post("../Ruter/HentAlleRuter", () => {
		lagRuteoversikt(res, link);
	}).fail(() => {
		lagNyknapp(link);
	});
});

function slettRad(id) {
	$.get(`../Ruter/FjernRute?linjekode=${id}`, () => {
		$(`#${id}`).remove();
	}).fail(() => {
		console.log("Ruten ble IKKE fjernet");
	});
}
