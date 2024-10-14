using Microsoft.AspNetCore.Mvc;
using Questao5.Domain.Entities;
using Questao5.Domain.Repository;
using Questao5.Infrastructure.Database;

namespace Questao5.Infrastructure.Services.Controllers
{
    public class ContaCorrenteController : ControllerBase
    {

        private readonly ContaCorrenteRepository _contaCorrenteRepository;
        private readonly MovimentacaoRepository _movimentacaoRepository;


        public ContaCorrenteController(
            ContaCorrenteRepository contaCorrenteRepository,
            MovimentacaoRepository movimentacaoRepository
            )
        {
            _contaCorrenteRepository = contaCorrenteRepository;
            _movimentacaoRepository = movimentacaoRepository;
        }

        [HttpPost("Movimentar")]
        public IActionResult Movimentar([FromBody] Movimento request)
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

                if (request.TipoMovimento != TipoMovimento.C && request.TipoMovimento != TipoMovimento.D)
                {
                    return BadRequest(new { mensagem = "Tipo de movimenta��o inv�lido.", tipo = "INVALID_TYPE" });

                }

                // Cria a movimenta��o
                var movimentacao = new Movimento(contaCorrente.Id, request.Valor, request.TipoMovimento, request.DataMovimento);

                // Persiste a movimenta��o no banco de dados
                var movimentoId = _movimentacaoRepository.Criar(movimentacao);

                // Retorna o ID da movimenta��o
                return Ok(new { id = movimentoId });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = "Erro interno do servidor.", tipo = "INTERNAL_ERROR" });
            }
        }

        [HttpGet("Saldo/{numero}")]
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
                return Ok(new { saldo = saldo });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = "Erro interno do servidor.", tipo = "INTERNAL_ERROR" });
            }
        }
    }
}
