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

        [Fact]
        public async Task FullforOrdreTest()
        {

        // Arrange
            var billettype = new List<string>();
            billettype.Add("Student");
            billettype.Add("Barn");

            var kundeOrdre = new KundeOrdre
            {
                Epost = "hvrustad@gmail.com",
                StartStopp = "Bergen",
                SluttStopp = "Trondheim",
                Sum = 379,
                Linjekode = "NW431",
                Avganger = "2",
                Billettype = billettype
            };

            mockRepo.Setup(br => br.FullforOrdre(kundeOrdre)).ReturnsAsync(true);
            var bussController = new BussController(mockRepo.Object, mockLog.Object);

        // Act
            bool resultat = await bussController.FullforOrdre(kundeOrdre);

        // Assert
            Assert.True(resultat);



        /*
            var ordre = new Ordre()
            {
                Epost = "hvrustad@gmail.com",
                StartStopp = "Bergen",
                SluttStopp = "Trondheim",
                Sum = 379,
                Rute = NW431
                {
                    Linjekode = "NW431",
                    Rutenavn = "Fjordekspressen",
                    Startpris = 79,
                    TilleggPerStopp = 30,
                    Kapasitet = 55
                },
                Avgang = 1
                {
                    Id = 1,
                    Avreise = date1,
                    SolgteBilletter = 0,
                    Rute = NW431Rute
                };
        */
        }
    }
}
