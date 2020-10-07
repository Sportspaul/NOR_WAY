using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NOR_WAY.Controllers;
using NOR_WAY.DAL;
using NOR_WAY.DAL.Interfaces;
using Xunit;
using Xunit.Abstractions;

namespace NOR_WAY_Tests
{
    public class AdminControllerTests
    {
        private readonly Mock<IAdminRepository> mockRepo = new Mock<IAdminRepository>();
        private readonly Mock<ILogger<AdminController>> mockLog = new Mock<ILogger<AdminController>>();
        private readonly ITestOutputHelper output;

        public AdminControllerTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        // TODO: Fjern Eksempel
        [Fact]
        public void HeiVerden()
        {

        }
    }
}
