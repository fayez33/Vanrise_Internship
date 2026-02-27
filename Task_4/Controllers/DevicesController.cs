using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;
using System.Web.Services.Description;
using Task_4.Models;


namespace Task_4.Controllers
{
    [Authorize]
    public class DevicesController : ApiController
    {
        // 1. Connection String
        private string _connString = ConfigurationManager.ConnectionStrings["InternshipConn"].ConnectionString;

        // 2. Updated Mapper: Matches your SQL columns (device_id, device_name)
        // This ensures the C# model (ID, Name) gets filled correctly from the DB columns.
        private Func<IDataRecord, Device> _deviceMapper = (record) => new Device
        {
            ID = (int)record["device_id"],       // Maps SQL 'device_id' to C# 'ID'
            Name = record["device_name"].ToString() // Maps SQL 'device_name' to C# 'Name'
        };

        [HttpGet]
        [Route("api/Devices/GetAll")]
        public IHttpActionResult GetAll()
        {
            List<Device> devices = new List<Device>();

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                // Use the Stored Procedure name
                SqlCommand cmd = new SqlCommand("sp_GetAllDevices", conn);
                cmd.CommandType = CommandType.StoredProcedure; // Important!

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        devices.Add(_deviceMapper(reader));
                    }
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }
            return Ok(devices);
        }

        [HttpGet]
        [Route("api/Devices/Search")]
        public IHttpActionResult Search(string name)
        {
            List<Device> foundDevices = new List<Device>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                SqlCommand cmd = new SqlCommand("sp_SearchDevices", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                // Pass nulls if the user didn't select a filter
                cmd.Parameters.AddWithValue("@Name", (object)name ?? DBNull.Value);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        foundDevices.Add(_deviceMapper(reader));
                    }
                }
                catch (Exception ex) { return InternalServerError(ex); }
            }
            return Ok(foundDevices);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/Devices/Add")]
        public IHttpActionResult Add(Device device)
        {
            if (device == null) return BadRequest();

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                SqlCommand cmd = new SqlCommand("sp_AddDevice", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                // Matches the parameter @Name in your Stored Procedure
                cmd.Parameters.AddWithValue("@Name", device.Name);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }
            return Ok(device);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/Devices/Update")]
        public IHttpActionResult Update(Device device)
        {
            if (device == null) return BadRequest();

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateDevice", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                // Matches parameters @ID and @Name in your Stored Procedure
                cmd.Parameters.AddWithValue("@ID", device.ID);
                cmd.Parameters.AddWithValue("@Name", device.Name);

                try
                {
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected == 0) return NotFound();
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }
            return Ok(device);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("api/Devices/Delete")]
        public IHttpActionResult Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteDevice", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", id);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    // Catch our custom error (Foreign Key violation)
                    if (ex.Number == 51000 || ex.Number == 547)
                    {
                        // Return 409 Conflict with the message
                        return Content(System.Net.HttpStatusCode.Conflict, ex.Message);
                    }
                    return InternalServerError(ex);
                }
            }
            return Ok();
        }
    }
}