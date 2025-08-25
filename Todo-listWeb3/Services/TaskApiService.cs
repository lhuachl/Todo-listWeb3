using System.Text;
using System.Text.Json;
using Todo_listWeb3.Models;

namespace Todo_listWeb3.Services
{
    public class TaskApiService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
        private const string BASE_URL = "http://localhost:8000";

        // ============================================================================
        // CONSTRUCTOR
        // ============================================================================
        public TaskApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(BASE_URL);
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                PropertyNameCaseInsensitive = true
            };
        }

        // ============================================================================
        // MÉTODOS DE USUARIO
        // ============================================================================
        public async Task<List<UserDto>> GetUsersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("users/");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<UserDto>>(json, _jsonOptions) ?? new List<UserDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener usuarios: {ex.Message}");
                return new List<UserDto>();
            }
        }

        public async Task<UserDto?> GetUserAsync(int id)
        {
            try
            {
                var users = await GetUsersAsync();
                return users.FirstOrDefault(u => u.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener usuario: {ex.Message}");
                return null;
            }
        }

        public async Task<UserDto?> GetUserByUsernameAsync(string username)
        {
            try
            {
                var users = await GetUsersAsync();
                return users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al buscar usuario: {ex.Message}");
                return null;
            }
        }

        public async Task<UserDto?> CreateUserAsync(UserCreateDto user)
        {
            try
            {
                var json = JsonSerializer.Serialize(user, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PostAsync("users/", content);
                
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error al crear usuario: {error}");
                    return null;
                }

                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<UserDto>(responseJson, _jsonOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear usuario: {ex.Message}");
                return null;
            }
        }

        // ============================================================================
        // MÉTODOS DE TAREA
        // ============================================================================
        public async Task<List<TaskDto>> GetTasksAsync(int? userId = null, string? status = null)
        {
            try
            {
                var queryParams = new List<string>();
                
                if (userId.HasValue)
                    queryParams.Add($"user_id={userId}");
                if (!string.IsNullOrEmpty(status))
                    queryParams.Add($"status={Uri.EscapeDataString(status)}");

                var query = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";
                var response = await _httpClient.GetAsync($"tasks/{query}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<TaskDto>>(json, _jsonOptions) ?? new List<TaskDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener tareas: {ex.Message}");
                return new List<TaskDto>();
            }
        }

        public async Task<TaskDto?> GetTaskAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"tasks/{id}");
                
                if (!response.IsSuccessStatusCode)
                    return null;

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<TaskDto>(json, _jsonOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener tarea: {ex.Message}");
                return null;
            }
        }

        public async Task<TaskDto?> CreateTaskAsync(TaskCreateDto task)
        {
            try
            {
                var json = JsonSerializer.Serialize(task, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PostAsync("tasks/", content);
                
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error al crear tarea: {error}");
                    return null;
                }

                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<TaskDto>(responseJson, _jsonOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear tarea: {ex.Message}");
                return null;
            }
        }

        public async Task<TaskDto?> UpdateTaskAsync(int id, TaskUpdateDto task)
        {
            try
            {
                var json = JsonSerializer.Serialize(task, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PutAsync($"tasks/{id}", content);
                
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error al actualizar tarea: {error}");
                    return null;
                }

                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<TaskDto>(responseJson, _jsonOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar tarea: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"tasks/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar tarea: {ex.Message}");
                return false;
            }
        }

        // ============================================================================
        // MÉTODOS DE UTILIDAD
        // ============================================================================
        public async Task<bool> IsApiHealthyAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}