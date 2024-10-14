using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database
{
    public class ContaCorrenteRepository : IContaCorrenteRepository
    {
        private readonly DatabaseConfig databaseConfig;

        public ContaCorrenteRepository(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public ContaCorrente ObterPorNumero(int numero)
        {
            var connect = new SqliteConnection(databaseConfig.Name);

            using (SqliteConnection connection = connect)
            {
                string query = "SELECT * FROM ContaCorrente WHERE Numero = @numero";
                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@numero", numero);
                    connection.Open();
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new ContaCorrente
                            {
                                Id = reader.GetGuid(0),
                                Numero = reader.GetInt32(1),
                                Nome = reader.GetString(2),
                                Ativo = reader.GetInt16(3)
                            };
                        }
                    }
                }
            }

            return null;
        }
    }
}
