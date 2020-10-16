$(function () {
    let res = $.post("../Stopp/HentAlleStopp");
    lagTabell(res, "U");
    filterStoppListe();
    Hjelpemetoder.endreBakgrunn();
});

function filterStoppListe() {
    let filter = document.querySelector("#filterStopp");

    filter.addEventListener("keyup", (e) => {
        let brukerInput = e.target.value.toLowerCase();
        let stoppNavn = document.querySelectorAll("#navn");

        for(let i = 1; i < stoppNavn.length; i++){
            if (stoppNavn[i].textContent.substr(0, brukerInput.length).toLowerCase() == brukerInput.toLowerCase()) {
                stoppNavn[i].closest("tr").style.display = "";
            }
            else {
                stoppNavn[i].closest("tr").style.display = "none";
            }
        }
    });
}