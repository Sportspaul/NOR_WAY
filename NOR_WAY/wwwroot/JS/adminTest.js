// Test av FjernRute
function FjernRute(linjekode) {

    const rute = {
        Linjekode: linjekode
    }

    // Kaller C# Metoden FullforOrdre()
    $.post("Ruter/FjernRute", rute);
}

// Test av NyRute
function NyRute() {

    const ruteStopp1 = {
        StoppNummer: 1,
        MinutterTilNesteStopp: 45,
        Stoppnavn: "Bergen",
    }
    const ruteStopp2 = {
        StoppNummer: 2,
        MinutterTilNesteStopp: 55,
        Stoppnavn: "Vadheim",
    }
    const ruteStopp3 = {
        StoppNummer: 3,
        MinutterTilNesteStopp: 35,
        Stoppnavn: "Østre-nordheim",
    }

    const rute = {
        Linjekode: "NW625",
        Rutenavn: "Melkeruta",
        Startpris: 750,
        TilleggPerStopp: 50,
        Kapasitet: 245,
        RuteStopp: [ ruteStopp1, ruteStopp2, ruteStopp3 ]
    }

    $.post("Ruter/NyRute", rute);
}

function OppdaterRute(linjekode) {
    const rute = {
        Linjekode: linjekode,
        Rutenavn: "Melkeruta",
        Startpris: 750,
        TilleggPerStopp: 50,
        Kapasitet: 245
    }

    $.post("Ruter/OppdaterRute", rute);
}

function HentAvganger(linjekode, sidenummer) {
    const url = `Avgang/HentAvganger?linjekode=${linjekode}&sidenummer=${sidenummer}`
    $.get(url);
}