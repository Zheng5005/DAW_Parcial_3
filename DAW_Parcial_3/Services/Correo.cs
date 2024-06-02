using Microsoft.Data.SqlClient;

namespace DAW_Parcial_3.Services
{
    public class Correo
    {
        private IConfiguration _configuration;

        public Correo(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void enviar(String destino, String Asunto, String cuerpo)
        {
            try
            {
                String connectionString = _configuration.GetSection("ConnectionStrings").GetSection("DbConnection").Value;
                String sqlquery = "EXEC msdb.dbo.sp_send_dbmail" +
                                    "@profile_name = 'SQL_Catolica' ," +
                                    "@recipients = @Destinatario ," +
                                    "@body = @CuerpoMSG ,"+
                                    "@subject = @AsuntoMSG";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(sqlquery, connection))
                    {
                        command.Parameters.AddWithValue("Destinatario", destino);
                        command.Parameters.AddWithValue("CuerpoMSG", cuerpo);
                        command.Parameters.AddWithValue("AsuntoMSG", Asunto);

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
