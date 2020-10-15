class NyRuteStoppEvents {
    constructor(id1, id2, id3, id4) {
        this.linjekodeInput($(id1));
        this.stoppnavnInput($(id2));
        this.stoppnummerInput($(id3));
        this.minutterTilNesteStoppInput($(id4));
    }

    linjekodeInput(elmt) {
        elmt.blur(function () {
            validerLinjekode(elmt, '#feilLinjekode');
        });
    }

    stoppnavnInput(elmt) {
        elmt.blur(function () {
            validerEtStoppnavn(elmt, '#feilStoppnavn');
        });
    }

    stoppnummerInput(elmt) {
        elmt.blur(function () {
            validerStoppnummer(elmt, '#feilStoppnummer')
        });
    }

    minutterTilNesteStoppInput(elmt) {
        elmt.blur(function () {
            validerMinutterTilNesteStopp(elmt, '#feilMinutterTilNesteStopp')
        });
    }
}