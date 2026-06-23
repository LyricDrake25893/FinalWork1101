using ReadCity.DAL.Models;
using ReadCity.WPF.ViewModels;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ReadCity.WPF.Views
{
    public partial class MainWindow : Window
    {
        private List<BookViewModel> _allBooks = new();

        public MainWindow()
        {
            InitializeComponent();
            LoadLogo();
            LoadBooks();
            LoadPublisherFilter();
            LoadSortOptions();
            UpdateHeader();
        }

        // Загрузка логотипа приложения
        private void LoadLogo()
        {
            try
            {
                var logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                            "Resources", "icon.png");
                if (File.Exists(logoPath))
                    LogoImage.Source = new BitmapImage(new Uri(logoPath));
            }
            catch { /* logo is optional */ }
        }

        // Загрузка списка книг из базы данных
        private void LoadBooks()
        {
            try
            {
                var books = App.BookService.GetAllBooks();
                _allBooks = books.Select(b => new BookViewModel(b)).ToList();
                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки каталога:\n{ex.Message}",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Заполнение выпадающего списка производителей
        private void LoadPublisherFilter()
        {
            try
            {
                var publishers = App.BookService.GetAllPublishers();
                PublisherCombo.Items.Clear();
                PublisherCombo.Items.Add("Все производители");
                foreach (var p in publishers)
                    PublisherCombo.Items.Add(p);
                PublisherCombo.SelectedIndex = 0;
            }
            catch { /* non-critical */ }
        }

        // Заполнение списка вариантов сортировки
        private void LoadSortOptions()
        {
            SortCombo.Items.Clear();
            SortCombo.Items.Add("По умолчанию");
            SortCombo.Items.Add("Название ↑");
            SortCombo.Items.Add("Название ↓");
            SortCombo.Items.Add("Цена ↑");
            SortCombo.Items.Add("Цена ↓");
            SortCombo.SelectedIndex = 0;
        }

        // Вызов фильтрации при изменении критериев
        private void Filter_Changed(object sender, EventArgs e) => ApplyFilters();

        // Логика фильтрации и сортировки списка книг
        private void ApplyFilters()
        {
            var filtered = _allBooks.AsEnumerable();

            // Поиск по названию
            var searchText = SearchBox.Text.Trim();
            if (!string.IsNullOrEmpty(searchText))
                filtered = filtered.Where(b =>
                    b.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase));

            // Фильтр по производителю
            var publisher = PublisherCombo.SelectedItem as string;
            if (!string.IsNullOrEmpty(publisher) && publisher != "Все производители")
                filtered = filtered.Where(b => b.Publisher == publisher);

            // Фильтр по диапазону цен
            if (decimal.TryParse(PriceMinBox.Text, out var minPrice))
                filtered = filtered.Where(b => b.Price >= minPrice);
            if (decimal.TryParse(PriceMaxBox.Text, out var maxPrice))
                filtered = filtered.Where(b => b.Price <= maxPrice);

            // Применение сортировки
            filtered = SortCombo.SelectedIndex switch
            {
                1 => filtered.OrderBy(b => b.Title),
                2 => filtered.OrderByDescending(b => b.Title),
                3 => filtered.OrderBy(b => b.Price),
                4 => filtered.OrderByDescending(b => b.Price),
                _ => filtered
            };

            // Вывод результата и обновление счетчика записей
            var result = filtered.ToList();
            BooksList.ItemsSource = result;
            CounterText.Text = $"Показано: {result.Count} из {_allBooks.Count}";
        }

        public void UpdateHeader()
        {
            // Настройка видимости элементов управления в зависимости от роли пользователя
            if (App.CurrentUser != null)
            {
                UserNameText.Text = App.CurrentUser.FullName;
                UserNameText.Visibility = Visibility.Visible;
                LoginButton.Content = "Выйти";

                var role = App.CurrentUser.Role;
                OrdersButton.Visibility =
                    (role == "Администратор" || role == "Менеджер")
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
            else
            {
                UserNameText.Visibility = Visibility.Collapsed;
                LoginButton.Content = "Войти";
                OrdersButton.Visibility = Visibility.Collapsed;
            }

            // Отображение кнопки корзины при наличии товаров
            ViewOrderButton.Visibility =
                (App.CurrentOrder != null && App.CurrentOrder.Items.Count > 0)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        // Обработчик кнопки авторизации / выхода из системы
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (App.CurrentUser == null)
            {
                var loginWindow = new LoginWindow { Owner = this };
                if (loginWindow.ShowDialog() == true)
                    UpdateHeader();
            }
            else
            {
                App.CurrentUser = null;
                UpdateHeader();
            }
        }

        // Переход к окну управления заказами
        private void OrdersButton_Click(object sender, RoutedEventArgs e)
        {
            var ordersWindow = new OrdersWindow { Owner = this };
            ordersWindow.ShowDialog();
        }

        // Переход к окну корзины
        private void ViewOrderButton_Click(object sender, RoutedEventArgs e)
        {
            var cartWindow = new CartWindow { Owner = this };
            cartWindow.ShowDialog();
            UpdateHeader();
        }

        // Обработчик нажатия кнопки заказа книги
        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is BookViewModel vm)
                AddToOrder(vm.Book);
        }

        // Добавление выбранной книги в текущий заказ
        private void AddToOrder(Book book)
        {
            // Инициализация нового заказа, если корзина пуста
            if (App.CurrentOrder == null)
            {
                App.CurrentOrder = new Order
                {
                    OrderNumber = App.OrderService.GetNextOrderNumber(),
                    OrderDate = DateTime.Now,
                    DeliveryDate = DateTime.Now.AddDays(7),
                    ClientFullName = App.CurrentUser?.FullName,
                    ReceiptCode = new Random().Next(100, 1000),
                    OrderStatus = "Новый"
                };
            }

            // Проверка на дублирование товара в корзине
            var existing = App.CurrentOrder.Items
                .FirstOrDefault(i => i.ArticleCode == book.ArticleCode);
            if (existing != null)
            {
                existing.Quantity++;
            }
            else
            {
                App.CurrentOrder.Items.Add(new OrderItem
                {
                    ArticleCode = book.ArticleCode,
                    Quantity = 1,
                    Book = book
                });
            }

            UpdateHeader();
            MessageBox.Show($"«{book.Title}» добавлен в заказ.",
                            "Товар добавлен", MessageBoxButton.OK,
                            MessageBoxImage.Information);
        }
    }
}