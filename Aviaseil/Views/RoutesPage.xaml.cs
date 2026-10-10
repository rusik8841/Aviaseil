using AirlinesApp.Models;

namespace AirlinesApp.Views;

public partial class RoutesPage : ContentPage
{
    private Route? _selected;

    public RoutesPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadData();
    }

    private async void LoadData()
    {
        ItemsList.ItemsSource = await App.Database.GetRoutesAsync();
    }

    private void OnLoadClicked(object? sender, EventArgs e) => LoadData();

    private void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        _selected = e.CurrentSelection.FirstOrDefault() as Route;
        if (_selected is not null)
        {
            EntNum.Text = _selected.route_number.ToString();
            EntDep.Text = _selected.departure_airport;
            EntArr.Text = _selected.arrival_airport;
            EntPrice.Text = _selected.ticket_price.ToString();
            EntDur.Text = _selected.flight_duration;
        }
    }

    private async void OnSaveClicked(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(EntDep.Text) || string.IsNullOrWhiteSpace(EntArr.Text))
        {
            await DisplayAlertAsync("Ошибка", "Заполните аэропорты", "ОК");
            return;
        }

        var item = _selected ?? new Route();
        item.route_number = int.TryParse(EntNum.Text, out int n) ? n : 0;
        item.departure_airport = EntDep.Text;
        item.arrival_airport = EntArr.Text;
        item.ticket_price = decimal.TryParse(EntPrice.Text, out decimal p) ? p : 0;
        item.flight_duration = EntDur.Text ?? string.Empty;

        await App.Database.SaveRouteAsync(item);
        ClearFields();
        LoadData();
        await DisplayAlertAsync("Успех", "Сохранено", "ОК");
    }

    private async void OnDeleteClicked(object? sender, EventArgs e)
    {
        if (_selected is null) return;
        bool ok = await DisplayAlertAsync("Удалить?", "Вы уверены?", "Да", "Нет");
        if (ok)
        {
            await App.Database.DeleteRouteAsync(_selected);
            ClearFields();
            LoadData();
        }
    }

    private void OnClearClicked(object? sender, EventArgs e) => ClearFields();

    private void ClearFields()
    {
        _selected = null;
        ItemsList.SelectedItem = null;
        EntNum.Text = EntDep.Text = EntArr.Text = EntPrice.Text = EntDur.Text = string.Empty;
    }
}