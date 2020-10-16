async function lagTabell(res, CUD, link) {
        let data = await res;
        // Primary key fra JSON objektet
        let id;
        for (let key in data[0]) {
            id = key;
            break;
        }

        // Sjekker om tabellen skal inneholde en "Create" knapp
        if (CUD.includes("C")) {
            const knapp = `
                <a href="${link}">
                    <button class="btn btn-success btn-sm">Opprett ny</button>
                </a>`;

            // Legger knappen til en container
            const knappContainer = $("#knapp");
            knappContainer.html(knapp); 
        }

        // Sjekker om tabellen skal inneholde en "Update" knapp
        if (CUD.includes("U")) {
            data.forEach((rad) => {
                rad.oppdater = `
                    <a href="${link}?${rad[id]}">
                        <button class="btn btn-primary btn-sm" value="${rad[id]}">Oppdater</button>
                    </a>`;
            });
          
    }
        // Sjekker om tabellen skal inneholde en "Delete" knapp
        if (CUD.includes("D")) {    
            data.forEach((rad) => {
                rad.slett = `<button class="btn btn-danger btn-sm" value="${rad[id]}" onclick="slettRad('${rad[id]}')">Slett</button>`;
            })
        }

        // Henter nøklene i JSON objektet for overskrift i tabell. Ignorerer nøkler som heter "id"
        const kolonner = [];
        
        data.forEach((rad) => {
            for (let key in rad) {
                if (kolonner.indexOf(key) === -1) {
                    if (key != "id") {
                        kolonner.push(key);
                    }
                }
            }
        });

        // Lager tabell
        let tabell = document.createElement("table");
        tabell.className = "table";
        tabell.id = "tabell";

        // Lager overskriftene i tabellen
        let tr = tabell.insertRow(-1);
        tr.id = "overskrifter";

        for (let i = 0; i < kolonner.length; i++) {
            let th = document.createElement("th");
            th.innerHTML = Hjelpemetoder.fjernCamelCasing(kolonner[i]);
                tr.appendChild(th);
        }

        // Legger til all dataen fra JSON objektet i rader
        for (let i = 0; i < data.length; i++) {
            tr = tabell.insertRow(-1);
            tr.id = data[i][id];

            for (let j = 0; j < kolonner.length; j++) {
                let tabCell = tr.insertCell(-1);
                tabCell.innerHTML = data[i][kolonner[j]];
            }
        }

        // Legger tabellen til en container
        const tabellContainer = document.getElementById("tabellContainer");
        tabellContainer.appendChild(tabell);
}

async function lagRuteoversikt(res, link) {
    let data = await res;
    let url;
    data.forEach((rad) => {
        url = `avganger.html?linjekode=${rad.linjekode}&side=0`
        rad.avganger = `<a href = ${url}><button class = "btn btn-dark btn-sm">Avganger</button></a>`;
    });
    data.forEach((rad) => {
        url = `ruteStopp.html?linjekode=${rad.linjekode}`;
        rad.rutestopp = `<a href = ${url}><button class = "btn btn-dark btn-sm">Rutestopp</button></a>`;
            
    });
    lagTabell(data, "CUD", link);
}

async function lagAvgangOversikt(res, CUD, link) {
    let data = await res;
    let dato;
    data.forEach((rad) => {
        rad.avreise = Hjelpemetoder.formaterDatoOgTid(rad.avreise, "dato") + " " + Hjelpemetoder.formaterDatoOgTid(rad.avreise, "tid");
    });
    lagTabell(data, CUD, link);
}

async function lagStoppoversikt(res) {
    let data = await res;
    data.forEach((rad) => {
    //    url = `ruteStopp.html?linjekode=${rad.linjekode}&side=0`
    //    rad.avganger ;
    });
    lagTabell(data, "U");
}

async function sokEtterOrdre() {
    const epost = $("#ordreEpost").val();
    let res = $.get(`../Ordre/HentOrdre?epost=${epost}`);
    lagTabell(res, "D");
}