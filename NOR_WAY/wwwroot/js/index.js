﻿// Kalles når siden laster inn
$(function () {
    hentAlleStopp();
    hentAlleBillettyper();
    leggTilDato();
    bakgrunnOverlay();
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

/* Legger til et mørkt overlay over bakgrunnsbildet,
   som matcher høyden på dokumentet */
function bakgrunnOverlay() {
    var h = $(document).height();
    $("#overlay").css('height', h);
    console.log(h);
}

// Henter alle stoppene i databasen og 
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

    // Kaller serveren for å finne neste avgang
    $.post("Buss/FinnNesteAvgang", avgangParam, function (avgang) {

        /* HTML-komponent som inneholder en oversikt over billettonfomasjonen,
           og et betalingskjema */
        ut = `<div id="billettInfo">
                <h4 id="billettInfoOverskrift"><strong>${avgang.rutenavn}, ${avgang.linjekode}</strong></h4>
                <div id="billettInfoBody">
                    <h6>
                        Avresise: 20. November &nbsp;|&nbsp; ${startStopp}, 09:30 &nbsp;→&nbsp; ${sluttStopp}, 10:50</h6>
                    <h6 class="mt-4">
                        Reisetid: 1 timer og 30 minutter
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
                    <input type="text" name="navn" placeholder="Ola Nordmann" class="form-control shadow-sm">
                </div>

                <div class="form-group">
                    <label for="epost">Epost</label>
                    <input type="text" id="epost" placeholder="ola@normann.no" class="form-control shadow-sm" />
                </div>

                <div class="form-group">
                    <label for="kortnummer">Kortnummer</label>
                    <div class="input-group">
                        <input type="text" id="kortnummer" name="kortnummer" placeholder="0000 0000 0000 0000"
                            class="form-control shadow-sm" onKeyPress="if(this.value.length == 19) return false;">
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
                                <input type="number" placeholder="MM" class="form-control shadow-sm"
                                    onKeyPress="if(this.value.length == 2) return false;" >
                                <input type="number" placeholder="ÅÅ" class="form-control shadow-sm"
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

                <input type="submit" class="btn btn-success shadow form-control font-weight-bold" value="Betal">
            </form>`;

        // Skriver til document, fjerner feilmeldinger og scroller ned
        $("#avgang").html(ut);
        $("#feilAvgang").html("");
        $("#avgang").css("display", "block");
        document.getElementById('avgang').scrollIntoView();
        bakgrunnOverlay(); // Får overlay til å matche den endrede skjermhøyden

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

// Legger til en ny select og fyller den med billettyer-data
function leggTilBillett() {
    const antall = $('.billettype').length;
    const id = `billettype${antall+1}`;

    $('#billetter').append(`<select id="${id}" class="form-control billettype mb-2"></select>`);
    console.log(billettyper);
    var $dropdown = $(`#${id}`);
    $.each(billettyper, function () {
        $dropdown.append($("<option />").val(this.billettype).text(this.billettype));
    });
}
