using System.Windows.Controls;
using XamlDS.Itk.Helpers;
using XamlDS.Showcases.ItkThemes.ViewModels.Controls.Buttons;
using XamlDS.Showcases.ItkThemes.ViewModels.Controls.CheckBoxes;
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
        DataTemplateHelper.AddDataTemplate(this, typeof(CheckBoxesPanelVm), typeof(CheckBoxsPanelView));
        DataTemplateHelper.AddDataTemplate(this, typeof(RadioButtonsPanelVm), typeof(RadioButtonsPanelView));
        DataTemplateHelper.AddDataTemplate(this, typeof(SlidersPanelVm), typeof(SlidersPanelView));
        DataTemplateHelper.AddDataTemplate(this, typeof(TextBoxsPanelVm), typeof(TextBoxsPanelView));
    }
}
