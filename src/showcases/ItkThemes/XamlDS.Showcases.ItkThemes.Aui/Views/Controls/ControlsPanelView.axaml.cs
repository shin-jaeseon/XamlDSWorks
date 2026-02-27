using Avalonia.Controls;
using Avalonia.Controls.Templates;
using XamlDS.Showcases.ItkThemes.ViewModels.Controls.Buttons;
using XamlDS.Showcases.ItkThemes.ViewModels.Controls.CheckBoxs;
using XamlDS.Showcases.ItkThemes.ViewModels.Controls.RadioButtons;
using XamlDS.Showcases.ItkThemes.ViewModels.Controls.Sliders;
using XamlDS.Showcases.ItkThemes.ViewModels.Controls.TextBoxs;
using XamlDS.Showcases.ItkThemes.ViewModels.Controls.TextFields;
using XamlDS.Showcases.ItkThemes.Views.Controls.Buttons;
using XamlDS.Showcases.ItkThemes.Views.Controls.CheckBoxs;
using XamlDS.Showcases.ItkThemes.Views.Controls.RadioButtons;
using XamlDS.Showcases.ItkThemes.Views.Controls.Sliders;
using XamlDS.Showcases.ItkThemes.Views.Controls.TextBoxs;
using XamlDS.Showcases.ItkThemes.Views.Controls.TextFields;

namespace XamlDS.Showcases.ItkThemes.Views.Controls;

public partial class ControlsPanelView : UserControl
{
    public ControlsPanelView()
    {
        InitializeComponent();

        this.DataTemplates.Add(new FuncDataTemplate<ButtonsPanelVm>((value, namescope) => new ButtonsPanelView()));
        this.DataTemplates.Add(new FuncDataTemplate<CheckBoxsPanelVm>((value, namescope) => new CheckBoxsPanelView()));
        this.DataTemplates.Add(new FuncDataTemplate<RadioButtonsPanelVm>((value, namescope) => new RadioButtonsPanelView()));
        this.DataTemplates.Add(new FuncDataTemplate<SlidersPanelVm>((value, namescope) => new SlidersPanelView()));
        this.DataTemplates.Add(new FuncDataTemplate<TextBoxsPanelVm>((value, namescope) => new TextBoxsPanelView()));
        this.DataTemplates.Add(new FuncDataTemplate<TextFieldsPanelVm>((value, namescope) => new TextFieldsPanelView()));
    }
}
