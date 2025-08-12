using Getask.Repository.Models;
using Getask.Services;
using Getask.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class PainelController : Controller
{
    private readonly IColunaTarefasService _colunaTarefasService;
    private readonly ITarefaService _tarefaService;

    public PainelController(IColunaTarefasService colunaTarefasService, ITarefaService tarefaService)
    {
        _colunaTarefasService = colunaTarefasService;
        _tarefaService = tarefaService;
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        var colunas = await _colunaTarefasService.ObterColunasTarefasComTarefas();
        return View(colunas);
    }

    [HttpPut]
    public async Task<IActionResult> MoverTarefa(int tarefaId, int novaColunaId, int novaPosicao)
    {
        try
        {
            await _tarefaService.MoverTarefaAsync(tarefaId, novaColunaId, novaPosicao);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Erro ao mover a tarefa.");
        }
    }

    [HttpPost]
    [Route("Painel/AdicionarTarefa")]
    public async Task<IActionResult> AdicionarTarefa([FromBody] Tarefa tarefa)
    {
        if (tarefa == null)
        {
            return BadRequest();
        }
        try
        {
            await _tarefaService.AdicionarTarefaAsync(tarefa);
            return CreatedAtAction(nameof(Index), new { id = tarefa.Id }, tarefa);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ops não foi possivel mover a tarefa");
        }
    }

    [HttpDelete]
    [Route("Painel/DeletarTarefa/{id}")]
    public async Task<IActionResult> DeletarTarefa(int id)
    {
        try
        {
            await _tarefaService.DeletarTarefaAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Erro interno do servidor ao deletar a tarefa.");
        }
    }

    [HttpPost]
    [Route("Painel/CriarColuna")]
    public async Task<IActionResult> CriarColuna([FromBody] ColunaTarefas coluna)
    {
        try
        {
            await _colunaTarefasService.CriarColunaAsync(coluna.Titulo);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Erro interno do servidor.");
        }
    }

    [HttpPut]
    [Route("Painel/MoverColuna")]
    public async Task<IActionResult> MoverColuna(int colunaId, int novaPosicao)
    {
        try
        {
            await _colunaTarefasService.MoverColunaAsync(colunaId, novaPosicao);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Erro interno do servidor.");
        }
    }

    [HttpDelete]
    [Route("Painel/DeletarColuna/{colunaId}")]
    public async Task<IActionResult> DeletarColuna(int colunaId)
    {
        try
        {
            await _colunaTarefasService.DeletarColunaAsync(colunaId);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Erro interno do servidor.");
        }
    }

    [HttpPut]
    public async Task<IActionResult> AtualizarColuna([FromBody] ColunaTarefas coluna)
    {
        if (coluna == null || coluna.Id <= 0 || string.IsNullOrWhiteSpace(coluna.Titulo))
        {
            return BadRequest("Dados da coluna inválidos.");
        }

        var colunaExistente = await _colunaTarefasService.ObterPorId(coluna.Id);
        if (colunaExistente == null)
        {
            return NotFound("Coluna não encontrada.");
        }

        colunaExistente.Titulo = coluna.Titulo;

        await _colunaTarefasService.AtualizarColunaTarefas(colunaExistente);

        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> AtualizarTarefa([FromBody] Tarefa tarefa)
    {
        if (tarefa == null || tarefa.Id <= 0 || string.IsNullOrWhiteSpace(tarefa.Titulo))
        {
            return BadRequest("Dados da tarefa inválidos.");
        }

        var tarefaExistente = await _tarefaService.ObterTarefaPorId(tarefa.Id);
        if (tarefaExistente == null)
        {
            return NotFound("Tarefa não encontrada.");
        }

        tarefaExistente.Titulo = tarefa.Titulo;
        tarefaExistente.Descricao = tarefa.Descricao;
        tarefaExistente.DataAtualizacao = DateTime.UtcNow;

        await _tarefaService.AtualizarTarefaAsync(tarefaExistente);

        return Ok();
    }
}