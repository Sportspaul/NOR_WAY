// Kalles når siden laster inn
$(function () {
    hentAlleStopp();
    hentAlleBillettyper();
    leggTilDato();
    endreBakgrunn();
});

// Global liste med alle stoppene hentet i databasen
let StoppListe = new Array();

// Legger til dagens dato i datofeltet
function leggTilDato() {
    (function () {
        var date = new Date().toISOString().substring(0, 10),
            field = document.querySelector('#dato');
        field.value = date;
    })()
}

// Henter alle billettypene i databasen 
function hentAlleBillettyper() {
    $.get("Buss/HentAlleBillettyper", function (alleBillettyper) {

        // Fyller nedtrekksmenyen med billettypene som ble hentet
        var nedtrekk = $("#billettype1");
        billettyper = alleBillettyper;
        $.each(alleBillettyper, function () {
            nedtrekk.append($("<option />").val(this.billettype).text(this.billettype));
        });
    });
}

/* Hvis bruker skriver inn gyldig startstopp blir listen for gyldige sluttstopp
 * inskrenket til å kun vise stopp som kommer senere i en felles rute */
let StartStopp;
function finnMuligeSluttStopp() {
    input = $("#startStopp").val();
    if (validerStoppnavnSimple(input) && input != StartStopp) {
        StartStopp = input;
        const url = "Buss/finnMuligeSluttStopp";
        console.log("yes");
        $.post(url, { Navn: input }, function (stopp) {
            let stoppListe = new Array();
            for (let i = 0; i < stopp.length; i++) {
                stoppListe.push(stopp[i].navn)
            }
            stoppforslag($("#sluttStopp"), $("#auto2"), stoppListe, $("#feilSluttStopp"));
        });
    } else {
        stoppforslag($("#sluttStopp"), $("#auto2"), StoppListe, $("#feilSluttStopp"));
    } 
}

/* Hvis bruker skriver inn gyldig sluttstopp blir listen for gyldige startstopp
 * inskrenket til å kun vise stopp som kommer tidligere i en felles rute */
let SluttStopp;
function finnMuligeStartStopp(input) {
    input = $("#sluttStopp").val();

    if (validerStoppnavnSimple(input) && input != SluttStopp) {
        SluttStopp = input;
        const url = "Buss/finnMuligeStartStopp";
        $.post(url, { Navn: input }, function (stopp) {
            let stoppListe = new Array();
            for (let i = 0; i < stopp.length; i++) {
                stoppListe.push(stopp[i].navn)
            }
            stoppforslag($("#startStopp"), $("#auto1"), stoppListe, $("#feilStartStopp"));
        });
    } else {
        stoppforslag($("#startStopp"), $("#auto1"), StoppListe, $("#feilStartStopp"));
    }
}


// Legger til et nytt select-element og fyller den med billettyer-data fra DB
function leggTilBillett() {
    console.log("Ja")
    const antall = $('.billettype').length;
    const id = `billettype${antall + 1}`;

    $('#billetter').append(`<select id="${id}" class="form-control billettype mb-2"></select>`);
    var dropdown = $(`#${id}`);
    $.each(billettyper, function () {
        dropdown.append($("<option />").val(this.billettype).text(this.billettype));
    });
}

/* Henter alle stoppene i databasen, 
 * og kaller funksjoner for å legge til stoppforslag ved input i stoppnavn-feltene */
function hentAlleStopp() {
    $.get("Buss/HentAlleStopp", function (alleStopp) {
        let stoppListe = new Array();
        for (let i = 0; i < alleStopp.length; i++) {
            stoppListe.push(alleStopp[i].navn)
        }
        StoppListe = stoppListe; // Legger stoppene i den globalen listen

        // Gir brukeren live-stoppforslag for begge inputfeltene 
        stoppforslag($("#startStopp"), $("#auto1"), stoppListe, $("#feilStartStopp"));
        stoppforslag($("#sluttStopp"), $("#auto2"), stoppListe, $("#feilSluttStopp"));
    });
}

