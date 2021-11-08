using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Project1.StoreApplication.Domain.InputModels;

namespace Project1.StoreApplication.Tests.Model
{
    public class OrderTests
    {
        //OrderInput addItem = new OrderInput() { OrderId = Guid.NewGuid()};
        
        public static IEnumerable<object[]> OrderInputs =
        new List<object[]>
        {
            new object[] { "xUnit", 1 },
           
        };
        [Theory, MemberData(nameof(OrderInputs))]
        public void update_allScenarios(string blah, int ha) 
        { }
    }
}
