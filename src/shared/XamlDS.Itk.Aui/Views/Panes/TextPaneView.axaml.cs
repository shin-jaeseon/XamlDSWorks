using Avalonia;
using Avalonia.Controls.Primitives;

namespace XamlDS.Itk.Views.Panes;

public class TextPaneView : TemplatedControl
{
    /// <summary>
    /// Defines the <see cref="Text"/> property.
    /// </summary>
    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<TextPaneView, string>(nameof(Text), defaultValue: string.Empty);

    /// <summary>
    /// Gets or sets the text to display.
    /// </summary>
    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    //protected override void OnAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e)
    //{
    //    base.OnAttachedToLogicalTree(e);
    //    DataContextChanged += OnDataContextChanged;
    //}

    //protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
    //{
    //    base.OnDetachedFromLogicalTree(e);
    //    DataContextChanged -= OnDataContextChanged;
    //}

    //private void OnDataContextChanged(object? sender, EventArgs e)
    //{
    //    // Type check for TextPaneVm
    //    if (DataContext is TextPaneVm textPaneVm)
    //    {
    //        // Bind Text property to ViewModel
    //        this.Bind(TextProperty, textPaneVm.GetObservable(nameof(TextPaneVm.Text)));
    //    }
    //    else
    //    {
    //        // Clear binding if DataContext is not TextPaneVm
    //        Text = string.Empty;
    //    }
    //}

    //protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    //{
    //    base.OnPropertyChanged(change);

    //    if (change.Property == DataContextProperty)
    //    {
    //        OnDataContextChanged(this, EventArgs.Empty);
    //    }
    //}
}
