class NyAvgangEvents {
    constructor(id1, id2, id3) {
        this.linjekodeInput($(id1));
        this.datoInput($(id2));
        this.tidspunktInput($(id3));
    }

    linjekodeInput(elmt) {
        elmt.blur(function () {
            validerLinjekode(elmt, '#feilLinjekode');
        });
    }

    datoInput(elmt) {
        elmt.blur(function () {
            validerDato(elmt, '#feilDato')
        });
    }

    tidspunktInput(elmt) {
        elmt.blur(function () {
            validerTidspunkt(elmt, '#feilTidspunkt')
        });
    }
}