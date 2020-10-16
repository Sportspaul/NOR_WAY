// Kalles når siden laster inn
$(function () {
    Hjelpemetoder.settKorrektNavigering();
    hentAlleStopp();
    hentAlleBillettyper();
    Hjelpemetoder.leggTilDato();
    Hjelpemetoder.leggTilTidspunkt()
    Hjelpemetoder.endreBakgrunn();
    if (window.location.href.includes("bestilling=ok")) {
        Swal.fire(
            "Bestillingen var vellykket",
            "Billetten har blitt sendt via mail",
            "success"
        )
    }
    $('#startStopp').focus(); // Hopper inn i første input felt
});

// Globale variabler
let StartStopp;
let SluttStopp;
let avgangParam;
let avgang;

// Global liste med alle stoppene hentet i databasen
let StoppListe = new Array();

/* Henter alle stoppene i databasen,
 * og kaller funksjoner for å legge til stoppforslag ved input i stoppnavn-feltene */
function hentAlleStopp() {
    $.get("Stopp/HentAlleStopp", function (alleStopp) {
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
    $.get("Billettyper/HentAlleBillettyper", function (alleBillettyper) {
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
function finnMuligeSluttStopp() {
    input = $("#startStopp").val();
    if (validerStoppnavnSimple(input) && input != StartStopp) {
        StartStopp = input;
        const url = "Stopp/finnMuligeSluttStopp";
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
function finnMuligeStartStopp(input) {
    input = $("#sluttStopp").val();
    if (validerStoppnavnSimple(input) && input != SluttStopp) {
        SluttStopp = input;
        const url = "Stopp/finnMuligeStartStopp";
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
    if (validerReisevalgInput()) {
        const avgangElmt = $("#avgang");
        const feilAvgangElmt = $("#feilAvgang");
        avgangParam = new AvgangParam();
        // Kaller serveren for å finne neste avgang
        $.post("Avgang/FinnNesteAvgang", avgangParam, function (respons) {
            // Modell for å ta imot et Avgang-obj fra backend
            StartStopp = avgangParam.StartStopp;
            SluttStopp = avgangParam.SluttStopp;
            Billettyper = avgangParam.Billettyper
            avgang = new Avgang(respons);
            /* HTML-komponent som inneholder en oversikt over billettonfomasjonen,
               og et betalingskjema */
            ut = `<div id="billettInfo" class="antialised">
                    <h4 id="billettInfoOverskrift"><strong>${avgang.Rutenavn}, ${avgang.Linjekode}</strong></h4>
                    <div id="billettInfoBody">
                        <h6>
                            Avreise: ${Hjelpemetoder.formaterDatoOgTid(avgang.Avreise, "dato")} &nbsp;|&nbsp;
                            ${StartStopp}, ${Hjelpemetoder.formaterDatoOgTid(avgang.Avreise, "tid")}
                            &nbsp;→&nbsp; ${SluttStopp}, ${Hjelpemetoder.formaterDatoOgTid(avgang.Ankomst, "tid")}
                        </h6>
                        <h6 class="mt-4">
                            Reisetid: ${Hjelpemetoder.finnReisetid(avgang.Reisetid)}
                        </h6>
                        <h6 class="mt-4">
                            Billetter:${Hjelpemetoder.formaterValgteBillettyper(Hjelpemetoder.hentValgteBillettyper())}
                        </h6>
                        <h6 class="mt-4">
                            Pris: ${avgang.Pris} kr 
                        </h6>
                           <input id="endre"  class="btn btn-sm btn-primary mt-4 font-weight-bold shadow antialised" type="button" value="Endre" onclick="endreOrdre()">
                    </div>
                </div>
                
                <form role="form" id="betaling">
                    <div class="form-group">
                        <label for="navn">Fullt navn</label>
                        <input type="text" id="navn" name="navn" placeholder="Ola Nordmann" class="form-control shadow-sm" maxlength="50" />
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
                                class="form-control shadow-sm" maxlength="19"/>
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

                    <input type="button" class="btn btn-success mt-4 form-control shadow font-weight-bold antialised" value="Betal" onclick="fullforOrdre()")>
                </form>`;

            // Skriver til document, fjerner feilmeldinger, hopper inn i første inputfelt og scroller ned
            feilAvgangElmt.html("");
            avgangElmt.html(ut);
            avgangElmt.css("display", "block");
            $('#navn').focus(); // Hopper inn i førte input felt
            const offset = avgangElmt.offset();
            $('html, body').animate({
                scrollTop: offset.top - 25,
                scrollLeft: offset.left
            }, 0);

            Hjelpemetoder.endreBakgrunn(); // Får bakgrunn til å matche den endrede skjermhøyden
            new BetalingEvents('#navn', '#epost', '#kortnummer', '#MM', '#AA', '#CVC'); // Gir inputfeltene EventListners
        }).fail(function () {
            // Gir brukeren tilbakemelding hvis ingen avganger ble hentet
            feilAvgangElmt.html("Vi finner desverre ingen avgang som oppfyller søkekriteriene dine");
            avgangElmt.css("display", "none");
            avgangElmt.html("");
            Hjelpemetoder.endreBakgrunn(); // Får overlay til å matche den endrede skjermhøyden
        });
    }
}

function fullforOrdre() {
    // Henter en avgang fra DB, og lager en kundeordre med den og informasjonen hentet over
    const epost = $("#epost").val();

    // Lager en kundeordre
    const kundeordre = new KundeOrdre(epost, StartStopp, SluttStopp, avgang.Linjekode, avgang.AvgangId, avgangParam.Billettyper);

    if (validerBetalingsInput() === false) {
        return;
    }
    else {
        // Kaller C# Metoden FullforOrdre()
        $.post("Ordre/FullforOrdre", kundeordre, function(){
            window.location.replace("index.html?bestilling=ok");
        });
    }
}

function endreOrdre() {
   window.location.replace("#"); 
   $("#bestill").prop("value", "Oppdater reisen ");
}