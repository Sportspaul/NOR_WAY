$(() => {
	let url = "nyStopp.html";
	let res = $.get("../Stopp/HentAlleStoppMedRuter", () => {
		lagTabell(res, "U", url);
	});
	filterStoppListe();
});

function filterStoppListe() {
	let filter = document.querySelector("#filterStopp");

	filter.addEventListener("keyup", (e) => {
		let brukerInput = e.target.value.toLowerCase();
		let stoppNavn = document.querySelectorAll(".stoppnavn");

		for (let i = 1; i < stoppNavn.length; i++) {
			if (
				stoppNavn[i].textContent.substr(0, brukerInput.length).toLowerCase() ==
				brukerInput.toLowerCase()
			) {
				stoppNavn[i].closest("tr").style.display = "";
			} else {
				stoppNavn[i].closest("tr").style.display = "none";
			}
		}
	});
}
