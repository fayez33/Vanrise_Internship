using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;
using Task_4.Models;

namespace Task_4.Controllers
{
    public class ClientsController : ApiController
    {
        private string _connString = ConfigurationManager.ConnectionStrings["InternshipConn"].ConnectionString;

        // Mapper for Clients
        private Func<IDataRecord, Client> _clientMapper = (record) => new Client
        {
            ID = (int)record["ClientID"],
            Name = record["Name"].ToString(),
            // Cast the integer from DB to our Enum
            Type = (ClientType)record["Type"],
            // Handle Nullable DateTime safely
            BirthDate = record["BirthDate"] == DBNull.Value ? (DateTime?)null : (DateTime)record["BirthDate"]
        };

        [HttpGet]
        [Route("api/Clients/GetAll")]
        public IHttpActionResult GetAll()
        {
            List<Client> clients = new List<Client>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                SqlCommand cmd = new SqlCommand("sp_GetAllClients", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        clients.Add(_clientMapper(reader));
                    }
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }
            return Ok(clients);
        }
        [HttpGet]
        [Route("api/Clients/Search")]
        public IHttpActionResult Search(string name = null, int? type = null)
        {
            List<Client> foundClients = new List<Client>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                SqlCommand cmd = new SqlCommand("sp_SearchClients", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                // Pass nulls if the user didn't select a filter
                cmd.Parameters.AddWithValue("@Name", (object)name ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Type", (object)type ?? DBNull.Value);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        foundClients.Add(_clientMapper(reader));
                    }
                }
                catch (Exception ex) { return InternalServerError(ex); }
            }
            return Ok(foundClients);
        }

        [HttpPost]
        [Route("api/Clients/Add")]
        public IHttpActionResult Add(Client client)
        {
            if (client == null) return BadRequest();

            if (client.Type == ClientType.Organization)
            {
                client.BirthDate = null;
            }

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                SqlCommand cmd = new SqlCommand("sp_AddClient", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Name", client.Name);
                cmd.Parameters.AddWithValue("@Type", (int)client.Type); // Store Enum as Int

                // Handle Null value for SQL parameter
                if (client.BirthDate.HasValue)
                    cmd.Parameters.AddWithValue("@BirthDate", client.BirthDate.Value);
                else
                    cmd.Parameters.AddWithValue("@BirthDate", DBNull.Value);

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
            return Ok(client);
        }

        [HttpPost]
        [Route("api/Clients/Update")]
        public IHttpActionResult Update(Client client)
        {
            if (client == null) return BadRequest();

            if (client.Type == ClientType.Organization)
            {
                client.BirthDate = null;
            }

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateClient", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID", client.ID);
                cmd.Parameters.AddWithValue("@Name", client.Name);
                cmd.Parameters.AddWithValue("@Type", (int)client.Type);

                if (client.BirthDate.HasValue)
                    cmd.Parameters.AddWithValue("@BirthDate", client.BirthDate.Value);
                else
                    cmd.Parameters.AddWithValue("@BirthDate", DBNull.Value);

                try
                {
                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    if (rows == 0) return NotFound();
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }
            return Ok(client);
        }
        [HttpDelete]
        [Route("api/Clients/Delete")]
        public IHttpActionResult Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteClient", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", id);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
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
    }
}