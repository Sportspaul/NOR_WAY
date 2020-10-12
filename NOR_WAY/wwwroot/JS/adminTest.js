const red = "color: #f94e4f";
const green = "color: #28a745";

// Test av FjernRute
function FjernRute(linjekode) {

    const rute = {
        Linjekode: linjekode
    }

    // Kaller C# Metoden FullforOrdre()
    $.post("Ruter/FjernRute", rute)
    .done(function () {
        console.log("%c Ruten ble fjernet", green);
    })
    .fail(function () {
        console.log("%c Ruten ble IKKE fjernet", red);
    });;
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

    $.post("Ruter/NyRute", rute)
    .done(function (avganger) {
        console.log("%c Ny rute ble lagt til", green);
    })
    .fail(function () {
        console.log("%c Ny rute ble ikke lagt til", red);
    });;
}

function OppdaterRute(linjekode) {
    const rute = {
        Linjekode: linjekode,
        Rutenavn: "Melkeruta",
        Startpris: 750,
        TilleggPerStopp: 50,
        Kapasitet: 245
    }

    $.post("Ruter/OppdaterRute", rute)
    .done(function () {
        console.log("%c Ruten ble oppdatert", green)
    })
    .fail(function () {
        console.log("%c Ruten ble IKKE oppdatert", red);
    });;
}

function HentAvganger(linjekode, sidenummer) {
    const url = `Avgang/HentAvganger?linjekode=${linjekode}&sidenummer=${sidenummer}`
    $.get(url)
    .done(function (avganger) {
        console.table(avganger);
    })
    .fail(function () {
        console.log("Avgangen ble ikke oppdatert");
    });
}

function FjernAvgang(id) {
    const url = `Avgang/FjernAvgang?id=${id}`;
    $.get(url)   
    .done(function () {
        console.log("%c Avgangen ble slettet", green);
    })
    .fail(function () {
        console.log("%c Avgangen ble IKKE slettet", red);
    });
}

function NyAvgang() {
    const avgang = {
        Dato: "2028-10-20",
        Tidspunkt: "17:00",
        SolgteBilletter: 0,
        Linjekode: "NW431"
    }

    $.post("Avgang/NyAvgang", avgang)   
    .done(function () {
        console.log("%c Ny avgang ble lagt til", green);
    })
    .fail(function () {
        console.log("%c Ny avgang ble IKKE lagt til", red);
    });
}

function OppdaterAvgang() {
    const avreisetid = {
        Id: 1,
        Dato: "2028-10-20",
        Tidspunkt: "17:00"
    }

    $.post("Avgang/OppdaterAvgang", avreisetid)
    .done(function () {
        console.log("%c Avgangen ble oppdatert", green);
    })
    .fail(function () {
        console.log("%c Avgangen ble ikke oppdatert", red);
    });
}

function HentRuteStopp(linjekode) {
    const url = `RuteStopp/HentRuteStopp?linjekode=${linjekode}`;
    $.get(url, function(ruteStoppListe){
        console.table(ruteStoppListe);
    });
}

