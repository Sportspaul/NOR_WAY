using System.Collections.Generic;
using System.Net;
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
            ordreController.ModelState.AddModelError("Epost", "Feil i inputvalideringen på server");

            // Act
            var resultat = await ordreController.FullforOrdre(kundeOrdre) as BadRequestObjectResult;

            // Assert
            Assert.Equal("Feil i inputvalideringen på server", resultat.Value);
        }

        // Adminmetoder som krever innlogging
        [Fact]
        public async Task HentOrdre_RiktigeVerdier()
        {
            // Arrange
            string epost = "ola@nordmann.no";
            List<OrdreModel> ordreliste = HentOrdreListe(epost);
            mockRepo.Setup(br => br.HentOrdre(epost)).ReturnsAsync(ordreliste);
            MockSession(_innlogget);

            // Act
            var resultat = await ordreController.HentOrdre(epost) as OkObjectResult;
            List<OrdreModel> faktiskOrdreliste = (List<OrdreModel>) resultat.Value;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            for(int i = 0; i < ordreliste.Count; i++) {
                Assert.Equal(ordreliste[i].StartStopp, faktiskOrdreliste[i].StartStopp);
                Assert.Equal(ordreliste[i].Sum, faktiskOrdreliste[i].Sum);
                Assert.Equal(ordreliste[i].Linjekode, faktiskOrdreliste[i].Linjekode);
            }
        }

        [Fact]
        public async Task HentOrdre_IkkeTilgang()
        {
            // Arrange
            string epost = "ola@nordmann.no";
            List<OrdreModel> ordreliste = HentOrdreListe(epost);
            mockRepo.Setup(br => br.HentOrdre(epost)).ReturnsAsync(ordreliste);
            MockSession(_ikkeInnlogget);

            // Act
            var resultat = await ordreController.HentOrdre(epost) as UnauthorizedObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke innlogget", resultat.Value);
        }


        [Fact]
        public async Task HentOrdre_IkkeOK()
        {
            // Arrange
            string epost = "ola@nordmann.no";
            mockRepo.Setup(br => br.HentOrdre(epost)).ReturnsAsync(() => null);
            MockSession(_innlogget);

            // Act
            var resultat = await ordreController.HentOrdre(epost) as NotFoundObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.NotFound, resultat.StatusCode);
            Assert.Equal("Ingen ordre ble funnet", resultat.Value);
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

        private List<OrdreModel> HentOrdreListe(string epost)
        {
            OrdreModel ordre1 = new OrdreModel
            {
                Id = 1,
                AvgangId = 1,
                Billettyper = HentBillettyperStringListe(),
                Epost = epost,
                StartStopp = "Bergen",
                SluttStopp = "Vadheim",
                Sum = "69",
                Linjekode = "NW1"
            };
            OrdreModel ordre2 = new OrdreModel
            {
                Id = 2,
                AvgangId = 5,
                Billettyper = HentBillettyperStringListe(),
                Epost = epost,
                StartStopp = "Oslo",
                SluttStopp = "Bergen",
                Sum = "420",
                Linjekode = "NW4"
            };
            List<OrdreModel> ordreliste = new List<OrdreModel> { ordre1, ordre2 };
            return ordreliste;
        }

        private void MockSession(string innlogging)
        {
            mockSession[_innlogget] = innlogging;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            ordreController.ControllerContext.HttpContext = mockHttpContext.Object;
        }
    }
}