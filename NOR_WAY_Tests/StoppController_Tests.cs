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
    public class StoppController_Tests
    {
        private readonly Mock<IStoppRepository> mockRepo = new Mock<IStoppRepository>();
        private readonly Mock<ILogger<StoppController>> mockLogCtr = new Mock<ILogger<StoppController>>();
        private readonly StoppController stoppController;

        public StoppController_Tests()
        {
            stoppController = new StoppController(mockRepo.Object, mockLogCtr.Object);
        }

        /* Enhetstester for HentAlleStopp */

        /* Tester at ikke listen med Stopp fra BussRepo endrer seg i controlleren
           for HentAlleStopp() */

        [Fact]
        public async Task HentAlleStopp_RiktigeVerdier()
        {
            // Arrange
            List<Stopp> forventedeStopp = HentStoppListe();
            mockRepo.Setup(b => b.HentAlleStopp()).ReturnsAsync(HentStoppListe());

            // Act
            var resultat = await stoppController.HentAlleStopp() as OkObjectResult;
            List<Stopp> faktiskeStopp = (List<Stopp>)resultat.Value;

            // Assert
            Assert.Equal(forventedeStopp.Count, faktiskeStopp.Count);   // Tester om listene er like lange
            // Tester om alle verdiene er like i alle elementene
            for (int i = 0; i < forventedeStopp.Count; i++)
            {
                Assert.Equal(forventedeStopp[i].Id, faktiskeStopp[i].Id);
                Assert.Equal(forventedeStopp[i].Navn, faktiskeStopp[i].Navn);
            }
        }

        /* Enhetstester for FinnMuligeStartStopp */

        /* Tester at ikke listen med Stopp fra BussRepo endrer seg i controlleren
           for FinnMuligeStartStopp() */

        [Fact]
        public async Task FinnMuligStartStopp_RiktigeVerdier()
        {
            // Arrange
            StoppModel StoppModel = HentUgyldigInnStopp();
            List<Stopp> forventedeStopp = HentStoppListe();
            mockRepo.Setup(b => b.FinnMuligeStartStopp(StoppModel)).ReturnsAsync(HentStoppListe());

            // Act
            var resultat = await stoppController.FinnMuligeStartStopp(StoppModel) as OkObjectResult;
            List<Stopp> faktiskeStopp = (List<Stopp>)resultat.Value;

            // Assert
            Assert.Equal(forventedeStopp.Count, faktiskeStopp.Count);   // Tester om listene er like lange
            // Tester om alle verdiene er like i alle elementeneƒ
            for (int i = 0; i < forventedeStopp.Count; i++)
            {
                Assert.Equal(forventedeStopp[i].Id, faktiskeStopp[i].Id);
                Assert.Equal(forventedeStopp[i].Navn, faktiskeStopp[i].Navn);
            }
        }

        // Tester at FinnMulgeStartStopp i controlleren håndterer Tom liste
        [Fact]
        public async Task FinnMuligeStartStopp_TomListe()
        {
            // Arrange
            StoppModel StoppModel = HentInnStopp();
            List<Stopp> tomStoppListe = new List<Stopp>();
            mockRepo.Setup(b => b.FinnMuligeStartStopp(StoppModel)).ReturnsAsync(tomStoppListe);

            // Act
            var resultat = await stoppController.FinnMuligeStartStopp(StoppModel) as NotFoundObjectResult;

            // Assert
            Assert.Equal("Ingen mulige StartStopp ble funnet", resultat.Value);
        }

        // Tester at FinnMulgeStartStopp i controlleren håndterer InvalidModelState
        [Fact]
        public async Task FinnMuligeStartStopp_RegEx()
        {
            // Arrange
            StoppModel StoppModel = HentUgyldigInnStopp();
            List<Stopp> forventedeStopp = HentStoppListe();
            mockRepo.Setup(b => b.FinnMuligeStartStopp(StoppModel)).ReturnsAsync(forventedeStopp);
            stoppController.ModelState.AddModelError("StartStopp", "Feil i inputvalideringen på server");
            stoppController.ModelState.AddModelError("SluttStopp", "Feil i inputvalideringen på server");
            stoppController.ModelState.AddModelError("Dato", "Feil i inputvalideringen på server");
            stoppController.ModelState.AddModelError("Tidspunkt", "Feil i inputvalideringen på server");

            // Act
            var resultat = await stoppController.FinnMuligeStartStopp(StoppModel) as BadRequestObjectResult;

            // Assert
            Assert.Equal("Feil i inputvalideringen på server", resultat.Value);
        }

        /* Enhetstester for FinnMuligeSluttStopp */

        /* Tester at ikke listen med Stopp fra BussRepo endrer seg i controlleren
           for FinnMuligeSluttStopp() */

        [Fact]
        public async Task FinnMuligSluttStopp_RiktigeVerdier()
        {
            // Arrange
            List<Stopp> forventet = HentStoppListe();
            StoppModel StoppModel = new StoppModel { Navn = "Bergen" };
            mockRepo.Setup(b => b.FinnMuligeSluttStopp(StoppModel)).ReturnsAsync(HentStoppListe());

            // Act
            var resultat = await stoppController.FinnMuligeSluttStopp(StoppModel) as OkObjectResult;
            List<Stopp> faktisk = (List<Stopp>)resultat.Value;

            // Assert
            Assert.Equal(forventet.Count, faktisk.Count);   // Tester om listene er like lange
            // Tester om alle verdiene er like i alle elementene
            for (int i = 0; i < forventet.Count; i++)
            {
                Assert.Equal(forventet[i].Id, faktisk[i].Id);
                Assert.Equal(forventet[i].Navn, faktisk[i].Navn);
            }
        }

        // Tester at FinnMulgeStartStopp i controlleren håndterer Tom liste
        [Fact]
        public async Task FinnMuligeSluttStopp_TomListe()
        {
            // Arrange
            StoppModel StoppModel = HentInnStopp();
            List<Stopp> tomStoppListe = new List<Stopp>();
            mockRepo.Setup(b => b.FinnMuligeSluttStopp(StoppModel)).ReturnsAsync(tomStoppListe);

            // Act
            var resultat = await stoppController.FinnMuligeSluttStopp(StoppModel) as NotFoundObjectResult;

            // Assert
            Assert.Equal("Ingen mulige SluttStopp ble funnet", resultat.Value);
        }

        // Tester at FinnMuligeSluttStopp i controlleren håndterer InvalidModelState
        [Fact]
        public async Task FinnMuligeSluttStopp_RegEx()
        {
            // Arrange
            StoppModel StoppModel = HentUgyldigInnStopp();
            List<Stopp> forventedeStopp = HentStoppListe();
            mockRepo.Setup(b => b.FinnMuligeSluttStopp(StoppModel)).ReturnsAsync(forventedeStopp);
            stoppController.ModelState.AddModelError("StartStopp", "Feil i inputvalideringen på server");
            stoppController.ModelState.AddModelError("SluttStopp", "Feil i inputvalideringen på server");
            stoppController.ModelState.AddModelError("Dato", "Feil i inputvalideringen på server");
            stoppController.ModelState.AddModelError("Tidspunkt", "Feil i inputvalideringen på server");

            // Act
            var resultat = await stoppController.FinnMuligeSluttStopp(StoppModel) as BadRequestObjectResult;

            // Assert
            Assert.Equal("Feil i inputvalideringen på server", resultat.Value);
        }

        // Returnerer en List med Stopp-objekter
        private List<Stopp> HentStoppListe()
        {
            Stopp stopp1 = new Stopp { Id = 1, Navn = "Bergen" };
            Stopp stopp2 = new Stopp { Id = 1, Navn = "Oslo" };
            Stopp stopp3 = new Stopp { Id = 1, Navn = "Vadheim" };
            Stopp stopp4 = new Stopp { Id = 1, Navn = "Trondheim" };
            return new List<Stopp> { stopp1, stopp2, stopp3, stopp4 };
        }

        private StoppModel HentInnStopp()
        {
            return new StoppModel { Navn = "Bergen" };
        }

        // Returnerer et StoppModel-objekt med ugyldig Navn
        private StoppModel HentUgyldigInnStopp()
        {
            return new StoppModel { Navn = "" };
        }
    }
}