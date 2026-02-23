using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Task_4.Models;
using HttpDeleteAttribute = System.Web.Http.HttpDeleteAttribute;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;

namespace Task_4.Controllers
{
    public class PhoneNumberReservationsController : ApiController
    {
        private string _connString = ConfigurationManager.ConnectionStrings["InternshipConn"].ConnectionString;

        // Mapper: Handles Joins (ClientName, PhoneNumber) and Nullable EED
        private Func<IDataRecord, PhoneNumberReservation> _resMapper = (record) => new PhoneNumberReservation
        {
            ID = (int)record["ReservationID"],
            ClientID = (int)record["ClientID"],
            ClientName = record["ClientName"].ToString(),
            PhoneNumberID = (int)record["PhoneNumberID"],
            PhoneNumber = record["PhoneNumber"].ToString(),
            BED = (DateTime)record["BED"],
            EED = record["EED"] == DBNull.Value ? (DateTime?)null : (DateTime)record["EED"]
        };

        [HttpGet]
        [Route("api/Reservations/GetAll")]
        public IHttpActionResult GetAll()
        {
            List<PhoneNumberReservation> list = new List<PhoneNumberReservation>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                SqlCommand cmd = new SqlCommand("sp_GetAllReservations", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read()) list.Add(_resMapper(reader));
                }
                catch (Exception ex) { return InternalServerError(ex); }
            }
            return Ok(list);
        }

        [HttpGet]
        [Route("api/Reservations/Search")]
        public IHttpActionResult Search(int? clientId = null, int? phoneNumberId = null)
        {
            List<PhoneNumberReservation> list = new List<PhoneNumberReservation>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                SqlCommand cmd = new SqlCommand("sp_SearchReservations", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ClientID", (object)clientId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PhoneNumberID", (object)phoneNumberId ?? DBNull.Value);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read()) list.Add(_resMapper(reader));
                }
                catch (Exception ex) { return InternalServerError(ex); }
            }
            return Ok(list);
        }

        [HttpPost]
        [Route("api/Reservations/Add")]
        public IHttpActionResult Add(PhoneNumberReservation reservation)
        {
            if (reservation == null) return BadRequest("Invalid reservation data.");

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                SqlCommand cmd = new SqlCommand("sp_AddReservation", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ClientID", reservation.ClientID);
                cmd.Parameters.AddWithValue("@PhoneNumberID", reservation.PhoneNumberID);
                cmd.Parameters.AddWithValue("@BED", reservation.BED);
                if (reservation.EED.HasValue)
                    cmd.Parameters.AddWithValue("@EED", reservation.EED.Value);
                else
                    cmd.Parameters.AddWithValue("@EED", DBNull.Value);

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
            return Ok(reservation);
        }
    }
}
