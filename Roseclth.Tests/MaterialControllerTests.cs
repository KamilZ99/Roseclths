using Roseclth.DataAccess.Repository.Interfaces;
using Roseclth.Models;
using RoseclthWeb.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System.Linq.Expressions;

namespace Roseclth.Tests
{
    public class MaterialControllerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly MaterialController _materialController;

        public MaterialControllerTests()
        {
            _mockUnitOfWork = GetIUnitOfWorkMock();
            _materialController = new MaterialController(_mockUnitOfWork.Object) { TempData = new TempDataDictionary(new DefaultHttpContext(), new Mock<ITempDataProvider>().Object) };
        }

        public static Mock<IUnitOfWork> GetIUnitOfWorkMock()
        {
            var mock = new Mock<IUnitOfWork>();

            mock.Setup(r => r.MaterialRepository.GetFirstOrDefault(It.IsAny<Expression<Func<Material, bool>>>(), It.IsAny<string>()))
                .Returns(new Material() { Id = 1 });

            mock.Setup(r => r.MaterialRepository.GetAll(It.IsAny<string>()))
                .Returns(new List<Material> { new Material { Id = 1 } });

            mock.Setup(r => r.MaterialRepository.Add(It.IsAny<Material>()))
                .Callback((Material material) => material.Id = 2);

            mock.Setup(r => r.MaterialRepository.Update(It.IsAny<Material>()))
                .Callback((Material material) => material.Name = "UpdatedName");

            mock.Setup(r => r.Save());

            return mock;
        }

        [Fact]
        public void Index_ReturnsAViewResult_WithAListOfMaterials()
        {
            var result = _materialController.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Material>>(viewResult.ViewData.Model);
            var material = Assert.Single(model);
            Assert.Equal(1, material.Id);
        }

        [Fact]
        public void Create_ReturnsAViewResult()
        {
            var result = _materialController.Create();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Create_Post_AddsMaterial_AndReturnsARedirect()
        {
            var material = new Material { Name = "TestName" };

            var result = _materialController.Create(material);

            Assert.Equal(2, material.Id);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void Create_Post_InvalidModel_ReturnsAViewResult()
        {
            _materialController.ModelState.AddModelError("Name", "Required");
            var material = new Material();

            var result = _materialController.Create(material);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(material, viewResult.ViewData.Model);
        }

        [Fact]
        public void Edit_ReturnsAViewResult_WithTheMaterial()
        {
            var result = _materialController.Edit(1);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Material>(viewResult.ViewData.Model);
            Assert.Equal(1, model.Id);
        }

        [Fact]
        public void Edit_InvalidId_ReturnsNotFound()
        {
            var result = _materialController.Edit(0);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Edit_Post_UpdatesMaterial_AndReturnsARedirect()
        {
            var material = new Material { Id = 1, Name = "TestName" };

            var result = _materialController.Edit(material);

            Assert.Equal("UpdatedName", material.Name);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void Edit_Post_InvalidModel_ReturnsAViewResult()
        {
            _materialController.ModelState.AddModelError("Name", "Required");
            var material = new Material { Id = 1 };

            var result = _materialController.Edit(material);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(material, viewResult.ViewData.Model);
        }


        [Fact]
        public void Delete_DeletesMaterial_AndReturnsARedirect()
        {
            var result = _materialController.Delete(1);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void Delete_InvalidId_ReturnsRedirect()
        {
            var result = _materialController.Delete(0);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
    }
}
