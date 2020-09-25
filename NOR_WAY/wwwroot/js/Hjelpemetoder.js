// Klasse med hjelpemetoder
class Hjelpemetoder{
    /* Legger til et mørkt overlay over bakgrunnsbildet,
   som matcher høyden på dokumentet */
    static endreBakgrunn() {
        var h = $(document).height();
        $("#overlay").css('height', h);
    }

    // Legger til dagens dato i datofeltet
    static leggTilDato() {
        (function () {
            var date = new Date().toISOString().substring(0, 10),
                field = document.querySelector('#dato');
            field.value = date;
        })()
    }

    // Setter stor forbokstav og fjerner mellomrom forran og bak strengen
    static rensStoppInput(input) {
        return (input.charAt(0).toUpperCase() + input.slice(1)).trim();
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

    // Legger til et nytt select-element og fyller den med billettyer-data fra DB
    static leggTilBillett() {
        const antall = $('.billettype').length;
        const id = `billettype${antall + 1}`;

        $('#billetter').append(`<select id="${id}" class="form-control billettype mb-2"></select>`);
        var dropdown = $(`#${id}`);
        $.each(billettyper, function () {
            dropdown.append($("<option />").val(this.billettype).text(this.billettype));
        });
    }
}