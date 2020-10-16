$(function () {
    Hjelpemetoder.endreBakgrunn();
    let res = $.post("../Billettyper/HentAlleBillettyper");
    lagTabell(res, "CU", "nyBillettype.html");
});
