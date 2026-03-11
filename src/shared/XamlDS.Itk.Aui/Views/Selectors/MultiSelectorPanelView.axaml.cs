using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using XamlDS.Itk.ViewModels.Selectors;

namespace XamlDS.Itk.Views.Selectors;

public class MultiSelectorPanelView : TemplatedControl
{
    private Border? _layoutHost;
    private ISelectorPanelVm? _selectorPanelVm;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        // Get the PART_layoutContainer from the template
        _layoutHost = e.NameScope.Find<Border>("PART_layoutHost");

        // Apply children if DataContext is already set
        if (_selectorPanelVm != null && _layoutHost != null)
        {
            RebuildControls(_selectorPanelVm);
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
        if (_selectorPanelVm != null)
        {
            _selectorPanelVm.PropertyChanged -= ViewModel_PropertyChanged;
            _selectorPanelVm = null;
            _layoutHost?.Child = null; // Clear layout if DataContext is not correct
        }

        // Check for MultiSelectorPanelVm<any type>
        if (DataContext is ISelectorPanelVm &&
            DataContext.GetType().GetGenericTypeDefinition() == typeof(MultiSelectorPanelVm<>))
        {
            _selectorPanelVm = (ISelectorPanelVm)DataContext;
            _selectorPanelVm.PropertyChanged += ViewModel_PropertyChanged;
            RebuildControls(_selectorPanelVm);
        }
        else if (DataContext != null)
        {
            throw new InvalidOperationException("DataContext must be of type MultiSelectorPanelVm<T>");
        }
    }

    private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == "Layout")
        {
            RebuildControls((ISelectorPanelVm)DataContext!);
        }
    }

    private void RebuildControls(ISelectorPanelVm selectorPanelVm)
    {
        if (_layoutHost == null)
            return;

        var panel = CreateLayoutPanel(selectorPanelVm);

        // Add all children from ViewModel
        var children = ((dynamic)selectorPanelVm).Children as System.Collections.IList;
        foreach (var child in children!)
        {
            var childView = CreateViewForChild(selectorPanelVm, child);
            panel!.Children.Add(childView);
        }
        _layoutHost.Child = panel;
    }

    private Panel CreateLayoutPanel(ISelectorPanelVm selectorPanelVm)
    {
        Panel? panel = null;
        switch (selectorPanelVm.Layout)
        {
            case SelectorPanelLayout.Horizontal:
                panel = new StackPanel { Orientation = Orientation.Horizontal };
                break;
            case SelectorPanelLayout.Vertical:
                panel = new StackPanel { Orientation = Orientation.Vertical };
                break;
            case SelectorPanelLayout.Wrap:
                panel = new WrapPanel();
                break;
            default:
                throw new InvalidOperationException("Unknown layout type");
        }
        return panel;
    }

    private Control CreateViewForChild(ISelectorPanelVm selectorPanelVm, object childVm)
    {
        var view = new SelectableItemView();
        view.Layout = selectorPanelVm.Layout;
        view.IsSingleSelector = false;
        view.DataContext = childVm;
        return view;
    }
}
