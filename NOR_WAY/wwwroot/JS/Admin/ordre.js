$(function () {
    Hjelpemetoder.endreBakgrunn();
});

async function sokEtterOrdre() {
    const epost = $("#ordreEpost").val();
    let res = $.get(`../Ordre/HentOrdre?epost=${epost}`);
    lagTabell(res, "D");
}