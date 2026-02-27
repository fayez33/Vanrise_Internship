using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using Task_4.Models;

namespace Task_4.Controllers
{
    [Authorize]
    [RoutePrefix("api/Reports")]
    public class ReportsController : ApiController
    {
        // Change this line to match how you get your connection string in your other controllers
        private string _connString = ConfigurationManager.ConnectionStrings["InternshipConn"].ConnectionString;

        // --- Task 1: Clients Per Type Report ---
        [HttpGet]
        [Route("ClientsPerType")]
        public IHttpActionResult GetClientsPerType(int? type = null)
        {
            List<ClientTypeReportDTO> reportData = new List<ClientTypeReportDTO>();

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                SqlCommand cmd = new SqlCommand("sp_GetClientsPerTypeReport", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                // Handle the nullable type parameter
                cmd.Parameters.AddWithValue("@Type", type.HasValue ? (object)type.Value : DBNull.Value);

                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reportData.Add(new ClientTypeReportDTO
                            {
                                Type = Convert.ToInt32(reader["Type"]),
                                NoOfClients = Convert.ToInt32(reader["NoOfClients"])
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }
            return Ok(reportData);
        }

        // --- Task 2: Phone Numbers Status Per Device Report ---
        [HttpGet]
        [Route("PhoneStatus")]
        public IHttpActionResult GetPhoneStatusReport(int? deviceId = null, bool? isReserved = null)
        {
            List<PhoneStatusReportDTO> reportData = new List<PhoneStatusReportDTO>();

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                SqlCommand cmd = new SqlCommand("sp_GetPhoneStatusReport", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                // Handle the nullable parameters
                cmd.Parameters.AddWithValue("@DeviceID", deviceId.HasValue ? (object)deviceId.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@IsReserved", isReserved.HasValue ? (object)isReserved.Value : DBNull.Value);

                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reportData.Add(new PhoneStatusReportDTO
                            {
                                Device = reader["Device"].ToString(),
                                Status = reader["Status"].ToString(),
                                NoOfPhoneNumbers = Convert.ToInt32(reader["NoOfPhoneNumbers"])
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }
            return Ok(reportData);
        }
    }
}