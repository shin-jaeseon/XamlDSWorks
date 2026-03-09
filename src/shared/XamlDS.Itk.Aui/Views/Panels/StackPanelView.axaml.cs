using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.LogicalTree;
using System.Collections.Specialized;
using XamlDS.Itk.ViewModels;
using XamlDS.Itk.ViewModels.Panels;

namespace XamlDS.Itk.Views.Panels;

public class StackPanelView : TemplatedControl
{
    private StackPanel? _stackPanel;
    private StackPanelVm? _viewModel;

    protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromLogicalTree(e);
        UnsubscribeFromViewModel();
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _stackPanel = e.NameScope.Find<StackPanel>("PART_StackPanel");

        if (_viewModel != null && _stackPanel != null)
        {
            RebuildChildren();
        }
    }

    private void OnDataContextChanged(object? sender, EventArgs e)
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

        _stackPanel.Children.Clear();

        foreach (var childVm in _viewModel.Children)
        {
            var childView = CreateViewForChild(childVm);
            _stackPanel.Children.Add(childView);
        }
    }

    private Control CreateViewForChild(ViewModelBase childVm)
    {
        // Use Avalonia's DataTemplate system to create the view
        // This assumes there are DataTemplates registered for ViewModels
        var view = new ContentControl
        {
            Content = childVm,
            DataContext = childVm
        };
        return view;
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == DataContextProperty)
        {
            OnDataContextChanged(this, EventArgs.Empty);
        }
    }
}
