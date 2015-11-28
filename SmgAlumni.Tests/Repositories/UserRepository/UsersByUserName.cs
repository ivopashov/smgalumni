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
    public class UsersByUserName
    {
        private Mock<SmgAlumniContext> context = new Mock<SmgAlumniContext>();

        [TestInitialize]
        public void Init()
        {
            var data = new List<User>
            {
                new User { UserName = "pesho" },
                new User { UserName = "Pesho" },
                new User { UserName= "Gosho" },
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
        public void ReturnTheCorrectUser()
        {
            //Arrange
            var repo = new Data.Repositories.UserRepository(context.Object);

            //Act
            var result = repo.UsersByUserName("pesho");

            //Assert
            Assert.AreEqual(result.Count(), 1);
        }

        [TestMethod]
        public void ReturnEmptyCollectionIfNoMatch()
        {
            //Arrange
            var repo = new Data.Repositories.UserRepository(context.Object);

            //Act
            var result = repo.UsersByUserName("pesho1");

            //Assert
            Assert.AreEqual(result.Count(), 0);
        }

    }
}
