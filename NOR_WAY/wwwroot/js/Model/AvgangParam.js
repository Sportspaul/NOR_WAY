// Modell av objekt som skal sendes til server ved finnNesteAvgang()
class AvgangParam{
    // Henter data fra inputfeltene
    constructor(){
        this.StartStopp = Hjelp.rensStoppInput($("#startStopp").val());
        this.SluttStopp = Hjelp.rensStoppInput($("#sluttStopp").val());
        this.Dato = $("#dato").val();
        this.Tidspunkt = $("#tidspunkt").val();
        this.AvreiseEtter = this.trueEllerFalse($('input[name="avreiseEtter"]:checked').val());
        this.Billettyper = ["Student"];
    }

    trueEllerFalse(input){
        if (input == "true") {
            return true;
        } else {
            return false;
        }
    }
}