using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SmgAlumni.EF.DAL;
using SmgAlumni.EF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SmgAlumni.Tests.Repositories.AttachmentRepository
{
    [TestClass]
    public class FindByTempKey
    {
        private Mock<SmgAlumniContext> context = new Mock<SmgAlumniContext>();
        private Guid searchedForGuid = Guid.NewGuid();

        [TestInitialize]
        public void Init()
        {
            var data = new List<Attachment>
            {
               new Attachment() {TempKey=searchedForGuid},
               new Attachment() {TempKey=Guid.NewGuid()},
               new Attachment() {TempKey=null},
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Attachment>>();
            mockSet.As<IQueryable<Attachment>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Attachment>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Attachment>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Attachment>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<SmgAlumniContext>();
            context.Setup(c => c.Attachments).Returns(mockSet.Object);
        }

        [TestMethod]
        public void FindTheRightAttachment()
        {
            //Arrange
            var repo = new Data.Repositories.AttachmentRepository(context.Object);

            //Act
            var result = repo.FindByTempKey(searchedForGuid);

            //Assert
            Assert.AreNotEqual(result, null);
        }
    }
}
