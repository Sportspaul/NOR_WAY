using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NOR_WAY.Controllers;
using NOR_WAY.DAL;
using NOR_WAY.Model;
using Xunit;
using Xunit.Abstractions;

namespace NOR_WAY_Tests
{
    public class BussTests
    {
        private readonly Mock<IBussRepository> mockRepo = new Mock<IBussRepository>();
        private readonly Mock<ILogger<BussController>> mockLog = new Mock<ILogger<BussController>>();
        private readonly ITestOutputHelper output;

        public BussTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public async Task FinnNesteAvgangTest()
        {
            var param = new AvgangParam
            {
                StartStopp = "Bergen",
                SluttStopp = "Vadheim",
                Dato = "2020-11-20",
                Tidspunkt = "16:00",
                AvreiseEtter = true
            };

            var forventetAvgang = new Avgang
            {
                AvgangId = 1,
                Rutenavn = "Fjordekspressen",
                Linjekode = "NW431",
                Pris = 100,
                Avreise = "2020-11-25 17:00",
                Ankomst = "2020-11-25 18:20",
                Reisetid = 80
            };

            mockRepo.Setup(b => b.FinnNesteAvgang(param)).ReturnsAsync(forventetAvgang);
            var bussController = new BussController(mockRepo.Object, mockLog.Object);
            var resultat = await bussController.FinnNesteAvgang(param) as OkObjectResult;
            Avgang avgang = (Avgang)resultat.Value;

            output.WriteLine("AvgangId " + avgang.AvgangId);
            output.WriteLine("Rutenavn " + avgang.Rutenavn);
            output.WriteLine("Reisetid " + avgang.Reisetid);

            Assert.Equal(forventetAvgang.AvgangId, avgang.AvgangId);
            Assert.Equal(forventetAvgang.Rutenavn, avgang.Rutenavn);
            Assert.Equal(forventetAvgang.Linjekode, avgang.Linjekode);
            Assert.Equal(forventetAvgang.Pris, avgang.Pris);
            Assert.Equal(forventetAvgang.Avreise, avgang.Avreise);
            Assert.Equal(forventetAvgang.Ankomst, avgang.Ankomst);
            Assert.Equal(forventetAvgang.Reisetid, avgang.Reisetid);
        }

        [Fact]
        public async Task FullforOrdreTest()
        {
            // Arrange
            var billettype = new List<string>
            {
                "Student",
                "Barn"
            };

            var kundeOrdre = new KundeOrdre
            {
                Epost = "hvrustad@gmail.com",
                StartStopp = "Bergen",
                SluttStopp = "Trondheim",
                Linjekode = "NW431",
                AvgangId = 2,
                Billettyper = billettype
            };

            mockRepo.Setup(br => br.FullforOrdre(kundeOrdre)).ReturnsAsync(true);
            var bussController = new BussController(mockRepo.Object, mockLog.Object);
            // Act
            var resultat = await bussController.FullforOrdre(kundeOrdre) as OkObjectResult;
            // Assert
            Assert.Equal("Ordren ble lagret!", resultat.Value);
        }
    }
}