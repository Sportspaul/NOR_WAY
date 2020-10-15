$(function () {
    let url = window.location.href.split("=");
    sidenummer = url[2];
    linjekode = url[1].split("&")
    linjekode = linjekode[0];
    
    let res = $.get(`../Avgang/HentAvganger?linjekode=${linjekode}&sidenummer=${sidenummer}`);
    lagTabell(res, "CUD");
});
