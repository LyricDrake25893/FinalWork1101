using Microsoft.Data.SqlClient;
using System.Data;

namespace ReadCity.DAL.Database
{
    /// <summary>
    /// Предоставляет метод фабрики для создания подключений к базе данных SQL Server.
    /// </summary>
    public class DatabaseConnection
    {
        // Строка подключения к базе данных
        private readonly string _connectionString;

        /// <summary>
        /// Инициализирует новый экземпляр класса с заданной строкой подключения.
        /// </summary>
        /// <param name="connectionString">Строка подключения ADO.NET.</param>
        public DatabaseConnection(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Создает и возвращает открытый объект соединения <see cref="IDbConnection"/>.
        /// </summary>
        /// <returns>Открытое подключение к базе данных.</returns>
        public IDbConnection CreateConnection()
        {
            // Возвращаем новый экземпляр SqlConnection для работы с SQL Server
            return new SqlConnection(_connectionString);
        }
    }
}