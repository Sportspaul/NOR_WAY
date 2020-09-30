// Modell av objekt som skal sendes til server ved finnNesteAvgang()
class AvgangParam {
    // Henter data fra inputfeltene
    constructor() {
        this.StartStopp = Hjelpemetoder.rensStoppInput($("#startStopp").val());
        this.SluttStopp = Hjelpemetoder.rensStoppInput($("#sluttStopp").val());
        this.Dato = $("#dato").val();
        this.Tidspunkt = $("#tidspunkt").val();
        this.AvreiseEtter = this.trueEllerFalse($('input[name="avreiseEtter"]:checked').val());
        this.Billettyper = Hjelpemetoder.hentValgteBillettyper();
    }

    trueEllerFalse(input) {
        if (input == "true") {
            return true;
        } else {
            return false;
        }
    }
}