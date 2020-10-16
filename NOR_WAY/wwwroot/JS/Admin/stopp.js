$(function () {
    Hjelpemetoder.endreBakgrunn();
});

function filterStoppListe() {
    let filter = document.querySelector("#filterStopp");

    filter.addEventListener("keyup", (e) => {
        let brukerInput = e.target.value.toLowerCase();
        let stoppNavn = document.querySelectorAll("#navn");


        stoppNavn.forEach((item) => {
            if (item.textContent.toLowerCase().indexOf(brukerInput) != -1) {
                console.log(item);
                item.closest("tr").style.display = "block";
            }
            else {
                item.closest("tr").style.display = "none";
            }
        });
    });
}