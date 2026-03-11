using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Layout;
using XamlDS.Itk.ViewModels.Selectors;

namespace XamlDS.Itk.Views.Selectors;

public class SelectableItemView : TemplatedControl
{
    public static readonly StyledProperty<bool> IsSingleSelectorProperty =
        AvaloniaProperty.Register<SelectableItemView, bool>(nameof(IsSingleSelector), defaultValue: false);

    public static readonly StyledProperty<bool> SelectedProperty =
    AvaloniaProperty.Register<SelectableItemView, bool>(nameof(Selected), defaultValue: false);

    public static readonly StyledProperty<SelectorPanelLayout> LayoutProperty =
        AvaloniaProperty.Register<SelectableItemView, SelectorPanelLayout>(nameof(Layout), defaultValue: SelectorPanelLayout.Horizontal);

    public static readonly StyledProperty<HorizontalAlignment> HorizontalContentAlignmentProperty =
        AvaloniaProperty.Register<SelectableItemView, HorizontalAlignment>(nameof(HorizontalContentAlignment), defaultValue: HorizontalAlignment.Center);

    public static readonly StyledProperty<VerticalAlignment> VerticalContentAlignmentProperty =
        AvaloniaProperty.Register<SelectableItemView, VerticalAlignment>(nameof(VerticalContentAlignment), defaultValue: VerticalAlignment.Bottom);

    public bool IsSingleSelector
    {
        get => GetValue(IsSingleSelectorProperty);
        set => SetValue(IsSingleSelectorProperty, value);
    }

    public bool Selected
    {
        get => GetValue(SelectedProperty);
        set => SetValue(SelectedProperty, value);
    }

    public SelectorPanelLayout Layout
    {
        get => GetValue(LayoutProperty);
        set => SetValue(LayoutProperty, value);
    }

    public HorizontalAlignment HorizontalContentAlignment
    {
        get => GetValue(HorizontalContentAlignmentProperty);
        set => SetValue(HorizontalContentAlignmentProperty, value);
    }

    public VerticalAlignment VerticalContentAlignment
    {
        get => GetValue(VerticalContentAlignmentProperty);
        set => SetValue(VerticalContentAlignmentProperty, value);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        UpdateSelectorClasses();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == SelectedProperty)
        {
            PseudoClasses.Set(":selected", change.GetNewValue<bool>());
        }
        else if (change.Property == IsSingleSelectorProperty || change.Property == LayoutProperty)
        {
            UpdateSelectorClasses();
        }
    }

    private void UpdateSelectorClasses()
    {
        if (IsSingleSelector)
        {
            PseudoClasses.Set(":singleSelector", true);
            PseudoClasses.Set(":multiSelector", false);
        }
        else
        {
            PseudoClasses.Set(":singleSelector", false);
            PseudoClasses.Set(":multiSelector", true);
        }

        switch (Layout)
        {
            case SelectorPanelLayout.Horizontal:
                PseudoClasses.Set(":horizontalLayout", true);
                PseudoClasses.Set(":verticalLayout", false);
                PseudoClasses.Set(":wrapLayout", false);
                break;
            case SelectorPanelLayout.Vertical:
                PseudoClasses.Set(":horizontalLayout", false);
                PseudoClasses.Set(":verticalLayout", true);
                PseudoClasses.Set(":wrapLayout", false);
                break;
            case SelectorPanelLayout.Wrap:
                PseudoClasses.Set(":horizontalLayout", false);
                PseudoClasses.Set(":verticalLayout", false);
                PseudoClasses.Set(":wrapLayout", true);
                break;
            default:
                break;
        }
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            if (DataContext is ISelectableItem selectableItem)
            {
                selectableItem.OnClicked();
            }
        }
    }
}
