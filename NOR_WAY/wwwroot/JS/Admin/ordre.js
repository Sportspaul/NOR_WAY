$(() => {
  Hjelpemetoder.endreBakgrunn();
});

async function sokEtterOrdre() {
  const epost = $("#ordreEpost").val();
  $.get(`../Ordre/HentOrdre?epost=${epost}`, (data) => {
    data.forEach((rad) => {
      rad.billettyper = Hjelpemetoder.formaterValgteBillettyper(
        rad.billettyper
      );
    });
    $("#tabellContainer").html("");
    lagTabell(data, "D");
  }).fail(() => {
    $("#tabellContainer").html("");
    $("#feilEpost").html(
      "Det er ingen ordre knyttet til e-postadressen i søket ditt"
    );
  });
}

function slettRad(id) {
  $.get(`../Ordre/SlettOrdre?id=${id}`, () => {
    $(`#${id}`).remove();
  }).fail(() => {
    console.log("Ordren ble IKKE fjernet");
  });
}
