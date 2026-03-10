using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using XamlDS.Itk.ViewModels.Selectors;

namespace XamlDS.Itk.Views.Selectors;

public class SingleSelectorPanelView : TemplatedControl
{
    private object? _viewModel;
    private SelectorLayout _layout = SelectorLayout.Horizontal;
    private Border? _layoutContainer;
    private Panel? _panel;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        // Get the PART_layoutContainer from the template
        _layoutContainer = e.NameScope.Find<Border>("PART_layoutContainer");

        // Apply children if DataContext is already set
        if (_viewModel != null && _layoutContainer != null)
        {
            RebuildChildren();
        }
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == DataContextProperty)
        {
            OnDataContextChanged(this, EventArgs.Empty);
        }
    }

    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        // Check for SingleSelectorPanelVm<any type>
        if (DataContext?.GetType().IsGenericType == true &&
            DataContext.GetType().GetGenericTypeDefinition() == typeof(SingleSelectorPanelVm<>))
        {
            _viewModel = DataContext;

            // Use dynamic to access Layout property regardless of generic type parameter
            _layout = ((dynamic)DataContext).Layout;
            RebuildChildren();
        }
        else
        {
            _viewModel = null;
            _layoutContainer?.Child = null; // Clear layout if DataContext is not correct

        }
    }

    private void RebuildChildren()
    {
        if (_viewModel == null || _layoutContainer == null)
            return;

        SetLayoutPanel();

        // Add all children from ViewModel
        var children = ((dynamic)_viewModel).Children as System.Collections.IList;
        foreach (var child in children!)
        {
            var childView = CreateViewForChild(child);
            _panel!.Children.Add(childView);
        }
    }

    private void SetLayoutPanel()
    {
        _layoutContainer!.Child = null;

        switch (_layout)
        {
            case SelectorLayout.Horizontal:
                _panel = new StackPanel { Orientation = Orientation.Horizontal };
                break;
            case SelectorLayout.Vertical:
                _panel = new StackPanel { Orientation = Orientation.Vertical };
                break;
            case SelectorLayout.Wrapping:
                _panel = new WrapPanel();
                break;
            default:
                throw new InvalidOperationException("Unknown layout type");
        }
        _layoutContainer.Child = _panel;
    }

    private Control CreateViewForChild(object childVm)
    {
        var view = new SelectableItemView();
        view.DataContext = childVm;
        return view;
    }
}
