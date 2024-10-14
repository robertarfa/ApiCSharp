using System;
using System.Globalization;

namespace Questao1
{
    class ContaBancaria
    {

        private readonly int numeroConta;
        private string nomeTitular;
        public double saldo;


        public ContaBancaria(int numeroConta, string nomeTitular, double depositoInicial = 0)
        {
            this.numeroConta = numeroConta;
            this.nomeTitular = nomeTitular;
            this.saldo = depositoInicial;
        }

        public int NumeroConta
        {
            get { return numeroConta; }
        }

        public string NomeTitular
        {
            get { return nomeTitular; }
            set { nomeTitular = value; }
        }


        // Método para realizar um depósito
        public void Deposito(double valor)
        {
            if (valor > 0)
            {
                saldo += valor;

            }
        }

        // Método para realizar um saque
        public void Saque(double valor)
        {
            double taxaSaque = 3.50;
            if (valor > 0)
            {
                saldo -= (valor + taxaSaque);

            }
        }

        public static void ExibirDados(ContaBancaria conta)
        {
            Console.WriteLine($"Conta {conta.NumeroConta}, Titular: {conta.NomeTitular}, Saldo: ${conta.saldo}");
        }
    }
}
