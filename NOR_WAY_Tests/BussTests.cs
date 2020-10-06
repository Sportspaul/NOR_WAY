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
        private readonly Mock<ILogger<BussController>> mockLogCtr = new Mock<ILogger<BussController>>();
        private readonly ITestOutputHelper output;

        public BussTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        // Tester at ikke Avgang fra BussRepo endrer seg i controlleren
        [Fact]
        public async Task FinnNesteAvgangRiktigeVerdier()
        {
            AvgangParam param = HentAvgangParam();
            Avgang forventetAvgang = HentEnAvgang();

            // Lager en mock av IBussRepo og sier at FinnNesteAvgang-metoden (BussRepo) alltid skal returnerer Avgang fra enAvgang()
            mockRepo.Setup(b => b.FinnNesteAvgang(param)).ReturnsAsync(forventetAvgang);

            /* Nytt instans av BussCtr som tar inn IBussRepo-mock som argument.
             * Altså objektet med FinnNesteAvgang-metoden som alltid returnerer Avgang fra enAvgang() */
            var bussController = new BussController(mockRepo.Object, mockLogCtr.Object);

            /* Kaller FinnNesteAvgang-metoden (BussCtr),
             * som igjen kaller FinnNesteAvgang (BussRepo), 
             * men siden vi har mocket vil den metoden alltid returnere Avgang fra enAvgang() */
            var resultat = await bussController.FinnNesteAvgang(param) as OkObjectResult;

            // Henter ut Avgang-objektet fra OkObjectResult som FinnNesteAvgang fra Controlleren returnerte
            Avgang faktiskAvgang = (Avgang)resultat.Value;

            /* Disse testene sjekker da om Avgang-objetet ble modifisert i FinnNesteAvgang (BussCtr)
             * Det betyr at vi har isolert metoden fra resten av programmet 
             * Vi luker ut eventuelle feil som kan oppså i DB */
            Assert.Equal(forventetAvgang.AvgangId, faktiskAvgang.AvgangId);
            Assert.Equal(forventetAvgang.Rutenavn, faktiskAvgang.Rutenavn);
            Assert.Equal(forventetAvgang.Linjekode, faktiskAvgang.Linjekode);
            Assert.Equal(forventetAvgang.Pris, faktiskAvgang.Pris);
            Assert.Equal(forventetAvgang.Avreise, faktiskAvgang.Avreise);
            Assert.Equal(forventetAvgang.Ankomst, faktiskAvgang.Ankomst);
            Assert.Equal(forventetAvgang.Reisetid, faktiskAvgang.Reisetid);

            /* Skriver vi tester for alle metodene i BussCtr kan vi sikre oss mot skrivefeil
             * og forsikre oss om at metodene i BussCtr alltid vil returnere riktig verider så lenge 
             * FinnNesteAvgang (BussRepo) fungerer slik den skal */
        }

        private Avgang HentEnAvgang()
        {
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

            return forventetAvgang;
        }

        private AvgangParam HentAvgangParam()
        {
            var param = new AvgangParam
            {
                StartStopp = "Bergen",
                SluttStopp = "Vadheim",
                Dato = "2020-11-20",
                Tidspunkt = "16:00",
                AvreiseEtter = true
            };

            return param;
        }

        // Tester at ikke listen med Stopp fra BussRepo endrer seg i controlleren
        [Fact]
        public async Task HentAlleStoppRiktigeVerdier()
        {
            List<Stopp> forventedeStopp = HentStoppListe();

            mockRepo.Setup(b => b.HentAlleStopp()).ReturnsAsync(forventedeStopp);
            var bussController = new BussController(mockRepo.Object, mockLogCtr.Object);
            var resultat = await bussController.HentAlleStopp() as OkObjectResult;
            List<Stopp> faktiskeStopp = (List<Stopp>)resultat.Value;

            Assert.Equal(forventedeStopp.Count, faktiskeStopp.Count);   // Tester om listene er like lange
            // Tester om alle verdiene er like i alle elementene
            for (int i = 0; i < forventedeStopp.Count; i++)
            {
                Assert.Equal(forventedeStopp[i].Id, faktiskeStopp[i].Id);
                Assert.Equal(forventedeStopp[i].Navn, faktiskeStopp[i].Navn);
            }
        }

        // Returnerer en liste med Stopp
        private List<Stopp> HentStoppListe() {
            Stopp stopp1 = new Stopp { Id = 1, Navn = "Bergen" };
            Stopp stopp2 = new Stopp { Id = 1, Navn = "Oslo" };
            Stopp stopp3 = new Stopp { Id = 1, Navn = "Vadheim" };
            Stopp stopp4 = new Stopp { Id = 1, Navn = "Trondheim" };
            return new List<Stopp> { stopp1, stopp2, stopp3, stopp4 };
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
            var bussController = new BussController(mockRepo.Object, mockLogCtr.Object);
            // Act
            var resultat = await bussController.FullforOrdre(kundeOrdre) as OkObjectResult;
            // Assert
            Assert.Equal("Ordren ble lagret!", resultat.Value);
        }
    }
}