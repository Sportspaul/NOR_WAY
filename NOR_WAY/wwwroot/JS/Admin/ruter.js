$(function () {    
    Hjelpemetoder.endreBakgrunn();    
    const res = $.post("../Ruter/HentAlleRuter");
    const link = "nyRute.html";
    lagRuteoversikt(res, link);
});