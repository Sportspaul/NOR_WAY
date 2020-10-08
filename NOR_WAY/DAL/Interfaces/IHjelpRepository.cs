using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NOR_WAY.DAL.Repositories
{
    public interface IHjelpRepository
    {
        Task<List<Ruter>> FinnRuteneTilStopp(Stopp stopp);

        // Hjelpemetode som finner stoppnummeret til et spesifikt stopp i en spesifikk rute
        Task<int> FinnStoppNummer(Stopp stopp, Ruter fellesRute);

        // Hjelpemetode som beregner reisetiden fra startStopp til sluttStopp
        Task<int> BeregnReisetid(int startStopp, int sluttStopp, Ruter fellesRute);

        // Hjelpemetode som finner neste avgang som passer for brukeren
        Task<Avganger> NesteAvgang(Ruter fellesRute, int reisetid,
            bool avreiseEtter, DateTime innTid, int antallBilletter);

        // Endrer avreisetiden hvis påstigning ikke er første stopp i ruten
        Task<DateTime> BeregnAvreisetid(DateTime avreise, int stoppNummer, Ruter fellesRute);

        // Hjelpemetode som finner Ruten to lister med Ruter har til felles
        Ruter FinnFellesRute(List<Ruter> startStoppRuter, List<Ruter> sluttStoppRuter);

        // Oversetter string verdiene av dato og tidspunkt til et DateTime-objekt
        DateTime StringTilDateTime(string dato, string tidspunkt);

        int BeregnPris(Ruter rute, int antallStopp, List<string> billettyper, List<Billettyper> billettListe);
    }
}