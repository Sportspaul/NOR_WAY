class NyBillettypeEvents {
	constructor(id1, id2) {
		this.billettypeInput($(id1));
		this.rabattsatsInput($(id2));
	}

	billettypeInput(elmt) {
		elmt.blur(function () {
			validerBillettype(elmt, "#feilBillettype");
		});
	}

	rabattsatsInput(elmt) {
		elmt.blur(function () {
			validerRabattsats(elmt, "#feilRabattsats");
		});
	}
}
