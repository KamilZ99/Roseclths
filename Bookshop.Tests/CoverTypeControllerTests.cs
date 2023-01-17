using Bookshop.DataAccess.Repository.Interfaces;
using Bookshop.Models;
using BookshopWeb.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System.Linq.Expressions;

namespace Bookshop.Tests
{
    public class CoverTypeControllerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly CoverTypeController _coverTypeController;

        public CoverTypeControllerTests()
        {
            _mockUnitOfWork = GetIUnitOfWorkMock();
            _coverTypeController = new CoverTypeController(_mockUnitOfWork.Object) { TempData = new TempDataDictionary(new DefaultHttpContext(), new Mock<ITempDataProvider>().Object) };
        }

        public static Mock<IUnitOfWork> GetIUnitOfWorkMock()
        {
            var mock = new Mock<IUnitOfWork>();

            mock.Setup(r => r.CoverTypeRepository.GetFirstOrDefault(It.IsAny<Expression<Func<CoverType, bool>>>(), It.IsAny<string>()))
                .Returns(new CoverType() { Id = 1 });

            mock.Setup(r => r.CoverTypeRepository.GetAll(It.IsAny<string>()))
                .Returns(new List<CoverType> { new CoverType { Id = 1 } });

            mock.Setup(r => r.CoverTypeRepository.Add(It.IsAny<CoverType>()))
                .Callback((CoverType coverType) => coverType.Id = 2);

            mock.Setup(r => r.CoverTypeRepository.Update(It.IsAny<CoverType>()))
                .Callback((CoverType coverType) => coverType.Name = "UpdatedName");

            mock.Setup(r => r.Save());

            return mock;
        }

        [Fact]
        public void Index_ReturnsAViewResult_WithAListOfCoverTypes()
        {
            var result = _coverTypeController.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<CoverType>>(viewResult.ViewData.Model);
            var coverType = Assert.Single(model);
            Assert.Equal(1, coverType.Id);
        }

        [Fact]
        public void Create_ReturnsAViewResult()
        {
            var result = _coverTypeController.Create();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Create_Post_AddsCoverType_AndReturnsARedirect()
        {
            var coverType = new CoverType { Name = "TestName" };

            var result = _coverTypeController.Create(coverType);

            Assert.Equal(2, coverType.Id);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void Create_Post_InvalidModel_ReturnsAViewResult()
        {
            _coverTypeController.ModelState.AddModelError("Name", "Required");
            var coverType = new CoverType();

            var result = _coverTypeController.Create(coverType);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(coverType, viewResult.ViewData.Model);
        }

        [Fact]
        public void Edit_ReturnsAViewResult_WithTheCoverType()
        {
            var result = _coverTypeController.Edit(1);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CoverType>(viewResult.ViewData.Model);
            Assert.Equal(1, model.Id);
        }

        [Fact]
        public void Edit_InvalidId_ReturnsNotFound()
        {
            var result = _coverTypeController.Edit(0);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Edit_Post_UpdatesCoverType_AndReturnsARedirect()
        {
            var coverType = new CoverType { Id = 1, Name = "TestName" };

            var result = _coverTypeController.Edit(coverType);

            Assert.Equal("UpdatedName", coverType.Name);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void Edit_Post_InvalidModel_ReturnsAViewResult()
        {
            _coverTypeController.ModelState.AddModelError("Name", "Required");
            var coverType = new CoverType { Id = 1 };

            var result = _coverTypeController.Edit(coverType);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(coverType, viewResult.ViewData.Model);
        }


        [Fact]
        public void Delete_DeletesCoverType_AndReturnsARedirect()
        {
            var coverType = new CoverType { Id = 1 };

            var result = _coverTypeController.Delete(1);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void Delete_InvalidId_ReturnsRedirect()
        {
            var result = _coverTypeController.Delete(0);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
    }
}
