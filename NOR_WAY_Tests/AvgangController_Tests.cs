using System.Collections.Generic;
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
    public class AvgangControllerTests
    {
        private readonly Mock<IAvgangRepository> mockRepo = new Mock<IAvgangRepository>();
        private readonly Mock<ILogger<AvgangController>> mockLogCtr = new Mock<ILogger<AvgangController>>();
        private readonly AvgangController avgangController;

        public AvgangControllerTests()
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

        [Fact]
        public async Task FinnNesteAvgang_Null()
        {
            // Arrange
            var param = new Avgangkriterier();
            mockRepo.Setup(b => b.FinnNesteAvgang(param)).ReturnsAsync(() => null);

            // Act
            var resultat = await avgangController.FinnNesteAvgang(param) as NotFoundObjectResult;
            
            // Assert
            Assert.Equal("Avgang ikke funnet", resultat.Value);
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
    }
}