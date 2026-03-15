using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.LogicalTree;
using System.Collections.Specialized;
using XamlDS.Itk.ViewModels;
using XamlDS.Itk.ViewModels.ExtendedProperties;
using XamlDS.Itk.ViewModels.Layouts;

namespace XamlDS.Itk.Views.Layouts;

public class DockPanelView : TemplatedControl
{
    private DockPanel? _dockPanel;
    private DockPanelVm? _viewModel;

    protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromLogicalTree(e);
        UnsubscribeFromViewModel();
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        // Get the PART_DockPanel from the template
        _dockPanel = e.NameScope.Find<DockPanel>("PART_DockPanel");

        // Apply children if DataContext is already set
        if (_viewModel != null && _dockPanel != null)
        {
            RebuildChildren();
        }
    }

    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        // Unsubscribe from previous ViewModel
        UnsubscribeFromViewModel();

        // Type check for DockPanelVm
        if (DataContext is DockPanelVm dockPanelVm)
        {
            _viewModel = dockPanelVm;
            SubscribeToViewModel();
            RebuildChildren();
        }
        else
        {
            _viewModel = null;
            if (_dockPanel != null)
            {
                _dockPanel.Children.Clear();
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
        if (_dockPanel == null || _viewModel == null)
            return;

        // Clear existing children
        _dockPanel.Children.Clear();

        // Add all children from ViewModel
        var children = _viewModel.Children;
        for (int i = 0; i < children.Count; i++)
        {
            var childVm = children[i];
            var childView = CreateViewForChild(childVm);

            // Last child: ignore Dock property, use LastChildFill
            if (i == children.Count - 1)
            {
                // LastChildFill will handle this automatically
                // No need to set Dock property
            }
            else
            {
                // Get Dock value from DockProperty
                var dock = DockProperty.Get(_viewModel, childVm) ?? DockPositon.Left;
                DockPanel.SetDock(childView, ConvertToAvaloniaUIDock(dock));
            }

            _dockPanel.Children.Add(childView);
        }
    }

    private Control CreateViewForChild(ViewModelBase childVm)
    {
        var presenter = new ViewPresenter
        {
            DataContext = childVm,
        };
        return presenter;
    }

    private Avalonia.Controls.Dock ConvertToAvaloniaUIDock(DockPositon dock)
    {
        return dock switch
        {
            DockPositon.Left => Dock.Left,
            DockPositon.Top => Dock.Top,
            DockPositon.Right => Dock.Right,
            DockPositon.Bottom => Dock.Bottom,
            _ => Dock.Left
        };
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
