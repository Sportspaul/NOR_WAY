class NyStoppEvents {
	constructor(id) {
		this.stoppnavnInput($(id));
	}

	stoppnavnInput(elmt) {
		elmt.blur(() => {
			validerEtStoppnavn(elmt, "#feilStoppnavn");
		});
	}
}
