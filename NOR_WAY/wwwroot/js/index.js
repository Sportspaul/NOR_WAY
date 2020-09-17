$(function () {
    hentAlleStopp();
    hentAlleBillettyper();
});

let billettyper;

function hentAlleStopp() {
    $.get("Buss/HentAlleStopp", function (alleStopp) {
        console.table(alleStopp)
        printStopp(alleStopp);
    });
}


function hentAlleBillettyper() {
    $.get("Buss/HentAlleBillettyper", function (alleBillettyper) {

        var $dropdown = $("#billettype1");
        billettyper = alleBillettyper;
        $.each(alleBillettyper, function () {
            $dropdown.append($("<option />").val(this.billettype).text(this.billettype + ", -" + this.rabattsats + "%"));
        });
    });
}

function finnNesteAvgang() {
    const startStopp = $("#startStopp").val();
    const sluttStopp = $("#sluttStopp").val();
    const dato = $("#dato").val();
    const tidspunkt = $("#tidspunkt").val();
    let avreiseEtter = $('input[name="avreiseEtter"]:checked').val();
    if (avreiseEtter == "true") {
        avreiseEtter = true
    } else {
        avreiseEtter = false;
    }

    const avgangParam = {
        StartStopp: startStopp,
        SluttStopp: sluttStopp,
        Dato: dato,
        Tidspunkt: tidspunkt,
        AvreiseEtter: avreiseEtter
    }

    console.table(avgangParam);

    $.post("Buss/FinnNesteAvgang", avgangParam, function (avgang) {
        ut = `<p>
                Rutenavn: ${avgang.rutenavn}<br>
                Linjekode: ${avgang.linjekode}<br>
                Pris: ${avgang.pris}<br>
                Avreise: ${avgang.avreise}<br>
                Ankomst: ${avgang.avreise}<br>
                Reisetid: ${avgang.reisetid}<br>
            </p>`;

        $("#avgang").html(ut)
    });
}

// Legger til en ny select og fyller den med billettyer-data
function leggTilBillett() {
    const antall = $('.billettype').length;
    const id = `billettype${antall+1}`;

    $('#billetter').append(`<select id="${id}" class="form-control billettype"></select>`);
    console.log(billettyper);
    var $dropdown = $(`#${id}`);
    $.each(billettyper, function () {
        $dropdown.append($("<option />").val(this.billettype).text(this.billettype + ", -" + this.rabattsats + "%"));
    });
}

// Kun for testing

function printStopp(alleStopp) {
    let stoppListe = new Array();

    for (stopp of alleStopp) {
        stoppListe.push(stopp.navn.toLowerCase());
    }

    let input = "O"
    const filtrerteStopp = stoppListe.filter(s => s.includes(input.toLowerCase()));

    let ut = "<p>";
    for (let stopp of filtrerteStopp) {
        ut += titleCase(stopp) + ", ";
    }
    ut += "</p>";
    $("#alleStopp").html(ut);
}

// Gjør første bokstav i hvert ord om til uppercase
function titleCase(str) {
    var splitStr = str.toLowerCase().split(' ');
    for (var i = 0; i < splitStr.length; i++) {
        splitStr[i] = splitStr[i].charAt(0).toUpperCase() + splitStr[i].substring(1);
    }
    return splitStr.join(' ');
}