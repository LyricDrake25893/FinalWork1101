namespace ReadCity.Web.ViewModels
{
    /// <summary>
    /// Модель представления товара для отображения на веб-странице.
    /// </summary>
    public class ProductViewModel
    {
        /// <summary>
        /// Уникальный идентификатор товара.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название товара.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Описание товара.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Полная цена товара (без скидки).
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Цена товара с учётом скидки.
        /// </summary>
        public decimal DiscountedPrice { get; set; }

        /// <summary>
        /// Производитель товара.
        /// </summary>
        public string? Manufacturer { get; set; }

        /// <summary>
        /// Количество товара на складе.
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// Имя файла изображения товара.
        /// </summary>
        public string? Photo { get; set; }
    }
}