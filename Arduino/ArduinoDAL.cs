
using System;
using Microsoft.Data.Sqlite;

namespace Arduino
{
    public class ArduinoDAL
    {
        private readonly string conexao =
            @"Data Source=C:\UNI\Automacao\Arduino\bin\Debug\Automacao.db";

        public void Registrar(string dispositivo, string acao)
        {
            using (var conn = new SqliteConnection(conexao))
            {
                conn.Open();

                string sql = @"
                    INSERT INTO Historico
                    (DataHora, Dispositivo, Acao)
                    VALUES (@data, @dispositivo, @acao)";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@data",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@dispositivo", dispositivo);
                    cmd.Parameters.AddWithValue("@acao", acao);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
