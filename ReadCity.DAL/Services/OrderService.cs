using Dapper;
using ReadCity.DAL.Database;
using ReadCity.DAL.Models;

namespace ReadCity.DAL.Services
{
    /// <summary>
    /// Предоставляет операции доступа к данным для сущностей <see cref="Order"/>.
    /// </summary>
    public class OrderService
    {
        private readonly DatabaseConnection _db;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="OrderService"/>.
        /// </summary>
        /// <param name="db">Фабрика подключений к базе данных.</param>
        public OrderService(DatabaseConnection db)
        {
            _db = db;
        }

        /// <summary>
        /// Возвращает все заказы вместе с их составом (позициями) и данными о книгах.
        /// </summary>
        /// <returns>Список заказов.</returns>
        public List<Order> GetAllOrders()
        {
            using var connection = _db.CreateConnection();

            var orderDict = new Dictionary<int, Order>();

            // Выполнение сложного SQL-запроса с объединением (JOIN) через Dapper
            connection.Query<Order, OrderItem, Book, Order>(
                @"SELECT o.*, oi.*, b.*
                  FROM Orders o
                  LEFT JOIN OrderItems oi ON oi.OrderId = o.OrderId
                  LEFT JOIN Books b ON b.ArticleCode = oi.ArticleCode",
                (order, item, book) =>
                {
                    if (!orderDict.TryGetValue(order.OrderId, out var existingOrder))
                    {
                        existingOrder = order;
                        orderDict[order.OrderId] = existingOrder;
                    }
                    if (item != null)
                    {
                        item.Book = book;
                        existingOrder.Items.Add(item);
                    }
                    return existingOrder;
                },
                splitOn: "OrderItemId,BookId");

            return orderDict.Values.ToList();
        }

        /// <summary>
        /// Возвращает следующий доступный номер заказа (максимальный + 1).
        /// </summary>
        /// <returns>Следующий номер заказа.</returns>
        public int GetNextOrderNumber()
        {
            using var connection = _db.CreateConnection();
            var max = connection.ExecuteScalar<int?>("SELECT MAX(OrderNumber) FROM Orders");
            return (max ?? 0) + 1;
        }

        /// <summary>
        /// Сохраняет новый заказ и все его позиции в базе данных с использованием транзакции.
        /// </summary>
        /// <param name="order">Объект заказа для сохранения.</param>
        public void SaveOrder(Order order)
        {
            using var connection = _db.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            // Вставка записи о заказе и получение его идентификатора
            var orderId = connection.ExecuteScalar<int>(
                @"INSERT INTO Orders (OrderNumber, OrderDate, DeliveryDate, ClientFullName, ReceiptCode, OrderStatus)
                  VALUES (@OrderNumber, @OrderDate, @DeliveryDate, @ClientFullName, @ReceiptCode, @OrderStatus);
                  SELECT SCOPE_IDENTITY();",
                order, transaction);

            // Поочередная вставка всех элементов заказа
            foreach (var item in order.Items)
            {
                connection.Execute(
                    @"INSERT INTO OrderItems (OrderId, ArticleCode, Quantity)
                      VALUES (@OrderId, @ArticleCode, @Quantity)",
                    new { OrderId = orderId, item.ArticleCode, item.Quantity },
                    transaction);
            }

            transaction.Commit();
            order.OrderId = orderId;
        }

        /// <summary>
        /// Обновляет дату доставки и статус существующего заказа.
        /// </summary>
        /// <param name="orderId">Первичный ключ заказа.</param>
        /// <param name="deliveryDate">Новая дата доставки.</param>
        /// <param name="status">Новое значение статуса.</param>
        public void UpdateOrder(int orderId, DateTime deliveryDate, string status)
        {
            using var connection = _db.CreateConnection();
            connection.Execute(
                @"UPDATE Orders SET DeliveryDate = @DeliveryDate, OrderStatus = @Status
                  WHERE OrderId = @OrderId",
                new { DeliveryDate = deliveryDate, Status = status, OrderId = orderId });
        }
    }
}