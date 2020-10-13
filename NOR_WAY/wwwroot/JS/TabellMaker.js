function CreateTableFromJSON(JSONresponse, CUD) {
    JSONresponse.then(function (JSONdataset) {        
        console.log(JSONdataset);

        if (CUD.includes("C")) {
            console.log("create");
            const button = document.createElement("button");
            button.innerHTML = "Ny";
            var body = document.getElementsByTagName("body")[0];
            body.appendChild(button);

            }
        if (CUD.includes("U")) {
            console.log("update")
            for (let i = 0; i < JSONdataset.length; i++) {
                for (let key in JSONdataset[i]) {
                    JSONdataset[i].Oppdater = `<button value="${JSONdataset[i].linjekode}">Oppdater</button>`;
                }
            }
        }
        if (CUD.includes("D")) {
            console.log("delete")
            for (let i = 0; i < JSONdataset.length; i++) {
                for (let key in JSONdataset[i]) {
                    JSONdataset[i].Delete = `<button value="${JSONdataset[i].linjekode}">Slett</button>`;
                }
            }
        }

        // Henter nøklene i JSON objektet for overskrift i tabell
        const kolonner = [];
        for (let i = 0; i < JSONdataset.length; i++) {

            for (let key in JSONdataset[i]) {
                if (kolonner.indexOf(key) === -1) {
                    kolonner.push(key);
                }
            }
        }

        // Lager tabell
        const tabell = document.createElement("table");

        // Lager overskriftene i tabellen
        let tr = tabell.insertRow(-1);

        for (let i = 0; i < kolonner.length; i++) {
                let th = document.createElement("th");
                th.innerHTML = kolonner[i];
                tr.appendChild(th);
        }

        // Legger til all dataen fra JSON objektet i rader
        for (let i = 0; i < JSONdataset.length; i++) {
            tr = tabell.insertRow(-1);

            for (let j = 0; j < kolonner.length; j++) {
                let tabCell = tr.insertCell(-1);
                tabCell.innerHTML = JSONdataset[i][kolonner[j]];
            }
        }



        // Legger tabellen til en container
        const divContainer = document.getElementById("showData");
        divContainer.innerHTML = "";
        divContainer.appendChild(tabell);
    });
}