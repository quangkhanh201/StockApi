using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using StockApp.Model;

namespace StockApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            try
            {
                string connectionString = "Host = localhost;Port = 3306;Database = stock_database;User Id = root;Password = 12345678";
                var mySqlConnection = new MySqlConnection(connectionString);

                string getAllUser = "SELECT * FROM user";

                var users = mySqlConnection.Query<object>(getAllUser);

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


        [HttpGet("{userID}")]
        public IActionResult GetUserByID([FromRoute] Guid userID)
        {
            try
            {
                string connectionString = "Host = localhost;Port = 3306;Database = stock_database;User Id = root;Password = 12345678";
                var mySqlConnection = new MySqlConnection(connectionString);

                string store = "Proc_GetUserByID";

                var parameters = new DynamicParameters();
                parameters.Add("$UserID", userID);

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


        [HttpPut("{userID}")]
        public IActionResult UpdateUser([FromRoute] Guid userID, [FromBody] User user)
        {
            try
            {
                // Khởi tạo kết nối tới db
                string connectionString = "Host = localhost;Port = 3306;Database = stock_database;User Id = root;Password = 12345678";
                var mySqlConnection = new MySqlConnection(connectionString);

                // Chuẩn bị câu lệnh Update
                string updateUserCommand = "UPDATE user " +
                    "SET UserName = @UserName, " +
                        "Password = @Password, " +
                        "Avatar = @Avatar " +
                        "WHERE UserID = @UserID;";

                // Chuẩn bị tham số đầu vào cho câu lệnh Update
                var parameters = new DynamicParameters();
                parameters.Add("@UserID", userID);
                parameters.Add("@UserName", user.UserName);
                parameters.Add("@Password", user.Password);
                parameters.Add("@Avatar", user.Avatar);

                // Thực hiện gọi vào db để chạy câu lệnh UPDATE với tham số đầu vào ở trên
                var affectedRows = mySqlConnection.Execute(updateUserCommand, parameters);

                // Xử lý kết quả trả về ở db
                if (affectedRows > 0)
                {
                    // Trả về dữ liệu cho client
                    return StatusCode(StatusCodes.Status200OK, userID);
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



        [HttpDelete("{userID}")]
        public IActionResult DeleteUser([FromRoute] Guid userID)
        {
            try
            {
                string connectionString = "Host = localhost;Port = 3306;Database = stock_database;User Id = root;Password = 12345678";
                var mySqlConnection = new MySqlConnection(connectionString);

                string deleteUserCommand = "DELETE FROM user WHERE UserID = @userID";

                var parameters = new DynamicParameters();
                parameters.Add("@userID", userID);

                var affectedRows = mySqlConnection.Execute(deleteUserCommand, parameters);

                if (affectedRows > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, userID);
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
    }
}
