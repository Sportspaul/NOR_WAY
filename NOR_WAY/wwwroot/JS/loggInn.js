function loggInn() {
	console.log($("#brukernavn").val());
	const brukernavnOK = validerBrukernavn($("#brukernavn"), "#feil");
	const passordOK = validerPassord($("#passord"), "#feil");

	if (brukernavnOK && passordOK) {
		const bruker = {
			brukernavn: $("#brukernavn").val(),
			passord: $("#passord").val(),
		};
		$.post("Brukere/LoggInn", bruker, function (OK) {
			window.location.href = "Adminsider/ruter.html";
		}).fail(function () {
			$("#feil").html("Feil brukernavn eller passord");
		});
	} else {
		$("#feil").html("Feil brukernavn eller passord");
	}
}
