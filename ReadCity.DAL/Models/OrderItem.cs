namespace ReadCity.DAL.Models
{
    public class OrderItem
    {
        /// <summary>Первичный ключ.</summary>
        public int OrderItemId { get; set; }

        /// <summary>Внешний ключ для связи с таблицей <see cref="Order"/>.</summary>
        public int OrderId { get; set; }

        /// <summary>Артикул книги.</summary>
        public string ArticleCode { get; set; } = string.Empty;

        /// <summary>Заказанное количество.</summary>
        public int Quantity { get; set; }

        /// <summary>Навигационное свойство: книга, к которой относится данная позиция.</summary>
        public Book? Book { get; set; }
    }
}