using Roseclth.DataAccess.Repository.Interfaces;
using Roseclth.Models;
using Roseclth.Models.ViewModels;
using RoseclthWeb.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System.Linq.Expressions;

namespace Roseclth.Tests
{
    public class ProductControllerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IWebHostEnvironment> _mockWebHostEnvironment;
        private readonly ProductController _productController;

        public ProductControllerTests()
        {
            _mockUnitOfWork = GetIUnitOfWorkMock();
            _mockWebHostEnvironment = GetIWebHostEnvironmentMock();
            _productController = new ProductController(_mockUnitOfWork.Object, _mockWebHostEnvironment.Object) { TempData = new TempDataDictionary(new DefaultHttpContext(), new Mock<ITempDataProvider>().Object) };
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

        public static Mock<IWebHostEnvironment> GetIWebHostEnvironmentMock()
        {
            var mock = new Mock<IWebHostEnvironment>();

            mock.SetupProperty(m => m.WebRootPath, "");

            return mock;
        }

        [Fact]
        public void Index_ReturnsViewResult()
        {
            var result = _productController.Index();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Upsert_WithoutId_ReturnsViewResult()
        {
            var result = _productController.Upsert(id: null);

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Upsert_WithId_ReturnsViewResult()
        {
            _mockUnitOfWork
                .Setup(r => r.ProductRepository.GetFirstOrDefault(It.IsAny<Expression<Func<Product, bool>>>(),"Material,Product"))
                .Returns(new Product());

            var result = _productController.Upsert(1);

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Upsert_InvalidModelState_ReturnsViewResult()
        {
            _productController.ModelState.AddModelError("", "Error");

            var result = _productController.Upsert(new ProductViewModel());

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Upsert_ValidModelState_RedirectsToAction()
        {
            var productViewModel = new ProductViewModel
            {
                Product = new Product()
            };

            var result = _productController.Upsert(productViewModel);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void GetAll_ReturnsJsonResult()
        {
            _mockUnitOfWork.Setup(r => r.ProductRepository.GetAll(It.IsAny<string>()))
                .Returns(new Product[] { });

            var result = _productController.GetAll();

            Assert.IsType<JsonResult>(result);
        }

        [Fact]
        public void Delete_InvalidId_ReturnsNotFound()
        {
            var result = _productController.Delete(null);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Delete_ValidId_ReturnsOk()
        {
            var result = _productController.Delete(1);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void Delete_ValidId_DeletesProduct()
        {
            _productController.Delete(1);

            _mockUnitOfWork.Verify(r => r.ProductRepository.Remove(It.IsAny<Product>()), Times.Once());
            _mockUnitOfWork.Verify(r => r.Save(), Times.Once());
        }
    }
}
