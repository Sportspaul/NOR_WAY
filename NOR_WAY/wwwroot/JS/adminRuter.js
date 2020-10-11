function FjernRute(linjekode) {

    const rute = {
        Linjekode: linjekode
    }

    // Kaller C# Metoden FullforOrdre()
    $.post("Ruter/FjernRute", rute);
}
