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

    // Lukker listen med stopp hvis bruker trykker utenfor listen
    $(document).on("click", function (e) {
        lukkAlleLister(e.target);
        validerStoppnavn("#startStopp", "#feilStartStopp");
        validerStoppnavn("#sluttStopp", "#feilSluttStopp");
    });
}