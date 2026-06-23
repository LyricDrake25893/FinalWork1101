using Microsoft.AspNetCore.Mvc.RazorPages;
using ReadCity.DAL.Services;
using ReadCity.Web.ViewModels;

namespace ReadCity.Web.Pages
{
    /// <summary>
    /// Модель страницы каталога товаров.
    /// Загружает список книг из базы данных и передаёт их в представление.
    /// </summary>
    public class IndexModel : PageModel
    {
        private readonly BookService _bookService;

        /// <summary>
        /// Список товаров для отображения на странице.
        /// </summary>
        public List<ProductViewModel> Products { get; set; } = new();

        /// <summary>
        /// Инициализирует новый экземпляр модели страницы через DI.
        /// </summary>
        public IndexModel(BookService bookService)
        {
            _bookService = bookService;
        }

        /// <summary>
        /// Выполняется при GET-запросе к странице.
        /// Загружает все книги из базы данных и преобразует их в модели представления.
        /// </summary>
        public void OnGet()
        {
            try
            {
                var books = _bookService.GetAllBooks();
                Products = books.Select(b => new ProductViewModel
                {
                    Id = b.BookId,
                    Name = b.Title,
                    Description = b.Description,
                    Price = b.Price,
                    DiscountedPrice = b.DiscountedPrice,
                    Manufacturer = b.Publisher,
                    Stock = b.StockQuantity,
                    Photo = b.Photo
                }).ToList();
            }
            catch
            {
                Products = new List<ProductViewModel>();
            }
        }
    }
}
