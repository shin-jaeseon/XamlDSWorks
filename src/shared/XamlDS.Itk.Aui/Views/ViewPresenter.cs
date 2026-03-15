using Avalonia;
using Avalonia.Controls;
using System.Diagnostics;

namespace XamlDS.Itk.Views;

/// <summary>
/// Decorator that presents the appropriate View for a given ViewModel.
/// Uses ViewLocator to find the View, falls back to ContentControl if not found.
/// 
/// DataContext is used as the ViewModel - no Content property needed!
/// </summary>
public class ViewPresenter : Decorator
{
    /// <summary>
    /// Override OnPropertyChanged to detect DataContext changes.
    /// </summary>
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs e)
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

        // If DataContext is already a Control, use it directly
        if (dataContext is Control control)
        {
            Child = control;
            return;
        }

        // Use ViewLocator to find the appropriate View
        var view = ViewLocator.GetView(dataContext);

        if (view != null)
        {
            // View found - set DataContext and use it
            view.DataContext = dataContext;
            Child = view;
        }
        else
        {
            // No View found - fallback to ContentControl
            Debug.WriteLine($"[ViewPresenter] No View found for {dataContext.GetType().Name}, using ContentControl fallback");

            var fallbackControl = new ContentControl
            {
                Content = dataContext,
                HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                VerticalContentAlignment = Avalonia.Layout.VerticalAlignment.Stretch
            };

            Child = fallbackControl;
        }
    }
}
