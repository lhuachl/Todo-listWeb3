using System.Text.Json;

namespace Todo_listWeb3.Models
{
    // ============================================================================
    // MODELOS DE USUARIO
    // ============================================================================
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
    }

    public class UserCreateDto
    {
        public string Username { get; set; } = string.Empty;
    }

    // ============================================================================
    // MODELOS DE TAREA (CON PRIORITY)
    // ============================================================================
    public class TaskDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Status { get; set; } = "To Do";
        public int Priority { get; set; } = 1; // 1: baja, 2: media, 3: alta
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class TaskCreateDto
    {
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Status { get; set; } = "To Do";
        public int Priority { get; set; } = 1; // 1: baja, 2: media, 3: alta
    }

    public class TaskUpdateDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public int? Priority { get; set; } // Opcional para actualizaciones
    }

    // ============================================================================
    // ENUMS Y CONSTANTES PARA PRIORIDADES
    // ============================================================================
    public static class TaskPriority
    {
        public const int Low = 1;    // Baja
        public const int Medium = 2; // Media  
        public const int High = 3;   // Alta
        
        public static string GetPriorityName(int priority)
        {
            return priority switch
            {
                1 => "Baja",
                2 => "Media", 
                3 => "Alta",
                _ => "Baja"
            };
        }
        
        public static string GetPriorityIcon(int priority)
        {
            return priority switch
            {
                1 => "??", // Verde para baja
                2 => "??", // Amarillo para media
                3 => "??", // Rojo para alta
                _ => "??"
            };
        }
    }

    // ============================================================================
    // MODELO DE RESPUESTA
    // ============================================================================
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
    }
}