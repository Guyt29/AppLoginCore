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
            var CPFExit = _clienteRepository.BuscaCpfCliente(cliente.CPF).CPF;
            var EmailExit = _clienteRepository.BuscaEmailCliente(cliente.Email).Email;

            if (!string.IsNullOrWhiteSpace(CPFExit))
            {
                //CPF Cadastrado
                ViewData["MSG_CPF"] = "CPF já cadastrado, por favor verifique os dados digitado";
                return View();
            }
            else if (!string.IsNullOrWhiteSpace(EmailExit))
            {
                //Email Cadastrado
                ViewData["MSG_Email"] = "E-mail já cadastrado, por favor verifique os dados digitado";
                return View();
            }
            else if (ModelState.IsValid)
            {
                _clienteRepository.Cadastrar(cliente);
                return RedirectToAction(nameof(Login));
            }
            return View();
        }
    }
}
