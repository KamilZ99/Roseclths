using Bookshop.DataAccess.Repository.Interfaces;
using Bookshop.Models;
using BookshopWeb.Areas.Customer.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Bookshop.Tests
{
    public class CartControllerTests
    {
        private readonly CartController _controller;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;

        public CartControllerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _controller = new CartController(_mockUnitOfWork.Object) { ShoppingCartViewModel = new Models.ViewModels.ShoppingCartViewModel() { OrderHeader = new OrderHeader(), ListCart = new List<ShoppingCart>() } };
        }

        [Fact]
        public void Index_UserAuthenticated_ReturnsViewResult()
        {
            // Arrange
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "testuser") }));
            _mockUnitOfWork.Setup(uow => uow.ShoppingCartRepository.GetAll(It.IsAny<Expression<Func<ShoppingCart, bool>>>(), It.IsAny<string>()))
                .Returns(new List<ShoppingCart>());

            // Act
            var result = _controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void GetPriceBasedOnQuantity_ValidQuantity_ReturnsCorrectPrice()
        {
            // Arrange
            var bookPrice = 10;
            var quantity = 5;
            var expectedPrice = bookPrice;

            // Act
            var result = CartController.GetPriceBasedOnQuantity(quantity, bookPrice);

            // Assert
            Assert.Equal(expectedPrice, result);
        }

        [Fact]
        public void GetPriceBasedOnQuantity_ValidInput_ReturnsCorrectPrice()
        {
            // Arrange
            var quantity = 75;
            var price = 100;
            var expectedPrice = price * 0.85;

            // Act
            var result = CartController.GetPriceBasedOnQuantity(quantity, price);

            // Assert
            Assert.Equal(expectedPrice, result);
        }

        [Theory]
        [InlineData(0, 100)]
        [InlineData(-1, 100)]
        [InlineData(100, 0)]
        [InlineData(100, -1)]
        public void GetPriceBasedOnQuantity_InvalidInput_ThrowsArgumentException(double quantity, double price)
        {
            // Arrange and Act and Assert
            Assert.Throws<ArgumentException>(() => CartController.GetPriceBasedOnQuantity(quantity, price));
        }

    }
}
