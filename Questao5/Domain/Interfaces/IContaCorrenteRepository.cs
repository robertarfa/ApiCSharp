﻿using Questao5.Domain.Entities;

namespace Questao5.Domain.Interfaces
{
    public interface IContaCorrenteRepository
    {
        ContaCorrente ObterPorId(int contaCorrenteId);
    }
}
