$(function () {
    hentAlleStopp();
    hentAlleBillettyper();
    
});

function hentAlleStopp() {
    $.get("Buss/HentAlleStopp", function (alleStopp) {
        console.table(alleStopp)
        printStopp(alleStopp);
    });
}


function hentAlleBillettyper() {
    $.get("Buss/HentAlleBillettyper", function (alleBillettyper) {
        console.table(alleBillettyper)

        var $dropdown = $("#Billettype");
        var $dropdown1 = $("#Billettype2");
        var $dropdown2 = $("#Billettype3");
        $.each(alleBillettyper, function () {
            $dropdown.append($("<option />").val(this.billettype).text(this.billettype + ", -" + this.rabattsats + "%"));
            $dropdown1.append($("<option />").val(this.billettype).text(this.billettype + ", -" + this.rabattsats + "%"));
            $dropdown2.append($("<option />").val(this.billettype).text(this.billettype + ", -" + this.rabattsats + "%"));
        });
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