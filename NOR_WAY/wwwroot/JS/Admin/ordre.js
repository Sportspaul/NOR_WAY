async function sokEtterOrdre() {
    const epost = $("#ordreEpost").val();
    let data = await $.get(`../Ordre/HentOrdre?epost=${epost}`);
    data.forEach((rad) => {
        rad.billettyper = Hjelpemetoder.formaterValgteBillettyper(rad.billettyper);
    });
    $("#tabellContainer").html("");
    lagTabell(data, "D");
}

function slettRad(id) {
    $.get(`../Ordre/SlettOrdre?id=${id}`, () => {
        $(`#${id}`).remove();
    })
        .fail(() => {
            console.log("Ordren ble IKKE fjernet");
        });;
}