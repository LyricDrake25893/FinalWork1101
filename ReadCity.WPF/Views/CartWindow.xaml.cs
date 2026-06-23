using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using ReadCity.DAL.Models;
using ReadCity.WPF.ViewModels;

namespace ReadCity.WPF.Views
{
    public partial class CartWindow : Window
    {
        public CartWindow()
        {
            InitializeComponent();

            // Отображение ФИО текущего пользователя или статуса гостя
            ClientText.Text = App.CurrentUser != null
                ? $"Клиент: {App.CurrentUser.FullName}"
                : "Гость";
            RefreshList();
        }

        // Обновление данных в корзине и пересчет общей стоимости
        private void RefreshList()
        {
            if (App.CurrentOrder == null) return;

            var vms = App.CurrentOrder.Items
                .Select(i => new CartItemViewModel(i))
                .ToList();
            CartList.ItemsSource = vms;

            var total = App.CurrentOrder.Items
                .Sum(i => i.Quantity * (i.Book?.Price ?? 0));
            TotalText.Text = $"Итого: {total:N2} ₽";
        }

        // Увеличение количества выбранной книги
        private void IncreaseQty_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is CartItemViewModel vm)
            {
                vm.Item.Quantity++;
                RefreshList();
            }
        }

        // Уменьшение количества с проверкой на полное удаление из корзины
        private void DecreaseQty_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is CartItemViewModel vm)
            {
                vm.Item.Quantity--;
                if (vm.Item.Quantity <= 0)
                {
                    var result = MessageBox.Show(
                        $"Удалить «{vm.Item.Book?.Title}» из заказа?",
                        "Подтверждение", MessageBoxButton.YesNo,
                        MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                        App.CurrentOrder!.Items.Remove(vm.Item);
                    else
                        vm.Item.Quantity = 1;
                }
                RefreshList();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e) => Close();

        // Оформление заказа и сохранение его в базу данных
        private void PlaceOrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (App.CurrentOrder == null || App.CurrentOrder.Items.Count == 0)
            {
                MessageBox.Show("Заказ пуст.", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                App.OrderService.SaveOrder(App.CurrentOrder);

                var savedOrder = App.CurrentOrder;
                App.CurrentOrder = null;

                // Экспорт талона заказа в текстовый файл
                SaveReceipt(savedOrder);

                MessageBox.Show(
                    $"Заказ №{savedOrder.OrderNumber} оформлен!\n" +
                    $"Код получения: {savedOrder.ReceiptCode}",
                    "Заказ оформлен", MessageBoxButton.OK,
                    MessageBoxImage.Information);

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении заказа:\n{ex.Message}",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Формирование структуры талона заказа и сохранение через SaveFileDialog
        private static void SaveReceipt(Order order)
        {
            var dialog = new SaveFileDialog
            {
                Title = "Сохранить талон",
                Filter = "Текстовые файлы (*.txt)|*.txt",
                FileName = $"Заказ_{order.OrderNumber}.txt"
            };

            if (dialog.ShowDialog() != true) return;

            var lines = new List<string>
            {
                "===== ЧИТАЙ ГОРОД — ТАЛОН ЗАКАЗА =====",
                $"Дата заказа:   {order.OrderDate:dd.MM.yyyy HH:mm}",
                $"Номер заказа:  {order.OrderNumber}",
                $"Код получения: {order.ReceiptCode}",
                string.Empty,
                "--- Состав заказа ---"
            };

            decimal total = 0;
            foreach (var item in order.Items)
            {
                var lineTotal = item.Quantity * (item.Book?.Price ?? 0);
                total += lineTotal;
                lines.Add($"  {item.Book?.Title ?? item.ArticleCode}  x{item.Quantity}  = {lineTotal:N2} ₽");
            }

            lines.Add(string.Empty);
            lines.Add($"ИТОГО: {total:N2} ₽");
            lines.Add("=======================================");

            File.WriteAllLines(dialog.FileName, lines, System.Text.Encoding.UTF8);
        }
    }
}