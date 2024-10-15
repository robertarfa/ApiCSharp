using Microsoft.AspNetCore.Mvc;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces;
using Questao5.Domain.Repository;
using Questao5.Infrastructure.Database;

namespace Questao5.Infrastructure.Services.Controllers
{
    public class ContaCorrenteController : ControllerBase
    {

        private readonly IContaCorrenteRepository _contaCorrenteRepository;
        private readonly IMovimentacaoRepository _movimentacaoRepository;


        public ContaCorrenteController(
            IContaCorrenteRepository contaCorrenteRepository,
            IMovimentacaoRepository movimentacaoRepository
            )
        {
            _contaCorrenteRepository = contaCorrenteRepository;
            _movimentacaoRepository = movimentacaoRepository;
        }

        ///// <summary>
        ///// Adiciona uma movimentação a conta corrente.
        ///// </summary>
        ///// <param name="número">Referente ao número da conta corrente.</param>
        ///// <returns>IActionResult</returns>
        ///// <response code="200">Caso a recuperação do saldo seja feito com sucesso.</response>
        [HttpPost("Movimentar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Movimentar([FromBody] MovimentoView request)
        {
            try
            {
                // Valida��es
                var contaCorrente = _contaCorrenteRepository.ObterPorNumero(request.Numero);

                if (contaCorrente == null)
                {
                    return BadRequest(new { mensagem = "Conta corrente inv�lida.", tipo = "INVALID_ACCOUNT" });

                }

                if (contaCorrente.Ativo == 0)
                {
                    return BadRequest(new { mensagem = "Conta corrente inativa.", tipo = "INACTIVE_ACCOUNT" });

                }

                if (request.Valor <= 0)
                {
                    return BadRequest(new { mensagem = "Valor inv�lido.", tipo = "INVALID_VALUE" });

                }

                if (request.TipoMovimento != "C" && request.TipoMovimento != "D")
                {
                    return BadRequest(new { mensagem = "Tipo de movimentação inválida, deve ser C=Crédito ou D=Débito.", tipo = "INVALID_TYPE" });

                }

                // Cria a movimenta��o
                var movimentacao = new Movimento(contaCorrente.Id, request.Valor, request.TipoMovimento, request.DataMovimento);

                // Persiste a movimenta��o no banco de dados
                var movimentoId = _movimentacaoRepository.Criar(movimentacao);

                // Retorna o ID da movimenta��o
                return Ok(new { id = movimentacao.Id });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = "Erro interno do servidor.", tipo = "INTERNAL_ERROR" });
            }
        }

        ///// <summary>
        ///// Recupera o saldo da conta corrente
        ///// </summary>
        ///// <param name="número">Referente ao número da conta corrente.</param>
        ///// <returns>IActionResult</returns>
        ///// <response code="200">Caso a recuperação do saldo seja feito com sucesso.</response>
        [HttpGet("Saldo/{numero}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Saldo(int numero)
        {
            try
            {
                //// Valida��es
                var contaCorrente = _contaCorrenteRepository.ObterPorNumero(numero);

                if (contaCorrente == null)
                {
                    return BadRequest(new { mensagem = "Conta corrente inválida.", tipo = "INVALID_ACCOUNT" });

                }

                if (contaCorrente.Ativo == 0)
                {

                    return BadRequest(new { mensagem = "Conta corrente inativa.", tipo = "INACTIVE_ACCOUNT" });

                }


                // Calcula o saldo da conta corrente
                decimal saldo = _movimentacaoRepository.CalcularSaldo(numero);

                // Retorna o saldo
                return Ok(new { saldo });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = "Erro interno do servidor.", tipo = "INTERNAL_ERROR" });
            }
        }
    }
}
