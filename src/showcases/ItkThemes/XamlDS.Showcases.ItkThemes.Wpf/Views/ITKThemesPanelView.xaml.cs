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
using System.Windows.Navigation;
using System.Windows.Shapes;
using XamlDS.Itk.Helpers;
using XamlDS.Showcases.ItkThemes.ViewModels.Controls;
using XamlDS.Showcases.ItkThemes.ViewModels.Showcase01;
using XamlDS.Showcases.ItkThemes.ViewModels.Showcase02;
using XamlDS.Showcases.ItkThemes.ViewModels.Showcase03;
using XamlDS.Showcases.ItkThemes.Views.Controls;
using XamlDS.Showcases.ItkThemes.Views.Showcase01;
using XamlDS.Showcases.ItkThemes.Views.Showcase02;
using XamlDS.Showcases.ItkThemes.Views.Showcase03;

namespace XamlDS.Showcases.ItkThemes.Views;
/// <summary>
/// Interaction logic for ItkThemesPanelView.xaml
/// </summary>
public partial class ItkThemesPanelView : UserControl
{
    public ItkThemesPanelView()
    {
        InitializeComponent();

        DataTemplateHelper.AddDataTemplate(this, typeof(ControlsPanelVm), typeof(ControlsPanelView));
        DataTemplateHelper.AddDataTemplate(this, typeof(Showcase01PanelVm), typeof(Showcase01PanelView));
        DataTemplateHelper.AddDataTemplate(this, typeof(Showcase02PanelVm), typeof(Showcase02PanelView));
        DataTemplateHelper.AddDataTemplate(this, typeof(Showcase03PanelVm), typeof(Showcase03PanelView));
    }
}
