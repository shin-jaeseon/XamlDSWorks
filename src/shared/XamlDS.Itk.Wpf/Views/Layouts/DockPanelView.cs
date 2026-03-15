using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using XamlDS.Itk.ViewModels.ExtendedProperties;
using XamlDS.Itk.ViewModels.Layouts;

namespace XamlDS.Itk.Views.Layouts;

public class DockPanelView : Control
{
    private DockPanel? _dockPanel;
    private DockPanelVm? _viewModel;

    static DockPanelView()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(DockPanelView),
            new FrameworkPropertyMetadata(typeof(DockPanelView)));
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        // Get the PART_DockPanel from the template
        _dockPanel = GetTemplateChild("PART_DockPanel") as DockPanel;

        // Apply children if DataContext is already set
        if (_viewModel != null && _dockPanel != null)
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

            // ViewPresenter handles ViewModel → View conversion automatically
            var childView = new ViewPresenter { DataContext = childVm };

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
                DockPanel.SetDock(childView, ConvertToWpfDock(dock));
            }

            _dockPanel.Children.Add(childView);
        }
    }

    private Dock ConvertToWpfDock(DockPositon dock)
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
}
