﻿using System.ComponentModel;
using System.Runtime.Serialization;

namespace Questao5.Domain.Entities
{
    public class Movimento
    {

        public Movimento(Guid idContaCorrente, decimal valor, string tipoMovimento, string dataMovimento)
        {
            Id = Guid.NewGuid();
            IdContaCorrente = idContaCorrente;
            TipoMovimento = tipoMovimento;
            Valor = valor;
            DataMovimento = dataMovimento;

        }

        public Guid Id { get; set; }
        public Guid IdContaCorrente { get; set; }
        public string DataMovimento { get; set; }
        //(C = Credito, D = Debito).
        public string TipoMovimento { get; set; }

        public decimal Valor { get; set; }

        public int Numero { get; set; }


    }

    public enum TipoMovimento
    {
        [Description("C")]
        C,

        [Description("D")]
        D
    }


}
