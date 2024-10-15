using System.ComponentModel;
using System.Runtime.Serialization;

namespace Questao5.Domain.Entities
{
    public class MovimentoView
    {

        public MovimentoView(decimal valor, string tipoMovimento, string dataMovimento)
        {

            TipoMovimento = tipoMovimento;
            Valor = valor;
            DataMovimento = dataMovimento;

        }

        public string DataMovimento { get; set; }
        //(C = Credito, D = Debito).
        public string TipoMovimento { get; set; }

        public decimal Valor { get; set; }

        public int Numero { get; set; }


    }



}
