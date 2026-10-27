using System.Security.Cryptography.X509Certificates;
using AppLoginCore.Models;
using AppLoginCore.Repository.Contract;
using Microsoft.AspNetCore.Mvc;

namespace AppLoginCore.Areas.Colaborador.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteAPIController : Controller
    {
        private IClienteRepository _clienteRepository;

        public ClienteAPIController(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        [HttpGet]
        public IActionResult ListarCliente()
        {
            var clientes = _clienteRepository.ObterTodosClientes();

            return Ok(clientes);
        }
        [HttpPost]
        public IActionResult Post(Cliente cliente)
        {
            if (cliente is null)
            {
                return BadRequest(new
                {
                    mensagem = "Os dados no cliente são obrigatórios"
                });
            }
            _clienteRepository.Cadastrar(cliente);

            return Ok(new
            {
                mensagem = "Cadastro realizado com sucesso",
                cliente = cliente
            });
        }
        //GET api/ClienteApi/1
        [HttpGet("{id}")]

        public IActionResult BuscarPorId(int id)
        {
            var cliente = _clienteRepository.ObterCliente(id);
            if (cliente is null)
            {
                return NotFound(new
                {
                    mensagem = "Cliente não encontrado"
                });
            }
            return Ok(cliente);
        }

        [HttpPut("{id}")]

        public IActionResult Atualizar(int id, Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var clienteExistente = _clienteRepository.ObterCliente(id);

            if(clienteExistente == null)
            {
                return NotFound(new
                {
                    mensagem = "cliente não encontrado"
                });
            }
            cliente.Id = id;

            _clienteRepository.Atualizar(cliente);

            return Ok(new
            {
                mensagem = "Cliente atualizado com sucesso"
            });

        }
        [HttpDelete("{id}")]
        public IActionResult Excluir(int id)
        {
            var cliente = _clienteRepository.ObterCliente(id);

            if(cliente == null)
            {
                return NotFound(new
                {
                    mensagem = "Cliente não encontrado"
                });
            }

            _clienteRepository.Excluir(id);

            return Ok(new
            {
                mensagem = "Cliente excluido com sucesso"
            });
        }
    }
}
