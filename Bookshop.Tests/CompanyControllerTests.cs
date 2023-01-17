using Bookshop.DataAccess.Repository.Interfaces;
using Bookshop.Models;
using BookshopWeb.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Bookshop.Tests
{
    public class CompanyControllerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly CompanyController _companyController;

        public CompanyControllerTests()
        {
            _mockUnitOfWork = GetIUnitOfWorkMock();
            _companyController = new CompanyController(_mockUnitOfWork.Object) { TempData = new TempDataDictionary(new DefaultHttpContext(), new Mock<ITempDataProvider>().Object) };
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

            mock.Setup(r => r.CompanyRepository.GetFirstOrDefault(It.IsAny<Expression<Func<Company, bool>>>(), It.IsAny<string>()))
                .Returns(new Company() { Id = 1 });

            return mock;
        }

        [Fact]
        public void Index_ReturnsViewResult()
        {
            var result = _companyController.Index();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Upsert_InvalidId_ReturnsViewResult()
        {
            var result = _companyController.Upsert(id: null);

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Upsert_ValidId_ReturnsViewResult()
        {
            var result = _companyController.Upsert(1);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Upsert_InvalidId_CreatesNewCompany()
        {
            var viewResult = _companyController.Upsert(id: null) as ViewResult;
            var model = viewResult.Model as Company;

            Assert.NotNull(model);
        }

        [Fact]
        public void Upsert_ValidId_UpdatesCompany()
        {
            var result = _companyController.Upsert(1);

            var viewResult = result as ViewResult;
            var model = viewResult.Model as Company;

            Assert.Equal(1, model.Id);
        }

        [Fact]
        public void Upsert_InvalidModelState_ReturnsViewResult()
        {
            _companyController.ModelState.AddModelError("error", "some error");

            var result = _companyController.Upsert(new Company());

            Assert.IsType<ViewResult>(result);
        }
        
        [Fact]
        public void Upsert_ValidModelState_RedirectsToIndex()
        {
            var result = _companyController.Upsert(new Company());

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void Upsert_ValidModelState_AddsCompany()
        {
            _companyController.Upsert(new Company());

            _mockUnitOfWork.Verify(r => r.CompanyRepository.Add(It.IsAny<Company>()), Times.Once());
            _mockUnitOfWork.Verify(r => r.Save(), Times.Once());
        }

        [Fact]
        public void Upsert_ValidModelState_UpdatesCompany()
        {
            _companyController.Upsert(new Company { Id = 1 });

            _mockUnitOfWork.Verify(r => r.CompanyRepository.Update(It.IsAny<Company>()), Times.Once());
            _mockUnitOfWork.Verify(r => r.Save(), Times.Once());
        }

        [Fact]
        public void GetAll_ReturnsJsonResult()
        {
            var result = _companyController.GetAll();

            Assert.IsType<JsonResult>(result);
        }

        [Fact]
        public void Delete_InvalidId_ReturnsNotFound()
        {
            var result = _companyController.Delete(null);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Delete_ValidId_ReturnsOk()
        {
            var result = _companyController.Delete(1);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void Delete_ValidId_DeletesCompany()
        {
            _companyController.Delete(1);

            _mockUnitOfWork.Verify(r => r.CompanyRepository.Remove(It.IsAny<Company>()), Times.Once());
            _mockUnitOfWork.Verify(r => r.Save(), Times.Once());
        }

    }
}
