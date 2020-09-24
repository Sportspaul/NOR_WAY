// Kalles når siden laster inn
$(function () {
    hentAlleRuter();
});

let ruteMatrise = new Array();

//Til rutedelen
function hentAlleRuter() {
    $.get("Buss/HentAlleRuter", function (allerutene) {
        let rutene = new Array();
        for (let i = 0; i < allerutene.length; i++) {
            rutene.push(allerutene[i]);
        }

        // Setter global liste for bruk utenfor functions
        ruteMatrise = rutene;

        // Skriver ut alle rutene
        utRuter = "<h4>Ruter</h4>"
        for (let i = 0; i < rutene.length; i++) {
            utRuter += `<h5 class="mt-4"
                            onclick="byttRute(${i})">${rutene[i].rutenavn}
                        </h5>`;
        }
        $("#ruter").html(utRuter);

        // Skriver ut stoppene til første ruten
        byttRute(0);

        // Endrer høyden på shadowBox til å matche dataen
        endreBoksHoyde();
    });
}

// Skriver ut stoppene til ruten med nummer til argumentet 
function byttRute(i) {
    let utStopp = `<h4 class='mb-4'>Stoppene i ${ruteMatrise[i].linjekode},<br>

    let utStopp = `<h4 class='mb-4'>Stoppene på linje ${ruteMatrise[i].linjekode},<br>
                    ${ruteMatrise[i].rutenavn}</h4>`;

    utStopp += `<table class="table">
                        <tr>
                            <th>Stoppnavn</th>
                            <th>Tid</th>
                        </tr>
                    `;

    ruteMatrise[i].stoppene.forEach(stopp =>
        utStopp += `<tr>
                        <td>${stopp}</td>
                        <td>00:00</td>
                    </tr>`
    );

    utStopp += "</table>";
    $("#stopp").html(utStopp);
}

// Endrer høyden på shadowBox til å matche dataen
function endreBoksHoyde() {
    var s = $("#stopp").height();
    var r = $("#ruter").height();

    if (s > r) {
        $(".shadowBox").css('height', s + 75);
    } else {
        $(".shadowBox").css('height', r + 75);
    }
    endreBakgrunn();
}

function endreBakgrunn() {
    var h = $(document).height();
    $("#overlay").css('height', h);
}