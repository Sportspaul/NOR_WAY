// Modell for å ta imot et Avgang-obj fra backend
class Avgang {
	constructor(response) {
		this.AvgangId = response.avgangId;
		this.Rutenavn = response.rutenavn;
		this.Linjekode = response.linjekode;
		this.Pris = response.pris;
		this.Avreise = response.avreise;
		this.Ankomst = response.ankomst;
		this.Reisetid = response.reisetid;
	}
}
