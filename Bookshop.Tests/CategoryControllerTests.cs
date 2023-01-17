using Bookshop.DataAccess.Repository.Interfaces;
using Bookshop.Models;
using BookshopWeb.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bookshop.Tests
{
    public class CategoryControllerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly CategoryController _categoryController;

        public CategoryControllerTests()
        {
            _mockUnitOfWork = GetIUnitOfWorkMock();
            _categoryController = new CategoryController(_mockUnitOfWork.Object) { TempData = new TempDataDictionary(new DefaultHttpContext(), new Mock<ITempDataProvider>().Object) };
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

        [Fact]
        public void Index_ReturnsViewResult()
        {
            var result = _categoryController.Index();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Create_ReturnsViewResult()
        {
            var result = _categoryController.Create();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Create_InvalidModelState_ReturnsViewResult()
        {
            _categoryController.ModelState.AddModelError("", "Error");

            var result = _categoryController.Create(new Category());

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Create_ValidModelState_RedirectsToAction()
        {
            var result = _categoryController.Create(new Category());

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void Edit_InvalidId_ReturnsNotFound()
        {
            var result = _categoryController.Edit(id: null);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Edit_ValidId_ReturnsViewResult()
        {
            var result = _categoryController.Edit(1);

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Edit_InvalidModelState_ReturnsViewResult()
        {
            _categoryController.ModelState.AddModelError("", "Error");

            var result = _categoryController.Edit(new Category());

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Edit_ValidModelState_RedirectsToAction()
        {
            var result = _categoryController.Edit(new Category());

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void Delete_InvalidId_RedirectsToIndex()
        {
            var result = _categoryController.Delete(null);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void Delete_ValidId_RedirectsToIndex()
        {
            var result = _categoryController.Delete(1);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void Delete_ValidId_RemovesCategory()
        {
            _categoryController.Delete(1);

            _mockUnitOfWork.Verify(r => r.CategoryRepository.Remove(It.IsAny<Category>()), Times.Once());
            _mockUnitOfWork.Verify(r => r.Save(), Times.Once());
        }
    }
}
