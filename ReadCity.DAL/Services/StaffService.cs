using Dapper;
using ReadCity.DAL.Database;
using ReadCity.DAL.Models;

namespace ReadCity.DAL.Services
{
    /// <summary>
    /// Предоставляет операции доступа к данным для сущностей <see cref="Staff"/>.
    /// </summary>
    public class StaffService
    {
        private readonly DatabaseConnection _db;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="StaffService"/>.
        /// </summary>
        /// <param name="db">Фабрика подключений к базе данных.</param>
        public StaffService(DatabaseConnection db)
        {
            _db = db;
        }

        /// <summary>
        /// Выполняет попытку аутентификации пользователя по логину и паролю.
        /// </summary>
        /// <param name="login">Логин пользователя (e-mail).</param>
        /// <param name="password">Пароль в открытом виде.</param>
        /// <returns>Найденный объект <see cref="Staff"/> или null, если пользователь не найден.</returns>
        public Staff? Authenticate(string login, string password)
        {
            using var connection = _db.CreateConnection();
            // Выполнение SQL-запроса для поиска пользователя с соответствующими учетными данными
            return connection.QueryFirstOrDefault<Staff>(
                "SELECT * FROM Staff WHERE Login = @Login AND Password = @Password",
                new { Login = login, Password = password });
        }
    }
}