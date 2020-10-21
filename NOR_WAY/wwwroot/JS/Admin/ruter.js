$(function () {
	const res = $.post("../Ruter/HentAlleRuter");
	const link = "nyRute.html";
	lagRuteoversikt(res, link);
});

function slettRad(id) {
	$.get(`../Ruter/FjernRute?linjekode=${id}`, () => {
		$(`#${id}`).remove();
	}).fail(() => {
		console.log("Ruten ble IKKE fjernet");
	});
}
