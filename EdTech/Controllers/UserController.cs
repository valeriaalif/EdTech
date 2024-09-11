using Dapper;
using EdTech.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using EdTech.Entities;

namespace EdTech.Controllers
{
  [Route("api/[controller]")]
   [ApiController]
   public class UserController : ControllerBase
   {
        private readonly IConnectionProvider _connectionProvider;
        private readonly ITools _tools;
        private readonly IBCryptHelper _bCryptHelper;

        public UserController(IConnectionProvider connectionProvider, ITools tools, IBCryptHelper bCryptHelper)
        {
            _connectionProvider = connectionProvider;
            _tools = tools;
            _bCryptHelper = bCryptHelper;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("RegisterAccount")]
        public async Task<IActionResult> RegisterAccount(User entity)
        {
            ApiResponse<string> response = new ApiResponse<string>();

            try
            {
                if (string.IsNullOrEmpty(entity.UserName) || string.IsNullOrEmpty(entity.UserEmail))
                {
                    response.ErrorMessage = "Name and email are required.";
                    response.Code = 400;
                    return BadRequest(response);
                }

                //HashRandomPassword
                var randomPassword = _tools.CreatePassword(8);
                var hashedPassword = _bCryptHelper.HashPassword(randomPassword);
                entity.UserPassword = hashedPassword;

                using (var context = _connectionProvider.GetConnection())
                {
                    // Use MakeHtmlNewUser method to create a customized HTML email
                    string body = _tools.MakeHtmlNewUser(entity, randomPassword);

                    // Check if the HTML body was generated successfully
                    if (body == "Error")
                    {
                        response.ErrorMessage = "Error creating HTML email.";
                        response.Code = 500;
                        return BadRequest(response);
                    }


                    string recipient = entity.UserEmail;

                    bool emailIsSend = _tools.SendEmail(recipient, "New Account", body);
                    if (emailIsSend)
                    {
                        var data = await context.ExecuteAsync("RegisterAccount",
                        new { entity.UserName, entity.UserEmail, entity.UserPassword, entity.UserType, entity.UserState },
                        commandType: CommandType.StoredProcedure);

                        if (data != 0)
                        {
                            response.Success = true;
                            response.Code = 200;
                            return Ok(response);
                        }
                        else
                        {
                            response.ErrorMessage = "Error saving new user";
                            response.Code = 500;
                            return BadRequest(response);
                        }
                    }
                    else
                    {
                        response.ErrorMessage = "Error Sending email";
                        response.Code = 500;
                        return BadRequest(response);
                    }
                }
            }
            catch (SqlException ex)
            {
                response.ErrorMessage = "Unexpected Error: " + ex.Message;
                response.Code = 500;
                return BadRequest(response);
            }
        }

    }

}
   
