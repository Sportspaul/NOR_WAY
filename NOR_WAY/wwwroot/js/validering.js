
// Sjekker om stoppnavn er gyldig
function validerStoppnavn(inId, utId) {
    const stoppnavn = $("#" + inId).val();  // Input i inputfeltet
    const regexp = /^[a-zA-ZæøåÆØÅ\.\ \-]{2,20}$/; // Regex

    // Sjekker om input er gyldig formatert i henhold til regexen over
    const ok = regexp.test(stoppnavn); 

    // Sjekker om stoppet fins i listen med stopp
    const stoppFins = StoppListe.includes(stoppnavn); 

    // Hvis formatering er GYLDIG og stoppet FINST i listen
    if (!ok || !stoppFins) {
        $(utId).html("");
        return true;

    // Hvis formatering er UGYLDIG og stoppet IKKE finst i listen   
    } else {

        /* Setter riktig preposisjon basert på om det er s
           tartStopp eller sluttStopp som valideres */
        let preposisjon;
        if (inId == "startStopp") {
            preposisjon = "fra"
        } else {
            preposisjon = "til"
        }

        // Hvis stoppnanet ikke består validering og det ikke er tomt
        if (stoppnavn != "") {

            // Tilbakemelding til bruker om at stoppet ikke fins
            $(utId).html(`Vi tilbyr desverre ikke reiser ${preposisjon} "${stoppnavn}"`);
        }
        return false;
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

// Finner dagens dato og formaterer i yyyy-mm-dd format, brukes av functionen over
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
