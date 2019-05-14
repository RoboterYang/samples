using System;
using FrameworkDesign.ClassLibrary;
using Xunit;

namespace FrameworkDesign.ClassLibrary.Tests
{
    [Trait("������", "��������")]
    public class OrderTest
    {
        [Fact(DisplayName = "��Ʒ�۸��Ƿ���ȷ-�쳣����")]
        public void Order_ItemsPrice_IsCorrect_WithExpectedParameters()
        {
            // arrange
            OperationNoormalPrices operationNoormalPrices = new OperationNoormalPrices();
            Order order = new Order(operationNoormalPrices);
            Items items = new Items();
            items.Add(new Item() { Name = "����", Price = 0.0 });

            // act
            order.Items = items;

            // assert
            Assert.Null(order.Items);
        }

        [Fact(DisplayName = "��Ʒ�۸��Ƿ���ȷ-��������")]
        public void Order_ItemsPrice_IsCorrect_WithUnExpectedParameters()
        {
            // arrange
            OperationNoormalPrices operationNoormalPrices = new OperationNoormalPrices();
            Order order = new Order(operationNoormalPrices);
            Items items = new Items();
            items.Add(new Item() { Name = "����", Price = 39.99 });

            // act
            order.Items = items;

            // assert
            Assert.NotNull(order.Items);
            Assert.True(order.Items.Count > 0);
        }
    }
}
