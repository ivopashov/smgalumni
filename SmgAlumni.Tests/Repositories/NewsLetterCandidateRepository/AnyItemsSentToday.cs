using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SmgAlumni.EF.DAL;
using SmgAlumni.EF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SmgAlumni.Tests.Repositories.NewsLetterCandidateRepository
{
    [TestClass]
    public class AnyItemsSentToday
    {
        private Mock<SmgAlumniContext> context = new Mock<SmgAlumniContext>();

        [TestMethod]
        public void ReturnFalseWhenAllItemsSentYesterday()
        {
            //Arrange
            var data = new List<NewsLetterCandidate>
            {
                new NewsLetterCandidate { Sent=true,SentOn=DateTime.Now.AddDays(-1)},
            }.AsQueryable();

            InitializeContext(data);
            var repo = new Data.Repositories.NewsLetterCandidateRepository(context.Object);

            //Act
            var result = repo.AnyItemsSentToday();

            //Assert
            Assert.AreEqual(result, false);
        }

        [TestMethod]
        public void ReturnFalseWhenAllUnsent()
        {
            //Arrange
            var data = new List<NewsLetterCandidate>
            {
                new NewsLetterCandidate { Sent=false},
            }.AsQueryable();

            InitializeContext(data);
            var repo = new Data.Repositories.NewsLetterCandidateRepository(context.Object);

            //Act
            var result = repo.AnyItemsSentToday();

            //Assert
            Assert.AreEqual(result, false);
        }

        [TestMethod]
        public void ReturnTrueWhenItemsSentTodayPresent()
        {
            //Arrange
            var data = new List<NewsLetterCandidate>
            {
                new NewsLetterCandidate { Sent=true, SentOn=DateTime.Now},
            }.AsQueryable();

            InitializeContext(data);
            var repo = new Data.Repositories.NewsLetterCandidateRepository(context.Object);

            //Act
            var result = repo.AnyItemsSentToday();

            //Assert
            Assert.AreEqual(result, true);
        }

        private void InitializeContext(IQueryable<NewsLetterCandidate> data)
        {
            var mockSet = new Mock<DbSet<NewsLetterCandidate>>();
            mockSet.As<IQueryable<NewsLetterCandidate>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<NewsLetterCandidate>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<NewsLetterCandidate>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<NewsLetterCandidate>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<SmgAlumniContext>();
            context.Setup(c => c.NewsLetterCandidates).Returns(mockSet.Object);
        }
    }
}