//Til rutedelen
function hentAlleRuter() {
    //TODO: gjøre noe med dataene
    $.get("Buss/HentAlleRuter", function (allerutene) {
        let rutene = new Array();
        for (let i = 0; i < allerutene.length; i++) {
            rutene.push(allerutene[i]);
        }
    });
}

/* Legger til et mørkt overlay over bakgrunnsbildet,
   som matcher høyden på dokumentet */
function endreBakgrunn() {
    var h = $(document).height();
    $("#overlay").css('height', h);
}

// Henter neste avgang fra server og skriver det ut til dokumentet
function finnNesteAvgang() {

    // Henter ut nødvendige verider fra inputfeltene
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
    let billettyper = ["Voksen", "Barn"]; // TODO: bytte ut denne med functions kall

    // Klargjør obj som skal sendes til server 
    const avgangParam = {
        StartStopp: startStopp,
        SluttStopp: sluttStopp,
        Dato: dato,
        Tidspunkt: tidspunkt,
        AvreiseEtter: avreiseEtter,
        Billettyper: billettyper
    }

    if (validerAvgangInput()) {

        // Kaller serveren for å finne neste avgang
        $.post("Buss/FinnNesteAvgang", avgangParam, function (avgang) {
            console.log(avgang.reisetid);
            console.log(avgang)
            /* HTML-komponent som inneholder en oversikt over billettonfomasjonen,
               og et betalingskjema */
            ut = `<div id="billettInfo">
                    <h4 id="billettInfoOverskrift"><strong>${avgang.rutenavn}, ${avgang.linjekode}</strong></h4>
                    <div id="billettInfoBody">
                        <h6>
                            Avreise: ${avgang.avreise.substr(0, 10)} &nbsp;|&nbsp; ${startStopp}, ${avgang.avreise.substr(11, 5)}  &nbsp;→&nbsp; ${sluttStopp}, 
                                     ${avgang.ankomst.substr(11, 5)} </h6>
                        <h6 class="mt-4">
                            Reisetid: ${finnReisetid(avgang.reisetid)}
                        </h6>
                        <h6 class="mt-4">
                            Biletter: 2 Voksen, 1 Student, 2 Hønnør
                        </h6>
                        <h6 class="mt-4">
                            Pris: ${avgang.pris} kr
                        </h6>
                    </div>
                </div>
                <form role="form" id="betaling">
                    <div class="form-group">
                        <label for="navn">Fullt navn</label>
                        <input type="text" id="navn" name="navn" placeholder="Ola Nordmann" class="form-control shadow-sm" maxlength="50"
                            onblur="validerNavn('#navn', '#feilNavn')" />
                        <div id="feilNavn" class="mt-1 rodTekst"></div>
                    </div>

                    <div class="form-group">
                        <label for="epost">Epost</label>
                        <input type="text" id="epost" placeholder="ola@normann.no" class="form-control shadow-sm" onfocusout="validerEpost('#epost', '#feilEpost')" maxlength="50"
                            onblur="validerEpost('#epost', '#feilEpost')"/>
                        <div id="feilEpost" class="mt-1 rodTekst"></div>
                    </div>

                    <div class="form-group">
                        <label for="kortnummer">Kortnummer</label>
                        <div class="input-group">
                            <input type="text" id="kortnummer" name="kortnummer" placeholder="0000 0000 0000 0000"
                                class="form-control shadow-sm" onblur="validerKortnummer('#kortnummer', '#feilKortnummer')" onKeyPress="if(this.value.length == 19) return false;" />
                            <div class="input-group-append">
                                <span class="input-group-text text-muted">
                                    <i class="fa fa-cc-visa mx-1"></i>
                                    <i class="fa fa-cc-mastercard mx-1"></i>
                                </span>
                            </div>
                        </div>
                        <div id="feilKortnummer" class="mt-1 rodTekst"></div>
                    </div>

                    <div class="row">
                        <div class="col-sm-8">
                                <label>Utløpsdato</label>
                                <div class="input-group">
                                    <input type="number" placeholder="MM" id="MM" class="form-control shadow-sm"
                                        onblur="validerMM('#MM', '#feilMM')" onKeyPress="if(this.value.length == 2) return false;" >
                                    <input type="number" placeholder="ÅÅ" id="AA" class="form-control shadow-sm"
                                        onblur="validerAA('#AA', '#feilAA')" onKeyPress="if(this.value.length == 2) return false;" >
                                </div>
                        
                        </div>
                        <div class="col-sm-4">
                                <label>CVC</label>
                                <input type="number" placeholder="xxx" id="CVC" class="form-control shadow-sm"
                                    onblur="validerCVC('#CVC', '#feilCVC')"onKeyPress="if(this.value.length==3) return false;">
                        </div>
                    </div>
                    <div class="row">
                        <div id="feilMM" class="col-sm-4 mt-1 rodTekst"></div>
                        <div id="feilAA" class="col-sm-4 mt-1 rodTekst"></div>
                        <div id="feilCVC" class="col-sm-4 mt-1 rodTekst"></div>
                    </div>

                    <input type="submit" class="btn btn-success shadow form-control font-weight-bold mt-4" value="Betal">
                </form>`;

            // Skriver til document, fjerner feilmeldinger og scroller ned
            $("#avgang").html(ut);
            $("#feilAvgang").html("");
            $("#avgang").css("display", "block");
            document.getElementById('avgang').scrollIntoView(); // Scroller til 
            endreBakgrunn(); // Får overlay til å matche den endrede skjermhøyden

            // Hinder brukeren å skrive tall i navn-feltet
            $("#navn").on("input", function (e) {
                $("#navn").val($("#navn").val().replace(/[^a-zA-Z- ]/, ''));
            });

            /* Hindere brukeren å skrive inn annet enn tall,
            og legger til mellomrom for hvert fjerde tall */
            $("#kortnummer").on("input", function (e) {
                $("#kortnummer").val($("#kortnummer").val().replace(/[^\d]/g, '').replace(/(.{4})/g, '$1 ').trim());
            });
        }).fail(function () {

            // Gir brukeren tilbakemelding hvis ingen avganger ble hentet 
            $("#avgang").css("display", "none");
            $("#avgang").html("");
            $("#feilAvgang").html("Vi tilbyr deverre ikke reisen du ønsker");
        });
    }


    function finnReisetid(reiseTid) {
        let intReise = parseInt(reiseTid,10);
        let min = intReise % 60;
        let time = Math.floor(intReise / 60);
        let utReisetid = "Reisetid: ";

        if (time > 0) {
            if (time != 1) {
                utReisetid += time + " timer";
            } else {
                utReisetid += time + " time";
            }
        }

        if (time > 0 && min > 0) {
            utReisetid += " og ";
        }

        if (min > 0) { 
            if (min != 1) {
                utReisetid += min + " minutter";

            } else {
                utReisetid += min + " minutt";
            }
        }

        return utReisetid;
    }

    // Henter ut data ut fra strenger
    function hentTidsdata(tidInput) {
        let datoVerdier = tidInput.substr(0, 10).split("-")
        let tidVerdier = tidInput.substr(11, 5).split(":").map(Number)
        let tidspunkt = [datoVerdier, tidVerdier] //todimensjonalt array
        return tidspunkt
    }

    // Legger til en ny select og fyller den med billettyer-data
    function leggTilBillett() {
        const antall = $('.billettype').length;
        const id = `billettype${antall + 1}`;

        $('#billetter').append(`<select id="${id}" class="form-control billettype mb-2"></select>`);
        console.log(billettyper);
        var $dropdown = $(`#${id}`);
        $.each(billettyper, function () {
            $dropdown.append($("<option />").val(this.billettype).text(this.billettype));
        });
    }

    // Gir brukeren live-stoppforslag
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

            // Looper gjennom alle stoppene i listen med stopp
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

        // EventListener på om en tast blir trykket
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
    }
}