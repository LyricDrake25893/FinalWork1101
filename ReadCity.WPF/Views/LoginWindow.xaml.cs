using System.Windows;
using System.Windows.Input;

namespace ReadCity.WPF.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();

            // Установка фокуса на поле ввода логина при открытии окна
            LoginBox.Focus();
        }

        // Обработчик нажатия на кнопку "Войти"
        private void LoginButton_Click(object sender, RoutedEventArgs e) => TryLogin();

        // Обработчик нажатия на кнопку "Отмена"
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        // Обработчик нажатия клавиш в поле ввода пароля (вход по нажатию Enter)
        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) TryLogin();
        }

        // Метод аутентификации пользователя в системе
        private void TryLogin()
        {
            var login = LoginBox.Text.Trim();
            var password = PasswordBox.Password;

            // Валидация заполнения полей
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите логин и пароль.",
                                "Ошибка входа", MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Проверка учетных данных через сервис авторизации
                var user = App.StaffService.Authenticate(login, password);
                if (user == null)
                {
                    MessageBox.Show("Неверный логин или пароль. Проверьте введённые данные.",
                                    "Ошибка входа", MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                    return;
                }

                // Сохранение данных авторизованного пользователя в сессии приложения
                App.CurrentUser = user;
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных:\n{ex.Message}",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}