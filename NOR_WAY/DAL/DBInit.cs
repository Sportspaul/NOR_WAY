﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace NOR_WAY.DAL
{
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

                // Injiserer Billettypene
                Billettyper barn = new Billettyper() { Billettype = "Barn", Rabattsats = 50 };
                Billettyper student = new Billettyper() { Billettype = "Student", Rabattsats = 25 };
                Billettyper honnor = new Billettyper() { Billettype = "Honnør", Rabattsats = 25 };
                Billettyper voksen = new Billettyper() { Billettype = "Voksen", Rabattsats = 0 };
                List<Billettyper> billettyper = new List<Billettyper> { barn, student, honnor, voksen };
                InjiserBillettyper(billettyper, context);
                
                // Injiserer Avganger
                DateTime idag = DateTime.Now;
                InjiserAvganger(idag, NW431Rute, 2.00, 200, context);
                InjiserAvganger(idag, NW194Rute, 2.00, 200, context);
                InjiserAvganger(idag, NW180Rute, 2.00, 200, context);
                InjiserAvganger(idag, NW194Rute, 2.00, 200, context);
                InjiserAvganger(idag, NW192Rute, 2.00, 200, context);
                InjiserAvganger(idag, NW400Rute, 2.00, 200, context);


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

                // Genererer tilfeldig tall mellom 10 og 25
                // Random rInt = new Random();
                // int tilfeldigTid = rInt.Next(15, 45);

                // Nytt instans av RuteStopp
                var ruteStopp = new RuteStopp()
                {
                    StoppNummer = stoppNummer,
                    MinutterTilNesteStopp = 45,
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