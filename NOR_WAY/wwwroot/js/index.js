$(function () {
    hentAlleStopp();
    hentAlleBillettyper();
    leggTilDato();
});


function hentAlleStopp() {
    $.get("Buss/HentAlleStopp", function (alleStopp) {
        console.log(alleStopp[0]);
        stoppforslag($("#startStopp"), $("#auto1"), alleStopp);
        stoppforslag($("#sluttStopp"), $("#auto2"), alleStopp);
    });
}


function hentAlleBillettyper() {
    $.get("Buss/HentAlleBillettyper", function (alleBillettyper) {

        var $dropdown = $("#billettype1");
        billettyper = alleBillettyper;
        $.each(alleBillettyper, function () {
            $dropdown.append($("<option />").val(this.billettype).text(this.billettype));
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

    $('#billetter').append(`<select id="${id}" class="form-control billettype mb-1"></select>`);
    console.log(billettyper);
    var $dropdown = $(`#${id}`);
    $.each(billettyper, function () {
        $dropdown.append($("<option />").val(this.billettype).text(this.billettype));
    });
}

function leggTilDato() {
    (function () {
        var date = new Date().toISOString().substring(0, 10),
            field = document.querySelector('#dato');
        field.value = date;
    })()
}

// Kun for testing

function printStopp(alleStopp) {
    let stoppListe = new stoppListeay();

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

// function for å gi brukeren live-stoppforslag,
function stoppforslag(inputfelt, utskrift, stoppArray) {
    var fokusert;

    // Eventlisner på intput feltet
    inputfelt.on("input", function (e) {
        let stoppListe, stoppElement, i
        let val = this.value;

        // Lukker eksiterende liste
        lukkAlleLister();
        if (!val) { return false; }
        fokusert = -1;

        // Lager en div som vil inneholde stoppene
        stoppListe = document.createElement("div");
        stoppListe.setAttribute("id", this.id + "stoppListe");
        stoppListe.setAttribute("class", "stoppListe");
        this.append(stoppListe);

        for (i = 0; i < stoppArray.length; i++) {

            // Sjekker om stopp i listen starter med de samme bokstavene som input
            if (stoppArray[i].navn.substr(0, val.length).toUpperCase() == val.toUpperCase()) {

                // Opretter en div for hvert stopp som marcher
                stoppElement = document.createElement("DIV");

                // Gjør bokstavene som matcher bold
                stoppElement.innerHTML = "<strong>" + stoppArray[i].navn.substr(0, val.length) + "</strong>";
                stoppElement.innerHTML += stoppArray[i].navn.substr(val.length);

                // Tar vare på verdien til stoppene som matcher i et input feltet
                stoppElement.innerHTML += "<input type='hidden' value='" + stoppArray[i].navn + "'>";

                // Event listner for om noen trykker på et av de foreslåtte stoppene
                stoppElement.addEventListener("click", function (e) {

                    // Fyller input feltet med stoppet som brukeren trykker på
                    inputfelt.val(this.getElementsByTagName("input")[0].value);

                    // Lukker listen med forslag til stopp
                    lukkAlleLister();
                });

                // Legger stopp elementet til stoppListe elementet og skriver listen ut til brukeren
                stoppListe.appendChild(stoppElement);
                utskrift.append(stoppListe);
            }
        }
    });

    inputfelt.on("keydown", function (e) {
        var elmt = document.getElementById(this.id + "stoppListe");
        if (elmt) elmt = elmt.getElementsByTagName("div");

        // Hvis brukeren trykker piltast ned
        if (e.keyCode == 40) {

            // Flytter pekeren til aktivt element en videre i listen
            fokusert++;

            // Endrer style på aktivt element
            leggTilAktiv(elmt);

        // Hvis brukeren trykker piltast opp
        } else if (e.keyCode == 38) { 

            // Flytter pekeren til aktivt element en tilbake i listen
            fokusert--;

            // Endrer style på aktivt element
            leggTilAktiv(elmt);
        } 
    });

    // Setter et element til å være aktivt
    function leggTilAktiv(elmt) {
        if (!elmt) return false;

        // Fjerner style for aktivt element fra alle elementene
        fjernAktiv(elmt);
        if (fokusert >= elmt.length) fokusert = 0;
        if (fokusert < 0) fokusert = (elmt.length - 1);

        // Legger til style-klasse for aktivt element 
        elmt[fokusert].classList.add("forslag-active");
    }

    // Fjerner style-klassen for aktivt element fra alle elementer
    function fjernAktiv(elmt) {
        for (var i = 0; i < elmt.length; i++) {
            elmt[i].classList.remove("forslag-active");
        }
    }

    // Lukker listen med forslag til stopp 
    function lukkAlleLister(elmt) {
        var elmt = document.getElementsByClassName("stoppListe");
        for (var i = 0; i < elmt.length; i++) {
            if (elmt != elmt[i] && elmt != inputfelt) {
                elmt[i].parentNode.removeChild(elmt[i]);
            }
        }
    }

    // Lukker listen med stopp hvis bruker trykker utenfor listen
    $(document).on("click", function (e) {
        lukkAlleLister(e.target);
    });
}
