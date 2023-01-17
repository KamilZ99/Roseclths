using Bookshop.DataAccess.Repository.Interfaces;
using Bookshop.Models;
using Bookshop.Models.ViewModels;
using BookshopWeb.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bookshop.Tests
{
    public class OrderControllerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly OrderController _orderController;

        public OrderControllerTests()
        {
            _mockUnitOfWork = GetIUnitOfWorkMock();
            _orderController = new OrderController(_mockUnitOfWork.Object);
        }

        public static Mock<IUnitOfWork> GetIUnitOfWorkMock()
        {
            var mock = new Mock<IUnitOfWork>();

            mock.Setup(r => r.OrderHeaderRepository.GetFirstOrDefault(It.IsAny<Expression<Func<OrderHeader, bool>>>(), It.IsAny<string>()))
                .Returns(new OrderHeader { Id = 1 });

            return mock;
        }

        [Fact]
        public void Index_ReturnsAViewResult()
        {
            var result = _orderController.Index();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Details_ReturnsAViewResult_WithOrderViewModel()
        {
            _mockUnitOfWork.Setup(r => r.OrderDetailsRepository.GetAll(It.IsAny<Expression<Func<OrderDetails, bool>>>(), It.IsAny<string>()))
                .Returns(new List<OrderDetails> { new OrderDetails { OrderId = 1 } });

            var result = _orderController.Details(1);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<OrderViewModel>(viewResult.ViewData.Model);
            Assert.NotNull(model.OrderHeader);
            Assert.NotEmpty(model.OrderDetails);
        }

        [Fact]
        public void Details_InvalidId_ReturnsNotFound()
        {
            _mockUnitOfWork.Setup(r => r.OrderHeaderRepository.GetFirstOrDefault(It.IsAny<Expression<Func<OrderHeader, bool>>>(), It.IsAny<string>()))
                .Returns((OrderHeader)null);

            var result = _orderController.Details(0);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void PaymentConfirmation_ReturnsAViewResult_WithOrderHeader()
        {
            var result = _orderController.PaymentConfirmation(1);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<int>(viewResult.ViewData.Model);
            Assert.Equal(1, model);
        }

        [Fact]
        public void PaymentConfirmation_InvalidId_ThrowsArgumentNullException()
        {
            _mockUnitOfWork.Setup(r => r.OrderHeaderRepository.GetFirstOrDefault(It.IsAny<Expression<Func<OrderHeader, bool>>>(), It.IsAny<string>()))
                .Returns((OrderHeader)null);

            Assert.Throws<ArgumentNullException>(() => _orderController.PaymentConfirmation(0));
        }
    }
}
