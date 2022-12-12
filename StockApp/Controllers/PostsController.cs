using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using StockApp.Model;

namespace StockApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllPosts()
        {
            try
            {
                string connectionString = "Host = localhost;Port = 3306;Database = stock_database;User Id = root;Password = 12345678";
                var mySqlConnection = new MySqlConnection(connectionString);

                string getAllPost = "SELECT * FROM post";

                var users = mySqlConnection.Query<object>(getAllPost);

                if (users != null)
                {
                    return StatusCode(StatusCodes.Status200OK, users);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }


        [HttpGet("{postID}")]
        public IActionResult GetPostByID([FromRoute] Guid postID)
        {
            try
            {
                string connectionString = "Host = localhost;Port = 3306;Database = stock_database;User Id = root;Password = 12345678";
                var mySqlConnection = new MySqlConnection(connectionString);

                string store = "Proc_GetPostByID";

                var parameters = new DynamicParameters();
                parameters.Add("$PostID", postID);

                var data = mySqlConnection.QueryFirstOrDefault<object>(store, parameters, commandType: System.Data.CommandType.StoredProcedure);

                if (data != null)
                {
                    return StatusCode(StatusCodes.Status200OK, data);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }

        }


        [HttpPut("{postID}")]
        public IActionResult UpdatePost([FromRoute] Guid postID, [FromBody] Post post)
        {
            try
            {
                // Khởi tạo kết nối tới db
                string connectionString = "Host = localhost;Port = 3306;Database = stock_database;User Id = root;Password = 12345678";
                var mySqlConnection = new MySqlConnection(connectionString);

                // Chuẩn bị câu lệnh Update
                string updateUserCommand = "UPDATE post " +
                    "SET Content = @Content, " +
                        "ImgUrl = @ImgUrl " +
                        "CommentNumber = @CommentNumber " +
                        "LikeNumber = @LikeNumber " +
                        "WHERE PostID = @PostID;";

                // Chuẩn bị tham số đầu vào cho câu lệnh Update
                var parameters = new DynamicParameters();
                parameters.Add("@PostID", postID);
                parameters.Add("@Content", post.Content);
                parameters.Add("@ImgUrl", post.ImgUrl);
                parameters.Add("@CommentNumber", post.CommentNumber);
                parameters.Add("@LikeNumber", post.LikeNumber);

                // Thực hiện gọi vào db để chạy câu lệnh UPDATE với tham số đầu vào ở trên
                var affectedRows = mySqlConnection.Execute(updateUserCommand, parameters);

                // Xử lý kết quả trả về ở db
                if (affectedRows > 0)
                {
                    // Trả về dữ liệu cho client
                    return StatusCode(StatusCodes.Status200OK, postID);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }
            }
            catch (MySqlException mySqlException)
            {
                if (mySqlException.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e003");
                }
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }



        [HttpDelete("{postID}")]
        public IActionResult DeletePost([FromRoute] Guid postID)
        {
            try
            {
                string connectionString = "Host = localhost;Port = 3306;Database = stock_database;User Id = root;Password = 12345678";
                var mySqlConnection = new MySqlConnection(connectionString);

                string deleteUserCommand = "DELETE FROM post WHERE PostID = @postID";

                var parameters = new DynamicParameters();
                parameters.Add("@postID", postID);

                var affectedRows = mySqlConnection.Execute(deleteUserCommand, parameters);

                if (affectedRows > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, postID);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }



        [HttpPost]
        public IActionResult InsertPost([FromBody] Post post)
        {
            try
            {
                // Khởi tạo kết nối tới db
                string connectionString = "Host = localhost;Port = 3306;Database = stock_database;User Id = root;Password = 12345678";
                var mySqlConnection = new MySqlConnection(connectionString);

                // Chuẩn bị câu lệnh Update
                string insertPostCommand = "INSERT INTO post (PostID, UserID, Content, ImgUrl, CommentNumber, LikeNumber, CreatedDate)" +
                    "VALUES(@PostID, @UserID, @Content, @ImgUrl, @CommentNumber, @LikeNumber, @CreatedDate) ";

                // Chuẩn bị tham số đầu vào cho câu lệnh Update
                var parameters = new DynamicParameters();
                var dateTimeNow = DateTime.Now;
                var GuidID = Guid.NewGuid();
                parameters.Add("@PostID", GuidID);
                parameters.Add("@UserID", post.UserID);
                parameters.Add("@Content", post.Content);
                parameters.Add("@ImgUrl", post.ImgUrl);
                parameters.Add("@CommentNumber", post.CommentNumber);
                parameters.Add("@LikeNumber", post.LikeNumber);
                parameters.Add("@CreatedDate", dateTimeNow);

                // Thực hiện gọi vào db để chạy câu lệnh UPDATE với tham số đầu vào ở trên
                var affectedRows = mySqlConnection.Execute(insertPostCommand, parameters);

                // Xử lý kết quả trả về ở db
                if (affectedRows > 0)
                {
                    // Trả về dữ liệu cho client
                    return StatusCode(StatusCodes.Status200OK, GuidID);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }
            }
            catch (MySqlException mySqlException)
            {
                if (mySqlException.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e003");
                }
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }


        [HttpGet("getPostByUserID/{userID}")]
        public IActionResult GetPostByUserID([FromRoute] Guid userID)
        {
            try
            {
                string connectionString = "Host = localhost;Port = 3306;Database = stock_database;User Id = root;Password = 12345678";
                var mySqlConnection = new MySqlConnection(connectionString);

                string store = "Proc_GetPostByUserID";

                var parameters = new DynamicParameters();
                parameters.Add("$UserID", userID);

                var data = mySqlConnection.Query<object>(store, parameters, commandType: System.Data.CommandType.StoredProcedure);

                if (data != null)
                {
                    return StatusCode(StatusCodes.Status200OK, data);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }

        }
    }
}
