using System;
using System.Collections.Generic;
using System.Text;

namespace NOR_WAY_Tests
{
    class RuterController_Tests
    {
        /* Enhetstester for HentAlleRuter */

        /* Tester at ikke listen med Stopp fra BussRepo endrer seg i controlleren
           for HentAlleRuter() */
        [Fact]
        public async Task HentAlleRuter_RiktigeVerdier()
        {
            // Arrange
            List<RuteData> forventet = HentRuteDataListe();
            mockRepo.Setup(b => b.HentAlleRuter()).ReturnsAsync(HentRuteDataListe());

            // Act
            var resultat = await bussController.HentAlleRuter() as OkObjectResult;
            List<RuteData> faktisk = (List<RuteData>)resultat.Value;

            // Assert
            Assert.Equal(forventet.Count, faktisk.Count);   // Tester om listene er like lange
            // Tester om alle verdiene er like i alle elementene
            for (int i = 0; i < forventet.Count; i++)
            {
                Assert.Equal(forventet[i].Stoppene, faktisk[i].Stoppene);
                Assert.Equal(forventet[i].MinutterTilNesteStopp, faktisk[i].MinutterTilNesteStopp);
                Assert.Equal(forventet[i].Linjekode, faktisk[i].Linjekode);
                Assert.Equal(forventet[i].Rutenavn, faktisk[i].Rutenavn);
                Assert.Equal(forventet[i].Startpris, faktisk[i].Startpris);
                Assert.Equal(forventet[i].TilleggPerStopp, faktisk[i].TilleggPerStopp);
            }
        }

        [Fact]
        public async Task HentAlleRuter_Null()
        {
            // Arrange 
            mockRepo.Setup(b => b.HentAlleRuter()).ReturnsAsync(() => null);

            //Act
            var resultat = await bussController.HentAlleRuter() as NotFoundObjectResult;

            // Assert
            Assert.Equal("Rutene ble ikke funnet", resultat.Value);
        }

        // Returnerer en List med RuteData-objekter
        private List<RuteData> HentRuteDataListe()
        {
            List<string> stoppene1 = new List<string> { "Bergen", "Vaheim", "Trondheim" };
            List<string> stoppene2 = new List<string> { "Oslo", "Røros", "Trondheim" };
            List<string> stoppene3 = new List<string> { "Kristiansand", "Stavanger", "Molde" };
            List<int> minuttListe1 = new List<int> { 20, 25, 35 };
            List<int> minuttListe2 = new List<int> { 20, 23, 35 };
            List<int> minuttListe3 = new List<int> { 20, 55, 60 };
            RuteData ruteData1 = new RuteData { Stoppene = stoppene1, MinutterTilNesteStopp = minuttListe1, Linjekode = "NW123", Rutenavn = "Bussturen", Startpris = 79, TilleggPerStopp = 25 };
            RuteData ruteData2 = new RuteData { Stoppene = stoppene2, MinutterTilNesteStopp = minuttListe2, Linjekode = "NW600", Rutenavn = "Ekspressruta", Startpris = 100, TilleggPerStopp = 15 };
            RuteData ruteData3 = new RuteData { Stoppene = stoppene3, MinutterTilNesteStopp = minuttListe3, Linjekode = "NW007", Rutenavn = "Bondespressen", Startpris = 50, TilleggPerStopp = 35 };
            return new List<RuteData> { ruteData1, ruteData2, ruteData3 };
        }



    }
}
