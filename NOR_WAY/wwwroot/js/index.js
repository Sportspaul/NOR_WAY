// Kalles når siden laster inn
$(function () {
    hentAlleStopp();
    hentAlleBillettyper();
    Hjelp.leggTilDato();
    Hjelp.endreBakgrunn();
});

// Global liste med alle stoppene hentet i databasen
let StoppListe = new Array();

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
        new Stoppforslag($("#startStopp"), $("#auto1"), stoppListe, $("#feilStartStopp"));
        new Stoppforslag($("#sluttStopp"), $("#auto2"), stoppListe, $("#feilSluttStopp"));
    });
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
        $.post(url, { Navn: input }, function (stopp) {
            let nyStoppListe = new Array();
            for (let i = 0; i < stopp.length; i++) {
                nyStoppListe.push(stopp[i].navn)
            }
            new Stoppforslag($("#sluttStopp"), $("#auto2"), nyStoppListe, $("#feilSluttStopp"));
        });
    } else {
        new Stoppforslag($("#sluttStopp"), $("#auto2"), StoppListe, $("#feilSluttStopp"));
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
            let nyStoppListe = new Array();
            for (let i = 0; i < stopp.length; i++) {
                nyStoppListe.push(stopp[i].navn)
            }
            new Stoppforslag($("#startStopp"), $("#auto1"), nyStoppListe, $("#feilStartStopp"));
        });
    } else {
        new Stoppforslag($("#startStopp"), $("#auto1"), StoppListe, $("#feilStartStopp"));
    }
}

// Henter neste avgang fra server og skriver det ut til dokumentet
let avgang;
function finnNesteAvgang() {
    if (validerAvgangInput()) {
        const avgangElmt = $("#avgang");
        const feilAvgangElmt = $("#feilAvgang");
        let avgangParam = new AvgangParam();
        // Kaller serveren for å finne neste avgang
        $.post("Buss/FinnNesteAvgang", avgangParam, function (respons) {
            // Modell for å ta imot et Avgang-obj fra backend
            avgang = new Avgang(respons);
            /* HTML-komponent som inneholder en oversikt over billettonfomasjonen,
               og et betalingskjema */
            ut = `<div id="billettInfo">
                    <h4 id="billettInfoOverskrift"><strong>${avgang.rutenavn}, ${avgang.linjekode}</strong></h4>
                    <div id="billettInfoBody">
                        <h6>
                            Avresise: ${formaterDatoOgTid(avreise, "dato")} &nbsp;|&nbsp; ${startStopp}, ${formaterDatoOgTid(avreise, "tid")} &nbsp;→&nbsp; ${sluttStopp}, ${formaterDatoOgTid(ankomst, "tid")}</h6>
                        <h6 class="mt-4">
                            Reisetid: ${Hjelp.finnReisetid(avgang.reisetid)}
                        </h6>
                        <h6 class="mt-4">
                            Billetter: Student
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
                        <input type="text" id="epost" placeholder="ola@normann.no" class="form-control shadow-sm"  maxlength="50" />
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
                                    <input type="number" placeholder="MM" id="MM" class="form-control shadow-sm">
                                    <input type="number" placeholder="ÅÅ" id="AA" class="form-control shadow-sm">
                                </div>

                        </div>
                        <div class="col-sm-4">
                                <label>CVC</label>
                                <input type="number" placeholder="xxx" id="CVC" class="form-control shadow-sm">
                        </div>
                    </div>
                    <div class="row">
                        <div id="feilMM" class="col-sm-4 mt-1 rodTekst"></div>
                        <div id="feilAA" class="col-sm-4 mt-1 rodTekst"></div>
                        <div id="feilCVC" class="col-sm-4 mt-1 rodTekst"></div>
                    </div>

                    <input type="button" class="btn btn-success form-control shadow font-weight-bold" value="Betal" onclick="fullforOrdre()")>
                </form>`;

            // Skriver til document, fjerner feilmeldinger og scroller ned
            feilAvgangElmt.html("");
            avgangElmt.html(ut);
            avgangElmt.css("display", "block");
            var offset = avgangElmt.offset();
            $('html, body').animate({
                scrollTop: offset.top,
                scrollLeft: offset.left
            }, 0);

            Hjelp.endreBakgrunn(); // Får bakgrunn til å matche den endrede skjermhøyden
            new BetalingEvents('#navn', '#epost', '#kortnummer', '#MM', '#AA', '#CVC');
        }).fail(function () {
            // Gir brukeren tilbakemelding hvis ingen avganger ble hentet
            feilAvgangElmt.html("Vi finner desverre ingen avgang som passer ditt søk");
            avgangElmt.css("display", "none");
            avgangElmt.html("");
            Hjelp.endreBakgrunn(); // Får overlay til å matche den endrede skjermhøyden
            });
        }
    }

    function formaterDatoOgTid(datoTid, tidEllerDato) {
        // Splitter DateTime stringen inn i dato og tid
        const datoTidSplittet = datoTid.split(" ");
        const dato = datoTidSplittet[0];

        // Formaterer dato hvis man har skrevet inn "dato" som input parameter
        if (tidEllerDato == "dato") {
            // Splitter dag, mnd år 
            const datoSplittet = dato.split("-");
            const dag = datoSplittet[2];
            const mnd = datoSplittet[1];
            // Liste med alle mnder
            const mnder = ["Januar", "Februar", "Mars", "April", "Mai", "Juni",
                "Juli", "August", "September", "Oktober", "November", "Desember"];
            // Gjør om mnd til formatert mnd
            console.log(datoTid);
            const mndFormatert = mnder[mnd - 1];

            // Utskrift
            const datoFormatert = dag + "." + " " + mndFormatert;

            return datoFormatert;
        }
        // Formaterer tid om han har skrvet inn "tid" som input parameter
        else if (tidEllerDato === "tid") {
            const datoTidSplittet = datoTid.split(" ");
            const tidFormatert = datoTidSplittet[1];

            return tidFormatert;
        }
        else {
            console.log("Du har feil input paramter i metoden");
            return false;
        }
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