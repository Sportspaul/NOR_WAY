using System.Threading.Tasks;
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
        private readonly Mock<IBrukereRepository> mockRepo = new Mock<IBrukereRepository>();
        private readonly Mock<ILogger<BrukereController>> mockLogCtr = new Mock<ILogger<BrukereController>>();
        private readonly BrukereController brukereController;

        public BrukereController_Tests()
        {
            brukereController = new BrukereController(mockRepo.Object, mockLogCtr.Object);
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

        [Fact]
        public async Task NyAdmin_RiktigeVerider()
        {
            // Arrange
            var bruker = HentEnBrukerModel();
            mockRepo.Setup(br => br.NyAdmin(bruker)).ReturnsAsync(true);

            // Act
            var resultat = await brukereController.NyAdmin(bruker) as OkObjectResult;

            // Assert
            Assert.Equal("Ny bruker ble lagret", resultat.Value);
        }

        [Fact]
        public async Task NyAdmin_IkkeOK()
        {
            // Arrange
            var bruker = HentEnBrukerModel();
            mockRepo.Setup(br => br.NyAdmin(bruker)).ReturnsAsync(false);

            // Act
            var resultat = await brukereController.NyAdmin(bruker) as BadRequestObjectResult;

            // Assert
            Assert.Equal("Ny bruker kunne ikke lagres", resultat.Value);
        }

        // Tester at NyAdmin i controlleren håndterer InvalidModelState
        [Fact]
        public async Task NyAdmin_RegEx()
        {
            // Arrange
            var bruker = HentEnBrukerModel();
            mockRepo.Setup(br => br.NyAdmin(bruker)).ReturnsAsync(false);
            brukereController.ModelState.AddModelError("Brukernavn", "Feil i inputvalideringen på server");
            brukereController.ModelState.AddModelError("Passord", "Feil i inputvalideringen på server");

            // Act
            var resultat = await brukereController.NyAdmin(bruker) as BadRequestObjectResult;

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
