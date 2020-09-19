// Sjekker om stoppnavn er gyldig
function validerStoppnavn(inId, utId) {
    const stoppnavn = $("#" + inId).val();
    const regexp = /^[a-zA-ZæøåÆØÅ\.\ \-]{2,20}$/;

    const ok = regexp.test(stoppnavn);
    const stoppFins = StoppListe.includes(stoppnavn);

    if (!ok || !stoppFins) {
        let preposisjon;
            if (inId == "startStopp") {
                preposisjon = "fra"
            } else {
                preposisjon = "til"
            }
            if (stoppnavn != "")
            {
                $(utId).html(`Vi tilbyr desverre ikke reiser ${preposisjon} "${stoppnavn}"`);
            }
            return false;
    } else {
            $(utId).html("");
            return true;
    }
}

// Skjekker om dato er gyldig
function validerDato(inDato) {
    if (idagISO() > inDato) {
        $("#feilDato").html("Du kan ikke reise tilbake i tid");
        return false;
    } else {
        $("#feilDato").html("");
        return true;
    }
}

// Finner dagens dato og formaterer i yyyy-mm-dd format
function idagISO() {
    var idag = new Date();
    var yyyy = idag.getFullYear();
    var dd = idag.getDate();
    var mm = idag.getMonth() + 1;

    // Hvis dagen er mindre enn 10 legges 0 til forran
    if (dd < 10) {
        dd = '0' + dd;
    }

    // Hvis måneden er mindre enn 10 legges 0 til forran
    if (mm < 10) {
        mm = '0' + mm;
    }
    idag = yyyy + '-' + mm + '-' + dd;

    return idag;
}
