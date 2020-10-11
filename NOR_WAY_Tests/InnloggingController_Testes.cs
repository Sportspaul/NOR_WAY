using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
    public class InnloggingController_Tests
    {
        private readonly Mock<IInnloggingRepository> mockRepo = new Mock<IInnloggingRepository>();
        private readonly Mock<ILogger<InnloggingController>> mockLogCtr = new Mock<ILogger<InnloggingController>>();
        private readonly InnloggingController innloggingController;

        public InnloggingController_Tests()
        {
            innloggingController = new InnloggingController(mockRepo.Object, mockLogCtr.Object);
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
            var resultat = await innloggingController.LoggInn(bruker) as OkObjectResult;

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
            var resultat = await innloggingController.LoggInn(bruker) as BadRequestObjectResult;

            // Assert
            Assert.Equal("Innlogging feilet for bruker: " + bruker.Brukernavn, resultat.Value);
        }

        // Tester at LoggInn i controlleren håndterer InvalidModelState
        [Fact]
        public async Task LoggInn_RegEx()
        {
            // Arrange
            var bruker = HentEnBrukerModel();
            mockRepo.Setup(br => br.LoggInn(bruker)).ReturnsAsync(false);
            innloggingController.ModelState.AddModelError("Brukernavn", "Feil i inputvalideringen på server");
            innloggingController.ModelState.AddModelError("Passord", "Feil i inputvalideringen på server");

            // Act
            var resultat = await innloggingController.LoggInn(bruker) as BadRequestObjectResult;

            // Assert
            Assert.Equal("Feil i inputvalidering på server", resultat.Value);
        }

        private BrukerModel HentEnBrukerModel()
        {
            BrukerModel bruker = new BrukerModel { Brukernavn = "brukernavn123", Passord = "passord123" };
            return bruker;
        }

    }
}
