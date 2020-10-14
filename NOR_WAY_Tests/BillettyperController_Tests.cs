using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NOR_WAY.Controllers;
using NOR_WAY.DAL;
using NOR_WAY.DAL.Interfaces;
using Xunit;

namespace NOR_WAY_Tests
{
    public class BillettyperController_Tests
    {
        private const string _innlogget = "Innlogget";
        private const string _ikkeInnlogget = "";

        private readonly Mock<IBillettyperRepository> mockRepo = new Mock<IBillettyperRepository>();
        private readonly Mock<ILogger<BillettyperController>> mockLog = new Mock<ILogger<BillettyperController>>();
        private readonly BillettyperController billettyperController;

        // Session
        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();

        public BillettyperController_Tests()
        {
            billettyperController = new BillettyperController(mockRepo.Object, mockLog.Object);
        }


        /* Enhetstester for HentAlleBillettyper */

        /* Tester at ikke listen med Stopp fra BussRepo endrer seg i controlleren
           for HentAlleBillettyper() */
        [Fact]
        public async Task HentAlleBillettyper_RiktigeVerdier()
        {
            // Arrange
            List<Billettyper> forventet = HentBillettyperListe();
            mockRepo.Setup(b => b.HentAlleBillettyper()).ReturnsAsync(HentBillettyperListe());

            // Act
            var resultat = await billettyperController.HentAlleBillettyper() as OkObjectResult;
            List<Billettyper> faktisk = (List<Billettyper>)resultat.Value;

            // Assert
            Assert.Equal(forventet.Count, faktisk.Count);   // Tester om listene er like lange
            // Tester om alle verdiene er like i alle elementene
            for (int i = 0; i < forventet.Count; i++)
            {
                Assert.Equal(forventet[i].Billettype, faktisk[i].Billettype);
                Assert.Equal(forventet[i].Rabattsats, faktisk[i].Rabattsats);
            }
        }

        // Tester at returverdi: null blir håndtert korrekt for HentAlleBillettyper()
        [Fact]
        public async Task HentAlleBillettyper_Null() {
            // Arrange
            mockRepo.Setup(b => b.HentAlleBillettyper()).ReturnsAsync(() => null);

            // Act
            var resultat = await billettyperController.HentAlleBillettyper() as NotFoundObjectResult;

            // Assert
            Assert.Equal("Ingen billettyper ble funnet", resultat.Value);
        }


        /* Enhetstester for OppdaterRabattsats */

        // Tester at OppdaterRabattsats returnerer forventet verdi
        [Fact]
        public async Task NyBillettype_RiktigeVerdier()
        {
            // Arrange
            var billettype = HentEnBillettype();
            mockRepo.Setup(br => br.NyBillettype(billettype)).ReturnsAsync(true);

            // Act
            SimulerInnlogget();
            var resultat = await billettyperController.NyBillettype(billettype) as OkObjectResult;

            // Assert
            Assert.Equal("Ny billettype ble lagret!", resultat.Value);
        }

        // Test for om retur verdi: false blir håndtert korrekt for NyBillettype()
        [Fact]
        public async Task NyBillettype_IkkeOK()
        {
            // Arrange
            var billettype = HentEnBillettype();
            mockRepo.Setup(br => br.NyBillettype(billettype)).ReturnsAsync(false);

            // Act
            SimulerInnlogget();
            var resultat = await billettyperController.NyBillettype(billettype) as BadRequestObjectResult;

            // Assert
            Assert.Equal("Ny billettype kunne ikke lagres!", resultat.Value);
        }

        // Tester at NyBillettype i controlleren håndterer InvalidModelState
        [Fact]
        public async Task NyBillettype_RegEx()
        {
            // Arrange
            var billettype = HentEnBillettype();
            mockRepo.Setup(br => br.NyBillettype(billettype)).ReturnsAsync(false);
            billettyperController.ModelState.AddModelError("Billettype", "Feil i inputvalideringen på server");

            // Act
            SimulerInnlogget();
            var resultat = await billettyperController.NyBillettype(billettype) as BadRequestObjectResult;

            // Assert
            Assert.Equal("Feil i inputvalidering på server", resultat.Value);
        }

