using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using StockApp.Model;

namespace StockApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SymbolsController : ControllerBase
    {
        [HttpGet("getFavorSymbol/{userID}")]
        public IActionResult GetPostByID([FromRoute] Guid userID)
        {
            try
            {
                string connectionString = "Host = localhost;Port = 3306;Database = stock_database;User Id = root;Password = 12345678";
                var mySqlConnection = new MySqlConnection(connectionString);

                string store = "Proc_GetSymbolByUserID";

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


        [HttpDelete("{symbolID}")]
        public IActionResult DeleteSymbolByID([FromRoute] Guid symbolID)
        {
            try
            {
                string connectionString = "Host = localhost;Port = 3306;Database = stock_database;User Id = root;Password = 12345678";
                var mySqlConnection = new MySqlConnection(connectionString);

                string deleteUserCommand = "DELETE FROM symbol WHERE SymbolID = @symbolID";

                var parameters = new DynamicParameters();
                parameters.Add("@symbolID", symbolID);

                var affectedRows = mySqlConnection.Execute(deleteUserCommand, parameters);

                if (affectedRows > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, symbolID);
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
        public IActionResult InsertPost([FromBody] Symbol symbol)
        {
            try
            {
                // Khởi tạo kết nối tới db
                string connectionString = "Host = localhost;Port = 3306;Database = stock_database;User Id = root;Password = 12345678";
                var mySqlConnection = new MySqlConnection(connectionString);

                // Chuẩn bị câu lệnh Update
                string insertPostCommand = "INSERT INTO symbol (SymbolID, UserID, ShortName, SymbolName)" +
                    "VALUES(@SymbolID, @UserID, @ShortName, @SymbolName) ";

                // Chuẩn bị tham số đầu vào cho câu lệnh Update
                var parameters = new DynamicParameters();
                var GuidID = Guid.NewGuid();
                parameters.Add("@SymbolID", GuidID);
                parameters.Add("@UserID", symbol.UserID);
                parameters.Add("@ShortName", symbol.ShortName);
                parameters.Add("@SymbolName", symbol.SymbolName);

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
    }
}
