using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NOR_WAY.DAL.Repositories;

namespace NOR_WAY.DAL
{
    [ExcludeFromCodeCoverage]
    public class DBInit
    {
        public static void SeedDB(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<BussContext>();

                // Datbasen blir slettet og opprettet på nytt
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                // Rute NW431 - Fjordekspressen
                var NW431Rute = new Ruter() { Linjekode = "NW431", Rutenavn = "Fjordekspressen", Startpris = 79, TilleggPerStopp = 30, Kapasitet = 55 };
                string[] NW431Stopp = { "Bergen", "Oppedal", "Lavik", "Vadheim", "Førde", "Byrkjelo", "Stryn", "Lom", "Otta", "Oppdal", "Trondheim" };
                // Injiserer dataen inn i databasen og lagrer endringene
                InjiserRute(NW431Rute, NW431Stopp, context);

                // Rute NW194 - Grenlandsekspressen
                var NW194Rute = new Ruter() { Linjekode = "NW194", Rutenavn = "Grenlandsekspressen", Startpris = 50, TilleggPerStopp = 35, Kapasitet = 45 };
                string[] NW194Stopp = { "Oslo", "Vippetangen", "Drammen", "Kronlia", "Sundbyfoss", "Hvittingfoss", "Svarstad", "Steinsholt", "Siljan", "Skien" };
                // Injiserer dataen inn i databasen og lagrer endringene
                InjiserRute(NW194Rute, NW194Stopp, context);

                // Rute NW180 - Haukeliekspressen
                var NW180Rute = new Ruter() { Linjekode = "NW180", Rutenavn = "Haukeliekspressen", Startpris = 149, TilleggPerStopp = 20, Kapasitet = 65 };
                string[] NW180Stopp = { "Oslo", "Kongsberg", "Notodden", "Sauland", "Seljord", "Åmot", "Haukeligrend", "Røldal", "Seljestad", "Ølen", "Haugesund" };
                // Injiserer dataen inn i databasen og lagrer endringene
                InjiserRute(NW180Rute, NW180Stopp, context);

                // Rute 192 - Konkurrenten
                var NW192Rute = new Ruter() { Linjekode = "NW192", Rutenavn = "Konkurrenten", Startpris = 49, TilleggPerStopp = 30, Kapasitet = 60 };
                string[] NW192Stopp = { "Oslo", "Drammen", "Fokserød", "Skjelsvik", "Tangen", "Vinterkjær", "Harebakken", "Grimstad", "Lillesand", "Kristiansand", "Mandal", "Lyngdal", "Flekkefjord", "Sandnes", "Stavanger Lufthavn", "Stavanger", "Bergen" };
                // Injiserer dataen inn i databasen og lagrer endringene
                InjiserRute(NW192Rute, NW192Stopp, context);

                // Rute 400 - Kystbussen
                var NW400Rute = new Ruter() { Linjekode = "NW400", Rutenavn = "Kystbussen", Startpris = 79, TilleggPerStopp = 32, Kapasitet = 40 };
                string[] NW400Stopp = { "Bergen", "Os", "Halhjem", "Sandvikvåg", "Leirvik", "Haukås", "Aksdal", "Mjåsund", "Arsvågen", "Mortavika", "Stavanger" };
                // Injiserer dataen inn i databasen og lagrer endringene
                InjiserRute(NW400Rute, NW400Stopp, context);

                //Rute 420 - Sognefjordekspressen
                var NW420Rute = new Ruter() { Linjekode = "NW420", Rutenavn = "Sognefjordekspressen", Startpris = 76, TilleggPerStopp = 21, Kapasitet = 35 };
                string[] NW420Stopp = { "Bergen", "Dale", "Voss", "Gudvangen", "Flåm", "Håbakken", "Lærdal", "Fodnes", "Manheller", "Sogndal" };
                // Injiserer dataen inn i databasen og lagrer endringene
                InjiserRute(NW420Rute, NW420Stopp, context);
                context.SaveChanges();

                //Rute 182 - Telemarkekspressen
                var NW182Rute = new Ruter() { Linjekode = "NW182", Rutenavn = "Telemarkekspressen", Startpris = 95, TilleggPerStopp = 16, Kapasitet = 85 };
                string[] NW182Stopp = { "Flatdal", "Bø", "Ulefoss", "Skien", "Porsgrunn", "Langangenkrysset", "Ringdal", "Fokserød", "Sandefjord Lufthavn Torp" };
                //TODO: Skien og Fokserød er mellomstopp på andre ruter
                // Injiserer dataen inn i databasen og lagrer endringene
                InjiserRute(NW182Rute, NW182Stopp, context);
                context.SaveChanges();

                //Rute 130 - Trysilekspressen
                var NW130Rute = new Ruter() { Linjekode = "NW130", Rutenavn = "Trysilekspressen", Startpris = 88, TilleggPerStopp = 28, Kapasitet = 65 };
                string[] NW130Stopp = { "Oslo",  "Oslo Lufthavn (OSL)", "Romedal", "Myklegard", "Terningmoen", "Elverum", "Kjernmoen", "Trysil Turistsenter",
                        "Radisson Blu Resort", "Trysil busstasjon", "Trysil Høyfjellssenter" };
                // Injiserer dataen inn i databasen og lagrer endringene
                InjiserRute(NW130Rute, NW130Stopp, context);
                context.SaveChanges();

                //Rute 160 - Valdresekspressen
                var NW160Rute = new Ruter() { Linjekode = "NW160", Rutenavn = "Valdresekspressen", Startpris = 82, TilleggPerStopp = 31, Kapasitet = 55 };
                string[] NW160Stopp = { "Oslo", "Hønefoss", "Nes i Ådal", "Fagernes", "Ryfoss", "Grindaheim", "Tyinkrysset", "Tyin", "Sletterust", "Øvre Årdal", "Årdal" };
                // Injiserer dataen inn i databasen og lagrer endringene
                InjiserRute(NW160Rute, NW160Stopp, context);
                context.SaveChanges();

                //Rute 162 - Øst-Vestekspressen  (Modisert for å passe vårt program)
                var NW162Rute = new Ruter() { Linjekode = "NW162", Rutenavn = "Øst-Vestekspressen", Startpris = 72, TilleggPerStopp = 29, Kapasitet = 65 };
                string[] NW162Stopp = { "Lillehammer", "Dokka", "Fagernes", "Gol", "Geilo", "Eidfjord", "Stanghelle", "Bergen" };
                // Injiserer dataen inn i databasen og lagrer endringeneE
                InjiserRute(NW162Rute, NW162Stopp, context);
                context.SaveChanges();

                // Injiserer Billettypene
                Billettyper barn = new Billettyper() { Billettype = "Barn", Rabattsats = 50 };
                Billettyper student = new Billettyper() { Billettype = "Student", Rabattsats = 25 };
                Billettyper honnor = new Billettyper() { Billettype = "Honnør", Rabattsats = 25 };
                Billettyper voksen = new Billettyper() { Billettype = "Voksen", Rabattsats = 0 };
                List<Billettyper> billettyper = new List<Billettyper> { voksen, honnor, student, barn };
                InjiserBillettyper(billettyper, context);


                Brukere admin = new Brukere();
                admin.Brukernavn = "Admin";
                admin.Tilgang = "Admin";
                admin.BrukerId = 1;
                string passord = "Admin";

                byte[] salt = InnloggingRepository.LagSalt();
                byte[] hash = InnloggingRepository.LagHash(passord, salt);
                admin.Passord = hash;
                admin.Salt = salt;
                context.Brukere.Add(admin);

                
                // Injiserer Avganger
                DateTime idag = DateTime.Now;
                InjiserAvganger(idag, NW431Rute, 2.00, 100, context);
                InjiserAvganger(idag, NW194Rute, 2.00, 100, context);
                InjiserAvganger(idag, NW180Rute, 2.00, 100, context);
                InjiserAvganger(idag, NW194Rute, 2.00, 100, context);
                InjiserAvganger(idag, NW192Rute, 2.00, 100, context);
                InjiserAvganger(idag, NW400Rute, 2.00, 100, context);
                InjiserAvganger(idag, NW420Rute, 2.00, 100, context);
                InjiserAvganger(idag, NW182Rute, 2.00, 100, context);
                InjiserAvganger(idag, NW130Rute, 2.00, 100, context);
                InjiserAvganger(idag, NW160Rute, 2.00, 100, context);
                InjiserAvganger(idag, NW162Rute, 2.00, 100, context);

                // Lagrer all seedet data
                context.SaveChanges();
            }
        }

