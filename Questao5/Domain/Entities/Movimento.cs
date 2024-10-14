namespace Questao5.Domain.Entities
{
    public class Movimento
    {

        public Movimento(Guid idContaCorrente, decimal valor, TipoMovimento tipoMovimento, string dataMovimento)
        {
            IdContaCorrente = idContaCorrente;
            TipoMovimento = tipoMovimento;
            Valor = valor;
            DataMovimento = dataMovimento;

        }

        public Guid Id { get; set; }
        public Guid IdContaCorrente { get; set; }
        public string DataMovimento { get; set; }
        //(C = Credito, D = Debito).
        public TipoMovimento TipoMovimento { get; set; }

        public decimal Valor { get; set; }

        public int Numero { get; set; }

    }

    public enum TipoMovimento
    {
        C = 0,
        D = 1
    }
}
