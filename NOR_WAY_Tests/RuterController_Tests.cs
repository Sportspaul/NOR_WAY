using System.Collections.Generic;
using System.Net;
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


        //Adminmetoder som krever innlogging
        /* Enhetstester for HentAlleRuter */

        /* Tester at ikke listen fra BussRepo endrer seg i controlleren
           for HentAlleRuter() */

        [Fact]
        public async Task HentAlleRuter_RiktigeVerdier()
        {
            // Arrange
            List<Ruter> forventet = HentRuterListe();
            mockRepo.Setup(b => b.HentAlleRuter()).ReturnsAsync(HentRuterListe());
            MockSession(_innlogget);

            // Act
            var resultat = await ruterController.HentAlleRuter() as OkObjectResult;
            List<Ruter> faktisk = (List<Ruter>)resultat.Value;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
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
        public async Task HentAlleRuter_IkkeTilgang()
        {
            // Arrange
            List<Ruter> forventet = HentRuterListe();
            mockRepo.Setup(b => b.HentAlleRuter()).ReturnsAsync(HentRuterListe());
            MockSession(_ikkeInnlogget);

            // Act
            var resultat = await ruterController.HentAlleRuter() as UnauthorizedObjectResult;

            // Assert
            Assert.Equal((int) HttpStatusCode.Unauthorized, resultat.StatusCode); 
            Assert.Equal("Ikke innlogget", resultat.Value);
               
        }

        [Fact]
        public async Task HentAlleRuter_Null()
        {
            // Arrange
            mockRepo.Setup(b => b.HentAlleRuter()).ReturnsAsync(() => null);
            MockSession(_innlogget);

            //Act
            var resultat = await ruterController.HentAlleRuter() as NotFoundObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.NotFound, resultat.StatusCode);
            Assert.Equal("Rutene ble ikke funnet", resultat.Value);
        }

        [Fact]
        public async Task HentEnRute_RiktigeVerdier()
        {
            // Arrange
            string linjekode = "NW431";
            Ruter enRute = HentRuterListe()[0];
            mockRepo.Setup(br => br.HentEnRute(linjekode)).ReturnsAsync(enRute);
            MockSession(_innlogget);

            // Act
            var resultat = await ruterController.HentEnRute(linjekode) as OkObjectResult;
            Ruter forventetRute = (Ruter) resultat.Value;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal($"{enRute}", forventetRute.ToString());
        }

        [Fact]
        public async Task HentEnRute_IkkeTilgang()
        {
            // Arrange
            string linjekode = "NW431";
            Ruter enRute = HentRuterListe()[0];
            mockRepo.Setup(br => br.HentEnRute(linjekode)).ReturnsAsync(enRute);
            MockSession(_ikkeInnlogget);

            // Act
            var resultat = await ruterController.HentEnRute(linjekode) as UnauthorizedObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke innlogget", resultat.Value);
        }

        [Fact]
        public async Task HentEnRute_IkkeOK()
        {
            // Arrange
            string linjekode = "NW431";
            mockRepo.Setup(br => br.HentEnRute(linjekode)).ReturnsAsync(() => null);
            MockSession(_innlogget);

            // Act
            var resultat = await ruterController.HentEnRute(linjekode) as NotFoundObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.NotFound, resultat.StatusCode);
            Assert.Equal("Ruten ble ikke funnet", resultat.Value);
        }

        [Fact]
        public async Task HentEnRute_RegEx()
        {
            // Arrange
            string linjekode = "";
            Ruter enRute = HentRuterListe()[0];
            mockRepo.Setup(br => br.HentEnRute(linjekode)).ReturnsAsync(enRute);
            MockSession(_innlogget);
            ruterController.ModelState.AddModelError("Linjekode", "Feil i inputvalideringen på server");

            // Act
            var resultat = await ruterController.HentEnRute(linjekode) as BadRequestObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal("Feil i inputvalideringen på server", resultat.Value);
        }


        [Fact]
        public async Task FjernRute_RiktigeVerdier()
        {
            // Arrange
            string linjekode = "NW431";
            mockRepo.Setup(br => br.FjernRute(linjekode)).ReturnsAsync(true);
            MockSession(_innlogget);

            // Act
            var resultat = await ruterController.FjernRute(linjekode) as OkObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal($"Ruten med linjekode: {linjekode}, ble slettet", resultat.Value);
        }

        [Fact]
        public async Task FjernRute_IkkeTilgang()
        {
            // Arrange
            string linjekode = "NW431";
            mockRepo.Setup(br => br.FjernRute(linjekode)).ReturnsAsync(true);
            MockSession(_ikkeInnlogget);

            // Act
            var resultat = await ruterController.FjernRute(linjekode) as UnauthorizedObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke innlogget", resultat.Value);
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
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal($"Ruten med linjekode: {linjekode}, kunne ikke slettes", resultat.Value);
        }

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
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal("Feil i inputvalideringen på server", resultat.Value);
        }

        [Fact]
        public async Task OppdaterRute_RiktigeVerdier()
        {
            // Arrange
            Ruter enRute = HentRuterListe()[0];
            mockRepo.Setup(br => br.OppdaterRute(enRute)).ReturnsAsync(true);
            MockSession(_innlogget);

            // Act
            var resultat = await ruterController.OppdaterRute(enRute) as OkObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal($"Endring av Ruten med linjekode: {enRute.Linjekode}, " +
                    $"ble utfør med verdiene: {enRute}", resultat.Value);
        }

        [Fact]
        public async Task OppdaterRute_IkkeTilgang()
        {
            // Arrange
            Ruter enRute = HentRuterListe()[0];
            mockRepo.Setup(br => br.OppdaterRute(enRute)).ReturnsAsync(true);
            MockSession(_ikkeInnlogget);

            // Act
            var resultat = await ruterController.OppdaterRute(enRute) as UnauthorizedObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke innlogget", resultat.Value);
        }

        [Fact]
        public async Task OppdaterRute_IkkeOK()
        {
            // Arrange
            Ruter enRute = HentRuterListe()[0];
            mockRepo.Setup(br => br.OppdaterRute(enRute)).ReturnsAsync(false);
            MockSession(_innlogget);

            // Act
            var resultat = await ruterController.OppdaterRute(enRute) as NotFoundObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.NotFound, resultat.StatusCode);
            Assert.Equal($"Endringen av Ruten med linjekode: {enRute.Linjekode}, " +
                        $"kunne ikke utføres med verdiene: {enRute}", resultat.Value);
        }

        [Fact]
        public async Task OppdaterRute_RegEx()
        {
            // Arrange
            Ruter enRute = HentRuterListe()[0];
            mockRepo.Setup(br => br.OppdaterRute(enRute)).ReturnsAsync(true);
            MockSession(_innlogget);
            ruterController.ModelState.AddModelError("Linjekode", "Feil i inputvalideringen på server");

            // Act
            var resultat = await ruterController.OppdaterRute(enRute) as BadRequestObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal("Feil i inputvalideringen på server", resultat.Value);
        }

        [Fact]
        public async Task NyRute_RiktigeVerdier()
        {
            // Arrange
            Ruter enRute = HentRuterListe()[0];
            mockRepo.Setup(br => br.NyRute(enRute)).ReturnsAsync(true);
            MockSession(_innlogget);

            // Act
            var resultat = await ruterController.NyRute(enRute) as OkObjectResult;
         
            // Assert
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal($"Ny Rute ble lagres med verdiene: {enRute}", resultat.Value);
        }

        [Fact]
        public async Task NyRute_IkkeTilgang()
        {
            // Arrange
            Ruter enRute = HentRuterListe()[0];
            mockRepo.Setup(br => br.NyRute(enRute)).ReturnsAsync(true);
            MockSession(_ikkeInnlogget);

            // Act
            var resultat = await ruterController.NyRute(enRute) as UnauthorizedObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke innlogget", resultat.Value);
        }

        [Fact]
        public async Task NyRute_IkkeOK()
        {
            // Arrange
            Ruter enRute = HentRuterListe()[0];
            mockRepo.Setup(br => br.NyRute(enRute)).ReturnsAsync(false);
            MockSession(_innlogget);

            // Act
            var resultat = await ruterController.NyRute(enRute) as BadRequestObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal($"Ny Rute kunne ikke lagres med verdiene: {enRute}", resultat.Value);
        }

        [Fact]
        public async Task NyRute_RegEx()
        {
            // Arrange
            Ruter enRute = HentRuterListe()[0];
            mockRepo.Setup(br => br.NyRute(enRute)).ReturnsAsync(true);
            MockSession(_innlogget);
            ruterController.ModelState.AddModelError("Linjekode", "Feil i inputvalideringen på server");

            // Act
            var resultat = await ruterController.NyRute(enRute) as BadRequestObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal("Feil i inputvalideringen på server", resultat.Value);
        }



        //Hjelpemetoder

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