using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace XamlDS.Itk.Views;

/// <summary>
/// Decorator that automatically renders the appropriate View for a given ViewModel.
/// Uses DataContext to determine which View to display - no Content property needed!
/// Supports both type-specific and conditional ViewModel-to-View mappings.
/// Falls back to ContentControl when no mapping is found.
/// </summary>
public class ViewPresenter : Decorator
{
    static ViewPresenter()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(ViewPresenter),
            new FrameworkPropertyMetadata(typeof(ViewPresenter)));
    }

    /// <summary>
    /// Override OnPropertyChanged to detect DataContext changes.
    /// When DataContext changes, update the visual child accordingly.
    /// </summary>
    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (e.Property == DataContextProperty)
        {
            UpdateChild(e.NewValue);
        }
    }

    /// <summary>
    /// Update the visual child based on the DataContext.
    /// </summary>
    private void UpdateChild(object? dataContext)
    {
        if (dataContext == null)
        {
            Child = null;
            return;
        }

        // If DataContext is already a FrameworkElement, use it directly
        if (dataContext is FrameworkElement element)
        {
            Child = element;
            return;
        }

        // Use ViewLocator to find the appropriate View
        var view = ViewLocator.GetView(dataContext);
        if (view != null)
        {
            view.DataContext = dataContext;
            Child = view;
            return;
        }
        else
        {
            // No View found - fallback to ContentControl
            Debug.WriteLine($"[ViewPresenter] No View found for {dataContext.GetType().Name}, using ContentControl fallback");

            var fallbackControl = new ContentControl
            {
                Content = dataContext,
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                VerticalContentAlignment = VerticalAlignment.Stretch
            };

            Child = fallbackControl;
        }
    }
}
