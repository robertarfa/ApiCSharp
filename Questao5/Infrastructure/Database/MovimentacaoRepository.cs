using Questao5.Domain.Entities;
using Questao5.Infrastructure.Sqlite;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Interfaces;

namespace Questao5.Domain.Repository
{
    public class MovimentacaoRepository : IMovimentacaoRepository
    {
        private readonly DatabaseConfig databaseConfig;

        public MovimentacaoRepository(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public int Criar(Movimento movimentacao)
        {
            var connect = new SqliteConnection(databaseConfig.Name);

            using (SqliteConnection connection = connect)
            {
                string query = "INSERT INTO Movimento(IdContaCorrente, Valor, TipoMovimento, DataMovimento) VALUES (@contaCorrenteId, @valor, @tipoMovimento, @dataMovimento); SELECT SCOPE_IDENTITY()";
                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@contaCorrenteId", movimentacao.IdContaCorrente);
                    command.Parameters.AddWithValue("@valor", movimentacao.Valor);
                    command.Parameters.AddWithValue("@tipoMovimento", movimentacao.TipoMovimento);
                    command.Parameters.AddWithValue("@dataMovimento", movimentacao.DataMovimento);

                    connection.Open();
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        public decimal CalcularSaldo(int numero)
        {
            using (SqliteConnection connection = new SqliteConnection(databaseConfig.Name))
            {
                string query = @"
            SELECT 
            SUM(CASE WHEN TipoMovimento = ""C"" THEN Valor ELSE 0 END) -
            SUM(CASE WHEN TipoMovimento = ""D"" THEN Valor ELSE 0 END) AS Saldo
            FROM Movimento m
            INNER JOIN ContaCorrente cc on cc.idcontacorrente = m.idcontacorrente
            WHERE cc.numero = @numero";

                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@numero", numero);
                    command.Parameters.AddWithValue("@Credito", TipoMovimento.C.ToString());
                    command.Parameters.AddWithValue("@Debito", TipoMovimento.D.ToString());

                    connection.Open();

                    object result = command.ExecuteScalar();
                    return result != DBNull.Value ? Convert.ToDecimal(result) : 0.00m;
                }
            }
        }
    }
}
