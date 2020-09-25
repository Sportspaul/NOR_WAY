$(function () {
    hentAlleStopp();
    hentAlleBillettyper();
    leggTilDato();
    bakgrunnOverlay();
    $('[data-toggle="tooltip"]');
});

let StoppListe = new Array();

function hentAlleStopp() {
    $.get("Buss/HentAlleStopp", function (alleStopp) {
        let stoppListe = new Array();
        for (let i = 0; i < alleStopp.length; i++) {
            stoppListe.push(alleStopp[i].navn)
        }
        StoppListe = stoppListe;
        stoppforslag($("#startStopp"), $("#auto1"), stoppListe, $("#feilStartStopp"));
        stoppforslag($("#sluttStopp"), $("#auto2"), stoppListe, $("#feilSluttStopp"));
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

let startStopp;
let sluttStopp;
let linjekode;
let avgangId;
let billettyper;

function finnNesteAvgang() {
    startStopp = $("#startStopp").val();
    sluttStopp = $("#sluttStopp").val();
    const dato = $("#dato").val();
    const tidspunkt = $("#tidspunkt").val();
    let avreiseEtter = $('input[name="avreiseEtter"]:checked').val();
    if (avreiseEtter == "true") {
        avreiseEtter = true
    } else {
        avreiseEtter = false;
    }

    // Finner alle billettypene brukeren har valgt, og putter dem inn i et array
    const billetter = document.querySelectorAll(".billettype");
    billettyper = new Array();
    billetter.forEach((billett) => billettyper.push(billett.value));

    const avgangParam = {
        StartStopp: startStopp,
        SluttStopp: sluttStopp,
        Dato: dato,
        Tidspunkt: tidspunkt,
        AvreiseEtter: avreiseEtter
    }


    $.post("Buss/FinnNesteAvgang", avgangParam, function (avgang) {
        linjekode = avgang.linjekode;
        avgangId = avgang.avgangId;
        ut = `<h4 class="mb-3"><strong>${avgang.rutenavn}, ${linjekode}</strong></h4>
                <h6 class="mt-3">
                    <strong>Avreise:</strong>&nbsp;
                    20. November &nbsp;|&nbsp; ${startStopp}, 09:30 &nbsp;→&nbsp; ${sluttStopp}, 10:50</h6>
                <h6 class="mt-3">
                    <strong>Reisetid:</strong>
                    1 timer og 30 minutter
                </h6>
                <h6 class="mt-3">
                    <strong>Billetter:</strong>&nbsp;
                    2 Voksen, 1 Student, 2 Hønnør
                </h6>
                <h6 class="mt-3">
                    <strong>Pris:</strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    ${avgang.pris} kr
                </h6>

                <form role="form" class="mt-5">
                    <div class="form-group">
                        <label for="navn">Fullt navn</label>
                        <input type="text" name="navn" placeholder="Ola Nordmann" class="form-control shadow-sm">
                    </div>

                    <div class="form-group">
                        <label for="epost">Epost</label>
                        <input type="text" id="epost" placeholder="ola@normann.no" class="form-control shadow-sm" />
                    </div>

                    <div class="form-group">
                        <label for="kortnummer">Kortnummer</label>
                        <div class="input-group">
                            <input type="text" name="kortnummer" placeholder="0000 0000 0000 0000"
                                class="form-control shadow-sm" maxlength="16">
                            <div class="input-group-append">
                                <span class="input-group-text text-muted">
                                    <i class="fa fa-cc-visa mx-1"></i>
                                    <i class="fa fa-cc-mastercard mx-1"></i>
                                </span>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-sm-8">
                            <div class="form-group">
                                <label>Utløpsdato</label>
                                <div class="input-group">
                                    <input type="number" placeholder="MM" name="" class="form-control shadow-sm"
                                        onKeyPress="if(this.value.length == 2) return false;" >
                                    <input type="number" placeholder="ÅÅ" name="" class="form-control shadow-sm"
                                        onKeyPress="if(this.value.length == 2) return false;" >
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group mb-4">
                                <label>CVC</label>
                                <input type="number" placeholder="xxx" class="form-control shadow-sm"
                                    onKeyPress="if(this.value.length==3) return false;">
                            </div>
                        </div>
                    </div>

                    <input type="button" class="btn btn-success form-control shadow font-weight-bold" value="Betal" onclick="fullforOrdre()")>
                </form>`;
        $("#feilAvgang").html("");
        $("#avgang").css("display", "block");
        $("#avgang").html(ut);
        document.getElementById('avgang').scrollIntoView();
        bakgrunnOverlay();
    }).fail(function () {
        $("#avgang").css("display", "none");
        $("#avgang").html("");
        $("#feilAvgang").html("Vi tilbyr deverre ikke reisen du ønsker");
    });
}

// Legger til en ny select og fyller den med billettyer-data
function leggTilBillett() {
    const antall = $('.billettype').length;
    const id = `billettype${antall+1}`;

    $('#billetter').append(`<select id="${id}" class="billettype form-control mb-2"></select>`);
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
        stoppListe.push(stopp.toLowerCase());
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
function stoppforslag(inputfelt, utskrift, stoppArray, feilmelding) {
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
            if (stoppArray[i].substr(0, val.length).toUpperCase() == val.toUpperCase()) {
                feilmelding.html("");

                // Opretter en div for hvert stopp som marcher
                stoppElement = document.createElement("DIV");

                // Gjør bokstavene som matcher bold
                stoppElement.innerHTML = "<strong>" + stoppArray[i].substr(0, val.length) + "</strong>";
                stoppElement.innerHTML += stoppArray[i].substr(val.length);

                // Tar vare på verdien til stoppene som matcher i et input feltet
                stoppElement.innerHTML += "<input type='hidden' value='" + stoppArray[i] + "'>";

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

        // Hvis bruker trykker ENTER på en fokusert stopp i listen
        } else if (e.keyCode == 13) {

            e.preventDefault();
            if (fokusert > -1) {
                if (elmt) {
                    elmt[fokusert].click();
                }
            }
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
        validerStoppnavn("startStopp", "#feilStartStopp");
        validerStoppnavn("sluttStopp", "#feilSluttStopp");
    });
}

function bakgrunnOverlay() {
    var h = $(document).height();
    $("#overlay").css('height', h);
    console.log(h);
}

function fullforOrdre() {    
    // Henter en avgang fra DB, og lager en kundeordre med den og informasjonen hentet over
    const epost = $("#epost").val();

    // Lager en kundeordre
    const kundeordre = {
        Epost: epost,
        StartStopp: startStopp,
        SluttStopp: sluttStopp,
        Linjekode: linjekode,
        AvgangId: avgangId,
        Billettype: billettyper
    }

    // Kaller C# Metoden FullforOrdre()
    $.post("Buss/FullforOrdre", kundeordre);
}

