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
function finnNesteAvgang() {
    if (validerAvgangInput()) {
        let avgangParam = new AvgangParam();
        // Kaller serveren for å finne neste avgang
        $.post("Buss/FinnNesteAvgang", avgangParam, function (avgang) {
            // Destructuring avgang-objektet til variabler
            let {avgangId, rutenavn, linjekode, pris, avreise, ankomst, reisetid} = avgang;
            /* HTML-komponent som inneholder en oversikt over billettonfomasjonen,
               og et betalingskjema */
            ut = `<div id="billettInfo">
                    <h4 id="billettInfoOverskrift"><strong>${rutenavn}, ${linjekode}</strong></h4>
                    <div id="billettInfoBody">
                        <h6>
                            Avresise: ${formaterDatoOgTid(avreise, "dato")} &nbsp;|&nbsp; ${startStopp}, ${formaterDatoOgTid(avreise, "tid")} &nbsp;→&nbsp; ${sluttStopp}, ${formaterDatoOgTid(ankomst, "tid")}</h6>
                        <h6 class="mt-4">
                            Reisetid: ${Hjelp.finnReisetid(reisetid)}
                        </h6>
                        <h6 class="mt-4">
                            Billetter: Student
                        </h6>
                        <h6 class="mt-4">
                            Pris: ${pris} kr
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
                                            onblur="if(this.value.length == 1){ this.value = '0' + this.value.charAt(0) } validerMM('#MM', '#feilMM')"
                                            onKeyPress="if(this.value.length == 2) return false;" >
                                    <input type="number" placeholder="ÅÅ" id="AA" class="form-control shadow-sm"
                                        onblur="validerAA('#AA', '#feilAA')"
                                        onKeyPress="if(this.value.length == 2) return false;" >
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
            Hjelp.endreBakgrunn(); // Får overlay til å matche den endrede skjermhøyden

            // Hinder brukeren å skrive tall i navn-feltet
            $("#navn").on("input", function (e) {
                $("#navn").val($("#navn").val().replace(/[^a-åA-Å- ]/, ''));
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
            $("#feilAvgang").html("Vi finner desverre ingen avgang som passer ditt søk");
        });
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
}
