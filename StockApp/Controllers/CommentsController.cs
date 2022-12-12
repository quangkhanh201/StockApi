using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using StockApp.Model;

namespace StockApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        [HttpGet("{commentID}")]
        public IActionResult GetPostByID([FromRoute] Guid commentID)
        {
            try
            {
                string connectionString = "Host = localhost;Port = 3306;Database = stock_database;User Id = root;Password = 12345678";
                var mySqlConnection = new MySqlConnection(connectionString);

                string store = "Proc_GetCommentByID";

                var parameters = new DynamicParameters();
                parameters.Add("$CommentID", commentID);

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


        [HttpPut("{commentID}")]
        public IActionResult UpdatePost([FromRoute] Guid commentID, [FromBody] Comment comment)
        {
            try
            {
                // Khởi tạo kết nối tới db
                string connectionString = "Host = localhost;Port = 3306;Database = stock_database;User Id = root;Password = 12345678";
                var mySqlConnection = new MySqlConnection(connectionString);

                // Chuẩn bị câu lệnh Update
                string updateUserCommand = "UPDATE comment " +
                    "SET PostID = @PostID, " +
                        "UserID = @UserID, " +
                        "Content = @Content " +
                        "CreatedDate = @CreatedDate " +
                        "WHERE CommentID = @CommentID;";

                // Chuẩn bị tham số đầu vào cho câu lệnh Update
                var parameters = new DynamicParameters();
                parameters.Add("@PostID", comment.PostID);
                parameters.Add("@UserID", comment.UserID);
                parameters.Add("@Content", comment.Content);
                parameters.Add("@CreatedDate", comment.CreatedDate);
                parameters.Add("@CommentID", commentID);

                // Thực hiện gọi vào db để chạy câu lệnh UPDATE với tham số đầu vào ở trên
                var affectedRows = mySqlConnection.Execute(updateUserCommand, parameters);

                // Xử lý kết quả trả về ở db
                if (affectedRows > 0)
                {
                    // Trả về dữ liệu cho client
                    return StatusCode(StatusCodes.Status200OK, commentID);
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



        [HttpDelete("{commentID}")]
        public IActionResult DeletePost([FromRoute] Guid commentID)
        {
            try
            {
                string connectionString = "Host = localhost;Port = 3306;Database = stock_database;User Id = root;Password = 12345678";
                var mySqlConnection = new MySqlConnection(connectionString);

                string deleteUserCommand = "DELETE FROM comment WHERE CommentID = @commentID";

                var parameters = new DynamicParameters();
                parameters.Add("@commentID", commentID);

                var affectedRows = mySqlConnection.Execute(deleteUserCommand, parameters);

                if (affectedRows > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, commentID);
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
        public IActionResult InsertPost([FromBody] Comment comment)
        {
            try
            {
                // Khởi tạo kết nối tới db
                string connectionString = "Host = localhost;Port = 3306;Database = stock_database;User Id = root;Password = 12345678";
                var mySqlConnection = new MySqlConnection(connectionString);

                // Chuẩn bị câu lệnh Update
                string insertPostCommand = "INSERT INTO comment (CommentID, PostID, UserID, Content, CreatedDate)" +
                    "VALUES(@CommentID, @PostID, @UserID, @Content, @CreatedDate) ";

                // Chuẩn bị tham số đầu vào cho câu lệnh Update
                var parameters = new DynamicParameters();
                var dateTimeNow = DateTime.Now;
                var GuidID = Guid.NewGuid();
                parameters.Add("@CommentID", GuidID);
                parameters.Add("@PostID", comment.PostID);
                parameters.Add("@UserID", comment.UserID);
                parameters.Add("@Content", comment.Content);
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


        [HttpGet("getCommentByPostID/{postID}")]
        public IActionResult GetCommentByPostID([FromRoute] Guid postID)
        {
            try
            {
                string connectionString = "Host = localhost;Port = 3306;Database = stock_database;User Id = root;Password = 12345678";
                var mySqlConnection = new MySqlConnection(connectionString);

                string store = "Proc_GetCommentByPostID";

                var parameters = new DynamicParameters();
                parameters.Add("$PostID", postID);

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
