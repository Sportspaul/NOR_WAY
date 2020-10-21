$(() => {
	let res = $.get("../Billettyper/HentAlleBillettyper", () => {
		lagTabell(res, "CU", "nyBillettype.html");
	});
});
