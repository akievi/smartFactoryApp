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
        ViewModel?.MoveUp();
        ViewModel?.MoveLeft();
    }
    private void MoveUpRight_Click(object sender, RoutedEventArgs e)
    {
        ViewModel?.MoveUp();
        ViewModel?.MoveRight();
    }
    private void MoveDownLeft_Click(object sender, RoutedEventArgs e)
    {
        ViewModel?.MoveDown();
        ViewModel?.MoveLeft();
    }
    private void MoveDownRight_Click(object sender, RoutedEventArgs e)
    {
        ViewModel?.MoveDown();
        ViewModel?.MoveRight();
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
}