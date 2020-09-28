class BetalingEvents {

    constructor (id1, id2, id3, id4, id5, id6) {
        this.navnInput($(id1));
        this.epostInput($(id2));
        this.kortnummerInput($(id3));
        this.MMInput($(id4));
        this.AAInput($(id5));
        this.CVCInput($(id6));
    }

    // Hinder brukeren å skrive tall i inputfeltet
    navnInput(elmt){
        elmt.blur(function () {
            validerNavn(elmt, '#feilNavn');
        });
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
        elmt.blur(function () {
            validerKortnummer(elmt, '#feilKortnummer');
        });
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

    MMInput(elmt){
        elmt.blur(function () {
            if(this.value.length == 1){ 
                this.value = '0' + this.value.charAt(0) 
            }
            validerMM(elmt, '#feilMM')
        });
        // Hvis 2 tall er fylt inn hopper vi til nesete felt
        $(elmt).keyup(function () {
            if(this.value.length == 2){ $('#AA').focus(); }
        });
        this.toSiffer(elmt);
    }

    AAInput(elmt){
        elmt.blur(function () {
            validerAA(elmt, '#feilAA')
        });
        this.toSiffer(elmt); 
        // Hvis siste tall i feltet blir fjernet hopper vi inn i forrige felt
        $(elmt).keyup(function () {
            let tast = event.keyCode;
            if( tast == 8 ){
                if(this.value.length == 0){ $('#MM').focus(); }
            }
        });                          
    }

    toSiffer(elmt){
        elmt.keypress(function () { 
            if(this.value.length == 2) return false
        });
    }
}