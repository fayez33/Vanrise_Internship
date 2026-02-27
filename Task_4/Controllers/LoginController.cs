using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http;
using Task_4.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Task_4.Controllers
{
    [RoutePrefix("api/Login")]
    public class LoginController : ApiController
    {
        // Adjust this if your connection string is named differently
        private string _connString = ConfigurationManager.ConnectionStrings["InternshipConn"].ConnectionString;

        [HttpPost]
        [Route("Authenticate")]
        public IHttpActionResult Authenticate([FromBody] LoginRequestDTO loginRequest)
        {
            // Basic validation
            if (loginRequest == null || string.IsNullOrWhiteSpace(loginRequest.Username) || string.IsNullOrWhiteSpace(loginRequest.Password))
            {
                return BadRequest("Username and password are required.");
            }

            string hashedPassword = ComputeSha256Hash(loginRequest.Password);

            // Step 2: Check the database for a matching username and hash combination
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                SqlCommand cmd = new SqlCommand("sp_ValidateUserLogin", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Username", loginRequest.Username);
                cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);

                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int userId = Convert.ToInt32(reader["UserID"]);
                            string username = reader["Username"].ToString();
                            string userRole = reader["UserRole"].ToString(); // Read the new role

                            // Pass the role to the token generator
                            string token = GenerateJwtToken(userId, username, userRole);

                            // Return the role to the frontend
                            return Ok(new { Success = true, Token = token, Username = username, Role = userRole });
                        }
                        else
                        {
                            // No record found: Invalid username or password
                            return Unauthorized(); // Returns an HTTP 401 status
                        }
                    }
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }
        }

        [HttpPost]
        [Route("Register")]
        public IHttpActionResult Register([FromBody] LoginRequestDTO registerRequest)
        {
            if (registerRequest == null || string.IsNullOrWhiteSpace(registerRequest.Username) || string.IsNullOrWhiteSpace(registerRequest.Password))
            {
                return BadRequest("Username and password are required.");
            }

            // Hash the password using the EXACT same helper method as Login
            string hashedPassword = ComputeSha256Hash(registerRequest.Password);

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                SqlCommand cmd = new SqlCommand("sp_RegisterUser", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Username", registerRequest.Username);
                cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return Ok(new { Success = true, Message = "Registration successful!" });
                }
                catch (SqlException ex)
                {
                    // Catch the custom THROW error we wrote in the SQL script
                    if (ex.Number == 50001)
                    {
                        return BadRequest("Username already exists. Please choose another.");
                    }
                    return InternalServerError(ex);
                }
            }
        }

        // --- Helper Method: SHA256 Hashing Logic ---
        private string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256 instance
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash returns a byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert the byte array back to a hexadecimal string representation
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2")); // "x2" formats it as lowercase hex
                }
                return builder.ToString();
            }
        }
        // --- Helper Method: Generate JWT ---
        private string GenerateJwtToken(int userId, string username, string userRole)
        {
            // In a real app, this key lives securely in Web.config. 
            // It must be at least 16 characters long.
            string secureKey = "MySuperSecretInternshipKey2026!!";
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secureKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Create the "Claims" (the data stored inside the token)
            var claims = new[]
            {
                 new Claim(JwtRegisteredClaimNames.Sub, username),
                 new Claim("userId", userId.ToString()),
                 new Claim(ClaimTypes.Role, userRole), // Adds the official Role claim
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Build the token
            var token = new JwtSecurityToken(
                issuer: "VanriseInternshipApp",
                audience: "VanriseInternshipApp",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}