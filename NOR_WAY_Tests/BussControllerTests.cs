using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NOR_WAY.Controllers;
using NOR_WAY.DAL;
using NOR_WAY.Model;
using Xunit;

namespace NOR_WAY_Tests
{
    public class BussControllerTests
    {
        private readonly Mock<IBussRepository> mockRepo = new Mock<IBussRepository>();
        private readonly Mock<ILogger<BussController>> mockLogCtr = new Mock<ILogger<BussController>>();
        private readonly BussController bussController;

        public BussControllerTests()
        {
            bussController = new BussController(mockRepo.Object, mockLogCtr.Object);
        }


        /* Enhetstester for HentAlleStopp */

        /* Tester at ikke listen med Stopp fra BussRepo endrer seg i controlleren
           for HentAlleStopp() */
        [Fact]
        public async Task HentAlleStopp_RiktigeVerdier()
        {
            // Arrange
            List<Stopp> forventedeStopp = HentStoppListe();
            mockRepo.Setup(b => b.HentAlleStopp()).ReturnsAsync(HentStoppListe());

            // Act
            var resultat = await bussController.HentAlleStopp() as OkObjectResult;
            List<Stopp> faktiskeStopp = (List<Stopp>)resultat.Value;

            // Assert
            Assert.Equal(forventedeStopp.Count, faktiskeStopp.Count);   // Tester om listene er like lange
            // Tester om alle verdiene er like i alle elementene
            for (int i = 0; i < forventedeStopp.Count; i++)
            {
                Assert.Equal(forventedeStopp[i].Id, faktiskeStopp[i].Id);
                Assert.Equal(forventedeStopp[i].Navn, faktiskeStopp[i].Navn);
            }
        }


        /* Enhetstester for FinnMuligeStartStopp */

        /* Tester at ikke listen med Stopp fra BussRepo endrer seg i controlleren
           for FinnMuligeStartStopp() */
        [Fact]
        public async Task FinnMuligStartStopp_RiktigeVerdier()
        {
            // Arrange
            InnStopp innStopp = HentUgyldigInnStopp();
            List<Stopp> forventedeStopp = HentStoppListe();
            mockRepo.Setup(b => b.FinnMuligeStartStopp(innStopp)).ReturnsAsync(HentStoppListe());

            // Act
            var resultat = await bussController.FinnMuligeStartStopp(innStopp) as OkObjectResult;
            List<Stopp> faktiskeStopp = (List<Stopp>)resultat.Value;

            // Assert
            Assert.Equal(forventedeStopp.Count, faktiskeStopp.Count);   // Tester om listene er like lange
            // Tester om alle verdiene er like i alle elementeneƒ
            for (int i = 0; i < forventedeStopp.Count; i++)
            {
                Assert.Equal(forventedeStopp[i].Id, faktiskeStopp[i].Id);
                Assert.Equal(forventedeStopp[i].Navn, faktiskeStopp[i].Navn);
            }
        }

        // Tester at FinnMulgeStartStopp i controlleren håndterer Tom liste
        [Fact]
        public async Task FinnMuligeStartStopp_TomListe()
        {
            // Arrange 
            InnStopp innStopp = HentInnStopp();
            List<Stopp> tomStoppListe = new List<Stopp>();
            mockRepo.Setup(b => b.FinnMuligeStartStopp(innStopp)).ReturnsAsync(tomStoppListe);

            // Act
            var resultat = await bussController.FinnMuligeStartStopp(innStopp) as NotFoundObjectResult;

            // Assert
            Assert.Equal("Ingen mulige StartStopp ble funnet", resultat.Value);
        }

        // Tester at FinnMulgeStartStopp i controlleren håndterer InvalidModelState
        [Fact]
        public async Task FinnMuligeStartStopp_RegEx()
        {
            // Arrange 
            InnStopp innStopp = HentUgyldigInnStopp();
            List<Stopp> forventedeStopp = HentStoppListe();
            mockRepo.Setup(b => b.FinnMuligeStartStopp(innStopp)).ReturnsAsync(forventedeStopp);
            bussController.ModelState.AddModelError("StartStopp", "Feil i inputvalideringen på server");
            bussController.ModelState.AddModelError("SluttStopp", "Feil i inputvalideringen på server");
            bussController.ModelState.AddModelError("Dato", "Feil i inputvalideringen på server");
            bussController.ModelState.AddModelError("Tidspunkt", "Feil i inputvalideringen på server");

            // Act
            var resultat = await bussController.FinnMuligeStartStopp(innStopp) as BadRequestObjectResult;

            // Assert
            Assert.Equal("Feil i inputvalideringen på server", resultat.Value);
        }


