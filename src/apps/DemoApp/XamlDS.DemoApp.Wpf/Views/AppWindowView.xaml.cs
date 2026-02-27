using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using XamlDS.DemoApp.ViewModels;
using XamlDS.Itk.Helpers;
using XamlDS.Showcases.ItkControls.ViewModels;
using XamlDS.Showcases.ItkControls.Views;
using XamlDS.Showcases.ItkThemes.ViewModels;
using XamlDS.Showcases.ItkThemes.Views;

namespace XamlDS.DemoApp.Views;
/// <summary>
/// Interaction logic for MainWindowView.xaml
/// </summary>
public partial class AppWindowView : Window
{
    public AppWindowView()
    {
        InitializeComponent();
        WindowStartupLocation = WindowStartupLocation.CenterScreen;

        DataTemplateHelper.AddDataTemplate(this, typeof(HomePanelVm), typeof(HomePanelView));
        DataTemplateHelper.AddDataTemplate(this, typeof(ItkControlsPanelVm), typeof(ItkControlsPanelView));
        DataTemplateHelper.AddDataTemplate(this, typeof(ItkThemesPanelVm), typeof(ItkThemesPanelView));
    }
}
