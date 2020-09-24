using System;
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
                Injiser(NW431Rute, NW431Stopp, context);
                context.SaveChanges();

                // Rute NW194 - Grenlandsekspressen
                var NW194Rute = new Ruter() { Linjekode = "NW194", Rutenavn = "Grenlandsekspressen", Startpris = 50, TilleggPerStopp = 35, Kapasitet = 45 };
                string[] NW194Stopp = { "Oslo", "Vippetangen", "Drammen", "Kronlia", "Sundbyfoss", "Hvittingfoss", "Svarstad", "Steinsholt", "Siljan", "Skien" };
                // Injiserer dataen inn i databasen og lagrer endringene
                Injiser(NW194Rute, NW194Stopp, context);
                context.SaveChanges();

                // Rute NW180 - Haukeliekspressen
                var NW180Rute = new Ruter() { Linjekode = "NW180", Rutenavn = "Haukeliekspressen", Startpris = 149, TilleggPerStopp = 20, Kapasitet = 65 };
                string[] NW180Stopp = { "Oslo", "Kongsberg", "Notodden", "Sauland", "Seljord", "Åmot", "Haukeligrend", "Røldal", "Seljestad", "Ølen", "Haugesund" };
                // Injiserer dataen inn i databasen og lagrer endringene
                Injiser(NW180Rute, NW180Stopp, context);
                context.SaveChanges();

                // Rute 192 - Konkurrenten
                var NW192Rute = new Ruter() { Linjekode = "NW192", Rutenavn = "Konkurrenten", Startpris = 49, TilleggPerStopp = 30, Kapasitet = 60 };
                string[] NW192Stopp = { "Oslo", "Drammen", "Fokserød", "Skjelsvik", "Tangen", "Vinterkjær", "Harebakken", "Grimstad", "Lillesand", "Kristiansand", "Mandal", "Lyngdal", "Flekkefjord", "Sandnes", "Stavanger Lufthavn", "Stavanger", "Bergen" };
                // Injiserer dataen inn i databasen og lagrer endringene
                Injiser(NW192Rute, NW192Stopp, context);
                context.SaveChanges();


                // Injiserer Billettypene
                Billettyper barn = new Billettyper() { Billettype = "Barn", Rabattsats = 50 };
                Billettyper student = new Billettyper() { Billettype = "Student", Rabattsats = 25 };
                Billettyper honnor = new Billettyper() { Billettype = "Honnør", Rabattsats = 25 };
                Billettyper voksen = new Billettyper() { Billettype = "Voksen", Rabattsats = 0 };
                context.Billettyper.Add(barn);
                context.Billettyper.Add(student);
                context.Billettyper.Add(honnor);
                context.Billettyper.Add(voksen);

                //Avganger
                DateTime dato1 = new DateTime(2020, 10, 25, 8, 30, 00);
                Avganger avgang1 = new Avganger() { Avreise = dato1, SolgteBilletter = 0, Rute = NW431Rute };
                DateTime dato2 = new DateTime(2020, 11, 20, 17, 00, 00);
                Avganger avgang2 = new Avganger() { Avreise = dato2, SolgteBilletter = 0, Rute = NW431Rute };

                DateTime dato3 = new DateTime(2020, 10, 26, 12, 30, 00);
                Avganger avgang3 = new Avganger() { Avreise = dato3, SolgteBilletter = 0, Rute = NW194Rute };
                DateTime dato4 = new DateTime(2020, 11, 21, 10, 00, 00);
                Avganger avgang4 = new Avganger() { Avreise = dato4, SolgteBilletter = 0, Rute = NW194Rute };

                DateTime dato5 = new DateTime(2020, 12, 12, 10, 30, 00);
                Avganger avgang5 = new Avganger() { Avreise = dato5, SolgteBilletter = 0, Rute = NW180Rute };
                DateTime dato6 = new DateTime(2020, 11, 22, 13, 00, 00);
                Avganger avgang6 = new Avganger() { Avreise = dato6, SolgteBilletter = 0, Rute = NW180Rute };

                DateTime dato7 = new DateTime(2021, 1, 12, 10, 30, 00);
                Avganger avgang7 = new Avganger() { Avreise = dato7, SolgteBilletter = 0, Rute = NW192Rute };
                DateTime dato8 = new DateTime(2020, 11, 23, 14, 30, 00);
                Avganger avgang8 = new Avganger() { Avreise = dato8, SolgteBilletter = 0, Rute = NW192Rute };


                context.Avganger.Add(avgang1);
                context.Avganger.Add(avgang2);
                context.Avganger.Add(avgang3);
                context.Avganger.Add(avgang4);
                context.Avganger.Add(avgang5);
                context.Avganger.Add(avgang6);
                context.Avganger.Add(avgang7);
                context.Avganger.Add(avgang8);

                context.SaveChanges();
            }
        }

        /* Metode som tar inn et Ruter-objekt en liste med Stopp og en 
        DB-context og injiserer det inn i databasen */
        private static void Injiser(Ruter rute, string[] stoppListe, BussContext context)
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
                // int randomTid = rInt.Next(15, 45);

                // Nytt instans av RuteStopp
                var ruteStopp = new RuteStopp()
                {
                    StoppNummer = stoppNummer,
                    MinutterTilNesteStopp = 20,
                    Stopp = stopp,
                    Rute = rute
                };

                // Nytt instans av RuteStopp i datbasen
                context.RuteStopp.Add(ruteStopp);
                stoppNummer++;
            }
        }
    }
}
