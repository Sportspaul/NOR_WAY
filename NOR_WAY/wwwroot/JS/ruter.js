// Kalles når siden laster inn
$(function () {
	Hjelpemetoder.settKorrektNavigering();
	hentAlleRuter();
});

let ruteMatrise = new Array();

//Til rutedelen
function hentAlleRuter() {
	$.get("Ruter/HentRuterMedStopp", function (allerutene) {
		let rutene = new Array();
		for (let i = 0; i < allerutene.length; i++) {
			rutene.push(allerutene[i]);
		}

		// Setter global liste for bruk utenfor functions
		ruteMatrise = rutene;

		// Skriver ut alle rutene
		utRuter = "<h4>Velg Rute</h4>";
		for (let i = 0; i < rutene.length; i++) {
			utRuter += `<h5 id="${rutene[i].linjekode}"class="mt-4 rute"
                            onclick="byttRute(${i}, ${rutene[i].linjekode})">${rutene[i].rutenavn}
                        </h5>`;
		}
		$("#ruter").html(utRuter);

		// Skriver ut stoppene til første ruten
		byttRute(0, "#" + rutene[0].linjekode);

		// Endrer høyden på shadowBox til å matche dataen
		tilpassHoyde();
	});
}

// Skriver ut stoppene til ruten med nummer til argumentet
function byttRute(i, id) {
	$(".rute").css("text-decoration", "none");
	$(id).css("text-decoration", "underline");

	let {
		rutenavn,
		linjekode,
		startpris,
		tilleggPerStopp,
		stoppene,
		minutterTilNesteStopp,
	} = ruteMatrise[i];

	// Legger til overskrift
	let utStopp = `<h4 class='mb-4'>Linje ${linjekode}, ${rutenavn}</h4>`;

	// Legger til rutedetaljer
	utStopp += `<h6>Startpris: ${startpris} kr</h6>
                <h6>Tillegg per stopp: ${tilleggPerStopp} kr</h6>
                <h6>
                    ${stoppene[0]} → ${stoppene[stoppene.length - 1]}:
                    ${beregnReisetid(
											minutterTilNesteStopp,
											minutterTilNesteStopp.length - 1
										)}
                </h6>
                <h6 class="mb-4">Antall stopp: ${stoppene.length}</h6>`;

	// Legger til overskrifter i tabellen
	utStopp += `<table class="table">
                        <tr>
                            <th>Stoppnavn</th>
                            <th>Reisetid fra første stopp</th>
                        </tr>`;

	// Legger til alle stoppene og reisetid
	for (let i = 0; i < stoppene.length; i++) {
		utStopp += `<tr>
                        <td>${stoppene[i]}</td>
                        <td>${beregnReisetid(minutterTilNesteStopp, i)}</td>
                    </tr>`;
	}

	// Lukker tabelltagen og skriver til dokumentet
	utStopp += "</table>";
	$("#stopp").html(utStopp);
	tilpassHoyde();
}

// Legger sammen en liste med tall fram til gitt index
function beregnReisetid(minutterTilNesteStopp, index) {
	let minutter = 0;
	for (j = 0; j < index; j++) {
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
	const offset = { top: 0, left: 0 };
	$("html, body").animate(
		{
			scrollTop: offset.top,
			scrollLeft: offset.left,
		},
		0
	);

	// Justerer høyden på shadowBox og overlay
	if (s > r) {
		elmt.css("height", s + 50);
		overlay.css("height", s + 175);
	} else {
		elmt.css("height", r + 50);
		overlay.css("height", r + 175);
	}
}
