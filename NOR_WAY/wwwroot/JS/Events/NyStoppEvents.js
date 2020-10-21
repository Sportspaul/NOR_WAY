class NyStoppEvents {
	constructor(id) {
		this.stoppnavnInput($(id));
	}

	stoppnavnInput(elmt) {
		elmt.blur(function () {
			validerEtStoppnavn(elmt, "#feilStoppnavn");
		});
	}
}
