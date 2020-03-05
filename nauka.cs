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
=========================================