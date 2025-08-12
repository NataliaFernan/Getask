using Getask.Repository.Models;
using Getask.Repository.Models.ViewModel;
using Getask.Services.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

public class LoginController : Controller
{
    private readonly IUsuarioService _usuarioService;
    private readonly IEmailService _emailService;

    public LoginController(IUsuarioService usuarioService, IEmailService emailService)
    {
        _usuarioService = usuarioService;
        _emailService = emailService;
    }

    public IActionResult Index()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Painel");
        }

        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        var usuario = await _usuarioService.ObterUsuarioPorEmailSenha(model.Email, Sha256_Criptografar(model.Senha));

        if (usuario is not null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Email),
                new Claim(ClaimTypes.Email, model.Email)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Painel");
        }

        ModelState.AddModelError(string.Empty, "Credenciais inválidas.");
        return View("Index");
    }

    public async Task<IActionResult> Sair()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Login");
    }
    public IActionResult Negado()
    {
        return View();
    }

    public IActionResult EsqueciSenha()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> EsqueciSenha(LoginViewModel model)
    {
        var usuario = await _usuarioService.ObterUsuarioPorEmail(model.Email);

        if (usuario != null)
        {
            var resetToken = Guid.NewGuid().ToString();
            usuario.Token = resetToken;
            usuario.DataExpiracaoToken = DateTime.Now.AddMinutes(15);

            await _usuarioService.AtualizarUsuario(usuario);

            var resetLink = Url.Action("RedefinirSenha", "Login", new { token = resetToken }, Request.Scheme);

            var subject = "Recuperação de Senha - Getask";
            var message = $"Olá, <br/><br/> Clique no link abaixo para trocar a senha: <br/><br/> <a href='{resetLink}'>Redefinir Senha</a> <br/><br/>";

            await _emailService.EnviarEmail(usuario.Email, subject, message);
        }

        ViewBag.Mensagem = "Se o e-mail estiver em nosso sistema, um link de redefinição será enviado.";
        return View();
    }

    public async Task<IActionResult> RedefinirSenha(string token)
    {
        var usuario = await _usuarioService.ObterUsuarioPorToken(token);
        if (usuario == null)
        {
            return View("TokenInvalido");
        }

        var model = new RedefinirSenhaViewModel { Token = token };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> RedefinirSenha(RedefinirSenhaViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var usuario = await _usuarioService.ObterUsuarioPorToken(model.Token);
        if (usuario == null)
        {
            return View("TokenInvalido");
        }

        var novaSenhaHash = Sha256_Criptografar(model.NovaSenha);
        await _usuarioService.AtualizarSenhaUsuario(usuario, novaSenhaHash);

        usuario.Token = "";
        usuario.DataExpiracaoToken = null;
        await _usuarioService.AtualizarUsuario(usuario);

        TempData["MensagemSucesso"] = "Sua senha foi redefinida com sucesso. Faça login com a nova senha.";
        return RedirectToAction("Index");
    }

    public static String Sha256_Criptografar(String value)
    {
        StringBuilder Sb = new StringBuilder();

        using (SHA256 hash = SHA256Managed.Create())
        {
            Encoding enc = Encoding.UTF8;
            Byte[] result = hash.ComputeHash(enc.GetBytes(value));

            foreach (Byte b in result)
                Sb.Append(b.ToString("x2"));
        }

        return Sb.ToString();
    }
}