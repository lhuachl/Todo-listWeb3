using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Todo_listWeb3.Models;
using Todo_listWeb3.Services;

namespace Todo_listWeb3.Pages
{
    public class LoginModel : PageModel
    {
        private readonly TaskApiService _apiService;

        public LoginModel(TaskApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty]
        public string Username { get; set; } = string.Empty;

        public List<UserDto> Users { get; set; } = new();

        public async Task OnGetAsync()
        {
            try
            {
                Users = await _apiService.GetUsersAsync();
            }
            catch
            {
                Users = new List<UserDto>();
            }
        }

        public async Task<IActionResult> OnPostLoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                ModelState.AddModelError("Username", "El nombre de usuario es requerido");
                await OnGetAsync();
                return Page();
            }

            try
            {
                var user = await _apiService.GetUserByUsernameAsync(Username);
                if (user != null)
                {
                    HttpContext.Session.SetInt32("UserId", user.Id);
                    HttpContext.Session.SetString("Username", user.Username);
                    return RedirectToPage("/Index");
                }
                else
                {
                    ModelState.AddModelError("Username", "Usuario no encontrado");
                }
            }
            catch
            {
                ModelState.AddModelError("Username", "Error al conectar con el servidor");
            }

            await OnGetAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostCreateUserAsync()
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                ModelState.AddModelError("Username", "El nombre de usuario es requerido");
                await OnGetAsync();
                return Page();
            }

            try
            {
                var newUser = await _apiService.CreateUserAsync(new UserCreateDto { Username = Username });
                if (newUser != null)
                {
                    HttpContext.Session.SetInt32("UserId", newUser.Id);
                    HttpContext.Session.SetString("Username", newUser.Username);
                    return RedirectToPage("/Index");
                }
                else
                {
                    ModelState.AddModelError("Username", "Error al crear el usuario");
                }
            }
            catch
            {
                ModelState.AddModelError("Username", "Error al conectar con el servidor o el usuario ya existe");
            }

            await OnGetAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostSelectUserAsync(int userId)
        {
            try
            {
                var user = await _apiService.GetUserAsync(userId);
                if (user != null)
                {
                    HttpContext.Session.SetInt32("UserId", user.Id);
                    HttpContext.Session.SetString("Username", user.Username);
                    return RedirectToPage("/Index");
                }
            }
            catch
            {
                // Error al cargar usuario
            }

            return RedirectToPage("/Login");
        }

        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Login");
        }
    }
}