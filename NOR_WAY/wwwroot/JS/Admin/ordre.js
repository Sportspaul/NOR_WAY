async function sokEtterOrdre() {
    $("#tabellContainer").html("");
    const epost = $("#ordreEpost").val();
    let data = await $.get(`../Ordre/HentOrdre?epost=${epost}`);
    data.forEach((rad) => {
        rad.billettyper = Hjelpemetoder.formaterValgteBillettyper(rad.billettyper);
    });
    lagTabell(data, "D");
}