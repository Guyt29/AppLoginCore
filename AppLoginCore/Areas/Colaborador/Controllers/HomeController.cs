using AppLoginCore.Libraries.Login;
using AppLoginCore.Models.Constant;
using AppLoginCore.Repository.Contract;
using Microsoft.AspNetCore.Mvc;

namespace AppLoginCore.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    public class HomeController : Controller
    {
        private IColaboradorRepository _repositoryColaborador;
        private LoginColaborador _loginColaborador;
        public HomeController(IColaboradorRepository repositoryColaborador, LoginColaborador loginColaborador)
        {
            _repositoryColaborador = repositoryColaborador;
            _loginColaborador = loginColaborador;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromForm] Models.Colaborador colaborador)
        {
            Models.Colaborador colaboradorDB = _repositoryColaborador.Login(colaborador.Email, colaborador.Senha);

            if (colaboradorDB.Email != null && colaboradorDB.Senha != null && colaboradorDB.Tipo != ColaboradorTipoConstant.Comum) 
            {
                _loginColaborador.Login(colaboradorDB);

                return new RedirectResult(Url.Action(nameof(PainelGerente)));
            }
            if (colaboradorDB.Email != null && colaboradorDB.Senha != null && colaboradorDB.Tipo != ColaboradorTipoConstant.Gerente)
            {
                _loginColaborador.Login(colaboradorDB);

                return new RedirectResult(Url.Action(nameof(PainelComum)));
            }
            else
            {
                ViewData["MSG_E"] = "Usuário não encontrado, verifique o email e senha digitado";
                return View(); 
            }


        }
        public IActionResult PainelGerente()
        {
            ViewBag.Nome = _loginColaborador.GetColaborador().Nome;
            ViewBag.Tipo = _loginColaborador.GetColaborador().Tipo;
            ViewBag.Email = _loginColaborador.GetColaborador().Email;
            // return new ContentResult() {content = "este é o painel do gerente"};
            return View();
        }

        public IActionResult PainelComum()
        {
            ViewBag.Nome = _loginColaborador.GetColaborador().Nome;
            ViewBag.Tipo = _loginColaborador.GetColaborador().Tipo;
            ViewBag.Email = _loginColaborador.GetColaborador().Email;
            // return new ContentResult() {content = "este é o painel do colaborador}
            return View();
        }

        public IActionResult Painel()
        {
            return View();
        }

        public IActionResult Logout()
        {
            _loginColaborador.Logout();
            return RedirectToAction("Login", 'Home');
        }
    }
}
