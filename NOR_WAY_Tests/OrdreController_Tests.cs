using System.Collections.Generic;
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
    public class OrdreController_Tests
    {
        private readonly Mock<IOrdreRepository> mockRepo = new Mock<IOrdreRepository>();
        private readonly Mock<ILogger<OrdreController>> mockLogCtr = new Mock<ILogger<OrdreController>>();
        private readonly OrdreController ordreController;

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();

        private const string _innlogget = "Innlogget";
        private const string _ikkeInnlogget = "";

        public OrdreController_Tests()
        {
            ordreController = new OrdreController(mockRepo.Object, mockLogCtr.Object);
        }

        /* Enhetstester for FullforOrdre */

        // Tester at FullforOrdre returnerer forventet verdi
        [Fact]
        public async Task FullforOrdre_RiktigeVerdier()
        {
            // Arrange
            var kundeOrdre = HentEnNyOrdre();
            mockRepo.Setup(br => br.FullforOrdre(kundeOrdre)).ReturnsAsync(true);
            MockSession(_innlogget);

            // Act
            var resultat = await ordreController.FullforOrdre(kundeOrdre) as OkObjectResult;

            // Assert
            Assert.Equal($"Ny ordre ble lagret med verdiene: {kundeOrdre}", resultat.Value);
        }

        [Fact]
        public async Task FullforOrdre_IkkeOK()
        {
            // Arrange
            var kundeOrdre = HentEnNyOrdre();
            mockRepo.Setup(br => br.FullforOrdre(kundeOrdre)).ReturnsAsync(false);
            MockSession(_innlogget);

            // Act
            var resultat = await ordreController.FullforOrdre(kundeOrdre) as BadRequestObjectResult;

            // Assert
            Assert.Equal($"Ny ordre kunne ikke lagres med verdiene: {kundeOrdre}", resultat.Value);
        }

        // Tester at FullforOrdre i controlleren håndterer InvalidModelState
        [Fact]
        public async Task FullforOrdre_RegEx()
        {
            // Arrange
            var kundeOrdre = HentEnNyOrdre();
            mockRepo.Setup(br => br.FullforOrdre(kundeOrdre)).ReturnsAsync(false);
            MockSession(_innlogget);
            ordreController.ModelState.AddModelError("Epost", "Feil i inputvalideringen på server");

            // Act
            var resultat = await ordreController.FullforOrdre(kundeOrdre) as BadRequestObjectResult;

            // Assert
            Assert.Equal("Feil i inputvalideringen på server", resultat.Value);
        }

        /* Private metoder som instansierer objekter til brukes i testmetodene */

        // Returnerer et KundeOrdre-objekt
        private NyOrdre HentEnNyOrdre()
        {
            return new NyOrdre { Epost = "hvrustad@gmail.com", StartStopp = "Bergen", SluttStopp = "Trondheim", Linjekode = "NW431", AvgangId = 2, Billettyper = HentBillettyperStringListe() };
        }

        // Returnerer en List med string billettypenavn
        private List<string> HentBillettyperStringListe()
        {
            return new List<string> { "Student", "Barn" };
        }

        private void MockSession(string innlogging)
        {
            mockSession[_innlogget] = innlogging;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            ordreController.ControllerContext.HttpContext = mockHttpContext.Object;
        }
    }
}