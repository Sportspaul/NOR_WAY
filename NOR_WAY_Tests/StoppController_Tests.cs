using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NOR_WAY.Controllers;
using NOR_WAY.DAL;
using NOR_WAY.DAL.Interfaces;
using NOR_WAY.Model;
using Xunit;
using System.Net;

namespace NOR_WAY_Tests
{
    public class StoppController_Tests
    {
        private readonly Mock<IStoppRepository> mockRepo = new Mock<IStoppRepository>();
        private readonly Mock<ILogger<StoppController>> mockLogCtr = new Mock<ILogger<StoppController>>();
        private readonly StoppController stoppController;

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();

        private const string _innlogget = "Innlogget";
        private const string _ikkeInnlogget = "";


        public StoppController_Tests()
        {
            stoppController = new StoppController(mockRepo.Object, mockLogCtr.Object);
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
            var resultat = await stoppController.HentAlleStopp() as OkObjectResult;
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

        [Fact]
        public async Task HentAlleStopp_Null()
        {
            // Arrange
            mockRepo.Setup(s => s.HentAlleStopp()).ReturnsAsync(() => null);

            // Act
            var resultat = await stoppController.HentAlleStopp() as NotFoundObjectResult;

            // Assert
            Assert.Equal("Ingen stopp ble funnet", resultat.Value);
        }



        /* Enhetstester for FinnMuligeStartStopp */

        /* Tester at ikke listen med Stopp fra BussRepo endrer seg i controlleren
           for FinnMuligeStartStopp() */

        [Fact]
        public async Task FinnMuligStartStopp_RiktigeVerdier()
        {
            // Arrange
            StoppModel StoppModel = HentUgyldigInnStopp();
            List<Stopp> forventedeStopp = HentStoppListe();
            mockRepo.Setup(b => b.FinnMuligeStartStopp(StoppModel)).ReturnsAsync(HentStoppListe());

            // Act
            var resultat = await stoppController.FinnMuligeStartStopp(StoppModel) as OkObjectResult;
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
            StoppModel StoppModel = HentInnStopp();
            List<Stopp> tomStoppListe = new List<Stopp>();
            mockRepo.Setup(b => b.FinnMuligeStartStopp(StoppModel)).ReturnsAsync(tomStoppListe);

            // Act
            var resultat = await stoppController.FinnMuligeStartStopp(StoppModel) as NotFoundObjectResult;

            // Assert
            Assert.Equal($"Ingen mulige StartStopp ble funnet for SluttStopp: {StoppModel.Navn}", resultat.Value);
        }

        // Tester at FinnMulgeStartStopp i controlleren håndterer InvalidModelState
        [Fact]
        public async Task FinnMuligeStartStopp_RegEx()
        {
            // Arrange
            StoppModel StoppModel = HentUgyldigInnStopp();
            List<Stopp> forventedeStopp = HentStoppListe();
            mockRepo.Setup(b => b.FinnMuligeStartStopp(StoppModel)).ReturnsAsync(forventedeStopp);
            stoppController.ModelState.AddModelError("StartStopp", "Feil i inputvalideringen på server");
            stoppController.ModelState.AddModelError("SluttStopp", "Feil i inputvalideringen på server");
            stoppController.ModelState.AddModelError("Dato", "Feil i inputvalideringen på server");
            stoppController.ModelState.AddModelError("Tidspunkt", "Feil i inputvalideringen på server");

            // Act
            var resultat = await stoppController.FinnMuligeStartStopp(StoppModel) as BadRequestObjectResult;

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
            StoppModel StoppModel = new StoppModel { Navn = "Bergen" };
            mockRepo.Setup(b => b.FinnMuligeSluttStopp(StoppModel)).ReturnsAsync(HentStoppListe());

            // Act
            var resultat = await stoppController.FinnMuligeSluttStopp(StoppModel) as OkObjectResult;
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
            StoppModel StoppModel = HentInnStopp();
            List<Stopp> tomStoppListe = new List<Stopp>();
            mockRepo.Setup(b => b.FinnMuligeSluttStopp(StoppModel)).ReturnsAsync(tomStoppListe);

            // Act
            var resultat = await stoppController.FinnMuligeSluttStopp(StoppModel) as NotFoundObjectResult;

            // Assert
            Assert.Equal($"Ingen mulige SluttStopp ble funnet for StartStopp: {StoppModel.Navn }", resultat.Value);
        }

        // Tester at FinnMuligeSluttStopp i controlleren håndterer InvalidModelState
        [Fact]
        public async Task FinnMuligeSluttStopp_RegEx()
        {
            // Arrange
            StoppModel StoppModel = HentUgyldigInnStopp();
            List<Stopp> forventedeStopp = HentStoppListe();
            mockRepo.Setup(b => b.FinnMuligeSluttStopp(StoppModel)).ReturnsAsync(forventedeStopp);
            stoppController.ModelState.AddModelError("StartStopp", "Feil i inputvalideringen på server");
            stoppController.ModelState.AddModelError("SluttStopp", "Feil i inputvalideringen på server");
            stoppController.ModelState.AddModelError("Dato", "Feil i inputvalideringen på server");
            stoppController.ModelState.AddModelError("Tidspunkt", "Feil i inputvalideringen på server");

            // Act
            var resultat = await stoppController.FinnMuligeSluttStopp(StoppModel) as BadRequestObjectResult;

            // Assert
            Assert.Equal("Feil i inputvalideringen på server", resultat.Value);
        }

        [Fact]
        public async Task OppdaterStoppnavn_Ok()
        {
            //arrange
            Stopp stopp = new Stopp { Id = 1, Navn = "Oslo" };
            mockRepo.Setup(s => s.OppdaterStoppnavn(stopp)).ReturnsAsync(true);

            MockSession(_innlogget);
            //act
            var resultat = await stoppController.OppdaterStoppnavn(stopp) as OkObjectResult;
            //assert
            Assert.Equal((int) HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal($"Endring av Stoppnavn ble utført med verdiene: { stopp}", resultat.Value);
        }

        [Fact]
        public async Task OppdaterStoppnavn_IkkeTilgang()
        {
            //arrange
            Stopp stopp = new Stopp { Id = 1, Navn = "Oslo" };
            mockRepo.Setup(s => s.OppdaterStoppnavn(stopp)).ReturnsAsync(true);

            MockSession(_ikkeInnlogget);
            //act
            var resultat = await stoppController.OppdaterStoppnavn(stopp) as UnauthorizedObjectResult;
            //assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
        }



        [Fact]
        public async Task OppdaterStoppnavn_Feil()
        {
            //arrange
            Stopp stopp = new Stopp { Id = 1, Navn = "Oslo" };
            mockRepo.Setup(s => s.OppdaterStoppnavn(stopp)).ReturnsAsync(false);

            MockSession(_innlogget);
            //act
            var resultat = await stoppController.OppdaterStoppnavn(stopp) as BadRequestObjectResult;
            //assert
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal($"Endring av Stoppnavn kunne ikke utføres med verdiene: { stopp}", resultat.Value);
        }

        [Fact]
        public async Task OppdaterStoppnavn_Regex()
        {
            //arrange
            Stopp stopp = new Stopp { Id = 1, Navn = "Ox" };
            mockRepo.Setup(s => s.OppdaterStoppnavn(stopp)).ReturnsAsync(true);
            MockSession(_innlogget);
            stoppController.ModelState.AddModelError("Navn", "Feil i inputvalideringen på server");
            //act
            var resultat = await stoppController.OppdaterStoppnavn(stopp) as BadRequestObjectResult;
            //assert
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal("Feil i inputvalideringen på server", resultat.Value);
        }


        [Fact]
        public async Task HentAlleStoppMedRuter_RiktigeVerdier()
        {
            // Arrange
            List<StoppMedLinjekoder> forventedeStopp = HentStoppMedLinjekoderListe();
            mockRepo.Setup(b => b.HentAlleStoppMedRuter()).ReturnsAsync(HentStoppMedLinjekoderListe());
            MockSession(_innlogget);
            // Act
            var resultat = await stoppController.HentAlleStoppMedRuter() as OkObjectResult;
            List<StoppMedLinjekoder> faktiskeStopp = (List<StoppMedLinjekoder>) resultat.Value;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            for (int i = 0; i < faktiskeStopp.Count; i++)
            {
                Assert.Equal(forventedeStopp[i].Linjekoder, faktiskeStopp[i].Linjekoder);
                Assert.Equal(forventedeStopp[i].Id, faktiskeStopp[i].Id);
                Assert.Equal(forventedeStopp[i].Stoppnavn, faktiskeStopp[i].Stoppnavn);
            }
        }

        [Fact]
        public async Task HentAlleStoppMedRuter_IkkeTilgang()
        {
            // Arrange
            List<StoppMedLinjekoder> forventedeStopp = HentStoppMedLinjekoderListe();
            mockRepo.Setup(b => b.HentAlleStoppMedRuter()).ReturnsAsync(HentStoppMedLinjekoderListe());
            MockSession(_ikkeInnlogget);
            // Act
            var resultat = await stoppController.HentAlleStoppMedRuter() as UnauthorizedObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
        }


        [Fact]
        public async Task HentAlleStoppMedRuter_Null()
        {
            // Arrange
            mockRepo.Setup(b => b.HentAlleStoppMedRuter()).ReturnsAsync(() => null);
            MockSession(_innlogget);
            // Act
            var resultat = await stoppController.HentAlleStoppMedRuter() as BadRequestObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal("Ingen Stopp ble funnet", resultat.Value);
        }


        [Fact]
        public async Task HentEtStopp_Riktig()
        {
            // Arrange
            Stopp forventetStopp = new Stopp { Id = 1, Navn= "Oslo" };
            mockRepo.Setup(b => b.HentEtStopp(forventetStopp.Id)).ReturnsAsync(forventetStopp);
            MockSession(_innlogget);
            // Act
            var resultat = await stoppController.HentEtStopp(forventetStopp.Id) as OkObjectResult;
            Stopp faktiskStopp = (Stopp) resultat.Value;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal(forventetStopp, faktiskStopp);
        }

        [Fact]
        public async Task HentEtStopp_Feil()
        {
            // Arrange
            Stopp forventetStopp = new Stopp { Id = 1, Navn = "Oslo" };
            mockRepo.Setup(b => b.HentEtStopp(forventetStopp.Id)).ReturnsAsync(() => null);
            MockSession(_innlogget);
            // Act
            var resultat = await stoppController.HentEtStopp(forventetStopp.Id) as NotFoundObjectResult;
            
            // Assert
            Assert.Equal((int)HttpStatusCode.NotFound, resultat.StatusCode);
            Assert.Equal("Stoppet ble ikke funnet", resultat.Value);
        }

        [Fact]
        public async Task HentEtStopp_Regex()
        {
            // Arrange
            Stopp forventetStopp = new Stopp { Id = 1, Navn = "34" };
            mockRepo.Setup(b => b.HentEtStopp(forventetStopp.Id)).ReturnsAsync(forventetStopp);
            stoppController.ModelState.AddModelError("Navn", "Feil i inputvalidering på server");
            MockSession(_innlogget);

            // Act
            var resultat = await stoppController.HentEtStopp(forventetStopp.Id) as BadRequestObjectResult;
           
            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal("Feil i inputvalidering på server", resultat.Value);
        }


        [Fact]
        public async Task HentEtStopp_IkkeTilgang()
        {
            // Arrange
            Stopp forventetStopp = new Stopp { Id = 1, Navn = "Oslo" };
            mockRepo.Setup(b => b.HentEtStopp(forventetStopp.Id)).ReturnsAsync(forventetStopp);
            MockSession(_ikkeInnlogget);

            // Act
            var resultat = await stoppController.HentEtStopp(forventetStopp.Id) as UnauthorizedObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
        }

        /*
         * Hjelpemetoder
         */
        // Returnerer en List med Stopp-objekter
        private List<Stopp> HentStoppListe()
        {
            Stopp stopp1 = new Stopp { Id = 1, Navn = "Bergen" };
            Stopp stopp2 = new Stopp { Id = 1, Navn = "Oslo" };
            Stopp stopp3 = new Stopp { Id = 1, Navn = "Vadheim" };
            Stopp stopp4 = new Stopp { Id = 1, Navn = "Trondheim" };
            return new List<Stopp> { stopp1, stopp2, stopp3, stopp4 };
        }

        private List<StoppMedLinjekoder> HentStoppMedLinjekoderListe()
        {
            StoppMedLinjekoder stopp1 = new StoppMedLinjekoder { Id = 1, Stoppnavn = "Bergen", Linjekoder ="NW1"};
            StoppMedLinjekoder stopp2 = new StoppMedLinjekoder { Id = 1, Stoppnavn = "Oslo", Linjekoder ="NW3" };
            StoppMedLinjekoder stopp3 = new StoppMedLinjekoder { Id = 1, Stoppnavn = "Vadheim", Linjekoder ="NW1"};
            StoppMedLinjekoder stopp4 = new StoppMedLinjekoder { Id = 1, Stoppnavn = "Trondheim", Linjekoder ="NW3" };
            return new List<StoppMedLinjekoder> { stopp1, stopp2, stopp3, stopp4 };
        }

        private StoppModel HentInnStopp()
        {
            return new StoppModel { Navn = "Bergen" };
        }

        // Returnerer et StoppModel-objekt med ugyldig Navn
        private StoppModel HentUgyldigInnStopp()
        {
            return new StoppModel { Navn = "" };
        }

        private void MockSession(string innlogging)
        {
            mockSession[_innlogget] = innlogging;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            stoppController.ControllerContext.HttpContext = mockHttpContext.Object;
        }
    }
}