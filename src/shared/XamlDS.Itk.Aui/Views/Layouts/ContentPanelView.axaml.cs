using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using XamlDS.Itk.ViewModels;

namespace XamlDS.Itk.Views.Layouts;

public class ContentPanelView : TemplatedControl
{
    public static readonly StyledProperty<ContentPanelStyle> PanelStyleProperty =
        AvaloniaProperty.Register<ContentPanelView, ContentPanelStyle>(nameof(PanelStyle), defaultValue: ContentPanelStyle.Default);

    public ContentPanelStyle PanelStyle
    {
        get => GetValue(PanelStyleProperty);
        set => SetValue(PanelStyleProperty, value);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        UpdateSelectorClasses();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == PanelStyleProperty)
        {
            UpdateSelectorClasses();
        }
    }

    private void UpdateSelectorClasses()
    {
        switch (PanelStyle)
        {
            case ContentPanelStyle.Default:
                PseudoClasses.Set(":default", true);
                PseudoClasses.Set(":bordered", false);
                break;
            case ContentPanelStyle.Bordered:
                PseudoClasses.Set(":default", false);
                PseudoClasses.Set(":bordered", true);
                break;
        }
    }
}
