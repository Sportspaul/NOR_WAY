using System;
using System.Collections.Generic;
using System.Text;

namespace NOR_WAY_Tests
{
    class OrdreController_Tests
    {
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


    }
}
