using System;
using System.Windows;
using ReadCity.DAL.Database;
using ReadCity.DAL.Services;

namespace ReadCity.WPF
{
    public partial class App : Application
    {
        // Строка подключения к базе данных
        private const string ConnectionString = "Data Source=localhost\\SQLEXPRESS01;Initial Catalog=master;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Command Timeout=0";

        /// <summary>Общий сервис для работы с книгами, доступный для всех окон.</summary>
        public static BookService BookService { get; private set; } = null!;

        /// <summary>Общий сервис для аутентификации персонала, доступный для всех окон.</summary>
        public static StaffService StaffService { get; private set; } = null!;

        /// <summary>Общий сервис для управления заказами, доступный для всех окон.</summary>
        public static OrderService OrderService { get; private set; } = null!;

        /// <summary>Текущий авторизованный пользователь (null = гость).</summary>
        public static DAL.Models.Staff? CurrentUser { get; set; }

        /// <summary>Текущий формируемый заказ / корзина (null, если пуст).</summary>
        public static DAL.Models.Order? CurrentOrder { get; set; }

        /// <inheritdoc/>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Инициализация подключения к БД и создание экземпляров сервисов (слой бизнес-логики)
            var db = new DatabaseConnection(ConnectionString);
            BookService = new BookService(db);
            StaffService = new StaffService(db);
            OrderService = new OrderService(db);
        }
    }
}