using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SmgAlumni.EF.DAL;
using SmgAlumni.EF.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SmgAlumni.Tests.Repositories.UserRepository
{
    [TestClass]
    public class UsersByEmail
    {
        private Mock<SmgAlumniContext> context = new Mock<SmgAlumniContext>();

        [TestInitialize]
        public void Init()
        {
            var data = new List<User>
            {
                new User { Email = "ivo@ivo.bg" },
                new User { Email = "Ivo@Ivo.bg" },
                new User { Email = "Gosho@gosho.bg" },
            }.AsQueryable();

            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<SmgAlumniContext>();
            context.Setup(c => c.Users).Returns(mockSet.Object);
        }

        [TestMethod]
        public void ReturnTheCorrectUsers()
        {
            //Arrange
            var repo = new Data.Repositories.UserRepository(context.Object);

            //Act
            var result = repo.UsersByEmail("ivo@ivo.bg");
            var result_withuppercaseinput = repo.UsersByEmail("IVO@IVO.BG");

            //Assert
            Assert.AreEqual(result.Count(), 2);
            Assert.AreEqual(result_withuppercaseinput.Count(), 2);
        }
    }
}
