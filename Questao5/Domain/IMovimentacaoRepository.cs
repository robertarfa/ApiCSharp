using Questao5.Domain.Entities;
using Questao5.Domain.Repository;
using Questao5.Domain;


namespace Questao5.Domain.Interfaces
{
    public interface IMovimentacaoRepository
    {
        int Criar(Movimento movimentacao);
        decimal CalcularSaldo(int IdContaCorrente);
    }
}
