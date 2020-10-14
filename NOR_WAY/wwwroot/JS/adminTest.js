const red = "color: #f94e4f";
const green = "color: #28a745";

function HentAlleRuter() {
    $.get("Ruter/HentAlleRuter", function(alleRuter) {
        console.table(alleRuter)
    })
}

// Test av FjernRute
function FjernRute(linjekode) {

    const rute = {
        Linjekode: linjekode
    }

    // Kaller C# Metoden FullforOrdre()
    $.post("Ruter/FjernRute", rute, function () {
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

    $.post("Ruter/NyRute", rute, function (avganger) {
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
        TilleggPerStopp: 200,
        Kapasitet: 99
    }

    $.post("Ruter/OppdaterRute", rute, function () {
        console.log("%c Ruten ble oppdatert", green)
    })
    .fail(function () {
        console.log("%c Ruten ble IKKE oppdatert", red);
    });;
}

function HentAvganger(linjekode, sidenummer) {
    const url = `Avgang/HentAvganger?linjekode=${linjekode}&sidenummer=${sidenummer}`
    $.get(url, function (avganger) {
        console.table(avganger);
    })
    .fail(function () {
        console.log("Avgangen ble ikke oppdatert");
    });
}

function FjernAvgang(id) {
    const url = `Avgang/FjernAvgang?id=${id}`;
    $.get(url, function () {
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

    $.post("Avgang/NyAvgang", avgang, function () {
        console.log("%c Ny avgang ble lagt til", green);
    })
    .fail(function () {
        console.log("%c Ny avgang ble IKKE lagt til", red);
    });
}

function OppdaterAvgang(id) {
    const avreisetid = {
        Id: id,
        Dato: "2028-10-20",
        Tidspunkt: "17:00"
    }

    $.post("Avgang/OppdaterAvgang", avreisetid, function(){
        console.log("%c Avgangen ble oppdatert", green);
    })
    .fail(function (feil) {
        console.log("%c" + feil.responseText, red);
    });
}

function HentRuteStopp(linjekode) {
    const url = `RuteStopp/HentRuteStopp?linjekode=${linjekode}`;
    $.get(url, function(ruteStoppListe){
        console.table(ruteStoppListe);
    });
}

function NyRuteStopp(linjekode) { 
    const innRuteStopp = {
        Stoppnummer: 1,
        MinutterTilNesteStopp: 33,
        Stoppnavn: "Bergersvingen",
        Linjekode: linjekode
    }

    $.post("RuteStopp/NyRuteStopp", innRuteStopp, function(){
        console.log("%c Nytt RuteStopp ble lagt til", green);
    })
    .fail(function (feil) {
        console.log("%c" + feil.responseText, red);
    });
}

function FjernRuteStopp(id) {
    const url = `RuteStopp/FjernRuteStopp?id=${id}`;
    $.get(url, function () {
        console.log("%c RuteStopp ble slettet", green);
    })
    .fail(function () {
        console.log("%c RuteStopp ble IKKE slettet", red);
    });
}

function OppdaterRuteStopp() {
    const ruteStoppOppdatert = {
        Id: 111,
        StoppNummer: 2,
        MinutterTilNesteStopp: 10,
        Stoppnavn: "Bergersen",
        Linjekode: "NW625"
    }

    $.post("RuteStopp/OppdaterRuteStopp", ruteStoppOppdatert, function(){
        console.log("%c RuteStopp ble endret", green);
    })
    .fail(function (feil) {
        console.log("%c" + feil.responseText, red);
    });
}

function NyBillettype() {
    const billettype = {
        Billettype: "Ufør",
        Rabattsats: 90,
    }

    $.post("Billettyper/NyBillettype", billettype, function(){
        console.log("%c Ny Billettype ble lagret", green);
    })
    .fail(function (feil) {
        console.log("%c" + feil.responseText, red);
    });
}

function HentAlleBillettyper() 
{
    $.get("Billettyper/HentAlleBillettyper", function(billettyper) {
        console.table(billettyper)
    })
}

function OppdaterBillettype() {
    const oppdatertBillettype = {
        Id: 1,
        Billettype: "Student",
        Rabattsats: 75
    }

    $.post("Billettyper/OppdaterBillettype", oppdatertBillettype, function () {
        console.log("%c Rabattsatsen ble oppdatert", green)
    })
    .fail(function () {
        console.log("%c Rabattsatsen ble IKKE oppdatert", red);
    });;
}

