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
    public class UsersByName
    {
        private Mock<SmgAlumniContext> context = new Mock<SmgAlumniContext>();

        [TestInitialize]
        public void Init()
        {
            var data = new List<User>
            {
                new User { FirstName = "Abc", MiddleName = "Klm", LastName = "Xyz" },
                new User { FirstName = "Georgi", MiddleName = "Georgiev", LastName = "Georgiev" },
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
        public void ReturnUser_IfFirstNameMatch()
        {
            //Arrange
            var repo = new Data.Repositories.UserRepository(context.Object);

            //Act
            var lowercase_result = repo.UsersByName("abc");
            var uppercase_result = repo.UsersByName("ABC");
            var result = repo.UsersByName("Abc");

            //Assert
            Assert.AreEqual(result.Count(), 1);
            Assert.AreEqual(uppercase_result.Count(), 1);
            Assert.AreEqual(lowercase_result.Count(), 1);
        }
    }
}
