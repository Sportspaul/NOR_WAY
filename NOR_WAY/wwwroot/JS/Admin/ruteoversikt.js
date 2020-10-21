$(() => {
	let res = $.get("../Ruter/HentAlleRuter", () => {
		lagRuteoversikt(res);
	});
});
