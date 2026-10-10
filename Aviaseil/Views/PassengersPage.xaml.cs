using AirlinesApp.Models;
using Java.Net;

namespace AirlinesApp.Views;

public partial class PassengersPage : ContentPage
{
    private Passenger? _selected;

    public PassengersPage()
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
        ItemsList.ItemsSource = await App.Database.GetPassengersAsync();
    }

    private void OnLoadClicked(object? sender, EventArgs e) => LoadData();

    private void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        _selected = e.CurrentSelection.FirstOrDefault() as Passenger;
        if (_selected is not null)
        {
            EntPassport.Text = _selected.passport_number.ToString();
            EntName.Text = _selected.full_name;
            EntAddress.Text = _selected.address;
            EntPhone.Text = _selected.phone;
            EntEmail.Text = _selected.email;
        }
    }

    private async void OnSaveClicked(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(EntName.Text))
        {
            await DisplayAlertAsync("Ошибка", "Введите ФИО", "ОК");
            return;
        }

        var item = _selected ?? new Passenger();
        item.passport_number = int.TryParse(EntPassport.Text, out int p) ? p : 0;
        item.full_name = EntName.Text;
        item.address = EntAddress.Text ?? string.Empty;
        item.phone = EntPhone.Text ?? string.Empty;
        item.email = EntEmail.Text ?? string.Empty;

        await App.Database.SavePassengerAsync(item);
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
            await App.Database.DeletePassengerAsync(_selected);
            ClearFields();
            LoadData();
        }
    }

    private void OnClearClicked(object? sender, EventArgs e) => ClearFields();

    private void ClearFields()
    {
        _selected = null;
        ItemsList.SelectedItem = null;
        EntPassport.Text = EntName.Text = EntAddress.Text = EntPhone.Text = EntEmail.Text = string.Empty;
    }
}