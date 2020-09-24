// Kalles når siden laster inn
$(function () {
    hentAlleRuter();
    endreBakgrunn();
});

function endreBakgrunn() {
    var h = $(document).height();
    $("#overlay").css('height', h);
}

//Til rutedelen
function hentAlleRuter() {
    //TODO: gjøre noe med dataene
    $.get("Buss/HentAlleRuter", function (allerutene) {
        console.log(allerutene.length); //TODO: Ta vekk
        let rutene = new Array();
        for (let i = 0; i < allerutene.length; i++) {
            rutene.push(allerutene[i]);
            console.table(allerutene[i]); //TODO: Ta vekk
        }

    });
}