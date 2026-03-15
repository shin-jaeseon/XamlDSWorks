using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using XamlDS.Itk.ViewModels.Layouts;

namespace XamlDS.Itk.Views.Layouts;

public class StackPanelView : Control
{
    private StackPanel? _stackPanel;
    private StackPanelVm? _viewModel;

    static StackPanelView()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(StackPanelView),
            new FrameworkPropertyMetadata(typeof(StackPanelView)));
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        // Get the PART_StackPanel from the template
        _stackPanel = GetTemplateChild("PART_StackPanel") as StackPanel;

        // Apply children if DataContext is already set
        if (_viewModel != null && _stackPanel != null)
        {
            RebuildChildren();
        }
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (e.Property == DataContextProperty)
        {
            OnDataContextChanged();
        }
    }

    private void OnDataContextChanged()
    {
        // Unsubscribe from previous ViewModel
        UnsubscribeFromViewModel();

        // Update ViewModel reference
        if (DataContext is StackPanelVm stackPanelVm)
        {
            _viewModel = stackPanelVm;
            SubscribeToViewModel();
            RebuildChildren();
        }
        else
        {
            _viewModel = null;
            if (_stackPanel != null)
            {
                _stackPanel.Children.Clear();
            }
        }
    }

    private void SubscribeToViewModel()
    {
        if (_viewModel != null && _viewModel.Children is INotifyCollectionChanged observable)
        {
            observable.CollectionChanged += OnChildrenCollectionChanged;
        }
    }

    private void UnsubscribeFromViewModel()
    {
        if (_viewModel != null && _viewModel.Children is INotifyCollectionChanged observable)
        {
            observable.CollectionChanged -= OnChildrenCollectionChanged;
        }
    }

    private void OnChildrenCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        RebuildChildren();
    }

    private void RebuildChildren()
    {
        if (_stackPanel == null || _viewModel == null)
            return;

        // Clear existing children
        _stackPanel.Children.Clear();

        // Add all children from ViewModel
        var children = _viewModel.Children;
        foreach (var childVm in children)
        {
            // ViewPresenter handles ViewModel → View conversion automatically
            var childView = new ViewPresenter { DataContext = childVm };
            _stackPanel.Children.Add(childView);
        }
    }
}
