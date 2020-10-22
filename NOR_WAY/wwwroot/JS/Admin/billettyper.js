$(function () {
	Hjelpemetoder.endreBakgrunn();
	const link = "nyBillettype.html";
	let res = $.get("../Billettyper/HentAlleBillettyper", () => {
		lagTabell(res, "CU", link);
	}).fail(() => {
		lagNyknapp(link);
	});
});
