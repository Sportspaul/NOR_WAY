using System;
using System.Collections.Generic;
using System.Net;
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

        [Fact]
        public async Task HentAlleRuteStopp_Riktig()
        {
            // arrange
           List<RuteStoppModel> ruteStoppene = RuteStoppListe();
            mockRepo.Setup(rs => rs.HentRuteStopp("NW2")).ReturnsAsync(RuteStoppListe());

            //act 
            var resultat = await ruteStoppController.HentRuteStopp("NW2") as OkObjectResult;
            List<RuteStoppModel> faktiskRuteStoppene = (List<RuteStoppModel>) resultat.Value;

            //assert
            for (int i = 0; i < ruteStoppene.Count; i++)
            {
                Assert.Equal(ruteStoppene[i].ToString(), faktiskRuteStoppene[i].ToString());
            }
        }

        [Fact]
        public async Task HentAlleRuteStopp_IkkeFunnet()
        {
            // arrange
            string linjekode = "NW2";
            List<RuteStoppModel> ruteStoppene = new List<RuteStoppModel>();
            mockRepo.Setup(rs => rs.HentRuteStopp(linjekode)).ReturnsAsync(ruteStoppene);

            //act 
            var resultat = await ruteStoppController.HentRuteStopp(linjekode) as NotFoundObjectResult;
           
            //assert
            Assert.Equal($"Ingen RuteStopp ble funnet med linjekode: {linjekode}", resultat.Value);
        }

        [Fact]
        public async Task HentAlleRuteStopp_Regex()
        {
            // arrange
            List<RuteStoppModel> ruteStoppene = RuteStoppListe();
            mockRepo.Setup(rs => rs.HentRuteStopp("NW2")).ReturnsAsync(RuteStoppListe());
            ruteStoppController.ModelState.AddModelError("Stoppnavn", "Feil i inputvalideringen på server");
            //act 
            var resultat = await ruteStoppController.HentRuteStopp("NW2") as BadRequestObjectResult;
           
            //assert
            Assert.Equal("Feil i inputvalideringen på server", resultat.Value);
        }


        //Metoder som krever innlogging

        [Fact]
        public async Task FjernRuteStopp_Riktig()
        {
            // arrange
            int id = 1;
            mockRepo.Setup(rs => rs.FjernRuteStopp(id)).ReturnsAsync(true);
            MockSession(_innlogget);
            //act 
            var resultat = await ruteStoppController.FjernRuteStopp(id) as OkObjectResult;

            //assert
            Assert.Equal((int) HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal($"RuteStopp med id: {id}, ble slettet", resultat.Value);
        }

        [Fact]
        public async Task FjernRuteStopp_IkkeTilgang()
        {
            // arrange
            int id = 1;
            mockRepo.Setup(rs => rs.FjernRuteStopp(id)).ReturnsAsync(true);
            MockSession(_ikkeInnlogget);
            //act 
            var resultat = await ruteStoppController.FjernRuteStopp(id) as UnauthorizedObjectResult;

            //assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke innlogget", resultat.Value);
        }

        [Fact]
        public async Task FjernRuteStopp_Feil()
        {
            // arrange
            int id = 1;
            mockRepo.Setup(rs => rs.FjernRuteStopp(id)).ReturnsAsync(false);
            MockSession(_innlogget);
            //act 
            var resultat = await ruteStoppController.FjernRuteStopp(id) as BadRequestObjectResult;

            //assert
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal($"RuteStopp med id: {id}, kunne ikke slettes", resultat.Value);
        }

        [Fact]
        public async Task FjernRuteStopp_Regex()
        {
            // arrange
            int id = 1;
            mockRepo.Setup(rs => rs.FjernRuteStopp(id)).ReturnsAsync(true);
            ruteStoppController.ModelState.AddModelError("Linjekode", "Feil i inputvalideringen på server");
            MockSession(_innlogget);

            //act 
            var resultat = await ruteStoppController.FjernRuteStopp(id) as BadRequestObjectResult;

            //assert
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
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

        private List<RuteStoppModel> RuteStoppListe()
        {
            RuteStoppModel rsm1 = new RuteStoppModel
            {
                Id = 1,
                Stoppnavn = "Bergen",
                StoppNummer = 1,
                MinutterTilNesteStopp = 30
            };
            RuteStoppModel rsm2 = new RuteStoppModel
            {
                Id = 2,
                Stoppnavn = "Vadheim",
                StoppNummer = 2,
                MinutterTilNesteStopp = 30
            };
            RuteStoppModel rsm3 = new RuteStoppModel
            {
                Id = 3,
                Stoppnavn = "Trondheim",
                StoppNummer = 3,
                MinutterTilNesteStopp = 30
            };
            List<RuteStoppModel> rutestoppene = new List<RuteStoppModel> { rsm1, rsm2, rsm3 };
            return rutestoppene;
        }

        private void MockSession(string innlogging)
        {
            mockSession[_innlogget] = innlogging;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            ruteStoppController.ControllerContext.HttpContext = mockHttpContext.Object;
        }
    }
}
