function loggUt() {
	$.get("Brukere/LoggUt", () => {
		window.location.href = "index.html";
	});
}
