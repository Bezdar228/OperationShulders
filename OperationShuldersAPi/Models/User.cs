namespace OperationShuldersAPi.Models
{
    public class Users
    {
        public int User_Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public DateTime Created_At { get; set; } = DateTime.UtcNow;

        // Навигационное свойство
        public Surgeon Surgeon { get; set; }
    }

}
