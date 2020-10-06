using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NOR_WAY.Controllers;
using NOR_WAY.DAL;
using NOR_WAY.Model;
using Xunit;
using Xunit.Abstractions;

namespace NOR_WAY_Tests
{
    public class BussTests
    {
        private readonly Mock<IBussRepository> mockRepo = new Mock<IBussRepository>();
        private readonly Mock<ILogger<BussController>> mockLogCtr = new Mock<ILogger<BussController>>();
        private readonly ITestOutputHelper output;

        public BussTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        /* Tester at ikke listen med Stopp fra BussRepo endrer seg i controlleren
            for HentAlleStopp() */
        [Fact]
        public async Task HentAlleStopp_RiktigeVerdier()
        {
            List<Stopp> forventedeStopp = HentStoppListe();

            mockRepo.Setup(b => b.HentAlleStopp()).ReturnsAsync(forventedeStopp);
            var bussController = new BussController(mockRepo.Object, mockLogCtr.Object);
            var resultat = await bussController.HentAlleStopp() as OkObjectResult;
            List<Stopp> faktiskeStopp = (List<Stopp>)resultat.Value;

            Assert.Equal(forventedeStopp.Count, faktiskeStopp.Count);   // Tester om listene er like lange
            // Tester om alle verdiene er like i alle elementene
            for (int i = 0; i < forventedeStopp.Count; i++)
            {
                Assert.Equal(forventedeStopp[i].Id, faktiskeStopp[i].Id);
                Assert.Equal(forventedeStopp[i].Navn, faktiskeStopp[i].Navn);
            }
        }

        /* Tester at ikke listen med Stopp fra BussRepo endrer seg i controlleren
           for FinnMuligeStartStopp() */
        [Fact]
        public async Task FinnMuligStartStopp_RiktigeVerdier()
        {
            List<Stopp> forventedeStopp = HentStoppListe();
            InnStopp innStopp = new InnStopp { Navn = "Bergen" };

            mockRepo.Setup(b => b.FinnMuligeStartStopp(innStopp)).ReturnsAsync(forventedeStopp);
            var bussController = new BussController(mockRepo.Object, mockLogCtr.Object);
            var resultat = await bussController.FinnMuligeStartStopp(innStopp) as OkObjectResult;
            List<Stopp> faktiskeStopp = (List<Stopp>)resultat.Value;

            Assert.Equal(forventedeStopp.Count, faktiskeStopp.Count);   // Tester om listene er like lange
            // Tester om alle verdiene er like i alle elementeneƒ
            for (int i = 0; i < forventedeStopp.Count; i++)
            {
                Assert.Equal(forventedeStopp[i].Id, faktiskeStopp[i].Id);
                Assert.Equal(forventedeStopp[i].Navn, faktiskeStopp[i].Navn);
            }
        }

        /* Tester at ikke listen med Stopp fra BussRepo endrer seg i controlleren
           for FinnMuligeSluttStopp() */
        [Fact]
        public async Task FinnMuligSluttStopp_RiktigeVerdier()
        {
            List<Stopp> forventet = HentStoppListe();
            InnStopp innStopp = new InnStopp { Navn = "Bergen" };

            mockRepo.Setup(b => b.FinnMuligeSluttStopp(innStopp)).ReturnsAsync(forventet);
            var bussController = new BussController(mockRepo.Object, mockLogCtr.Object);
            var resultat = await bussController.FinnMuligeSluttStopp(innStopp) as OkObjectResult;
            List<Stopp> faktisk = (List<Stopp>)resultat.Value;

            Assert.Equal(forventet.Count, faktisk.Count);   // Tester om listene er like lange
            // Tester om alle verdiene er like i alle elementene
            for (int i = 0; i < forventet.Count; i++)
            {
                Assert.Equal(forventet[i].Id, faktisk[i].Id);
                Assert.Equal(forventet[i].Navn, faktisk[i].Navn);
            }
        }

        // Returnerer en liste med Stopp
        private List<Stopp> HentStoppListe()
        {
            Stopp stopp1 = new Stopp { Id = 1, Navn = "Bergen" };
            Stopp stopp2 = new Stopp { Id = 1, Navn = "Oslo" };
            Stopp stopp3 = new Stopp { Id = 1, Navn = "Vadheim" };
            Stopp stopp4 = new Stopp { Id = 1, Navn = "Trondheim" };
            return new List<Stopp> { stopp1, stopp2, stopp3, stopp4 };
        }

