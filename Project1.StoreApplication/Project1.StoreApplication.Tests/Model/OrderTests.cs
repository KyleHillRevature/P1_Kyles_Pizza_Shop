using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Project1.StoreApplication.Domain.InputModels;
using Project1.StoreApplication.Domain.Models;
using Project1.StoreApplication.Domain.Interfaces.Repository;

namespace Project1.StoreApplication.Tests.Model
{
    public class OrderTests
    {
        //OrderInput addItem = new OrderInput() { OrderId = Guid.NewGuid()};
        
        //public static IEnumerable<object[]> OrderInputs =
        //new List<object[]>
        //{
        //    new object[] { "xUnit", 1 },
           
        //};
        //[Theory, MemberData(nameof(OrderInputs))]
        [Fact]
        public void addItem_itemIsAvailable_itemAddedToCart() 
        {
            //Arrange
            var orderItemRepoStub = new Mock<IOrderItemRepository>();
            var orderRepoStub = new Mock<IOrderRepository>();
            Order order1 = new Order()
            {
                Id = Guid.NewGuid(), OrderDate = DateTime.Parse(Order.cartOrderDate),
                

                };
            Order order = new Order();


            //Act
            //Assert

}
    }
}
