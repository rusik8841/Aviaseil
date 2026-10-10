using AirlinesApp.Services;
using Aviaseil;
using Aviaseil.Services;

namespace AirlinesApp;

public partial class App : Application
{
    public static DatabaseService Database { get; } = new DatabaseService();

    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new MainPage());
    }
}