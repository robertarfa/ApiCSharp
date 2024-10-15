using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces;
using Questao5.Infrastructure.Services.Controllers;
using System;
using Xunit;

namespace Questao5.Test
{
    public class ContaCorrentControllerTest
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository = Substitute.For<IContaCorrenteRepository>();
        private readonly IMovimentacaoRepository _movimentacaoRepository = Substitute.For<IMovimentacaoRepository>();
        private readonly ContaCorrenteController _contaCorrenteController;

        public ContaCorrentControllerTest()
        {
            _contaCorrenteController = new ContaCorrenteController(_contaCorrenteRepository, _movimentacaoRepository);
        }

        [Fact]
        public void RecuperaSaldo_TabelaMovimento()
        {
            // Arrange
            var numero = 123;
            var contaCorrente = new ContaCorrente
            {
                Id = Guid.NewGuid(),
                Numero = numero,
                Nome = "Teste",
                Ativo = 1
            };

            decimal saldoEsperado = 10.0m;

            _contaCorrenteRepository.ObterPorNumero(numero).Returns(contaCorrente);
            _movimentacaoRepository.CalcularSaldo(numero).Returns(saldoEsperado);

            // Act
            var resultado = _contaCorrenteController.Saldo(numero) as OkObjectResult;

            // Assert
            _contaCorrenteRepository.Received(1).ObterPorNumero(numero);
            _movimentacaoRepository.Received(1).CalcularSaldo(numero);

            Assert.NotNull(resultado);
            Assert.Equal(200, resultado.StatusCode);

            var saldoRetornado = resultado.Value.GetType().GetProperty("saldo").GetValue(resultado.Value);

            Assert.NotNull(saldoRetornado);
            Assert.Equal(saldoEsperado, saldoRetornado);
        }
    }
}