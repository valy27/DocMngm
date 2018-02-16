using DocumentManagement.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Text;
using DocumentManagement.Services;
using DocumentManagement.Infrastructure.UserResolver;
using DocumentManagement.Repository.Models;
using DocumentManagement.Repository.Models.Identity;
using DocumentManagement.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using Document = DocumentManagement.Repository.Models.Document;


namespace DocumentManagement.Services.UnitTests
{
   public class DocumentServiceTests
    {
        private readonly Mock<IGenericRepository<Document>> _documentRepositoryMock;
        private readonly Mock<IUserResolverService> _userResolverServiceMock;
        private readonly Mock<IAccountService> _accountServiceMock;
        private readonly Mock<IFileService> _fileServiceMock;

        private readonly List<Repository.Models.Document> _testData;

        private readonly DocumentService _target;
        
        public DocumentServiceTests()
        {
            _documentRepositoryMock = new Mock<IGenericRepository<Document>>();
            _userResolverServiceMock = new Mock<IUserResolverService>();
            _accountServiceMock = new Mock<IAccountService>();
            _fileServiceMock = new Mock<IFileService>();

            _testData = new List<Repository.Models.Document>
                {
                    new Repository.Models.Document
                    {
                        Id = 1,
                        Name = "1001_Name",
                        Created = DateTime.Parse("2018-02-16 10:05:30.3575787"),
                        FileSize = 15,
                        Descripion = "Document id 1",
                        OwnerId = 1,
                        Owner = new Account {IdentityId = "1", Identity = new ApplicationUser {Id = "1"}}
                    },
                    new Repository.Models.Document
                    {
                        Id = 2,
                        Name = "1002_Name",
                        Created = DateTime.Parse("2018-02-16 10:06:22.7670738"),
                        FileSize = 16,
                        Descripion = "Document id 2",
                        OwnerId = 1,
                        Owner = new Account {IdentityId = "1", Identity = new ApplicationUser {Id = "1"}}
                    },
                    new Repository.Models.Document
                    {
                        Id = 3,
                        Name = "1003_Name",
                        Created = DateTime.Parse("2018-02-16 10:06:58.8223058"),
                        FileSize = 17,
                        Descripion = "Document id 3",
                        OwnerId = 2,
                        Owner = new Account {IdentityId = "2", Identity = new ApplicationUser {Id = "2"}}
                    }
                };

            _target = new DocumentService(_documentRepositoryMock.Object, _userResolverServiceMock.Object, _accountServiceMock.Object, _fileServiceMock.Object);
        }

        [Fact]
        public void GetAll_NoUserConnected_ReturnsEmptyCollection()
        {
            //Arrange
            _userResolverServiceMock.Setup(s => s.GetUser()).Returns((ApplicationUser) null);

            //Act
            var result = _target.GetAll();

            //Assert
            Assert.True(!result.Any());
        }

        [Fact]
        public void GetAll_NoUserRoles_ReturnsEmptyCollection()
        {
            //Arrange
            var user = new ApplicationUser();
            _userResolverServiceMock.Setup(s => s.GetUser()).Returns(user);
            _userResolverServiceMock.Setup(s => s.GetUserRoles(user)).Returns(new List<string>{});
       
            //Act
            var result = _target.GetAll();

            //Assert
            Assert.True(!result.Any());
        }

        [Fact]
        public void GetAll_RegularUser_ReturnsOneDocument()
        {
            //Arrange
            var user= new ApplicationUser
            {
                Id ="2"
            };
            _userResolverServiceMock.Setup(s => s.GetUser()).Returns(user);
            _userResolverServiceMock.Setup(s => s.GetUserRoles(user)).Returns(new List<string>{"User"});
            _documentRepositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<Repository.Models.Document, bool>>>(), null, ""))
                .Returns(_testData.AsQueryable());

            //Act
            var result = _target.GetAll();

            //Assert
            Assert.True(result.Count() == 1);
        }

        [Fact]
        public void GetAll_AdminUser_ReturnsThreeDocuments()
        {
            //Arrange
            var user = new ApplicationUser
            {
                Id = "1"
            };
            _userResolverServiceMock.Setup(s => s.GetUser()).Returns(user);
            _userResolverServiceMock.Setup(s => s.GetUserRoles(user)).Returns(new List<string> { "Admin" });
            _documentRepositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<Repository.Models.Document, bool>>>(), null, ""))
                .Returns(_testData.AsQueryable());

            //Act
            var result = _target.GetAll();

            //Assert
            Assert.True(result.Count() == 3);
        }

        [Fact]
        public void Exist_ReceiveAnValidDocumentId_ReturnsTrue()
        {
            //Arrange
            _documentRepositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<Repository.Models.Document, bool>>>(), null, ""))
                .Returns(_testData.AsQueryable());

            //Act 
            var result = _target.Exists(2);

            Assert.True(result);
        }

        [Fact]
        public void Exist_ReceiveAnValidDocumentName_ReturnsTrue()
        {
            //Arrange
            _documentRepositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<Repository.Models.Document, bool>>>(), null, ""))
                .Returns(_testData.AsQueryable());

            //Act
            var result = _target.Exists("1002_Name");

            Assert.True(result);
        }

        [Fact]
        public void Exist_ReceiveAnInvalidDocumentId_ReturnsFalse()
        {
            //Arrange
            _documentRepositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<Repository.Models.Document, bool>>>(), null, ""))
                .Returns(_testData.AsQueryable());

            //Act
            var result = _target.Exists(4);

            Assert.False(result);
        }

        [Fact]
        public void Exist_ReceiveAnInvalidDocumentName_ReturnsFalse()
        {
            //Arrange
            _documentRepositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<Repository.Models.Document, bool>>>(), null, ""))
                .Returns(_testData.AsQueryable());

            //Act
            var result = _target.Exists("100_Name");

            Assert.False(result);
        }
    }
}
