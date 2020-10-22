// Denne filen håndterer sletting og listing av alle Ruter i DB
$(function () {
	Hjelpemetoder.endreBakgrunn(); // Tilpasser bakgrunn
	const link = "nyRute.html";
	const res = $.post("../Ruter/HentAlleRuter", () => {
		lagRuteoversikt(res, link);
	}).fail(() => {
		lagNyknapp(link);
	});
});

// Function for som kaller fjernRute(id) på server
function slettRad(id) {
	$.get(`../Ruter/FjernRute?linjekode=${id}`, () => {
		$(`#${id}`).remove();
	}).fail(() => {
		console.log("Ruten ble IKKE fjernet");
	});
}
