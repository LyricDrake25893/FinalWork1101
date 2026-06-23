using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReadCity.DAL.Services;

namespace ReadCity.Web.Pages
{
    public class AddToOrderModel : PageModel
    {
        // 1. Используем сервис через внедрение зависимостей (DI)
        private readonly BookService _bookService;

        public string Message { get; set; } = string.Empty;
        public string ErrorTitle { get; set; } = "Ошибка";
        public string ProductName { get; set; } = string.Empty;
        public decimal ProductPrice { get; set; }
        public bool IsSuccess { get; set; }

        // 2. Внедряем сервис через конструктор
        public AddToOrderModel(BookService bookService)
        {
            _bookService = bookService;
        }

        public void OnGet(int id)
        {
            try
            {
                var book = _bookService.GetAllBooks().FirstOrDefault(b => b.BookId == id);
                if (book == null)
                {
                    Message = "Товар не найден";
                    IsSuccess = false;
                    return;
                }

                if (book.StockQuantity <= 0)
                {
                    Message = "Товара нет в наличии";
                    ErrorTitle = "Нет в наличии";
                    IsSuccess = false;
                    return;
                }

                ProductName = book.Title;
                ProductPrice = book.DiscountedPrice;
                Message = $"Товар «{book.Title}» успешно добавлен в заказ!";
                IsSuccess = true;
            }
            catch (Exception ex)
            {
                Message = $"Произошла системная ошибка: {ex.Message}";
                IsSuccess = false;
            }
        }
    }
}
