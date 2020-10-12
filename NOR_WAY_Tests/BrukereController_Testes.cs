﻿using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NOR_WAY.Controllers;
using NOR_WAY.DAL.Interfaces;
using NOR_WAY.Model;
using Xunit;

namespace NOR_WAY_Tests
{
    public class BrukereController_Tests
    {
        private const string _innlogget = "Innlogget";
        private const string _ikkeInnlogget = "";

        private readonly Mock<IBrukereRepository> mockRepo = new Mock<IBrukereRepository>();
        private readonly Mock<ILogger<BrukereController>> mockLog = new Mock<ILogger<BrukereController>>();
        private readonly BrukereController brukereController;

        // Session
        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();

        public BrukereController_Tests()
        {
            brukereController = new BrukereController(mockRepo.Object, mockLog.Object);
        }

        /* Enhetstester for LoggInn */

        // Tester at LoggInn returnerer forventet verdi
        [Fact]
        public async Task LoggInn_RiktigeVerdier()
        {
            // Arrange
            var bruker = HentEnBrukerModel();
            mockRepo.Setup(br => br.LoggInn(bruker)).ReturnsAsync(true);

            // Act
            mockSession[_innlogget] = _innlogget;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            brukereController.ControllerContext.HttpContext = mockHttpContext.Object;
            var resultat = await brukereController.LoggInn(bruker) as OkObjectResult;

            // Assert
            Assert.Equal(true, resultat.Value);
        }


        [Fact]
        public async Task LoggInn_IkkeOK()
        {
            // Arrange
            var bruker = HentEnBrukerModel();
            mockRepo.Setup(br => br.LoggInn(bruker)).ReturnsAsync(false);

            // Act
            mockSession[_innlogget] = _innlogget;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            brukereController.ControllerContext.HttpContext = mockHttpContext.Object;
            var resultat = await brukereController.LoggInn(bruker) as BadRequestObjectResult;

            // Assert
            Assert.Equal("Innlogging feilet for bruker: " + bruker.Brukernavn, resultat.Value);
        }

        // Tester at LoggInn i controlleren håndterer InvalidModelState
        [Fact]
        public async Task LoggInn_RegEx()
        {
            // Arrange
            BrukerModel bruker = HentEnBrukerModel();
            mockRepo.Setup(br => br.LoggInn(bruker)).ReturnsAsync(false);
            brukereController.ModelState.AddModelError("Brukernavn", "Feil i inputvalideringen på server");
            brukereController.ModelState.AddModelError("Passord", "Feil i inputvalideringen på server");

            // Act
            var resultat = await brukereController.LoggInn(bruker) as BadRequestObjectResult;

            // Assert
            Assert.Equal("Feil i inputvalidering på server", resultat.Value);
        }

        // Test på at NyAdmin returnerer forventet verdi
        [Fact]
        public async Task NyAdmin_RiktigeVerider()
        {
            // Arrange
            var bruker = HentEnBrukerModel();
            mockRepo.Setup(br => br.NyAdmin(bruker)).ReturnsAsync(true);

            // Act
            mockSession[_innlogget] = _innlogget;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            brukereController.ControllerContext.HttpContext = mockHttpContext.Object;
            var resultat = await brukereController.NyAdmin(bruker) as OkObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal("Ny bruker ble lagret", resultat.Value);
        }

        // Test på at NyAdmin håndterer returnverdi: false på korrekt måte
        [Fact]
        public async Task NyAdmin_IkkeOK()
        {
            // Arrange
            var bruker = HentEnBrukerModel();
            mockRepo.Setup(br => br.NyAdmin(bruker)).ReturnsAsync(false);

            // Act
            mockSession[_innlogget] = _innlogget;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            brukereController.ControllerContext.HttpContext = mockHttpContext.Object;
            var resultat = await brukereController.NyAdmin(bruker) as BadRequestObjectResult;

            // Assert
            Assert.Equal("Ny bruker kunne ikke lagres", resultat.Value);
        }

        // Tester på at NyAdmin i controlleren håndterer InvalidModelState
        [Fact]
        public async Task NyAdmin_RegEx()
        {
            // Arrange
            var bruker = HentEnBrukerModel();
            mockRepo.Setup(br => br.NyAdmin(bruker)).ReturnsAsync(false);
            brukereController.ModelState.AddModelError("Brukernavn", "Feil i inputvalideringen på server");
            brukereController.ModelState.AddModelError("Passord", "Feil i inputvalideringen på server");

            // Act
            mockSession[_innlogget] = _innlogget;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            brukereController.ControllerContext.HttpContext = mockHttpContext.Object;
            var resultat = await brukereController.NyAdmin(bruker) as BadRequestObjectResult;

            // Assert
            Assert.Equal("Feil i inputvalidering på server", resultat.Value);
        }

        // Test på at NyAdmin håndterer tilfelle hvor bruker ikke er logget inn
        [Fact]
        public async Task NyAdmin_IkkeInnlogget() {
            // Arrange
            var bruker = HentEnBrukerModel();
            mockRepo.Setup(br => br.NyAdmin(bruker)).ReturnsAsync(true);

            // Act
            mockSession[_innlogget] = _ikkeInnlogget;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            brukereController.ControllerContext.HttpContext = mockHttpContext.Object;
            var resultat = await brukereController.NyAdmin(bruker) as UnauthorizedObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke innlogget", resultat.Value);
        }

        // Hjelpemetode som returnerer et BrukerModel-objekt
        private BrukerModel HentEnBrukerModel()
        {
            BrukerModel bruker = new BrukerModel { Brukernavn = "Admin", Passord = "Admin123" };
            return bruker;
        }
    }
}