        /* Tester at ikke listen med Stopp fra BussRepo endrer seg i controlleren
           for HentAlleBillettyper() */
        [Fact]
        public async Task HentAlleBillettyper_RiktigeVerdier()
        {
            List<Billettyper> forventet = HentBillettypeListe();

            mockRepo.Setup(b => b.HentAlleBillettyper()).ReturnsAsync(forventet);
            var bussController = new BussController(mockRepo.Object, mockLogCtr.Object);
            var resultat = await bussController.HentAlleBillettyper() as OkObjectResult;
            List<Billettyper> faktisk = (List<Billettyper>)resultat.Value;

            Assert.Equal(forventet.Count, faktisk.Count);   // Tester om listene er like lange
            // Tester om alle verdiene er like i alle elementene
            for (int i = 0; i < forventet.Count; i++)
            {
                Assert.Equal(forventet[i].Billettype, faktisk[i].Billettype);
                Assert.Equal(forventet[i].Rabattsats, faktisk[i].Rabattsats);
            }
        }

        private List<Billettyper> HentBillettypeListe()
        {
            Billettyper billettype1 = new Billettyper { Billettype = "Student", Rabattsats = 50 };
            Billettyper billettype2 = new Billettyper { Billettype = "Voksen", Rabattsats = 0};
            Billettyper billettype3 = new Billettyper{ Billettype = "Honør", Rabattsats = 25};
            return new List<Billettyper> { billettype1, billettype2, billettype3 };
        }

        /* Tester at ikke listen med Stopp fra BussRepo endrer seg i controlleren
           for HentAlleRuter() */
        [Fact]
        public async Task HentAlleRuter_RiktigeVerdier()
        {
            List<RuteData> forventet = HentRuteDataListe();

            mockRepo.Setup(b => b.HentAlleRuter()).ReturnsAsync(forventet);
            var bussController = new BussController(mockRepo.Object, mockLogCtr.Object);
            var resultat = await bussController.HentAlleRuter() as OkObjectResult;
            List<RuteData> faktisk = (List<RuteData>)resultat.Value;

            Assert.Equal(forventet.Count, faktisk.Count);   // Tester om listene er like lange
            // Tester om alle verdiene er like i alle elementene
            for (int i = 0; i < forventet.Count; i++)
            {
                Assert.Equal(forventet[i].Stoppene, faktisk[i].Stoppene);
                Assert.Equal(forventet[i].MinutterTilNesteStopp, faktisk[i].MinutterTilNesteStopp);
                Assert.Equal(forventet[i].Linjekode, faktisk[i].Linjekode);
                Assert.Equal(forventet[i].Rutenavn, faktisk[i].Rutenavn);
                Assert.Equal(forventet[i].Startpris, faktisk[i].Startpris);
                Assert.Equal(forventet[i].TilleggPerStopp, faktisk[i].TilleggPerStopp);
            }
        }

        private List<RuteData> HentRuteDataListe()
        {
            List<string> stoppene1 = new List<string> { "Bergen", "Vaheim", "Trondheim" };
            List<string> stoppene2 = new List<string> { "Oslo", "Røros", "Trondheim" };
            List<string> stoppene3 = new List<string> { "Kristiansand", "Stavanger", "Molde" };
            List<int> minuttListe1 = new List<int> { 20, 25, 35 };
            List<int> minuttListe2 = new List<int> { 20, 23, 35 };
            List<int> minuttListe3 = new List<int> { 20, 55, 60 };
            RuteData ruteData1 = new RuteData { Stoppene = stoppene1, MinutterTilNesteStopp = minuttListe1, Linjekode = "NW123", Rutenavn = "Bussturen", Startpris = 79, TilleggPerStopp = 25 };
            RuteData ruteData2 = new RuteData { Stoppene = stoppene2, MinutterTilNesteStopp = minuttListe2, Linjekode = "NW600", Rutenavn = "Ekspressruta", Startpris = 100, TilleggPerStopp = 15 };
            RuteData ruteData3 = new RuteData { Stoppene = stoppene3, MinutterTilNesteStopp = minuttListe3, Linjekode = "NW007", Rutenavn = "Bondespressen", Startpris = 50, TilleggPerStopp = 35 };
            return new List<RuteData> { ruteData1, ruteData2, ruteData3 };
        }

