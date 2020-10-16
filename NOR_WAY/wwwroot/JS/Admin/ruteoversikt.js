$(function () {
    Hjelpemetoder.endreBakgrunn();
    let res = $.post("../Ruter/HentAlleRuter");
    lagRuteoversikt(res);
});