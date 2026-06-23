using ReadCity.DAL.Models;
using System.Windows;
using System.Windows.Controls;

namespace ReadCity.WPF.Views
{
    public partial class OrdersWindow : Window
    {
        // Список объектов заказов для привязки к интерфейсу
        private List<Order> _orders = new();

        // Фиксированный перечень статусов для бизнес-логики
        private static readonly string[] Statuses =
            { "Новый", "В обработке", "Завершён" };

        public OrdersWindow()
        {
            InitializeComponent();

            // Первоначальное заполнение ComboBox списком статусов
            StatusCombo.ItemsSource = Statuses;
            StatusCombo.SelectedIndex = 0;

            LoadOrders();
        }

        // Получение данных из слоя DAL через сервис и привязка к DataGrid
        private void LoadOrders()
        {
            try
            {
                _orders = App.OrderService.GetAllOrders();
                OrdersGrid.ItemsSource = _orders;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки заказов:\n{ex.Message}",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Перенос данных из выбранной строки DataGrid в элементы редактирования (Data Binding)
        private void OrdersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OrdersGrid.SelectedItem is Order selected)
            {
                DeliveryDatePicker.SelectedDate = selected.DeliveryDate;
                StatusCombo.SelectedItem = selected.OrderStatus;
            }
        }

        // Обработка изменения параметров заказа и отправка обновлений в БД
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на наличие выбранного элемента в таблице
            if (OrdersGrid.SelectedItem is not Order selected)
            {
                MessageBox.Show("Выберите заказ для редактирования.",
                                "Внимание", MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            // Валидация входных данных компонента DatePicker
            if (DeliveryDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Укажите дату доставки.",
                                "Ошибка", MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            try
            {
                var newStatus = StatusCombo.SelectedItem as string ?? selected.OrderStatus;
                var newDelivery = DeliveryDatePicker.SelectedDate.Value;

                // Вызов метода обновления в сервисном слое приложения
                App.OrderService.UpdateOrder(selected.OrderId, newDelivery, newStatus);

                // Актуализация локальных данных в UI без повторного запроса к БД
                selected.DeliveryDate = newDelivery;
                selected.OrderStatus = newStatus;

                OrdersGrid.Items.Refresh();
                MessageBox.Show("Заказ успешно обновлён.",
                                "Сохранено", MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении:\n{ex.Message}",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Закрытие текущей формы и возврат на главное окно
        private void BackButton_Click(object sender, RoutedEventArgs e) => Close();
    }
}