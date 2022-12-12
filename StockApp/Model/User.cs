
namespace StockApp.Model
{
    public class User
    {
        public Guid? UserID { get; set; }

        public string? UserName { get; set; }

        public string? Password { get; set; }

        public string? Avatar { get; set; }

        public DateTime? CreatedDate { get; set; }

    }
}