using System;
using System.Collections.Generic;
using System.Text;
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
    public class OrdreController_Tests
    {

        private readonly Mock<IOrdreRepository> mockRepo = new Mock<IOrdreRepository>();
        private readonly Mock<ILogger<OrdreController>> mockLogCtr = new Mock<ILogger<OrdreController>>();
        private readonly OrdreController ordreController;

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
            var kundeOrdre = HentEnKundeOrdre();
            mockRepo.Setup(br => br.FullforOrdre(kundeOrdre)).ReturnsAsync(true);

            // Act
            var resultat = await ordreController.FullforOrdre(kundeOrdre) as OkObjectResult;

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
            var resultat = await ordreController.FullforOrdre(kundeOrdre) as BadRequestObjectResult;

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
            ordreController.ModelState.AddModelError("Epost", "Feil i inputvalideringen på server");

            // Act
            var resultat = await ordreController.FullforOrdre(kundeOrdre) as BadRequestObjectResult;

            // Assert
            Assert.Equal("Feil i inputvalidering på server", resultat.Value);
        }


        /* Private metoder som instansierer objekter til brukes i testmetodene */

        // Returnerer et KundeOrdre-objekt
        private OrdreModel HentEnKundeOrdre()
        {
            return new OrdreModel { Epost = "hvrustad@gmail.com", StartStopp = "Bergen", SluttStopp = "Trondheim", Linjekode = "NW431", AvgangId = 2, Billettyper = HentBillettyperStringListe() };
        }

        // Returnerer en List med string billettypenavn
        private List<string> HentBillettyperStringListe()
        {
            return new List<string> { "Student", "Barn" };
        }
    }
}
