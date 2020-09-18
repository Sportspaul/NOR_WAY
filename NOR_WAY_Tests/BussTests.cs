using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            Avgang resultat = await bussController.FinnNesteAvgang(param);

            Console.WriteLine("AvgangId " + resultat.AvgangId);
            Console.WriteLine("Rutenavn " + resultat.Rutenavn);
            Console.WriteLine("Reisetid " + resultat.Reisetid);

            Assert.Equal(forventetAvgang.AvgangId, resultat.AvgangId);
            Assert.Equal(forventetAvgang.Rutenavn, resultat.Rutenavn);
            Assert.Equal(forventetAvgang.Linjekode, resultat.Linjekode);
            Assert.Equal(forventetAvgang.Pris, resultat.Pris);
            Assert.Equal(forventetAvgang.Avreise, resultat.Avreise);
            Assert.Equal(forventetAvgang.Ankomst, resultat.Ankomst);
            Assert.Equal(forventetAvgang.Reisetid, resultat.Reisetid);
        }
    }
}
