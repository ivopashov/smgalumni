using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SmgAlumni.EF.DAL;
using SmgAlumni.EF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmgAlumni.Tests.Repositories.NewsRepository
{
    [TestClass]
    public class Page
    {
        private Mock<SmgAlumniContext> context = new Mock<SmgAlumniContext>();

        [TestInitialize]
        public void Init()
        {
            var data = new List<News>
            {
                new News { DateCreated = DateTime.Now.AddDays(3) },
                new News { DateCreated = DateTime.Now.AddDays(2) },
                new News { DateCreated = DateTime.Now.AddDays(6) },
                new News { DateCreated = DateTime.Now.AddDays(7) },
                new News { DateCreated = DateTime.Now.AddDays(8) },
                new News { DateCreated = DateTime.Now.AddDays(9) },
                new News { DateCreated = DateTime.Now.AddDays(10) },
                new News { DateCreated = DateTime.Now.AddDays(5) },
                new News { DateCreated = DateTime.Now.AddDays(4) },
                new News { DateCreated = DateTime.Now.AddDays(1) },
                new News { DateCreated = DateTime.Now.AddDays(11) },
                new News { DateCreated = DateTime.Now.AddDays(12) },
            }.AsQueryable();

            var mockSet = new Mock<DbSet<News>>();
            mockSet.As<IQueryable<News>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<News>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<News>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<News>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<SmgAlumniContext>();
            context.Setup(c => c.NewsCollection).Returns(mockSet.Object);
        }

        [TestMethod]
        public void PageCorrectlyWithOrderByDesc()
        {
            //Arrange
            var repo = new Data.Repositories.NewsRepository(context.Object);

            //Act
            var result = repo.Page(0,6);

            //Assert
            Assert.AreEqual(result.Count(), 6);
            Assert.AreEqual(result.ToList()[0].DateCreated.Date, DateTime.Now.AddDays(12).Date);
            Assert.AreEqual(result.ToList()[5].DateCreated.Date, DateTime.Now.AddDays(7).Date);
        }

        [TestMethod]
        public void PageCorrectlyWithOrderByDescAndSkip()
        {
            //Arrange
            var repo = new Data.Repositories.NewsRepository(context.Object);

            //Act
            var result = repo.Page(6, 6);

            //Assert
            Assert.AreEqual(result.Count(), 6);
            Assert.AreEqual(result.ToList()[0].DateCreated.Date, DateTime.Now.AddDays(6).Date);
            Assert.AreEqual(result.ToList()[5].DateCreated.Date, DateTime.Now.AddDays(1).Date);
        }

        [TestMethod]
        public void PageCorrectlyWithOrderByAsc()
        {
            //Arrange
            var repo = new Data.Repositories.NewsRepository(context.Object);

            //Act
            var result = repo.Page(0, 6,false);

            //Assert
            Assert.AreEqual(result.Count(), 6);
            Assert.AreEqual(result.ToList()[0].DateCreated.Date, DateTime.Now.AddDays(1).Date);
            Assert.AreEqual(result.ToList()[5].DateCreated.Date, DateTime.Now.AddDays(6).Date);
        }

        [TestMethod]
        public void PageCorrectlyWithOrderByAscAndSkip()
        {
            //Arrange
            var repo = new Data.Repositories.NewsRepository(context.Object);

            //Act
            var result = repo.Page(6, 6, false);

            //Assert
            Assert.AreEqual(result.Count(), 6);
            Assert.AreEqual(result.ToList()[0].DateCreated.Date, DateTime.Now.AddDays(7).Date);
            Assert.AreEqual(result.ToList()[5].DateCreated.Date, DateTime.Now.AddDays(12).Date);
        }
    }
}
