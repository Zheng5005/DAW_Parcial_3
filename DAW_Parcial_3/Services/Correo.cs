using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;

namespace DAW_Parcial_3.Services
{
    public class Correo
    {
        private readonly IConfiguration _configuration;

        public Correo(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Enviar(string destino, string asunto, string cuerpo)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DbConnection");
                string sqlQuery = "EXEC msdb.dbo.sp_send_dbmail " +
                                  "@profile_name = 'SQL_Catolica', " +
                                  "@recipients = @Destinatario, " +
                                  "@body = @CuerpoMSG, " +
                                  "@subject = @AsuntoMSG";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Destinatario", destino);
                        command.Parameters.AddWithValue("@CuerpoMSG", cuerpo);
                        command.Parameters.AddWithValue("@AsuntoMSG", asunto);

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción adecuadamente (log, etc.)
                Console.WriteLine(ex.ToString());
            }

        }
    }
}
