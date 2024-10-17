namespace Dashboard_Backend.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }

        // Simulación de una base de datos en memoria
        private static List<User> Users = new List<User>();

    }



}
