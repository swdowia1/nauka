
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;


namespace dapperSW
{
    class Program
    {
        static void Main(string[] args)
        {
            string sql = "SELECT TOP 10 * FROM Orders AS A INNER JOIN [Order Details] AS B ON A.OrderID = B.OrderID";
            using (var connection = new SqlConnection(classConst.Poloczenie))
            {
                var orderDictionary = new Dictionary<int, Order>();


                var list = connection.Query<Order, OrderDetail, Order>(
                    sql,
                    (order, orderDetail) =>
                    {
                        Order orderEntry;

                        if (!orderDictionary.TryGetValue(order.OrderID, out orderEntry))
                        {
                            orderEntry = order;
                            orderEntry.OrderDetails = new List<OrderDetail>();
                            orderDictionary.Add(orderEntry.OrderID, orderEntry);
                        }

                        orderEntry.OrderDetails.Add(orderDetail);
                        return orderEntry;
                    },
                    splitOn: "OrderID")
                .Distinct()
                .ToList();

                Console.WriteLine("Orders Count:" + list.Count);
            }
        }

        private static void NewMethod()
        {
            using (var connection = new SqlConnection(classConst.Poloczenie))
            {
                var sql = @"select productid, productname, p.categoryid, categoryname 
                from products p 
                inner join categories c on p.categoryid = c.categoryid";
                var products = connection.Query<Product, Category, Product>(sql, (product, category) =>
                {
                    product.Category = category;
                    return product;
                },
                splitOn: "CategoryId");
                products.ToList().ForEach(product => Console.WriteLine($"Product: {product.ProductName}, Category: {product.Category.CategoryName}"));
                Console.ReadLine();
            }
        }
    }
}
