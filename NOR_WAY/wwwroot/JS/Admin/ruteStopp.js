let url = window.location.href.split("=");
linjekode = url[1].split("&")
linjekode = linjekode[0];

let res = $.get(`../RuteStopp/HentRuteStopp?linjekode=${linjekode}`);
lagTabell(res, "CUD", "nyRuteStopp.html?NW431");