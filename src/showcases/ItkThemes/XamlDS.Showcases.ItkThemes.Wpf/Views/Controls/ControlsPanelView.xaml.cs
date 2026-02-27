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
using XamlDS.Showcases.ItkThemes.ViewModels.Controls.Buttons;
using XamlDS.Showcases.ItkThemes.ViewModels.Controls.CheckBoxs;
using XamlDS.Showcases.ItkThemes.ViewModels.Controls.RadioButtons;
using XamlDS.Showcases.ItkThemes.ViewModels.Controls.Sliders;
using XamlDS.Showcases.ItkThemes.ViewModels.Controls.TextBoxs;
using XamlDS.Showcases.ItkThemes.Views.Controls.Buttons;
using XamlDS.Showcases.ItkThemes.Views.Controls.CheckBoxs;
using XamlDS.Showcases.ItkThemes.Views.Controls.RadioButtons;
using XamlDS.Showcases.ItkThemes.Views.Controls.Sliders;
using XamlDS.Showcases.ItkThemes.Views.Controls.TextBoxs;

namespace XamlDS.Showcases.ItkThemes.Views.Controls;
/// <summary>
/// Interaction logic for ControlsPanelView.xaml
/// </summary>
public partial class ControlsPanelView : UserControl
{
    public ControlsPanelView()
    {
        InitializeComponent();

        DataTemplateHelper.AddDataTemplate(this, typeof(ButtonsPanelVm), typeof(ButtonsPanelView));
        DataTemplateHelper.AddDataTemplate(this, typeof(CheckBoxsPanelVm), typeof(CheckBoxsPanelView));
        DataTemplateHelper.AddDataTemplate(this, typeof(RadioButtonsPanelVm), typeof(RadioButtonsPanelView));
        DataTemplateHelper.AddDataTemplate(this, typeof(SlidersPanelVm), typeof(SlidersPanelView));
        DataTemplateHelper.AddDataTemplate(this, typeof(TextBoxsPanelVm), typeof(TextBoxsPanelView));
    }
}
