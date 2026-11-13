using RecipesApp.Models;
using RecipesApp.Services;

namespace RecipesApp;

public partial class RecipeEditPage : ContentPage
{
    private readonly DatabaseService databaseService = new DatabaseService();
    private readonly Recipe _recipe;
    private readonly bool _isNew;

    public RecipeEditPage(Recipe recipe)
    {
        InitializeComponent();
        _recipe = recipe;
        _isNew = recipe.Id == 0;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_isNew)
        {
            Title = "Новый рецепт";
            NameEntry.Text = "";
            DescriptionEditor.Text = "";
            IngredientsEditor.Text = "";
        }
        else
        {
            Title = "Редактирование рецепта";
            // Загружаем актуальные данные из БД по Id
            var loaded = await databaseService.GetRecipeByIdAsync(_recipe.Id);
            if (loaded != null)
            {
                NameEntry.Text = loaded.Name;
                DescriptionEditor.Text = loaded.Description;
                IngredientsEditor.Text = loaded.Ingredients;
            }
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        try
        {
            _recipe.Name = NameEntry.Text?.Trim() ?? "";
            _recipe.Description = DescriptionEditor.Text?.Trim() ?? "";
            _recipe.Ingredients = IngredientsEditor.Text?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(_recipe.Name))
            {
                await DisplayAlert("Ошибка", "Введите название рецепта", "OK");
                return;
            }

            if (_isNew)
                await databaseService.AddRecipeAsync(_recipe);
            else
                await databaseService.UpdateRecipeAsync(_recipe);

            await DisplayAlert("Готово", "Рецепт сохранён", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", $"Не удалось сохранить: {ex.Message}", "OK");
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}