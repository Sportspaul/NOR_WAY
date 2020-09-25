// Modell for å ta imot et Avgang-obj fra backend
class Avgang {
    constructor(response) {
        this.avgangId = response.avgangId;
        this.rutenavn = response.rutenavn;
        this.linjekode = response.linjekode;
        this.pris = response.pris;
        this.avreise = response.avreise;
        this.ankomst = response.ankomst;
        this.reisetid = response.reisetid;
    }
}