class BetalingEvents {

    constructor (id1, id2, id3, id4, id5, id6) {
        this.forbyTall($(id1));
        this.epostInput($(id2));
        this.kortnummerInput($(id3));
        this.MMInput($(id4));
        this.AAInput($(id5));
        this.CVCInput($(id6));
    }

    // Hinder brukeren å skrive tall i inputfeltet
    forbyTall(elmt){
        elmt.on("input", function (e) {
            elmt.val(elmt.val().replace(/[^a-åA-Å- ]/, ''));
        });
    }

    epostInput(elmt) {
        elmt.blur(function () {
            validerEpost(elmt, '#feilEpost')
        });
    }

    /* Hindere brukeren å skrive inn annet enn tall, og legger til mellomrom for hvert fjerde tall 
     * eks. "1234 1234 1234 1234" */
    kortnummerInput(elmt){
        elmt.on("input", function (e) {
            elmt.val($(elmt).val().replace(/[^\d]/g, '').replace(/(.{4})/g, '$1 ').trim());
        });
    }

    CVCInput(elmt){
        elmt.blur(function () {
            validerCVC(elmt, '#feilCVC');
            elmt.val(elmt.val().replace(/[^\d]/g, '').replace(/(.{4})/g, '$1 ').trim());
        });
        $(elmt).keypress(function () {
            if(this.value.length == 3){ return false; }
        });
    }

    AAInput(elmt){
        elmt.blur(function () {
            validerAA(elmt, '#feilAA')
        });
        this.toSiffer(elmt);                           
    }

    MMInput(elmt){
        elmt.blur(function () {
            if(this.value.length == 1){ 
                this.value = '0' + this.value.charAt(0) 
            }
            validerMM(elmt, '#feilMM')
        });
        this.toSiffer(elmt);
    }

    toSiffer(elmt){
        elmt.keypress(function () { 
            if(this.value.length == 2) return false
        });
    }
}