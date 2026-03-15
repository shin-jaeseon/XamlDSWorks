using System.Windows;
using System.Windows.Controls;

namespace ResourceSampleApp;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private StackPanel? _stackPanel;
    private ContentControl? _contentControl;
    public MainWindow()
    {
        InitializeComponent();
        Loaded += MainWindow_Loaded;
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        if (_stackPanel != null)
        {
            _stackPanel.Children.Add(new Border { Child = new TextBlock { Text = "Button 1" } });
        }
        if (_contentControl != null)
        {
            _contentControl.Content = new Border { Child = new TextBlock { Text = "Content Control Content" } };
        }
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _stackPanel = FindName("MainStackPanel") as StackPanel;
        _contentControl = FindName("MainContentControl") as ContentControl;
    }
}
