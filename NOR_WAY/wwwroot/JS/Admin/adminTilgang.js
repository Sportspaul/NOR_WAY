$(function () {
	harAdminTilgang();
	loggUt();
});

//Hvis bruker ikke er admin, sendes man til startsiden
function harAdminTilgang() {
	$.get("../Brukere/AdminTilgang", (innlogget) => {
		if (!innlogget) {
			window.location.href = "../index.html";
		}
	});
}

function loggUt() {
	$("#loggUt").click(() => {
		$.get("../Brukere/LoggUt", () => {
			location.replace("../index.html");
		});
	});
}