        // Tester at ikke Avgang fra BussRepo endrer seg i controlleren
        [Fact]
        public async Task FinnNesteAvgang_RiktigeVerdier()
        {
            AvgangParam param = HentAvgangParam();
            Avgang forventetAvgang = HentEnAvgang();

            // Lager en mock av IBussRepo og sier at FinnNesteAvgang-metoden (BussRepo) alltid skal returnerer Avgang fra enAvgang()
            mockRepo.Setup(b => b.FinnNesteAvgang(param)).ReturnsAsync(forventetAvgang);

            /* Nytt instans av BussCtr som tar inn IBussRepo-mock som argument.
             * Altså objektet med FinnNesteAvgang-metoden som alltid returnerer Avgang fra enAvgang() */
            var bussController = new BussController(mockRepo.Object, mockLogCtr.Object);

            /* Kaller FinnNesteAvgang-metoden (BussCtr),
             * som igjen kaller FinnNesteAvgang (BussRepo), 
             * men siden vi har mocket vil den metoden alltid returnere Avgang fra enAvgang() */
            var resultat = await bussController.FinnNesteAvgang(param) as OkObjectResult;

            // Henter ut Avgang-objektet fra OkObjectResult som FinnNesteAvgang fra Controlleren returnerte
            Avgang faktiskAvgang = (Avgang)resultat.Value;

            /* Disse testene sjekker da om Avgang-objetet ble modifisert i FinnNesteAvgang (BussCtr)
             * Det betyr at vi har isolert metoden fra resten av programmet 
             * Vi luker ut eventuelle feil som kan oppså i DB */
            Assert.Equal(forventetAvgang.AvgangId, faktiskAvgang.AvgangId);
            Assert.Equal(forventetAvgang.Rutenavn, faktiskAvgang.Rutenavn);
            Assert.Equal(forventetAvgang.Linjekode, faktiskAvgang.Linjekode);
            Assert.Equal(forventetAvgang.Pris, faktiskAvgang.Pris);
            Assert.Equal(forventetAvgang.Avreise, faktiskAvgang.Avreise);
            Assert.Equal(forventetAvgang.Ankomst, faktiskAvgang.Ankomst);
            Assert.Equal(forventetAvgang.Reisetid, faktiskAvgang.Reisetid);

            /* Skriver vi tester for alle metodene i BussCtr kan vi sikre oss mot skrivefeil
             * og forsikre oss om at metodene i BussCtr alltid vil returnere riktig verider så lenge 
             * FinnNesteAvgang (BussRepo) fungerer slik den skal */
        }


        private AvgangParam HentAvgangParam()
        {
            return new AvgangParam { StartStopp = "Bergen", SluttStopp = "Vadheim", Dato = "2020-11-20", Tidspunkt = "16:00", AvreiseEtter = true };
        }
        private Avgang HentEnAvgang()
        {
            return new Avgang { AvgangId = 1, Rutenavn = "Fjordekspressen", Linjekode = "NW431", Pris = 100, Avreise = "2020-11-25 17:00", Ankomst = "2020-11-25 18:20", Reisetid = 80 };
        }



        [Fact]
        public async Task FinnNesteAvgang_NullException()
        {
            // Arrange
            var param = new AvgangParam();
            
            mockRepo.Setup(b => b.FinnNesteAvgang(param)).ReturnsAsync(() => null);
            var bussController = new BussController(mockRepo.Object, mockLogCtr.Object);

            // Act
            var resultat = await bussController.FinnNesteAvgang(param) as NotFoundObjectResult;
            
            // Assert
            Assert.Equal("Avgang ikke funnet", resultat.Value);
        }

        [Fact]
        public async Task FinnNesteAvgang_RegEx()
        {
            // Arrange
            var param = new AvgangParam
            {
                StartStopp = "Bergen",
                SluttStopp = "Vadheim",
                Dato = "2020-11-20",
                Tidspunkt = "16:00",
                AvreiseEtter = true
            };
            var forventetAvgang = new Avgang
            {
                AvgangId = 1,
                Rutenavn = "Fjordekspressen",
                Linjekode = "",
                Pris = 100,
                Avreise = "2020-11-25 17:00",
                Ankomst = "2020-11-25 18:20",
                Reisetid = 80
            };

            mockRepo.Setup(b => b.FinnNesteAvgang(param)).ReturnsAsync(forventetAvgang);
            var bussController = new BussController(mockRepo.Object, mockLogCtr.Object);
            bussController.ModelState.AddModelError("Linjekode", "Feil i inputvalideringen på server");
           

            // Act
            var resultat = await bussController.FinnNesteAvgang(param) as BadRequestObjectResult;
          
            // Assert
            Assert.Equal("Feil i inputvalideringen på server", resultat.Value);
        }


        [Fact]
        public async Task FullforOrdre_RiktigeVerdier()
        {
            // Arrange
            var billettype = new List<string>
            {
                "Student",
                "Barn"
            };

            var kundeOrdre = new KundeOrdre
            {
                Epost = "hvrustad@gmail.com",
                StartStopp = "Bergen",
                SluttStopp = "Trondheim",
                Linjekode = "NW431",
                AvgangId = 2,
                Billettyper = billettype
            };

            mockRepo.Setup(br => br.FullforOrdre(kundeOrdre)).ReturnsAsync(true);
            var bussController = new BussController(mockRepo.Object, mockLogCtr.Object);
            // Act
            var resultat = await bussController.FullforOrdre(kundeOrdre) as OkObjectResult;
            // Assert
            Assert.Equal("Ordren ble lagret!", resultat.Value);
        }
    }
}