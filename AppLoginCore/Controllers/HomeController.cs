using AppLoginCore.Libraries.Filtro;
using AppLoginCore.Libraries.Login;
using AppLoginCore.Models;
using AppLoginCore.Models.Constant;
using AppLoginCore.Repository;
using AppLoginCore.Repository.Contract;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AppLoginCore.Controllers
{
    public class HomeController : Controller
    {
        private IClienteRepository _clienteRepository;
        private LoginCliente _loginCliente;

        public HomeController(IClienteRepository clienteRepository, LoginCliente loginCliente)
        {
            _clienteRepository = clienteRepository;
            _loginCliente = loginCliente;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromForm] Cliente cliente)
        {
            Cliente clienteDB = _clienteRepository.Login(cliente.Email, cliente.Senha);

            if(clienteDB.Email != null && clienteDB.Senha != null)
            {
                _loginCliente.Login(clienteDB);
                return new RedirectResult(Url.Action(nameof(PainelCliente)));
            }
            else
            {
                //ERRO NA SESSAO
                ViewData["MSG_E"] = "Usuário não localizado, por favor verifique o email e senha digitado";
                return View();
            }
        }
        [ClienteAutorizacao]
        public IActionResult PainelCliente()
        {
            ViewBag.Nome = _loginCliente.GetCliente().Nome;
            ViewBag.CPF = _loginCliente.GetCliente().CPF;
            ViewBag.Email = _loginCliente.GetCliente().Email;
            //return new ContentResult(){ Content = "Este é o Painel do Cliente!"};
            return View();
        }
        public IActionResult LogoutCliente()
        {
            _loginCliente.Logout();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Cadastrar()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Cadastrar([FromForm] Cliente cliente)
        {
            if (!string.IsNullOrEmpty(cliente.CPF))
            {
                cliente.CPF = new string(cliente.CPF.Where(char.IsDigit).ToArray());
            }
            if (!string.IsNullOrEmpty(cliente.Telefone))
            {
                cliente.Telefone = new string(cliente.Telefone.Where(char.IsDigit).ToArray());
            }

            var cpfBuscado = _clienteRepository.BuscaCpfCliente(cliente.CPF);
            var emailBuscado = _clienteRepository.BuscaEmailCliente(cliente.Email);

            // Verifica se o CPF retornado tem conteúdo preenchido
            if (cpfBuscado != null && !string.IsNullOrEmpty(cpfBuscado.CPF))
            {
                ViewData["MSG_CPF"] = "CPF já cadastrado, por favor verifique os dados digitados";
                return View(cliente); // IMPORTANTE: Passar o cliente de volta para a View!
            }
            else if (emailBuscado != null && !string.IsNullOrEmpty(emailBuscado.Email))
            {
                ViewData["MSG_Email"] = "E-mail já cadastrado, por favor verifique os dados digitados";
                return View(cliente); // IMPORTANTE: Passar o cliente de volta para a View!
            }
            else if (ModelState.IsValid)
            {
                _clienteRepository.Cadastrar(cliente);
                return RedirectToAction(nameof(Login));
            }
            _clienteRepository.Cadastrar(cliente);
            return View(nameof(Index));
        }   
    }
}
