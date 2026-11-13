using Microsoft.Data.SqlClient;
using RecipesApp.Models;

namespace RecipesApp.Services;

public class DatabaseService
{
    private readonly string connectionString =
        "Server=ИМЯ_СЕРВЕРА;Database=RecipesDb;Trusted_Connection=True;TrustServerCertificate=True;";

    // SELECT — получение списка рецептов
    public async Task<List<Recipe>> GetRecipesAsync()
    {
        var recipes = new List<Recipe>();
        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        const string sql = "SELECT Id, Name, Description, Ingredients FROM Recipes";
        using var command = new SqlCommand(sql, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            recipes.Add(new Recipe
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Description = reader.GetString(2),
                Ingredients = reader.GetString(3)
            });
        }
        return recipes;
    }

    // SELECT — получение одного рецепта по Id
    public async Task<Recipe?> GetRecipeByIdAsync(int id)
    {
        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        const string sql = "SELECT Id, Name, Description, Ingredients FROM Recipes WHERE Id = @Id";
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", id);
        using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Recipe
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Description = reader.GetString(2),
                Ingredients = reader.GetString(3)
            };
        }
        return null;
    }

    // INSERT — добавление рецепта
    public async Task AddRecipeAsync(Recipe recipe)
    {
        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        const string sql = "INSERT INTO Recipes (Name, Description, Ingredients) VALUES (@Name, @Description, @Ingredients)";
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Name", recipe.Name);
        command.Parameters.AddWithValue("@Description", recipe.Description);
        command.Parameters.AddWithValue("@Ingredients", recipe.Ingredients);
        await command.ExecuteNonQueryAsync();
    }

    // UPDATE — редактирование рецепта
    public async Task UpdateRecipeAsync(Recipe recipe)
    {
        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        const string sql = "UPDATE Recipes SET Name = @Name, Description = @Description, Ingredients = @Ingredients WHERE Id = @Id";
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Name", recipe.Name);
        command.Parameters.AddWithValue("@Description", recipe.Description);
        command.Parameters.AddWithValue("@Ingredients", recipe.Ingredients);
        command.Parameters.AddWithValue("@Id", recipe.Id);
        await command.ExecuteNonQueryAsync();
    }
}