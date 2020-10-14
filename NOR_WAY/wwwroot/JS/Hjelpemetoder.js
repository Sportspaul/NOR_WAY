// Klasse med hjelpemetoder
class Hjelpemetoder {
    /* Legger til et mørkt overlay over bakgrunnsbildet,
   som matcher høyden på dokumentet */
    static endreBakgrunn() {
        var h = $(document).height();
        $("#overlay").css('height', h);
    }

    // Legger til dagens dato i datofeltet
    static leggTilDato() {
        var dato = new Date().toISOString().substring(0, 10);
        dato.toLocaleString('no-NO')
        const inputfelt = $('#dato');
        inputfelt.val(dato);
    }

    static leggTilTidspunkt() {
        var dato = new Date();
        dato.toLocaleString('no-NO');
        let time = this.nullForran(dato.getHours())
        let minutt = this.nullForran(dato.getMinutes())
        const tidspunkt = time + ":" + minutt;
        const inputfelt = $('#tidspunkt');
        inputfelt.val(tidspunkt);
    }

    static nullForran(streng) {
        streng = streng.toString();
        if (streng.length == 1) {
            streng = "0" + streng;
        }
        return streng;
    }

    // Setter stor forbokstav og fjerner mellomrom forran og bak strengen
    static rensStoppInput(input) {
        return input.charAt(0).toUpperCase() + input.slice(1).trim();
    }

    // Formaterer reisetid (eks: argument 90, return "1 time og 30 minutter")
    static finnReisetid(reiseTid) {
        let intReise = parseInt(reiseTid, 10);
        let min = intReise % 60;
        let time = Math.floor(intReise / 60);
        let utReisetid = "";

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

    static formaterDatoOgTid(datoTid, tidEllerDato) {
        // Splitter DateTime stringen inn i dato og tid
        const datoTidSplittet = datoTid.split(" ");
        const dato = datoTidSplittet[0];

        // Formaterer dato hvis man har skrevet inn "dato" som input parameter
        if (tidEllerDato == "dato") {
            // Splitter dag, mnd år
            const datoSplittet = dato.split("-");
            const dag = datoSplittet[0];
            const mnd = datoSplittet[1];
            const aar = datoSplittet[2];

            // Liste med alle mnder
            const mnder = ["Januar", "Februar", "Mars", "April", "Mai", "Juni",
                "Juli", "August", "September", "Oktober", "November", "Desember"];
            // Gjør om mnd til formatert mnd

            const mndFormatert = mnder[mnd - 1];

            // Utskrift
            const datoFormatert = dag + "." + " " + mndFormatert + " " + aar;

            return datoFormatert;
        }
        // Formaterer tid om han har skrvet inn "tid" som input parameter
        else if (tidEllerDato === "tid") {
            const datoTidSplittet = datoTid.split(" ");
            const tidFormatert = datoTidSplittet[1];

            return tidFormatert;
        }
        else {
            console.log("Du har feil input parameter i metoden");
            return false;
        }
    }

    // Henter alle valgte billettyper
    static hentValgteBillettyper() {
        const billetter = document.querySelectorAll(".billettype");
        let billettyper = new Array();
        billetter.forEach((billett) => billettyper.push(billett.value));
        return billettyper;
    }

    // Formaterer de valgte billettypene
    static formaterValgteBillettyper(valgteBillettyper) {
        valgteBillettyper;
        valgteBillettyper.sort();
        let current = null;
        let billetTeller = 0;
        let utskrift = new Array();

        for (let billett of valgteBillettyper) {
            if (billett != current) {
                if (billetTeller > 0) {
                    let string = ` ${billetTeller}x ${current}`;
                    utskrift.push(string);
                }
                current = billett;
                billetTeller = 1;
            } else {
                billetTeller++;
            }
        }
        if (billetTeller > 0) {
            let string = ` ${billetTeller}x ${current}`;
            utskrift.push(string);
        }

        return utskrift;
    }

    // Legger til en nytt nedtrekksmeny og fyller den med billettyer-data fra DB
    static leggTilBillett() {
        const nyttAntall = $('.billettype').length + 1;
        const id = `billettype${nyttAntall}`;
        // Ny nedtrekksmeny for ny billett
        $('#billetter').append(`<select id="${id}" class="form-control billettype mb-2"></select>`);
        var dropdown = $(`#${id}`);
        // Legger til alle billettypene i den nye nedtrekksmenyen
        $.each(billettyper, function () {
            dropdown.append($("<option />").val(this.billettype).text(this.billettype));
        });
        $('#fjernBillett').css('display', 'inline-block');  // Viser knapp for å kunne fjerne en billett
    }

    // Fjener en billett
    static fjernBillett() {
        const antall = $('.billettype').length;
        const id = `#billettype${antall}`;
        if (antall > 1) {
            $(id).remove(); // Fjerner nedeste nedtrekksmeny
        }
        // Hvis antall billetter er 2 blir fjernbillett-knappen skult
        if (antall == 2) {
            $('#fjernBillett').css('display', 'none');
        }
    }
}