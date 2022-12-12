
namespace StockApp.Model
{
    public class Comment
    {
        public Guid? CommentID { get; set; }

        public Guid? PostID { get; set; }

        public Guid? UserID { get; set; }

        public string? Content { get; set; }

        public DateTime? CreatedDate { get; set; }

    }
}
