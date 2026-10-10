using AirlinesApp.Models;

namespace AirlinesApp.Views;

public partial class FlightsPage : ContentPage
{
    private Flight? _selected;

    public FlightsPage()
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
        ItemsList.ItemsSource = await App.Database.GetFlightsAsync();
    }

    private void OnLoadClicked(object? sender, EventArgs e) => LoadData();

    private void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        _selected = e.CurrentSelection.FirstOrDefault() as Flight;
        if (_selected is not null)
        {
            EntNum.Text = _selected.flight_number.ToString();
            EntDate.Date = _selected.departure_datetime.Date;
            EntTime.Time = _selected.departure_datetime.TimeOfDay;
            SwCancelled.IsToggled = _selected.is_cancelled;
            EntRouteId.Text = _selected.route_id.ToString();
            EntAircraftId.Text = _selected.aircraft_id.ToString();
            EntPassId.Text = _selected.passengers_id.ToString();
        }
    }

    private async void OnSaveClicked(object? sender, EventArgs e)
    {
        var item = _selected ?? new Flight();
        item.flight_number = int.TryParse(EntNum.Text, out int n) ? n : 0;
        // КЛЮЧЕВОЕ ИСПРАВЛЕНИЕ: .Value для DateTime?
        item.departure_datetime = EntDate.Date.Value.Add(EntTime.Time);
        item.is_cancelled = SwCancelled.IsToggled;
        item.route_id = int.TryParse(EntRouteId.Text, out int rid) ? rid : 0;
        item.aircraft_id = int.TryParse(EntAircraftId.Text, out int aid) ? aid : 0;
        item.passengers_id = int.TryParse(EntPassId.Text, out int pid) ? pid : 0;

        await App.Database.SaveFlightAsync(item);
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
            await App.Database.DeleteFlightAsync(_selected);
            ClearFields();
            LoadData();
        }
    }

    private void OnClearClicked(object? sender, EventArgs e) => ClearFields();

    private void ClearFields()
    {
        _selected = null;
        ItemsList.SelectedItem = null;
        EntNum.Text = EntRouteId.Text = EntAircraftId.Text = EntPassId.Text = string.Empty;
        SwCancelled.IsToggled = false;
    }
}