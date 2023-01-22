using Roseclth.DataAccess.Repository.Interfaces;
using Roseclth.Models;
using RoseclthWeb.Areas.Admin.Controllers;
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

namespace Roseclth.Tests
{
    public class TypeControllerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly TypeController _typeController;

        public TypeControllerTests()
        {
            _mockUnitOfWork = GetIUnitOfWorkMock();
            _typeController = new TypeController(_mockUnitOfWork.Object) { TempData = new TempDataDictionary(new DefaultHttpContext(), new Mock<ITempDataProvider>().Object) };
        }

        public static Mock<IUnitOfWork> GetIUnitOfWorkMock()
        {
            var mock = new Mock<IUnitOfWork>();

            mock.Setup(r => r.ProductRepository.GetFirstOrDefault(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<string>()))
                .Returns(new Product() { Id = 1 });

            mock.Setup<Models.Type>(r => r.TypeRepository.GetFirstOrDefault(It.IsAny<Expression<Func<Models.Type, bool>>>(), It.IsAny<string>()))
                .Returns(new Models.Type() { Id = 1 });


            mock.Setup(r => r.MaterialRepository.GetFirstOrDefault(It.IsAny<Expression<Func<Material, bool>>>(), It.IsAny<string>()))
                .Returns(new Material() { Id = 1 });

            return mock;
        }

        [Fact]
        public void Index_ReturnsViewResult()
        {
            var result = _typeController.Index();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Create_ReturnsViewResult()
        {
            var result = _typeController.Create();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Create_InvalidModelState_ReturnsViewResult()
        {
            _typeController.ModelState.AddModelError("", "Error");

            var result = _typeController.Create(new Models.Type());

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Create_ValidModelState_RedirectsToAction()
        {
            var result = _typeController.Create(new Models.Type());

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void Edit_InvalidId_ReturnsNotFound()
        {
            var result = _typeController.Edit(id: null);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Edit_ValidId_ReturnsViewResult()
        {
            var result = _typeController.Edit(1);

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Edit_InvalidModelState_ReturnsViewResult()
        {
            _typeController.ModelState.AddModelError("", "Error");

            var result = _typeController.Edit(new Models.Type());

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Edit_ValidModelState_RedirectsToAction()
        {
            var result = _typeController.Edit(new Models.Type());

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void Delete_InvalidId_RedirectsToIndex()
        {
            var result = _typeController.Delete(null);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void Delete_ValidId_RedirectsToIndex()
        {
            var result = _typeController.Delete(1);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void Delete_ValidId_RemovesType()
        {
            _typeController.Delete(1);

            _mockUnitOfWork.Verify(r => r.TypeRepository.Remove(It.IsAny<Models.Type>()), Times.Once());
            _mockUnitOfWork.Verify(r => r.Save(), Times.Once());
        }
    }
}