        /* Enhetstester for FinnMuligeSluttStopp */

        /* Tester at ikke listen med Stopp fra BussRepo endrer seg i controlleren
           for FinnMuligeSluttStopp() */
        [Fact]
        public async Task FinnMuligSluttStopp_RiktigeVerdier()
        {
            // Arrange
            List<Stopp> forventet = HentStoppListe();
            InnStopp innStopp = new InnStopp { Navn = "Bergen" };
            mockRepo.Setup(b => b.FinnMuligeSluttStopp(innStopp)).ReturnsAsync(HentStoppListe());

            // Act
            var resultat = await bussController.FinnMuligeSluttStopp(innStopp) as OkObjectResult;
            List<Stopp> faktisk = (List<Stopp>)resultat.Value;

            // Assert
            Assert.Equal(forventet.Count, faktisk.Count);   // Tester om listene er like lange
            // Tester om alle verdiene er like i alle elementene
            for (int i = 0; i < forventet.Count; i++)
            {
                Assert.Equal(forventet[i].Id, faktisk[i].Id);
                Assert.Equal(forventet[i].Navn, faktisk[i].Navn);
            }
        }

        // Tester at FinnMulgeStartStopp i controlleren håndterer Tom liste
        [Fact]
        public async Task FinnMuligeSluttStopp_TomListe()
        {
            // Arrange 
            InnStopp innStopp = HentInnStopp();
            List<Stopp> tomStoppListe = new List<Stopp>();
            mockRepo.Setup(b => b.FinnMuligeSluttStopp(innStopp)).ReturnsAsync(tomStoppListe);

            // Act
            var resultat = await bussController.FinnMuligeSluttStopp(innStopp) as NotFoundObjectResult;

            // Assert
            Assert.Equal("Ingen mulige SluttStopp ble funnet", resultat.Value);
        }

        // Tester at FinnMuligeSluttStopp i controlleren håndterer InvalidModelState
        [Fact]
        public async Task FinnMuligeSluttStopp_RegEx()
        {
            // Arrange 
            InnStopp innStopp = HentUgyldigInnStopp();
            List<Stopp> forventedeStopp = HentStoppListe();
            mockRepo.Setup(b => b.FinnMuligeSluttStopp(innStopp)).ReturnsAsync(forventedeStopp);
            bussController.ModelState.AddModelError("StartStopp", "Feil i inputvalideringen på server");
            bussController.ModelState.AddModelError("SluttStopp", "Feil i inputvalideringen på server");
            bussController.ModelState.AddModelError("Dato", "Feil i inputvalideringen på server");
            bussController.ModelState.AddModelError("Tidspunkt", "Feil i inputvalideringen på server");

            // Act
            var resultat = await bussController.FinnMuligeSluttStopp(innStopp) as BadRequestObjectResult;

            // Assert
            Assert.Equal("Feil i inputvalideringen på server", resultat.Value);
        }


        /* Enhetstester for HentAlleBillettyper */

        /* Tester at ikke listen med Stopp fra BussRepo endrer seg i controlleren
           for HentAlleBillettyper() */
        [Fact]
        public async Task HentAlleBillettyper_RiktigeVerdier()
        {
            // Arrange
            List<Billettyper> forventet = HentBillettyperListe();
            mockRepo.Setup(b => b.HentAlleBillettyper()).ReturnsAsync(HentBillettyperListe());

            // Act
            var resultat = await bussController.HentAlleBillettyper() as OkObjectResult;
            List<Billettyper> faktisk = (List<Billettyper>)resultat.Value;

            // Assert
            Assert.Equal(forventet.Count, faktisk.Count);   // Tester om listene er like lange
            // Tester om alle verdiene er like i alle elementene
            for (int i = 0; i < forventet.Count; i++)
            {
                Assert.Equal(forventet[i].Billettype, faktisk[i].Billettype);
                Assert.Equal(forventet[i].Rabattsats, faktisk[i].Rabattsats);
            }
        }


