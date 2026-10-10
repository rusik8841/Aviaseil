using AirlinesApp.Models;

namespace AirlinesApp.Views;

public partial class FleetPage : ContentPage
{
    private Aircraft? _selected;

    public FleetPage()
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
        ItemsList.ItemsSource = await App.Database.GetAircraftsAsync();
    }

    private void OnLoadClicked(object? sender, EventArgs e) => LoadData();

    private void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        _selected = e.CurrentSelection.FirstOrDefault() as Aircraft;
        if (_selected is not null)
        {
            EntTail.Text = _selected.tail_number.ToString();
            EntModel.Text = _selected.model;
            EntMfgDate.Date = _selected.manufacture_date;
            EntOpLife.Date = _selected.operational_life;
            SwReady.IsToggled = _selected.is_ready_for_flight;
            EntCmdId.Text = _selected.commander_ship_id.ToString();
        }
    }

    private async void OnSaveClicked(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(EntModel.Text))
        {
            await DisplayAlertAsync("Ошибка", "Введите модель", "ОК");
            return;
        }

        var item = _selected ?? new Aircraft();
        item.tail_number = int.TryParse(EntTail.Text, out int t) ? t : 0;
        item.model = EntModel.Text;
        item.manufacture_date = EntMfgDate.Date;
        item.operational_life = EntOpLife.Date;
        item.is_ready_for_flight = SwReady.IsToggled;
        item.commander_ship_id = int.TryParse(EntCmdId.Text, out int c) ? c : 0;

        await App.Database.SaveAircraftAsync(item);
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
            await App.Database.DeleteAircraftAsync(_selected);
            ClearFields();
            LoadData();
        }
    }

    private void OnClearClicked(object? sender, EventArgs e) => ClearFields();

    private void ClearFields()
    {
        _selected = null;
        ItemsList.SelectedItem = null;
        EntTail.Text = EntModel.Text = EntCmdId.Text = string.Empty;
        SwReady.IsToggled = true;
    }
}