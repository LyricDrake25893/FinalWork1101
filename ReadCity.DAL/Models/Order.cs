using System;
using System.Collections.Generic;

namespace ReadCity.DAL.Models
{
    /// <summary>
    /// Представляет заказ покупателя.
    /// </summary>
    public class Order
    {
        /// <summary>Первичный ключ.</summary>
        public int OrderId { get; set; }

        /// <summary>Удобный для чтения номер заказа.</summary>
        public int OrderNumber { get; set; }

        /// <summary>Дата и время оформления заказа.</summary>
        public DateTime OrderDate { get; set; }

        /// <summary>Ожидаемая дата доставки.</summary>
        public DateTime DeliveryDate { get; set; }

        /// <summary>ФИО авторизованного клиента (null для гостей).</summary>
        public string? ClientFullName { get; set; }

        /// <summary>Трехзначный код получения заказа.</summary>
        public int ReceiptCode { get; set; }

        /// <summary>Статус заказа (Новый / В обработке / Завершён).</summary>
        public string OrderStatus { get; set; } = "Новый";

        /// <summary>Навигационное свойство: состав (элементы) данного заказа.</summary>
        public List<OrderItem> Items { get; set; } = new();
    }
}