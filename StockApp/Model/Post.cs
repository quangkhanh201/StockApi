
namespace StockApp.Model
{
    public class Post
    {
        public Guid? PostID { get; set; }

        public Guid? UserID { get; set; }

        public string? Content { get; set; }

        public string? ImgUrl { get; set; }

        public int? CommentNumber { get; set; }

        public int? LikeNumber { get; set; }

        public DateTime? CreatedDate { get; set; }

    }
}
