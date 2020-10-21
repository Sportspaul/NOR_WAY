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
    public class AvgangController_Tests
    {
        private const string _innlogget = "Innlogget";
        private const string _ikkeInnlogget = "";

        private readonly Mock<IAvgangRepository> mockRepo = new Mock<IAvgangRepository>();
        private readonly Mock<ILogger<AvgangController>> mockLogCtr = new Mock<ILogger<AvgangController>>();
        private readonly AvgangController avgangController;

        // Session
        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();

        public AvgangController_Tests()
        {
            avgangController = new AvgangController(mockRepo.Object, mockLogCtr.Object);
        }

        /* Enhetstester for FinnNesteAvgang */

        // Tester at ikke Avgang fra BussRepo endrer seg i controlleren
        [Fact]
        public async Task FinnNesteAvgang_RiktigeVerdier()
        {
            // Arrange
            Avgangkriterier param = HentAvgangParam();
            Reisedetaljer forventetAvgang = HentAvgang();

            // Act
            mockRepo.Setup(b => b.FinnNesteAvgang(param)).ReturnsAsync(HentAvgang());
            var resultat = await avgangController.FinnNesteAvgang(param) as OkObjectResult;
            Reisedetaljer faktiskAvgang = (Reisedetaljer)resultat.Value;

            // Assert
            Assert.Equal(forventetAvgang.AvgangId, faktiskAvgang.AvgangId);
            Assert.Equal(forventetAvgang.Rutenavn, faktiskAvgang.Rutenavn);
            Assert.Equal(forventetAvgang.Linjekode, faktiskAvgang.Linjekode);
            Assert.Equal(forventetAvgang.Pris, faktiskAvgang.Pris);
            Assert.Equal(forventetAvgang.Avreise, faktiskAvgang.Avreise);
            Assert.Equal(forventetAvgang.Ankomst, faktiskAvgang.Ankomst);
            Assert.Equal(forventetAvgang.Reisetid, faktiskAvgang.Reisetid);
        }

        // Tester at returverdi: null blir håndtert korrekt for FinnNesteAvgang()
        [Fact]
        public async Task FinnNesteAvgang_Null()
        {
            // Arrange
            var param = new Avgangkriterier();
            mockRepo.Setup(b => b.FinnNesteAvgang(param)).ReturnsAsync(() => null);

            // Act
            var resultat = await avgangController.FinnNesteAvgang(param) as NotFoundObjectResult;
           

            // Assert
            Assert.Equal($"Ingen avgang ble funnet", resultat.Value);
        }

        // Tester at FinnNesteAvgang i controlleren håndterer InvalidModelState
        [Fact]
        public async Task FinnNesteAvgang_RegEx()
        {
            // Arrange
            var param = HentUgyldigAvgangParam();
            var forventetAvgang = HentAvgang();
            mockRepo.Setup(b => b.FinnNesteAvgang(param)).ReturnsAsync(forventetAvgang);
            avgangController.ModelState.AddModelError("StartStopp", "Feil i inputvalideringen på server");
            avgangController.ModelState.AddModelError("Dato", "Feil i inputvalideringen på server");

            // Act
            var resultat = await avgangController.FinnNesteAvgang(param) as BadRequestObjectResult;

            // Assert
            Assert.Equal("Feil i inputvalideringen på server", resultat.Value);
        }

        // Test for at FjernAvgang() håndterer riktig returnverdier på korrekt måte
        [Fact]
        public async Task FjernAvgang_RiktigeVerdier()
        {
            // Arrange
            int id = 1;
            mockRepo.Setup(br => br.FjernAvgang(id)).ReturnsAsync(true);

            // Act
            SimulerInnlogget();
            var resultat = await avgangController.FjernAvgang(id) as OkObjectResult;

            // Assert
            Assert.Equal($"Avgangen med id: {id}, ble slettet", resultat.Value);
        }

        // Test for at FjernAvgang() håndtrer feil returverdi på korrekt måte
        [Fact]
        public async Task FjernAvgang_IkkeOK()
        {
            // Arrange
            int id = 1;
            mockRepo.Setup(br => br.FjernAvgang(id)).ReturnsAsync(false);

            // Act
            SimulerInnlogget();
            var resultat = await avgangController.FjernAvgang(id) as BadRequestObjectResult;

            // Assert
            Assert.Equal($"Avgangen med id: {id}, kunne ikke slettes", resultat.Value);
        }

        // Tester at FjernAvgang håndterer InvalidModelState i controlleren
        [Fact]
        public async Task FjernAvgang_Regex()
        {
            // Arrange
            int id = 1;
            avgangController.ModelState.AddModelError("id", "Feil i inputvalideringen på server");

            // Act
            SimulerInnlogget();
            var resultat = await avgangController.FjernAvgang(id) as BadRequestObjectResult;

            // Assert
            Assert.Equal("Feil i inputvalideringen på server", resultat.Value);
        }

        // Test på at FjernAvgang håndterer tilfelle hvor bruker ikke er logget inn
        [Fact]
        public async Task NyBillettypen_IkkeInnlogget()
        {
            // Arrange
            int id = 1;

            // Act
            SimulerUtlogget();
            var resultat = await avgangController.FjernAvgang(id) as UnauthorizedObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke innlogget", resultat.Value);
        }

        [Fact]
        public async Task HentAvganger_RiktigeVerdier()
        {
            // Arrange
            string linjekode = "NW431";
            int sidenummer = 0;
            List<AvgangModel> forventet = HentAvgangModelListe();

            mockRepo.Setup(br => br.HentAvganger(linjekode, sidenummer)).ReturnsAsync(HentAvgangModelListe());

            // Act
            SimulerInnlogget();
            var resultat = await avgangController.HentAvganger(linjekode, sidenummer) as OkObjectResult;
            List<AvgangModel> faktisk = (List<AvgangModel>)resultat.Value;

            // Assert
            Assert.Equal(forventet.Count, faktisk.Count);
            for (int i = 0; i < forventet.Count; i++)
            {
                Assert.Equal(forventet[i].Id, faktisk[i].Id);
                Assert.Equal(forventet[i].Avreise, faktisk[i].Avreise);
                Assert.Equal(forventet[i].SolgteBilletter, faktisk[i].SolgteBilletter);
            }
        }

        // Returnerer et AvgangParam-objekt
        private Avgangkriterier HentAvgangParam()
        {
            return new Avgangkriterier { StartStopp = "Bergen", SluttStopp = "Vadheim", Dato = "2020-11-20", Tidspunkt = "16:00", AvreiseEtter = true };
        }

        private Avgangkriterier HentUgyldigAvgangParam()
        {
            return new Avgangkriterier { StartStopp = "", SluttStopp = "Vadheim", Dato = "", Tidspunkt = "16:00", AvreiseEtter = true };
        }

        // Returnerer et Avgang-objekt
        private Reisedetaljer HentAvgang()
        {
            return new Reisedetaljer { AvgangId = 1, Rutenavn = "Fjordekspressen", Linjekode = "NW431", Pris = 100, Avreise = "2020-11-25 17:00", Ankomst = "2020-11-25 18:20", Reisetid = 80 };
        }

        private List<AvgangModel> HentAvgangModelListe()
        {
            AvgangModel avgang1 = new AvgangModel { Id = 1, Avreise = "2020-10-10 17:00", SolgteBilletter = 0 };
            AvgangModel avgang2 = new AvgangModel { Id = 2, Avreise = "2020-10-12 17:00", SolgteBilletter = 0 };
            AvgangModel avgang3 = new AvgangModel { Id = 3, Avreise = "2020-10-14 17:00", SolgteBilletter = 0 };
            return new List<AvgangModel> { avgang1, avgang2, avgang3 };
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
            avgangController.ControllerContext.HttpContext = mockHttpContext.Object;
        }
    }
}