        /* Metode som tar inn et Ruter-objekt en liste med Stopp og en
        DB-context og injiserer det inn i databasen */

        private static void InjiserRute(Ruter rute, string[] stoppListe, BussContext context)
        {
            context.Ruter.Add(rute); // Nytt instans av Ruter i databasen

            // Lopper gjennom alle stoppene i listen med stopp
            int stoppNummer = 1;
            foreach (string stoppnavn in stoppListe)
            {
                /* Finner ut om stoppet allerede eksisterer i databasen,
                hvis ikke legges det til et nytt instans av Stopp i databasen */
                Stopp stopp = context.Stopp.FirstOrDefault(s => s.Navn == stoppnavn);
                if (stopp == null)
                {
                    stopp = new Stopp()
                    {
                        Navn = stoppnavn
                    };
                    context.Stopp.Add(stopp);
                }

                // Genererer tilfeldig tall mellom 35 og 45
                Random rInt = new Random();
                int tilfeldigTid = rInt.Next(35, 45);

                // Nytt instans av RuteStopp
                var ruteStopp = new RuteStopp()
                {
                    StoppNummer = stoppNummer,
                    MinutterTilNesteStopp = tilfeldigTid,
                    Stopp = stopp,
                    Rute = rute
                };

                // Nytt instans av RuteStopp i datbasen
                context.RuteStopp.Add(ruteStopp);
                stoppNummer++;
            }

            context.SaveChanges();
        }

