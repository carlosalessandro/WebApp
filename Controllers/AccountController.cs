using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AccountController> _logger;

        public AccountController(ApplicationDbContext context, ILogger<AccountController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            // Se já estiver logado, redireciona para a página inicial
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                try
                {
                    // Busca o usuário no banco de dados
                    var user = await _context.Users
                        .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);

                    if (user != null)
                    {
                        // Cria as claims do usuário
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim(ClaimTypes.Name, user.Name ?? user.Email)
                        };

                        // Cria a identidade do usuário
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                        // Propriedades de autenticação
                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = model.RememberMe,
                            ExpiresUtc = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(30) : DateTimeOffset.UtcNow.AddHours(1)
                        };

                        // Faz login do usuário
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authProperties);

                        _logger.LogInformation("Usuário {Email} fez login com sucesso", user.Email);

                        // Redireciona para a URL de retorno ou para a página inicial
                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Email ou senha inválidos.");
                        _logger.LogWarning("Tentativa de login falhada para o email: {Email}", model.Email);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro durante o processo de login para o email: {Email}", model.Email);
                    ModelState.AddModelError(string.Empty, "Ocorreu um erro durante o login. Tente novamente.");
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _logger.LogInformation("Usuário fez logout");
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register(User model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Verifica se já existe um usuário com este email
                    var existingUser = await _context.Users
                        .FirstOrDefaultAsync(u => u.Email == model.Email);

                    if (existingUser != null)
                    {
                        ModelState.AddModelError("Email", "Já existe um usuário cadastrado com este email.");
                        return View(model);
                    }

                    // Adiciona o novo usuário
                    _context.Users.Add(model);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Novo usuário registrado: {Email}", model.Email);
                    TempData["SuccessMessage"] = "Usuário registrado com sucesso! Faça login para continuar.";
                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro durante o registro do usuário: {Email}", model.Email);
                    ModelState.AddModelError(string.Empty, "Ocorreu um erro durante o registro. Tente novamente.");
                }
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Test()
        {
            return Content("Sistema funcionando! Teste de rota básica OK.");
        }
    }
}