        // Test på at NyBillettype håndterer tilfelle hvor bruker ikke er logget inn
        [Fact]
        public async Task NyBillettypen_IkkeInnlogget()
        {
            // Arrange
            var billettype = HentEnBillettype();
            mockRepo.Setup(br => br.NyBillettype(billettype)).ReturnsAsync(true);

            // Act
            SimulerUtlogget();
            var resultat = await billettyperController.NyBillettype(billettype) as UnauthorizedObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke innlogget", resultat.Value);
        }


        /* Enhetstester for OppdaterRabattsats */

        // Tester at OppdaterRabattsats returnerer forventet verdi
        [Fact]
        public async Task OppdaterRabattsats_RiktigeVerdier()
        {
            // Arrange
            var billettype = HentEnBillettype();
            mockRepo.Setup(br => br.OppdaterRabattsats(billettype)).ReturnsAsync(true);

            // Act
            SimulerInnlogget();
            var resultat = await billettyperController.OppdaterRabattsats(billettype) as OkObjectResult;

            // Assert
            Assert.Equal("Rabattsats ble endret", resultat.Value);
        }

        // Test for om retur verdi: false blir håndtert korrekt for OppdaterRabattsats()
        [Fact]
        public async Task OppdaterRabattsats_IkkeOK()
        {
            // Arrange
            var billettype = HentEnBillettype();
            mockRepo.Setup(br => br.OppdaterRabattsats(billettype)).ReturnsAsync(false);

            // Act
            SimulerInnlogget();
            var resultat = await billettyperController.OppdaterRabattsats(billettype) as NotFoundObjectResult;

            // Assert
            Assert.Equal("Endringen av rabattsats kunne ikke utføres", resultat.Value);
        }

        // Tester at OppdaterRabattsats i controlleren håndterer InvalidModelState
        [Fact]
        public async Task OppdaterRabattsats_RegEx()
        {
            // Arrange
            var billettype = HentEnBillettype();
            mockRepo.Setup(br => br.OppdaterRabattsats(billettype)).ReturnsAsync(false);
            billettyperController.ModelState.AddModelError("Billettype", "Feil i inputvalideringen på server");

            // Act
            SimulerInnlogget();
            var resultat = await billettyperController.OppdaterRabattsats(billettype) as BadRequestObjectResult;

            // Assert
            Assert.Equal("Feil i inputvalidering på server", resultat.Value);
        }

        // Test på at OppdaterRabattsats håndterer tilfelle hvor bruker ikke er logget inn
        [Fact]
        public async Task OppdaterRabattsats_IkkeInnlogget()
        {
            // Arrange
            var billettype = HentEnBillettype();
            mockRepo.Setup(br => br.OppdaterRabattsats(billettype)).ReturnsAsync(true);

            // Act
            SimulerUtlogget();
            var resultat = await billettyperController.OppdaterRabattsats(billettype) as UnauthorizedObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke innlogget", resultat.Value);
        }


        // Returnerer en List med Billettyper-objekter
        private List<Billettyper> HentBillettyperListe()
        {
            Billettyper billettype1 = new Billettyper { Billettype = "Student", Rabattsats = 50 };
            Billettyper billettype2 = new Billettyper { Billettype = "Voksen", Rabattsats = 0 };
            Billettyper billettype3 = new Billettyper { Billettype = "Honør", Rabattsats = 25 };
            return new List<Billettyper> { billettype1, billettype2, billettype3 };
        }

        private Billettyper HentEnBillettype() {
            return new Billettyper { Billettype = "Ufør", Rabattsats = 90 };
        }

        private void SimulerInnlogget()
        {
            mockSession[_innlogget] = _innlogget;
            EndreSession(mockSession);
        }

        private void SimulerUtlogget()
        {
            mockSession[_innlogget] = _ikkeInnlogget;
            EndreSession(mockSession);
        }

        private void EndreSession(MockHttpSession mockSession)
        {
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            billettyperController.ControllerContext.HttpContext = mockHttpContext.Object;
        }
    }
}
