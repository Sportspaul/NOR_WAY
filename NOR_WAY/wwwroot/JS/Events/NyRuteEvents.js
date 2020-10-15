class NyRuteEvents {
    constructor(id1, id2, id3, id4, id5) {
        this.linjekodeInput($(id1));
        this.rutenavnInput($(id2));
        this.startprisInput($(id3));
        this.tilleggPerStoppInput($(id4));
        this.kapasitetInput($(id5));
    }

    linjekodeInput(elmt) {
        elmt.blur(function () {
            validerLinjekode(elmt, '#feilLinjekode');
        });
    }

    rutenavnInput(elmt) {
        elmt.blur(function () {
            validerRutenavn(elmt, '#feilRutenavn')
        });
    }

    startprisInput(elmt) {
        elmt.blur(function () {
            validerStartpris(elmt, '#feilStartpris');
        });
    }

    tilleggPerStoppInput(elmt) {
        elmt.blur(function () {
            validerTilleggPerStopp(elmt, '#feilTilleggPerStopp');
        });
    }

    kapasitetInput(elmt) {
        elmt.blur(function () {
            validerKapasitet(elmt, '#feilKapasitet')
        });
    }
}