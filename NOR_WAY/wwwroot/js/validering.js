// Tester input mot regex
function testRegex(elmt, regex) {
    let input = elmt.val();
    if (regex.test(input)) { return true; }
    return false;
}

// Skriver ut feilmeling til bruker og returnerer en bool
function feilmelding(ok, melding, utId) {
    const feilElement = $(utId);
    if (ok) {
        feilElement.html("");
        endreBakgrunn();
        return true;
    } else {
        feilElement.html(melding);
        endreBakgrunn();
        return false;
    }
}

// Validerer alle feltene knyttet til avngang
function validerAvgangInput() {
    const startStopp = validerStoppnavn($("#startStopp"), "#feilStartStopp");
    const sluttStopp = validerStoppnavn($("#sluttStopp"), "#feilSluttStopp");
    const dato = validerDato("#dato");

    // Hvis alle valideringene over er GYLDIGE
    if (startStopp && sluttStopp && dato) { return true; }

    // Hvis et eller flere valideringer over er UGYLDIGE
    $("#avgang").css("display", "none"); // Fjerner betalingskomponenten
    $("#feilAvgang").html("Vi finner desverre ingen avgang som passer ditt søk"); // Skriver feilmeling til bruker
    document.querySelector('nav').scrollIntoView(); // scroller til toppen av siden
    return false;
}

// Validerer alle feltene knyttet til betaling
function validerBetalingsInput() {
    const navn = validerNavn("#navn", "#feilNavn");
    const epost = validerEpost("#epost", "#feilEpost");
    const kortnummer = validerKortnummer("#kortnummer", "#feilKortnummer");
    const mm = validerMM("#MM", "#feilMM");
    const aa = validerAA("#AA", "#feilAA");
    const cvc = validerCVC("#CVC", "#feilCVC");

    // Hvis alle valideringene over er gyldige
    if (navn & epost & kortnummer & mm & aa & cvc) {
        return true;
    }
    return false;
}

// Stoppnavn-validering
function validerStoppnavn(innElmt, utId) {
    const stoppnavn = Hjelpemetoder.rensStoppInput(innElmt.val());  // Renser Input i inputfeltet 
    const regex = /^[a-zA-ZæøåÆØÅ\.\ \-]{2,20}$/;
    const stoppFins = StoppListe.includes(stoppnavn); // Sjekker om stoppet fins i listen med stopp
    let melding = "";

    let ok = false;
    if (testRegex(innElmt, regex) && stoppFins) { ok = true; }

    let preposisjon;
    if (innElmt.attr('id') == "startStopp") {
        preposisjon = "fra"
    } else {
        preposisjon = "til"
    }

    if (stoppnavn != "") {
        melding = `Vi tilbyr desverre ikke reiser ${preposisjon} "${stoppnavn}"`
    }

    // Sjekker om input er gyldig formatert i henhold til regexen over
    return feilmelding(ok, melding, utId);
}

// Stoppnavn-validering simple versjon
function validerStoppnavnSimple(input) {
    const stoppnavn = Hjelpemetoder.rensStoppInput(input);  // Renser Input i inputfeltet 
    const regex = /^[a-zA-ZæøåÆØÅ\.\ \-]{2,20}$/;
    const stoppFins = StoppListe.includes(input);
    const ok = regex.test(input);
    if (ok && stoppFins) { return true; }
    return false;
}

// Dato-validering
function validerDato(inId, utId) {
    const input = $(inId).val();
    if (idagISO() > input) {
        $(utId).html("Du kan ikke reise tilbake i tid");
        endreBakgrunn();
        return false;
    } else {
        $(utId).html("");
        endreBakgrunn();
        return true;
    }
}

// Navn-validering
function validerNavn(innElmt, utId) {
    const regex = /^[a-åA-Å]([-']?[a-z]+)*( [a-åA-Å]([-']?[a-åA-Å]+)*)+$/
    const melding = "Ugyldig navn";
    return feilmelding(testRegex(innElmt, regex), melding, utId);
}

// Epost-validering
function validerEpost(elmt, utId) {
    const regex = /^[a-zA-Z0-9._%-]+@[a-zA-Z0-9.-]+\.[a-z]{2,4}/;
    const melding = "Ugyldig epost";
    return feilmelding(testRegex(innElmt, regex), melding, utId);
}

// Kortnummer-validering
function validerKortnummer(innElmt, utId) {
    const regex = /^[0-9]{4} [0-9]{4} [0-9]{4} [0-9]{4}$/
    const melding = "Ugyldig kortnummer";
    return feilmelding(testRegex(innElmt, regex), melding, utId);
}

// MM validering
function validerMM(innElmt, utId) {
    const regex = /^[0-9]{2}$/
    const melding = "Ugyldig måned";
    return feilmelding(testRegex(innElmt, regex), melding, utId);
}

// ÅÅ-validering
function validerAA(innElmt, utId) {
    const regex = /^[0-9]{2}$/
    const melding = "Ugyldig årstall";
    return feilmelding(testRegex(innElmt, regex), melding, utId);
}

// CVC-validering
function validerCVC(innElmt, utId) {
    const regex = /^[0-9]{3}$/
    const melding = "Ugyldig CVC";
    return feilmelding(testRegex(innElmt, regex), melding, utId);
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

/* Legger til et mørkt overlay over bakgrunnsbildet,
   som matcher høyden på dokumentet */
function endreBakgrunn() {
    var h = $(document).height();
    $("#overlay").css('height', h);
}