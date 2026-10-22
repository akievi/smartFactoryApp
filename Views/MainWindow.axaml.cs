using Avalonia.Controls;
using Avalonia.Interactivity;
using smartFactoryApp.ViewModels;

namespace smartFactoryApp.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    private MainViewModel? ViewModel => DataContext as MainViewModel;
    private void MoveUp_Click(object sender, RoutedEventArgs e)
    {
        ViewModel?.MoveUp();
    }
    private void MoveDown_Click(object sender, RoutedEventArgs e)
    {
        ViewModel?.MoveDown();
    }
    private void MoveLeft_Click(object sender, RoutedEventArgs e)
    {
        ViewModel?.MoveLeft();
    }
    private void MoveRight_Click(object sender, RoutedEventArgs e)
    {
        ViewModel?.MoveRight();
    }
    private void MoveUpLeft_Click(object sender, RoutedEventArgs e)
    {
        ViewModel?.MoveUpLeft();
    }
    private void MoveUpRight_Click(object sender, RoutedEventArgs e)
    {
        ViewModel?.MoveUpRight();
    }
    private void MoveDownLeft_Click(object sender, RoutedEventArgs e)
    {
        ViewModel?.MoveDownLeft();
    }
    private void MoveDownRight_Click(object sender, RoutedEventArgs e)
    {
        ViewModel?.MoveDownRight();
    }
    private void Stop_Click(object sender, RoutedEventArgs e)
    {
        ViewModel?.Stop();
    }
    private void RotateRight_Click(object sender, RoutedEventArgs e)
    {
        ViewModel?.RotateRight();
    }
    private void RotateLeft_Click(object sender, RoutedEventArgs e)
    {
        ViewModel?.RotateLeft();
    }
    private void ResetRotation_Click(object sender, RoutedEventArgs e)
    {
        ViewModel?.ResetRotation();
    }
    private void Undock_Click(object sender, RoutedEventArgs e)
    {
        ViewModel?.Undock();
    }
    private void StartTimer_Click(object sender, RoutedEventArgs e)
    {
        ViewModel?.StartTimer();
    }
    private void StopTimer_Click(object sender, RoutedEventArgs e)
    {
        ViewModel?.StopTimer();
    }
}