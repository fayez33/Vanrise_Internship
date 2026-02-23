using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Xml.Linq;
using Task_4.Models;
using HttpDeleteAttribute = System.Web.Http.HttpDeleteAttribute;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;
using RoutePrefixAttribute = System.Web.Http.RoutePrefixAttribute;

namespace Task_4.Controllers
{
    public class PhoneNumbersController : ApiController
    {
        // 1. Connection String from Web.config
        private string _connString = ConfigurationManager.ConnectionStrings["InternshipConn"].ConnectionString;

        // This maps SQL columns (PhoneNumberID, Number, DeviceID, DeviceName) to the C# Model
        private Func<IDataRecord, PhoneNumber> _phoneMapper = (record) => new PhoneNumber
        {
            ID = (int)record["PhoneNumberID"],
            Number = record["Number"].ToString(),
            DeviceID = (int)record["DeviceID"],
            DeviceName = record["DeviceName"].ToString()
        };

        [HttpGet]
        [Route("api/PhoneNumbers/GetAll")]
        public IHttpActionResult GetAll()
        {
            List<PhoneNumber> phoneNumbers = new List<PhoneNumber>();

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                SqlCommand cmd = new SqlCommand("sp_GetAllPhoneNumbers", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        phoneNumbers.Add(_phoneMapper(reader));
                    }
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }
            return Ok(phoneNumbers);
        }

        [HttpGet]
        [Route("api/PhoneNumbers/Search")]
        public IHttpActionResult Search(string number = null, int? deviceId = null)
        {
            List<PhoneNumber> foundNumbers = new List<PhoneNumber>();

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                SqlCommand cmd = new SqlCommand("sp_SearchPhoneNumbers", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                // Handle null parameters for the Stored Procedure
                cmd.Parameters.AddWithValue("@Number", (object)number ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DeviceID", (object)deviceId ?? DBNull.Value);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        foundNumbers.Add(_phoneMapper(reader));
                    }
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }
            return Ok(foundNumbers);
        }

        [System.Web.Http.HttpPost]
        [Route("api/PhoneNumbers/Add")]
        public IHttpActionResult Add(PhoneNumber phone)
        {
            if (phone == null) return BadRequest("Invalid phone data.");

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                SqlCommand cmd = new SqlCommand("sp_AddPhoneNumber", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Number", phone.Number);
                cmd.Parameters.AddWithValue("@DeviceID", phone.DeviceID);

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
            return Ok(phone);
        }
        [HttpDelete]
        [Route("api/PhoneNumbers/Delete")]
        public IHttpActionResult Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                SqlCommand cmd = new SqlCommand("sp_DeletePhoneNumber", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", id);

                try
                {
                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    if (rows == 0) return NotFound();
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 51000 || ex.Number == 547)
                        return Content(System.Net.HttpStatusCode.Conflict, ex.Message);
                    return InternalServerError(ex);
                }
            }
            return Ok();
        }

        [HttpPost]
        [Route("api/PhoneNumbers/Update")]
        public IHttpActionResult Update(PhoneNumber phone)
        {
            if (phone == null) return BadRequest("Invalid phone data.");

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                SqlCommand cmd = new SqlCommand("sp_UpdatePhoneNumber", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID", phone.ID);
                cmd.Parameters.AddWithValue("@Number", phone.Number);
                cmd.Parameters.AddWithValue("@DeviceID", phone.DeviceID);

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
            return Ok(phone);
        }
    }
}