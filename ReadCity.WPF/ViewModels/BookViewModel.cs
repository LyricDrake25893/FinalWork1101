using ReadCity.DAL.Models;

namespace ReadCity.WPF.ViewModels
{
    /// <summary>
    /// Presentation wrapper for <see cref="Book"/> adding UI-specific properties.
    /// </summary>
    public class BookViewModel
    {
        /// <summary>The underlying domain model.</summary>
        public Book Book { get; }

        /// <summary>Initializes the view-model from a domain model.</summary>
        /// <param name="book">Source book.</param>
        public BookViewModel(Book book)
        {
            Book = book;
        }

        /// <inheritdoc cref="Book.BookId"/>
        public int BookId => Book.BookId;

        /// <inheritdoc cref="Book.ArticleCode"/>
        public string ArticleCode => Book.ArticleCode;

        /// <inheritdoc cref="Book.Title"/>
        public string Title => Book.Title;

        /// <inheritdoc cref="Book.Author"/>
        public string Author => Book.Author;

        /// <inheritdoc cref="Book.Publisher"/>
        public string Publisher => Book.Publisher;

        /// <inheritdoc cref="Book.Category"/>
        public string Category => Book.Category;

        /// <inheritdoc cref="Book.Price"/>
        public decimal Price => Book.Price;

        /// <inheritdoc cref="Book.Discount"/>
        public decimal Discount => Book.Discount;

        /// <inheritdoc cref="Book.Description"/>
        public string? Description => Book.Description;

        /// <summary>True when the book has a non-zero discount.</summary>
        public bool HasDiscount => Book.Discount > 0;

        /// <summary>
        /// Pack URI to the cover image embedded in the assembly resources.
        /// Returns empty string when no photo is set.
        /// </summary>
        public string PhotoFullPath =>
            string.IsNullOrWhiteSpace(Book.Photo)
                ? string.Empty
                : $"pack://application:,,,/Resources/{Book.Photo}";
    }
}
