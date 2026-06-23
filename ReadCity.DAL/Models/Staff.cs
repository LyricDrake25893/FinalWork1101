namespace ReadCity.DAL.Models
{
    public class Staff
    {
        /// <summary>Первичный ключ.</summary>
        public int StaffId { get; set; }

        /// <summary>Наименование роли</summary>
        public string Role { get; set; } = string.Empty;

        /// <summary>Полное имя</summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>Логин</summary>
        public string Login { get; set; } = string.Empty;

        /// <summary>Пароль</summary>
        public string Password { get; set; } = string.Empty;
    }
}