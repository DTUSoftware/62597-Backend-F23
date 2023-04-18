using Microsoft.AspNetCore.Mvc;
using ShopBackend.Controllers;
using ShopBackend.Dtos;
using ShopBackend.Models;

namespace ShopBackend.Test.ControllersTest
{
    /**
     * @author: Karitas
     * @date: 18-04-2023
     */
    public TestOrderController(){
        //Mutual Arrange
        orderList = DataHelper.GetFakeOrderList();
        var mock = MockIRepositories.GetOrderRepository(orderList);
        orderController = new OrdersController(mock.Object);

    }

    [Fact]
    public async Task GetAllOrders_onOk(){
        //Act
        var actionResult = await orderController.Get();

        //Assert
        Assert.NotNull(actionResult);
        Assert.IsType<OkObject>(actionResult.Result);
        IEnumerable<OrderDto> list = Assert.IsAssignableFrom<IEnumerable<OrderDto>>(((OkObjectResult)actionResult.Result).Value);
        Assert.Equal(2,list.Count());

    }

    [Fact]
    public async Task GetAllOrders_onNotFound(){
        //Arrange
        var emptyOrderList = new List<Order>();
        var mock = MockIRepositories.GetOrderRepository(emptyOrderList);
        var Controller = new OrderController(mock.Object);

        //Act
        var actionResult = await Controller.Get();

        //Assert
        Assert.NotNull(actionResult);
        Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        string msg =Assert.IsType<string>(((NotFoundObjectResult)actionResult.Result).Value);
        Assert.Equal("The specified order does not exist!", msg);
    }

    [Theory]
    [InlineData("1a1b1c1d-d9cb-469f-a165-70867728555e")]
    public async Task GetOrderById_onOk(Guid orderId){
        //Act
        var actionResult = await orderController.Get(orderId);

        //Assert
        Assert.NotNull(actionResult);
        Assert.IsType<OkObjectResult>(actionResult.Result);
        var order = Assert.IsType<OrderDto>(((OkObjectResult)actionResult.Result).Value);
        Assert.Equal(orderList[0].Id, order.Id);
    }

    [Theory]
    [InlineData("5a1b1c1d-d9cb-469f-a165-70867728555e")]
    public async Task GetOrderById_onNotFound(Guid orderId){
        //Act
        var actionResult = await orderController.Get(orderId);

        //Assert
        Assert.NotNull(actionResult);
        Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        string msg = Assert.IsType<string>(((NotFoundObjectResult)actionResult.Result).Value);
        Assert.Equal("The specified order does not exist!", msg);
    }

    [Fact]
    public async Task CreateOrder_onOk(){
        //Arrange
        var newOrderDetails = new List<CreateOrderDetailDto>{
            new CreateOrderDetailDto{
                Quantity = 2,
                GiftWrap = true,
                RecurringOrder = true,
                OrderId = DataHelper.GetFakeOrderList().First().Id,
                ProductId= DataHelper.GetFakeProducs()[0].Id
            },
            new CreateOrderDetailDto{
                Quantity = 3,
                GiftWrap = false,
                RecurringOrder = false,
                OrderId = DataHelper.GetFakeOrderList().First().Id,
                ProductId= DataHelper.GetFakeProducs()[1].Id
            },

        };
        var newOrder = new CreateOrderDto{
            BillingAddress = DataHelper.GetFakeAddressList()[0].AsAddressDto(),
            ShippingAddress = DataHelper.GetFakeAddressList()[0].AsAddressDto(),
            OrderDetails = newOrderDetails
        };

        //Act
        var actionResult = await orderController.Create(newOrder);

        //Assert
        Assert.NotNull(actionResult);
        Assert.IsType<CreatedResult>(actionResult.Result);
        string msg = Assert.IsType<string>(((CreatedResult)actionResult.Result).Value);
        Assert.Equal("Order is inserted successfully!", msg);
        Assert.Equal(3, orderList.Count);
    }

    [Fact]
    public async Task CreateOrder_onBadRequest_EmptyProducts(){
        //Arrange
        var emptyOrderDetails = new List<CreateOrderDetailDto>();
        var newOrder = new CreateOrderDto{
            BillingAddress = DataHelper.GetFakeAddressList()[0].AsAddressDto(),
            ShippingAddress = DataHelper.GetFakeAddressList()[0].AsAddressDto(),
            OrderDetails = emptyOrderDetails
        };

        //Act
        var actionResult = await orderController.Create(newOrder);

        //Assert
        Assert.NotNull(actionResult);
        Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        string msg = Assert.IsType<string>(((BadRequestObjectResult)actionResult.Result).Value);
        Assert.Equal("ProductDetails is required to register the order!", msg);
        Assert.Equal(2, orderList.Count);
    }

    [Fact]
    public async Task UpdateOrder_onOk(){
        //Arrange 
        var orderId = orderList[0].Id;
        var targetOrder = new OrderDto { OrderStatus = Utils.OrderStatus.Canceled };

        //Act
        var actionResult = await orderController.Update(orderId, targetOrder);

        //Assert
        Assert.NotNull(actionResult);
        Assert.IsType<OkObjectResult>(actionResult.Result);
        string msg = Assert.IsType<string>(((OkObjectResult)actionResult.Result).Value);
        Assert.Equal("Order has been updated!", msg);
        Assert.Equal(Utils.OrderStatus.Canceled, orderList[0].OrderStatus);
    }

    [Fact]
    public async Task UpdateOrder_onNotFound(){
        //Arrange
        var orderId = Guid.Parse("5a1b1c1d-d9cb-469f-a165-70867728555e");
        var targetOrder = new OrderDto { OrderStatus = Utils.OrderStatus.Canceled };

        //Act
        var actionResult = await orderController.Update(orderId, targetOrder);

        //Assert
        Assert.NotNull(actionResult);
        Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        string msg = Assert.IsType<string>(((NotFoundObjectResult)actionResult.Result).Value);
        Assert.Equal("Order does not exsist!", msg);
    }

    [Theory]
    [InlineData("1a1b1c1d-d9cb-469f-a165-70867728555e")]
    public async Task DeleteOrder_onOk(Guid orderId){
        //Act
        var actionResult = await orderController.Delete(orderId);

        //Assert
        Assert.NotNull(actionResult);
        Assert.IsType<OkObjectResult>(actionResult.Result);
        string msg = Assert.IsType<string>(((OkObjectResult)actionResult.Result).Value);
        Assert.Equal("Order has been deleted!", msg);
        Assert.Single(orderList);
    }

    [Theory]
    [InlineData("5a1b1c1d-d9cb-469f-a165-70867728555e")]
    public async Task DeleteCustomer_onNotFound(Guid orderId){
        //Act
        var actionResult = await orderController.Delete(orderId);

        //Assert
        Assert.NotNull(actionResult);
        Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        string msg = Assert.IsType<string>(((NotFoundObjectResult)actionResult.Result).Value);
        Assert.Equal("Order could not be deleted!", msg);
        Assert.Equal(2, orderList.Count);
    }
}
