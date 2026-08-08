
using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace Gestion_avion.Views;

public partial class SignUpView : UserControl
{
    public SignUpView()
    {
        InitializeComponent();
        new_user.IsVisible = false;
        other_accounts.IsVisible = false;
    }

    private void other_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new_user.IsVisible = false;
        other_accounts.IsVisible = true;
        picker.IsVisible = false;
    }
    private void register_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new_user.IsVisible = true;
        other_accounts.IsVisible = false;
        picker.IsVisible = false;
    }

    private void pick_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new_user.IsVisible = false;
        other_accounts.IsVisible = false;
        picker.IsVisible = true;
    }
}