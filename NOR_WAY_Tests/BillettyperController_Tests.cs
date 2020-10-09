using System;
using System.Collections.Generic;
using System.Text;

namespace NOR_WAY_Tests
{
    class BillettyperController_Tests
    {

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
            var resultat = await bussController.HentAlleBillettyper() as OkObjectResult;
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

        // Returnerer en List med string billettypenavn
        private List<string> HentBillettyperStringListe()
        {
            return new List<string> { "Student", "Barn" };
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
