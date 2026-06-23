using Dapper;
using ReadCity.DAL.Database;
using ReadCity.DAL.Models;
using System.Collections.Generic;

namespace ReadCity.DAL.Services
{
    /// <summary>
    /// Предоставляет операции доступа к данным для сущностей <see cref="Book"/>.
    /// </summary>
    public class BookService
    {
        private readonly DatabaseConnection _db;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="BookService"/>.
        /// </summary>
        /// <param name="db">Фабрика подключений к базе данных.</param>
        public BookService(DatabaseConnection db)
        {
            _db = db;
        }

        /// <summary>
        /// Возвращает все книги из базы данных.
        /// </summary>
        /// <returns>Список всех книг.</returns>
        public List<Book> GetAllBooks()
        {
            using var connection = _db.CreateConnection();
            return connection.Query<Book>("SELECT * FROM Books").AsList();
        }

        /// <summary>
        /// Возвращает список всех уникальных наименований издателей.
        /// </summary>
        /// <returns>Список строк с именами издателей.</returns>
        public List<string> GetAllPublishers()
        {
            using var connection = _db.CreateConnection();
            return connection
                .Query<string>("SELECT DISTINCT Publisher FROM Books ORDER BY Publisher")
                .AsList();
        }

        /// <summary>
        /// Возвращает общее количество книг в базе данных.
        /// </summary>
        /// <returns>Общее количество записей книг.</returns>
        public int GetTotalCount()
        {
            using var connection = _db.CreateConnection();
            return connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Books");
        }
    }
}