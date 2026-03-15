using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace LayoutWork.Aui.Views;

public partial class AppWindowView : Window
{
    public AppWindowView()
    {
        InitializeComponent();
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
    }
}

