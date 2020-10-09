using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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

        private readonly Mock<IBillettyperRepository> mockRepo = new Mock<IBillettyperRepository>();
        private readonly Mock<ILogger<BillettyperController>> mockLogCtr = new Mock<ILogger<BillettyperController>>();
        private readonly BillettyperController billettyperController;

        public BillettyperController_Tests()
        {
            billettyperController = new BillettyperController(mockRepo.Object, mockLogCtr.Object);
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

        // Returnerer en List med Billettyper-objekter
        private List<Billettyper> HentBillettyperListe()
        {
            Billettyper billettype1 = new Billettyper { Billettype = "Student", Rabattsats = 50 };
            Billettyper billettype2 = new Billettyper { Billettype = "Voksen", Rabattsats = 0 };
            Billettyper billettype3 = new Billettyper { Billettype = "Honør", Rabattsats = 25 };
            return new List<Billettyper> { billettype1, billettype2, billettype3 };
        }
    }
}
