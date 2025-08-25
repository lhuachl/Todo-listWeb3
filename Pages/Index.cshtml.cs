using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Todo_listWeb3.Pages
{
    public class IndexModel : PageModel
    {
        public string Username { get; set; } = string.Empty;
        public int UserId { get; set; }

        public IActionResult OnGet()
        {
            // Verificar si el usuario está logueado
            var userId = HttpContext.Session.GetInt32("UserId");
            var username = HttpContext.Session.GetString("Username");

            if (userId == null || string.IsNullOrEmpty(username))
            {
                // Agregar logging para debugging
                Console.WriteLine("Usuario no logueado - redirigiendo a Login");
                return RedirectToPage("/Login");
            }

            UserId = userId.Value;
            Username = username;
            
            // Logging para debugging
            Console.WriteLine($"Usuario logueado: ID={UserId}, Username={Username}");
            
            return Page();
        }

        public IActionResult OnPostLogout()
        {
            Console.WriteLine("Cerrando sesión del usuario");
            HttpContext.Session.Clear();
            return RedirectToPage("/Login");
        }
    }
}
