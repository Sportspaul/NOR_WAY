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
    let { rutenavn, linjekode, startpris, tilleggPerStopp, stoppene, minutterTilNesteStopp } = ruteMatrise[i];
    
    // Legger til overskrift
    let utStopp = `<h4 class='mb-4'>Linje ${linjekode}, ${rutenavn}</h4>`;
    
    // Legger til rutedetaljer
    utStopp += `<h6>Startpris: ${startpris}</h6>
                <h6>Tillegg per stopp: ${tilleggPerStopp}</h6>
                <h6 class="mb-4">
                    ${stoppene[0]} → ${stoppene[stoppene.length - 1]}: 
                    ${beregnReisetid(minutterTilNesteStopp, minutterTilNesteStopp.length - 1)}
                </h6>`;

    // Legger til overskrifter i tabellen
    utStopp += `<table class="table">
                        <tr>
                            <th>Stoppnavn</th>
                            <th>Reisetid fra første stopp</th>
                        </tr>`;
                        
    // Legger til alle stoppene og reisetid
    for(let i = 0; i < stoppene.length; i++){
        utStopp += `<tr>
                        <td>${stoppene[i]}</td>
                        <td>${beregnReisetid(minutterTilNesteStopp, i)}</td>
                    </tr>`
    }

    // Lukker tabelltagen og skriver til dokumentet
    utStopp += "</table>";
    $("#stopp").html(utStopp);
    tilpassHoyde();
}

// Legger sammen en liste med tall fram til gitt index
function beregnReisetid(minutterTilNesteStopp, index) {
    let minutter = 0;
    for(j = 0; j < index; j++) {
        minutter += minutterTilNesteStopp[j];
    }
    return Hjelpemetoder.finnReisetid(minutter);
}

// Endrer høyden på shadowBox til å matche dataen
function tilpassHoyde() {
    const s = $("#stopp").height();
    const r = $("#ruter").height();
    const elmt = $(".shadowBox");
    const overlay = $("#overlay");
    
    // Scroller til toppen av siden
    const offset = {top: 0, left: 0};
    $('html, body').animate({
        scrollTop: offset.top,
        scrollLeft: offset.left
    }, 0);

    // Justerer høyden på shadowBox og overlay
    if (s > r) {
        elmt.css('height', s + 50);
        overlay.css('height', s + 175);
    } else {
        elmt.css('height', r + 50);
        overlay.css('height', r + 175);
    }

}