        /* Enhetstester for HentAlleRuter */

        /* Tester at ikke listen med Stopp fra BussRepo endrer seg i controlleren
           for HentAlleRuter() */
        [Fact]
        public async Task HentAlleRuter_RiktigeVerdier()
        {
            // Arrange
            List<RuteData> forventet = HentRuteDataListe();
            mockRepo.Setup(b => b.HentAlleRuter()).ReturnsAsync(HentRuteDataListe());

            // Act
            var resultat = await bussController.HentAlleRuter() as OkObjectResult;
            List<RuteData> faktisk = (List<RuteData>)resultat.Value;

            // Assert
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

        [Fact]
        public async Task HentAlleRuter_Null()
        {
            // Arrange 
            mockRepo.Setup(b => b.HentAlleRuter()).ReturnsAsync(() => null);
           
            //Act
            var resultat = await bussController.HentAlleRuter() as NotFoundObjectResult;
           
            // Assert
            Assert.Equal("Rutene ble ikke funnet", resultat.Value);
        }


        /* Enhetstester for FinnNesteAvgang */

        // Tester at ikke Avgang fra BussRepo endrer seg i controlleren
        [Fact]
        public async Task FinnNesteAvgang_RiktigeVerdier() 
        {
            // Arrange
            AvgangParam param = HentAvgangParam();
            Avgang forventetAvgang = HentAvgang();

            // Act
            mockRepo.Setup(b => b.FinnNesteAvgang(param)).ReturnsAsync(HentAvgang());
            var resultat = await bussController.FinnNesteAvgang(param) as OkObjectResult;
            Avgang faktiskAvgang = (Avgang)resultat.Value;

            // Assert
            Assert.Equal(forventetAvgang.AvgangId, faktiskAvgang.AvgangId);
            Assert.Equal(forventetAvgang.Rutenavn, faktiskAvgang.Rutenavn);
            Assert.Equal(forventetAvgang.Linjekode, faktiskAvgang.Linjekode);
            Assert.Equal(forventetAvgang.Pris, faktiskAvgang.Pris);
            Assert.Equal(forventetAvgang.Avreise, faktiskAvgang.Avreise);
            Assert.Equal(forventetAvgang.Ankomst, faktiskAvgang.Ankomst);
            Assert.Equal(forventetAvgang.Reisetid, faktiskAvgang.Reisetid);
        }

        [Fact]
        public async Task FinnNesteAvgang_Null()
        {
            // Arrange
            var param = new AvgangParam();
            mockRepo.Setup(b => b.FinnNesteAvgang(param)).ReturnsAsync(() => null);

            // Act
            var resultat = await bussController.FinnNesteAvgang(param) as NotFoundObjectResult;
            
            // Assert
            Assert.Equal("Avgang ikke funnet", resultat.Value);
        }

        // Tester at FinnNesteAvgang i controlleren håndterer InvalidModelState
        [Fact]
        public async Task FinnNesteAvgang_RegEx()
        {
            // Arrange
            var param = HentUgyldigAvgangParam();
            var forventetAvgang = HentAvgang();
            mockRepo.Setup(b => b.FinnNesteAvgang(param)).ReturnsAsync(forventetAvgang);
            bussController.ModelState.AddModelError("StartStopp", "Feil i inputvalideringen på server");
            bussController.ModelState.AddModelError("Dato", "Feil i inputvalideringen på server");

            // Act
            var resultat = await bussController.FinnNesteAvgang(param) as BadRequestObjectResult;
          
            // Assert
            Assert.Equal("Feil i inputvalideringen på server", resultat.Value);
        }


        /* Enhetstester for FullforOrdre */ 

        // Tester at FullforOrdre returnerer forventet verdi
        [Fact]
        public async Task FullforOrdre_RiktigeVerdier()
        {
            // Arrange
            var kundeOrdre = HentEnKundeOrdre();
            mockRepo.Setup(br => br.FullforOrdre(kundeOrdre)).ReturnsAsync(true);
           
            // Act
            var resultat = await bussController.FullforOrdre(kundeOrdre) as OkObjectResult;

            // Assert
            Assert.Equal("Ordren ble lagret!", resultat.Value);
        }

        [Fact]
        public async Task FullforOrdre_IkkeOK()
        {
            // Arrange
            var kundeOrdre = HentEnKundeOrdre();
            mockRepo.Setup(br => br.FullforOrdre(kundeOrdre)).ReturnsAsync(false);
           
            // Act
            var resultat = await bussController.FullforOrdre(kundeOrdre) as BadRequestObjectResult;

            // Assert
            Assert.Equal("Ordren kunne ikke lagres!", resultat.Value);
        }

        // Tester at FullforOrdre i controlleren håndterer InvalidModelState
        [Fact]
        public async Task FullforOrdre_RegEx()
        {
            // Arrange
            var kundeOrdre = HentEnKundeOrdre();
            mockRepo.Setup(br => br.FullforOrdre(kundeOrdre)).ReturnsAsync(false);
            bussController.ModelState.AddModelError("Epost", "Feil i inputvalideringen på server");

            // Act
            var resultat = await bussController.FullforOrdre(kundeOrdre) as BadRequestObjectResult;
            
            // Assert
            Assert.Equal("Feil i inputvalidering på server", resultat.Value);
        }


        /* Private metoder som instansierer objekter til brukes i testmetodene */

        // Returnerer et KundeOrdre-objekt
        private KundeOrdre HentEnKundeOrdre()
        {
            return new KundeOrdre { Epost = "hvrustad@gmail.com", StartStopp = "Bergen", SluttStopp = "Trondheim", Linjekode = "NW431", AvgangId = 2, Billettyper = HentBillettyperStringListe() };
        }

        // Returnerer en List med string billettypenavn
        private List<string> HentBillettyperStringListe()
        {
            return new List<string> { "Student", "Barn" };
        }

        // Returnerer et AvgangParam-objekt
        private AvgangParam HentAvgangParam()
        {
            return new AvgangParam { StartStopp = "Bergen", SluttStopp = "Vadheim", Dato = "2020-11-20", Tidspunkt = "16:00", AvreiseEtter = true };
        }

        private AvgangParam HentUgyldigAvgangParam()
        {
            return new AvgangParam { StartStopp = "", SluttStopp = "Vadheim", Dato = "", Tidspunkt = "16:00", AvreiseEtter = true };
        }

        // Returnerer et Avgang-objekt
        private Avgang HentAvgang()
        {
            return new Avgang { AvgangId = 1, Rutenavn = "Fjordekspressen", Linjekode = "NW431", Pris = 100, Avreise = "2020-11-25 17:00", Ankomst = "2020-11-25 18:20", Reisetid = 80 };
        }

        // Returnerer en List med Stopp-objekter
        private List<Stopp> HentStoppListe()
        {
            Stopp stopp1 = new Stopp { Id = 1, Navn = "Bergen" };
            Stopp stopp2 = new Stopp { Id = 1, Navn = "Oslo" };
            Stopp stopp3 = new Stopp { Id = 1, Navn = "Vadheim" };
            Stopp stopp4 = new Stopp { Id = 1, Navn = "Trondheim" };
            return new List<Stopp> { stopp1, stopp2, stopp3, stopp4 };
        }

        // Returnerer en List med Billettyper-objekter
        private List<Billettyper> HentBillettyperListe()
        {
            Billettyper billettype1 = new Billettyper { Billettype = "Student", Rabattsats = 50 };
            Billettyper billettype2 = new Billettyper { Billettype = "Voksen", Rabattsats = 0 };
            Billettyper billettype3 = new Billettyper { Billettype = "Honør", Rabattsats = 25 };
            return new List<Billettyper> { billettype1, billettype2, billettype3 };
        }

        // Returnerer en List med RuteData-objekter
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

        private InnStopp HentInnStopp()
        {
            return new InnStopp { Navn = "Bergen" };
        }

        // Returnerer et InnStopp-objekt med ugyldig Navn
        private InnStopp HentUgyldigInnStopp()
        {
            return new InnStopp { Navn = "" };
        }
    }
}