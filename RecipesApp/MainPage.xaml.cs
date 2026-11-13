using RecipesApp.Models;
using RecipesApp.Services;

namespace RecipesApp;

public partial class MainPage : ContentPage
{
    private readonly DatabaseService databaseService = new DatabaseService();

    public MainPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadRecipesAsync();
    }

    private async Task LoadRecipesAsync()
    {
        try
        {
            var recipes = await databaseService.GetRecipesAsync();
            RecipesCollection.ItemsSource = recipes;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", $"Не удалось загрузить рецепты: {ex.Message}", "OK");
        }
    }

    private async void OnAddClicked(object sender, EventArgs e)
    {
        // Открываем пустую страницу для нового рецепта (Id = 0)
        await Navigation.PushAsync(new RecipeEditPage(new Recipe { Id = 0 }));
    }

    private async void OnRecipeSelected(object sender, SelectionChangedEventArgs e)
    {
        var selected = e.CurrentSelection.FirstOrDefault() as Recipe;
        if (selected != null)
        {
            await Navigation.PushAsync(new RecipeEditPage(selected));
            ((CollectionView)sender).SelectedItem = null;
        }
    }
}