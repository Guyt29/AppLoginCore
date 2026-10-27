using System.Security.Cryptography.X509Certificates;
using AppLoginCore.Models;
using AppLoginCore.Repository.Contract;
using Microsoft.AspNetCore.Mvc;

namespace AppLoginCore.Areas.Colaborador.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class ColaboradorApiController : Controller
    {
        private IColaboradorRepository _colaboradorRepository;

        public ColaboradorApiController(IColaboradorRepository colaboradorRepository)
        {
            _colaboradorRepository = colaboradorRepository;
        }

        [HttpGet]
        public IActionResult ListarColaborador()
        {
            var Colaboradores = _colaboradorRepository.ObterTodosColaboradores();

            return Ok(Colaboradores);
        }
        [HttpPost]
        public IActionResult Post(AppLoginCore.Models.Colaborador colaborador)
        {
            if (colaborador is null)
            {
                return BadRequest(new
                {
                    mensagem = "Os dados no Colaborador são obrigatórios"
                });
            }
            _colaboradorRepository.Cadastrar(colaborador);

            return Ok(new
            {
                mensagem = "Cadastro realizado com sucesso",
                Colaborador = colaborador
            });
        }
        //GET api/ColaboradorApi/1
        [HttpGet("{id}")]

        public IActionResult BuscarPorId(int id)
        {
            var colaborador = _colaboradorRepository.ObterColaborador(id);
            if (colaborador is null)
            {
                return NotFound(new
                {
                    mensagem = "Colaborador não encontrado"
                });
            }
            return Ok(colaborador);
        }

        [HttpPut("{id}")]

        public IActionResult Atualizar(int id, AppLoginCore.Models.Colaborador colaborador)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var ColaboradorExistente = _colaboradorRepository.ObterColaborador(id);

            if(ColaboradorExistente == null)
            {
                return NotFound(new
                {
                    mensagem = "Colaborador não encontrado"
                });
            }
            colaborador.Id = id;

            _colaboradorRepository.Atualizar(colaborador);

            return Ok(new
            {
                mensagem = "Colaborador atualizado com sucesso"
            });

        }
        [HttpDelete("{id}")]
        public IActionResult Excluir(int id)
        {
            var Colaborador = _colaboradorRepository.ObterColaborador(id);

            if(Colaborador == null)
            {
                return NotFound(new
                {
                    mensagem = "Colaborador não encontrado"
                });
            }

            _colaboradorRepository.Excluir(id);

            return Ok(new
            {
                mensagem = "Colaborador excluido com sucesso"
            });
        }
    }
}