        private static void InjiserBillettyper(List<Billettyper> billettyper, BussContext context)
        {
            foreach (Billettyper billettype in billettyper)
            {
                context.Billettyper.Add(billettype);
            }
        }

        /* Legger inn en nye avganer
         * int antall: antall avganger som skal injiserers
         * double hyppighet: hvor lenge mellom hver avgang */

        private static void InjiserAvganger(DateTime idag, Ruter rute, double hyppighet, int antall, BussContext context)
        {
            Random tInt = new Random();
            int tilfeldigTime = tInt.Next(7, 11); // Velger Tilfeldig time 7 - 11

            // Velger tilfeldig minutt fra listen
            int tilfeldigIndex = tInt.Next(0, 3);
            List<int> minuttliste = new List<int> { 0, 15, 30, 45 };
            int tilfeldigMinutt = minuttliste[tilfeldigIndex];

            // Ny dato basert på tilfeldig time og minutt
            TimeSpan tilfeldigTidspunkt = new TimeSpan(tilfeldigTime, tilfeldigMinutt, 0);
            DateTime tilfeldigAvreise = idag.Date + tilfeldigTidspunkt;

            // Produserer 100 avganger plasser annen hver dag
            for (int i = 0; i < antall; i++)
            {
                Avganger nyAvgang = new Avganger()
                {
                    Avreise = tilfeldigAvreise,
                    SolgteBilletter = 0,
                    Rute = rute
                };
                tilfeldigAvreise = tilfeldigAvreise.AddDays(hyppighet);
                context.Avganger.Add(nyAvgang);
            }
        }
    }
}