using System;
using System.Collections.Generic;
using System.Text;
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
    public class RuteStoppController_Tests
    {
        private const string _innlogget = "Innlogget";
        private const string _ikkeInnlogget = "";

        private readonly Mock<IRuteStoppRepository> mockRepo = new Mock<IRuteStoppRepository>();
        private readonly Mock<ILogger<RuteStoppController>> mockLogCtr = new Mock<ILogger<RuteStoppController>>();
        private readonly RuteStoppController ruteStoppController;

        // Session
        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();

        public RuteStoppController_Tests()
        {
            ruteStoppController = new RuteStoppController(mockRepo.Object, mockLogCtr.Object);
        }

        [Fact]
        public async Task HentEtRuteStopp_Riktig()
        {
            // arrange
            NyRuteStopp utRuteStopp = nyttRuteStopp();
            mockRepo.Setup(rs => rs.HentEtRuteStopp(1)).ReturnsAsync(utRuteStopp);

            //act 
            var resultat = await ruteStoppController.HentEtRuteStopp(1) as OkObjectResult;
            NyRuteStopp faktiskRuteStopp = (NyRuteStopp)resultat.Value;

            //assert
            Assert.Equal(utRuteStopp.ToString(), faktiskRuteStopp.ToString());
        }

        [Fact]
        public async Task HentEtRuteStopp_Null()
        {
            // arrange
            NyRuteStopp utRuteStopp = nyttRuteStopp();
            mockRepo.Setup(rs => rs.HentEtRuteStopp(1)).ReturnsAsync(() => null);

            //act 
            var resultat = await ruteStoppController.HentEtRuteStopp(1) as NotFoundObjectResult;
            
            //assert
            Assert.Equal($"RuteStoppet med id: {utRuteStopp.Id}, bli ikke funnet", resultat.Value);
        }

        [Fact]
        public async Task HentEtRuteStopp_Regex()
        {
            // arrange
            NyRuteStopp utRuteStopp = nyttRuteStopp();
            mockRepo.Setup(rs => rs.HentEtRuteStopp(1)).ReturnsAsync(() => null);
            ruteStoppController.ModelState.AddModelError("Stoppnavn", "Feil i inputvalideringen på server");

            //act 
            var resultat = await ruteStoppController.HentEtRuteStopp(1) as BadRequestObjectResult;

            //assert
            Assert.Equal("Feil i inputvalideringen på server", resultat.Value);
        }




        //Hjelpemetoder

        private NyRuteStopp nyttRuteStopp()
        {
            return new NyRuteStopp
            {
                Id = 1,
                Stoppnavn = "Oslo",
                StoppNummer = 1,
                MinutterTilNesteStopp = 30,
                Linjekode = "NW1"
            };
        }

        private void MockSession(string innlogging)
        {
            mockSession[_innlogget] = innlogging;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            ruteStoppController.ControllerContext.HttpContext = mockHttpContext.Object;
        }
    }
}
