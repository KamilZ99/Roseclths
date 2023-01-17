using Bookshop.DataAccess.Repository.Interfaces;
using Bookshop.Models;
using Bookshop.Models.ViewModels;
using BookshopWeb.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System.Linq.Expressions;

namespace Bookshop.Tests
{
    public class BookControllerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IWebHostEnvironment> _mockWebHostEnvironment;
        private readonly BookController _bookController;

        public BookControllerTests()
        {
            _mockUnitOfWork = GetIUnitOfWorkMock();
            _mockWebHostEnvironment = GetIWebHostEnvironmentMock();
            _bookController = new BookController(_mockUnitOfWork.Object, _mockWebHostEnvironment.Object) { TempData = new TempDataDictionary(new DefaultHttpContext(), new Mock<ITempDataProvider>().Object) };
        }

        public static Mock<IUnitOfWork> GetIUnitOfWorkMock()
        {
            var mock = new Mock<IUnitOfWork>();

            mock.Setup(r => r.BookRepository.GetFirstOrDefault(It.IsAny<Expression<Func<Book, bool>>>(), It.IsAny<string>()))
                .Returns(new Book() { Id = 1 });

            mock.Setup(r => r.CategoryRepository.GetFirstOrDefault(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<string>()))
                .Returns(new Category() { Id = 1 });


            mock.Setup(r => r.CoverTypeRepository.GetFirstOrDefault(It.IsAny<Expression<Func<CoverType, bool>>>(), It.IsAny<string>()))
                .Returns(new CoverType() { Id = 1 });

            return mock;
        }

        public static Mock<IWebHostEnvironment> GetIWebHostEnvironmentMock()
        {
            var mock = new Mock<IWebHostEnvironment>();

            mock.SetupProperty(m => m.WebRootPath, "");

            return mock;
        }

        [Fact]
        public void Index_ReturnsViewResult()
        {
            var result = _bookController.Index();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Upsert_WithoutId_ReturnsViewResult()
        {
            var result = _bookController.Upsert(id: null);

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Upsert_WithId_ReturnsViewResult()
        {
            _mockUnitOfWork
                .Setup(r => r.BookRepository.GetFirstOrDefault(It.IsAny<Expression<Func<Book, bool>>>(),"CoverType,Book"))
                .Returns(new Book());

            var result = _bookController.Upsert(1);

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Upsert_InvalidModelState_ReturnsViewResult()
        {
            _bookController.ModelState.AddModelError("", "Error");

            var result = _bookController.Upsert(new BookViewModel());

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Upsert_ValidModelState_RedirectsToAction()
        {
            var bookViewModel = new BookViewModel
            {
                Book = new Book()
            };

            var result = _bookController.Upsert(bookViewModel);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void GetAll_ReturnsJsonResult()
        {
            _mockUnitOfWork.Setup(r => r.BookRepository.GetAll(It.IsAny<string>()))
                .Returns(new Book[] { });

            var result = _bookController.GetAll();

            Assert.IsType<JsonResult>(result);
        }

        [Fact]
        public void Delete_InvalidId_ReturnsNotFound()
        {
            var result = _bookController.Delete(null);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Delete_ValidId_ReturnsOk()
        {
            var result = _bookController.Delete(1);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void Delete_ValidId_DeletesBook()
        {
            _bookController.Delete(1);

            _mockUnitOfWork.Verify(r => r.BookRepository.Remove(It.IsAny<Book>()), Times.Once());
            _mockUnitOfWork.Verify(r => r.Save(), Times.Once());
        }
    }
}
