using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NOR_WAY.Controllers;
using NOR_WAY.DAL;
using Xunit;
using Xunit.Abstractions;

namespace NOR_WAY_Tests
{
    public class AdminTests
    {
        private readonly Mock<IAdminRepository> mockRepo = new Mock<IAdminRepository>();
        private readonly Mock<ILogger<AdminController>> mockLog = new Mock<ILogger<AdminController>>();
        private readonly ITestOutputHelper output;

        public AdminTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        // TODO: Fjern Eksempel
        [Fact]
        public void HeiVerden()
        {
            mockRepo.Setup(b => b.HeiVerden()).ReturnsAsync("Hei Verden");
            var adminController = new AdminController(mockRepo.Object, mockLog.Object);
            var resultat = adminController.HeiVerden() as OkObjectResult;
        }
    }
}
