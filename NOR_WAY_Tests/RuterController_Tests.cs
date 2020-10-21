using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NOR_WAY.Controllers;
using NOR_WAY.DAL;
using NOR_WAY.DAL.Interfaces;
using NOR_WAY.Model;
using Xunit;

namespace NOR_WAY_Tests
{
    public class RuterController_Tests
    {
        private readonly Mock<IRuterRepository> mockRepo = new Mock<IRuterRepository>();
        private readonly Mock<ILogger<RuterController>> mockLogCtr = new Mock<ILogger<RuterController>>();
        private readonly RuterController ruterController;

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();

        private const string _innlogget = "Innlogget";
        private const string _ikkeInnlogget = "";


        public RuterController_Tests()
        {
            ruterController = new RuterController(mockRepo.Object, mockLogCtr.Object);
        }

        /* Enhetstester for HentRuterMedStop */

        /* Tester at ikke listen med Stopp fra BussRepo endrer seg i controlleren
           for HentRuterMedStopp() */

        [Fact]
        public async Task HentRuterMedStopp_RiktigeVerdier()
        {
            // Arrange
            List<RuteMedStopp> forventet = HentRuteMedStoppListe();
            mockRepo.Setup(b => b.HentRuterMedStopp()).ReturnsAsync(HentRuteMedStoppListe());

            // Act
            var resultat = await ruterController.HentRuterMedStopp() as OkObjectResult;
            List<RuteMedStopp> faktisk = (List<RuteMedStopp>)resultat.Value;

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
        public async Task HentRuterMedStopp_Null()
        {
            // Arrange
            mockRepo.Setup(b => b.HentRuterMedStopp()).ReturnsAsync(() => null);

            //Act
            var resultat = await ruterController.HentRuterMedStopp() as NotFoundObjectResult;

            // Assert
            Assert.Equal("Rutene ble ikke funnet", resultat.Value);
        }

        /* Enhetstester for HentAlleRuter */

        /* Tester at ikke listen fra BussRepo endrer seg i controlleren
           for HentAlleRuter() */

        [Fact]
        public async Task HentAlleRuter_RiktigeVerdier()
        {
            // Arrange
            List<Ruter> forventet = HentRuterListe();
            mockRepo.Setup(b => b.HentAlleRuter()).ReturnsAsync(HentRuterListe());

            // Act
            var resultat = await ruterController.HentAlleRuter() as OkObjectResult;
            List<Ruter> faktisk = (List<Ruter>)resultat.Value;

            // Assert
            Assert.Equal(forventet.Count, faktisk.Count);   // Tester om listene er like lange
            // Tester om alle verdiene er like i alle elementene
            for (int i = 0; i < forventet.Count; i++)
            {
                Assert.Equal(forventet[i].Linjekode, faktisk[i].Linjekode);
                Assert.Equal(forventet[i].Rutenavn, faktisk[i].Rutenavn);
                Assert.Equal(forventet[i].Startpris, faktisk[i].Startpris);
                Assert.Equal(forventet[i].TilleggPerStopp, faktisk[i].TilleggPerStopp);
                Assert.Equal(forventet[i].Kapasitet, faktisk[i].Kapasitet);
            }
        }

        [Fact]
        public async Task HentAlleRuter_Null()
        {
            // Arrange
            mockRepo.Setup(b => b.HentAlleRuter()).ReturnsAsync(() => null);

            //Act
            var resultat = await ruterController.HentAlleRuter() as NotFoundObjectResult;

            // Assert
            Assert.Equal("Rutene ble ikke funnet", resultat.Value);
        }

        // Tester at FullforOrdre returnerer forventet verdi
        [Fact]
        public async Task FjernRute_RiktigeVerdier()
        {
            // Arrange
            string linjekode = "NW431";
            mockRepo.Setup(br => br.FjernRute(linjekode)).ReturnsAsync(true);
            MockSession(_innlogget);

            // Act
            var resultat = await ruterController.FjernRute(linjekode) as OkObjectResult;
            string fjernetRute = (string) resultat.Value;
            // Assert
            Assert.Equal($"Ruten med linjekode: {linjekode}, ble slettet", fjernetRute);
        }

        [Fact]
        public async Task FjernRute_IkkeOK()
        {
            // Arrange
            string linjekode = "NW431";
            mockRepo.Setup(br => br.FjernRute(linjekode)).ReturnsAsync(false);
            MockSession(_innlogget);

            // Act
            var resultat = await ruterController.FjernRute(linjekode) as BadRequestObjectResult;

            // Assert
            Assert.Equal($"Ruten med linjekode: {linjekode}, kunne ikke slettes", resultat.Value);
        }

        // Tester at FullforOrdre i controlleren håndterer InvalidModelState
        [Fact]
        public async Task FjernRute_RegEx()
        {
            // Arrange
            string linjekode = "";
            mockRepo.Setup(br => br.FjernRute(linjekode)).ReturnsAsync(false);
            MockSession(_innlogget);
            ruterController.ModelState.AddModelError("Linjekode", "Feil i inputvalideringen på server");

            // Act
            var resultat = await ruterController.FjernRute(linjekode) as BadRequestObjectResult;

            // Assert
            Assert.Equal("Feil i inputvalideringen på server", resultat.Value);
        }

        // Returnerer en List med RuteMedStopp-objekter
        private List<RuteMedStopp> HentRuteMedStoppListe()
        {
            List<string> stoppene1 = new List<string> { "Bergen", "Vaheim", "Trondheim" };
            List<string> stoppene2 = new List<string> { "Oslo", "Røros", "Trondheim" };
            List<string> stoppene3 = new List<string> { "Kristiansand", "Stavanger", "Molde" };
            List<int> minuttListe1 = new List<int> { 20, 25, 35 };
            List<int> minuttListe2 = new List<int> { 20, 23, 35 };
            List<int> minuttListe3 = new List<int> { 20, 55, 60 };
            RuteMedStopp RuteMedStopp1 = new RuteMedStopp { Stoppene = stoppene1, MinutterTilNesteStopp = minuttListe1, Linjekode = "NW123", Rutenavn = "Bussturen", Startpris = 79, TilleggPerStopp = 25 };
            RuteMedStopp RuteMedStopp2 = new RuteMedStopp { Stoppene = stoppene2, MinutterTilNesteStopp = minuttListe2, Linjekode = "NW600", Rutenavn = "Ekspressruta", Startpris = 100, TilleggPerStopp = 15 };
            RuteMedStopp RuteMedStopp3 = new RuteMedStopp { Stoppene = stoppene3, MinutterTilNesteStopp = minuttListe3, Linjekode = "NW007", Rutenavn = "Bondespressen", Startpris = 50, TilleggPerStopp = 35 };
            return new List<RuteMedStopp> { RuteMedStopp1, RuteMedStopp2, RuteMedStopp3 };
        }

        // Returnerer en List med Ruter-objekter
        private List<Ruter> HentRuterListe()
        {
            var rute1 = new Ruter() { Linjekode = "NW431", Rutenavn = "Fjordekspressen", Startpris = 79, TilleggPerStopp = 30, Kapasitet = 55 };
            var rute2 = new Ruter() { Linjekode = "NW194", Rutenavn = "Grenlandsekspressen", Startpris = 50, TilleggPerStopp = 35, Kapasitet = 45 };
            var rute3 = new Ruter() { Linjekode = "NW180", Rutenavn = "Haukeliekspressen", Startpris = 149, TilleggPerStopp = 20, Kapasitet = 65 };
            return new List<Ruter> { rute1, rute2, rute3 };
        }

        private void MockSession(string innlogging)
        {
            mockSession[_innlogget] = innlogging;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            ruterController.ControllerContext.HttpContext = mockHttpContext.Object;
        }
    }
}