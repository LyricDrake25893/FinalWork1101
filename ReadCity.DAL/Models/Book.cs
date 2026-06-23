namespace ReadCity.DAL.Models
{
    /// <summary>
    /// Представляет книгу (товар) в каталоге.
    /// </summary>
    public class Book
    {
        /// <summary>Первичный ключ.</summary>
        public int BookId { get; set; }

        /// <summary>Артикул товара (например, A112T4).</summary>
        public string ArticleCode { get; set; } = string.Empty;

        /// <summary>Название книги.</summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>Единица измерения.</summary>
        public string Unit { get; set; } = string.Empty;

        /// <summary>Цена в рублях.</summary>
        public decimal Price { get; set; }

        /// <summary>Автор(ы).</summary>
        public string Author { get; set; } = string.Empty;

        /// <summary>Наименование издателя.</summary>
        public string Publisher { get; set; } = string.Empty;

        /// <summary>Категория (жанр) книги.</summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>Процент скидки (0–100).</summary>
        public decimal Discount { get; set; }

        /// <summary>Количество на складе.</summary>
        public int StockQuantity { get; set; }

        /// <summary>Полное описание книги.</summary>
        public string? Description { get; set; }

        /// <summary>Имя файла фотографии (локальное).</summary>
        public string? Photo { get; set; }

        /// <summary>Цена с учетом скидки.</summary>
        public decimal DiscountedPrice =>
            Discount > 0 ? Math.Round(Price * (1 - Discount / 100), 2) : Price;
    }
}