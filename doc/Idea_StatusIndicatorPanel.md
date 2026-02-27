# StatusIndicatorPanel Control Design Document

## Overview

`StatusIndicatorPanel` is a read-only status display control designed for scenarios where multiple options exist, but only one is currently active. Unlike interactive controls like `ListBox` or `RadioButton`, this control is optimized purely for **visual state indication** without selection or interaction capabilities.

### Use Cases

- **Aircraft Instrument Panel**: Display multiple flags/indicators with only the active one illuminated
- **Theme/Mode Display**: Show available themes with the current one highlighted (read-only view)
- **Status Monitors**: Display system states, connection statuses, or operational modes
- **Process Step Indicators**: Show current step in a multi-step workflow
- **Configuration Displays**: Show current configuration among available options

### Key Characteristics

- **Read-Only**: No user interaction for changing state
- **Performance Optimized**: No selection logic, event handlers, or focus management
- **Clear Semantics**: Purpose-built API that clearly communicates intent
- **Lightweight**: Minimal overhead compared to repurposing selection controls

---

## Design Principles

### 1. Semantic Clarity
```xml
<!-- Bad: Repurposing interactive control -->
<ListBox IsEnabled="False" SelectedItem="{Binding CurrentMode}"/>

<!-- Good: Purpose-built control -->
<StatusIndicatorPanel ActiveItem="{Binding CurrentMode}"
                      ItemsSource="{Binding AvailableModes}"/>
```

### 2. Domain-Specific Properties
```csharp
// Clear, domain-appropriate property names
public object? ActiveItem { get; set; }
public IBrush ActiveBrush { get; set; }
public IBrush InactiveBrush { get; set; }
public IndicatorShape Shape { get; set; }
```

### 3. Visual-Only Binding
```xml
<!-- One-way binding only - no selection events -->
<StatusIndicatorPanel ActiveItem="{Binding CurrentTheme, Mode=OneWay}"/>
```

---

## API Design

### Core Properties

```csharp
/// <summary>
/// The currently active item that should be highlighted
/// </summary>
public object? ActiveItem { get; set; }

/// <summary>
/// Collection of all available indicators
/// </summary>
public IEnumerable? ItemsSource { get; set; }

/// <summary>
/// Template for displaying each indicator item
/// </summary>
public IDataTemplate? ItemTemplate { get; set; }

/// <summary>
/// Panel template for arranging indicators
/// </summary>
public ITemplate<Panel>? ItemsPanel { get; set; }

/// <summary>
/// Brush for the active indicator
/// </summary>
public IBrush ActiveBrush { get; set; }

/// <summary>
/// Brush for inactive indicators
/// </summary>
public IBrush InactiveBrush { get; set; }

/// <summary>
/// Spacing between indicators
/// </summary>
public double Spacing { get; set; }

/// <summary>
/// Orientation of the indicator panel
/// </summary>
public Orientation Orientation { get; set; }

/// <summary>
/// Display mode for indicators
/// </summary>
public IndicatorDisplayMode DisplayMode { get; set; }
```

### Enumerations

```csharp
public enum IndicatorDisplayMode
{
    /// <summary>Show all indicators, highlight active</summary>
    ShowAll,
    
    /// <summary>Show only the active indicator</summary>
    ActiveOnly,
    
    /// <summary>Dim inactive indicators</summary>
    DimInactive
}

public enum IndicatorShape
{
    Rectangle,
    Circle,
    Pill,
    Custom
}
```

---

## Implementation

### Class Structure

```csharp
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Layout;
using Avalonia.Media;
using System.Collections;
using System.Collections.Specialized;

namespace XamlDS.ITK.Aui.Controls
{
    /// <summary>
    /// A read-only control for displaying status indicators where only one item is active at a time.
    /// Optimized for visual state display without user interaction.
    /// </summary>
    public class StatusIndicatorPanel : TemplatedControl
    {
        #region Styled Properties

        public static readonly StyledProperty<object?> ActiveItemProperty =
            AvaloniaProperty.Register<StatusIndicatorPanel, object?>(
                nameof(ActiveItem));

        public static readonly StyledProperty<IEnumerable?> ItemsSourceProperty =
            AvaloniaProperty.Register<StatusIndicatorPanel, IEnumerable?>(
                nameof(ItemsSource));

        public static readonly StyledProperty<IDataTemplate?> ItemTemplateProperty =
            AvaloniaProperty.Register<StatusIndicatorPanel, IDataTemplate?>(
                nameof(ItemTemplate));

        public static readonly StyledProperty<ITemplate<Panel>?> ItemsPanelProperty =
            AvaloniaProperty.Register<StatusIndicatorPanel, ITemplate<Panel>?>(
                nameof(ItemsPanel));

        public static readonly StyledProperty<IBrush?> ActiveBrushProperty =
            AvaloniaProperty.Register<StatusIndicatorPanel, IBrush?>(
                nameof(ActiveBrush),
                defaultValue: Brushes.Green);

        public static readonly StyledProperty<IBrush?> InactiveBrushProperty =
            AvaloniaProperty.Register<StatusIndicatorPanel, IBrush?>(
                nameof(InactiveBrush),
                defaultValue: Brushes.Gray);

        public static readonly StyledProperty<double> SpacingProperty =
            AvaloniaProperty.Register<StatusIndicatorPanel, double>(
                nameof(Spacing),
                defaultValue: 4.0);

        public static readonly StyledProperty<Orientation> OrientationProperty =
            AvaloniaProperty.Register<StatusIndicatorPanel, Orientation>(
                nameof(Orientation),
                defaultValue: Orientation.Horizontal);

        public static readonly StyledProperty<IndicatorDisplayMode> DisplayModeProperty =
            AvaloniaProperty.Register<StatusIndicatorPanel, IndicatorDisplayMode>(
                nameof(DisplayMode),
                defaultValue: IndicatorDisplayMode.ShowAll);

        #endregion

        #region Properties

        public object? ActiveItem
        {
            get => GetValue(ActiveItemProperty);
            set => SetValue(ActiveItemProperty, value);
        }

        public IEnumerable? ItemsSource
        {
            get => GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public IDataTemplate? ItemTemplate
        {
            get => GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        public ITemplate<Panel>? ItemsPanel
        {
            get => GetValue(ItemsPanelProperty);
            set => SetValue(ItemsPanelProperty, value);
        }

        public IBrush? ActiveBrush
        {
            get => GetValue(ActiveBrushProperty);
            set => SetValue(ActiveBrushProperty, value);
        }

        public IBrush? InactiveBrush
        {
            get => GetValue(InactiveBrushProperty);
            set => SetValue(InactiveBrushProperty, value);
        }

        public double Spacing
        {
            get => GetValue(SpacingProperty);
            set => SetValue(SpacingProperty, value);
        }

        public Orientation Orientation
        {
            get => GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        public IndicatorDisplayMode DisplayMode
        {
            get => GetValue(DisplayModeProperty);
            set => SetValue(DisplayModeProperty, value);
        }

        #endregion

        #region Fields

        private ItemsControl? _itemsControl;
        private readonly AvaloniaList<StatusIndicatorItem> _indicatorItems = new();

        #endregion

        static StatusIndicatorPanel()
        {
            ActiveItemProperty.Changed.AddClassHandler<StatusIndicatorPanel>(
                (x, e) => x.OnActiveItemChanged(e));
            ItemsSourceProperty.Changed.AddClassHandler<StatusIndicatorPanel>(
                (x, e) => x.OnItemsSourceChanged(e));
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            
            _itemsControl = e.NameScope.Find<ItemsControl>("PART_ItemsControl");
            
            if (_itemsControl != null)
            {
                _itemsControl.ItemsSource = _indicatorItems;
            }
            
            RebuildIndicators();
        }

        private void OnActiveItemChanged(AvaloniaPropertyChangedEventArgs e)
        {
            UpdateActiveStates();
        }

        private void OnItemsSourceChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (e.OldValue is INotifyCollectionChanged oldCollection)
            {
                oldCollection.CollectionChanged -= OnSourceCollectionChanged;
            }

            if (e.NewValue is INotifyCollectionChanged newCollection)
            {
                newCollection.CollectionChanged += OnSourceCollectionChanged;
            }

            RebuildIndicators();
        }

        private void OnSourceCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            RebuildIndicators();
        }

        private void RebuildIndicators()
        {
            _indicatorItems.Clear();

            if (ItemsSource == null)
                return;

            foreach (var item in ItemsSource)
            {
                var indicatorItem = new StatusIndicatorItem
                {
                    Content = item,
                    ContentTemplate = ItemTemplate,
                    IsActive = Equals(item, ActiveItem),
                    ActiveBrush = ActiveBrush,
                    InactiveBrush = InactiveBrush,
                    DisplayMode = DisplayMode
                };

                _indicatorItems.Add(indicatorItem);
            }
        }

        private void UpdateActiveStates()
        {
            foreach (var item in _indicatorItems)
            {
                item.IsActive = Equals(item.Content, ActiveItem);
            }
        }
    }

    /// <summary>
    /// Internal item container for StatusIndicatorPanel
    /// </summary>
    internal class StatusIndicatorItem : ContentControl
    {
        public static readonly StyledProperty<bool> IsActiveProperty =
            AvaloniaProperty.Register<StatusIndicatorItem, bool>(nameof(IsActive));

        public static readonly StyledProperty<IBrush?> ActiveBrushProperty =
            AvaloniaProperty.Register<StatusIndicatorItem, IBrush?>(nameof(ActiveBrush));

        public static readonly StyledProperty<IBrush?> InactiveBrushProperty =
            AvaloniaProperty.Register<StatusIndicatorItem, IBrush?>(nameof(InactiveBrush));

        public static readonly StyledProperty<IndicatorDisplayMode> DisplayModeProperty =
            AvaloniaProperty.Register<StatusIndicatorItem, IndicatorDisplayMode>(nameof(DisplayMode));

        public bool IsActive
        {
            get => GetValue(IsActiveProperty);
            set => SetValue(IsActiveProperty, value);
        }

        public IBrush? ActiveBrush
        {
            get => GetValue(ActiveBrushProperty);
            set => SetValue(ActiveBrushProperty, value);
        }

        public IBrush? InactiveBrush
        {
            get => GetValue(InactiveBrushProperty);
            set => SetValue(InactiveBrushProperty, value);
        }

        public IndicatorDisplayMode DisplayMode
        {
            get => GetValue(DisplayModeProperty);
            set => SetValue(DisplayModeProperty, value);
        }
    }

    public enum IndicatorDisplayMode
    {
        ShowAll,
        ActiveOnly,
        DimInactive
    }
}
```

### Control Theme (AXAML)

```xml
<!-- StatusIndicatorPanelTheme.axaml -->
<ResourceDictionary xmlns="https://github.com/avaloniAui"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                    xmlns:controls="using:XamlDS.ITK.Aui.Controls">

    <!-- Default ItemTemplate -->
    <DataTemplate x:Key="DefaultIndicatorItemTemplate">
        <TextBlock Text="{Binding}" 
                   FontSize="12"
                   FontWeight="SemiBold"/>
    </DataTemplate>

    <!-- Default ItemsPanel -->
    <ItemsPanelTemplate x:Key="DefaultIndicatorItemsPanel">
        <StackPanel Orientation="{Binding $parent[controls:StatusIndicatorPanel].Orientation}"
                    Spacing="{Binding $parent[controls:StatusIndicatorPanel].Spacing}"/>
    </ItemsPanelTemplate>

    <!-- StatusIndicatorItem Style -->
    <Style Selector="controls|StatusIndicatorItem">
        <Setter Property="Padding" Value="8,4"/>
        <Setter Property="CornerRadius" Value="4"/>
        <Setter Property="BorderThickness" Value="1"/>
        <Setter Property="Template">
            <ControlTemplate>
                <Border Background="{TemplateBinding Background}"
                        BorderBrush="{TemplateBinding BorderBrush}"
                        BorderThickness="{TemplateBinding BorderThickness}"
                        CornerRadius="{TemplateBinding CornerRadius}"
                        Padding="{TemplateBinding Padding}">
                    <ContentPresenter Content="{TemplateBinding Content}"
                                      ContentTemplate="{TemplateBinding ContentTemplate}"
                                      HorizontalAlignment="Center"
                                      VerticalAlignment="Center"/>
                </Border>
            </ControlTemplate>
        </Setter>
        
        <!-- Active State -->
        <Style Selector="^[IsActive=True]">
            <Setter Property="Background" Value="{Binding $parent[controls:StatusIndicatorPanel].ActiveBrush}"/>
            <Setter Property="BorderBrush" Value="{Binding $parent[controls:StatusIndicatorPanel].ActiveBrush}"/>
            <Setter Property="Opacity" Value="1"/>
        </Style>
        
        <!-- Inactive State - ShowAll Mode -->
        <Style Selector="^[IsActive=False][DisplayMode=ShowAll]">
            <Setter Property="Background" Value="Transparent"/>
            <Setter Property="BorderBrush" Value="{Binding $parent[controls:StatusIndicatorPanel].InactiveBrush}"/>
            <Setter Property="Opacity" Value="1"/>
        </Style>
        
        <!-- Inactive State - DimInactive Mode -->
        <Style Selector="^[IsActive=False][DisplayMode=DimInactive]">
            <Setter Property="Background" Value="Transparent"/>
            <Setter Property="BorderBrush" Value="{Binding $parent[controls:StatusIndicatorPanel].InactiveBrush}"/>
            <Setter Property="Opacity" Value="0.3"/>
        </Style>
        
        <!-- Inactive State - ActiveOnly Mode -->
        <Style Selector="^[IsActive=False][DisplayMode=ActiveOnly]">
            <Setter Property="IsVisible" Value="False"/>
        </Style>
    </Style>

    <!-- StatusIndicatorPanel Theme -->
    <ControlTheme x:Key="ITKStatusIndicatorPanelTheme" TargetType="controls:StatusIndicatorPanel">
        <Setter Property="ItemTemplate" Value="{StaticResource DefaultIndicatorItemTemplate}"/>
        <Setter Property="ItemsPanel" Value="{StaticResource DefaultIndicatorItemsPanel}"/>
        <Setter Property="Template">
            <ControlTemplate>
                <ItemsControl Name="PART_ItemsControl"
                              ItemsPanel="{TemplateBinding ItemsPanel}"/>
            </ControlTemplate>
        </Setter>
    </ControlTheme>

</ResourceDictionary>
```

---

## Usage Examples

### Basic Usage

```xml
<controls:StatusIndicatorPanel ItemsSource="{Binding AvailableThemes}"
                               ActiveItem="{Binding CurrentTheme}"
                               Orientation="Horizontal"
                               Spacing="8"/>
```

### With Custom Template

```xml
<controls:StatusIndicatorPanel ItemsSource="{Binding ConnectionStates}"
                               ActiveItem="{Binding CurrentState}"
                               ActiveBrush="#00FF00"
                               InactiveBrush="#666666">
    <controls:StatusIndicatorPanel.ItemTemplate>
        <DataTemplate>
            <StackPanel Orientation="Horizontal" Spacing="4">
                <Ellipse Width="12" Height="12" 
                         Fill="{Binding $parent[controls:StatusIndicatorItem].IsActive, 
                                Converter={StaticResource BoolToColorConverter}}"/>
                <TextBlock Text="{Binding DisplayName}" FontSize="11"/>
            </StackPanel>
        </DataTemplate>
    </controls:StatusIndicatorPanel.ItemTemplate>
</controls:StatusIndicatorPanel>
```

### Display Modes

```xml
<!-- Show All (Default) -->
<controls:StatusIndicatorPanel DisplayMode="ShowAll"
                               ItemsSource="{Binding Modes}"
                               ActiveItem="{Binding CurrentMode}"/>

<!-- Show Active Only -->
<controls:StatusIndicatorPanel DisplayMode="ActiveOnly"
                               ItemsSource="{Binding Modes}"
                               ActiveItem="{Binding CurrentMode}"/>

<!-- Dim Inactive -->
<controls:StatusIndicatorPanel DisplayMode="DimInactive"
                               ItemsSource="{Binding Modes}"
                               ActiveItem="{Binding CurrentMode}"/>
```

### Aircraft Panel Style Example

```xml
<controls:StatusIndicatorPanel ItemsSource="{Binding SystemFlags}"
                               ActiveItem="{Binding ActiveFlag}"
                               Orientation="Vertical"
                               DisplayMode="ShowAll"
                               ActiveBrush="#FF3333"
                               InactiveBrush="#333333">
    <controls:StatusIndicatorPanel.ItemTemplate>
        <DataTemplate>
            <Grid ColumnDefinitions="Auto,*" Width="120">
                <Ellipse Grid.Column="0" 
                         Width="16" Height="16" 
                         Fill="{Binding $parent[controls:StatusIndicatorItem].IsActive, 
                                Converter={StaticResource BoolToBrushConverter}}"/>
                <TextBlock Grid.Column="1" 
                           Text="{Binding Name}"
                           Margin="8,0,0,0"
                           VerticalAlignment="Center"
                           FontFamily="Consolas"
                           FontSize="10"/>
            </Grid>
        </DataTemplate>
    </controls:StatusIndicatorPanel.ItemTemplate>
</controls:StatusIndicatorPanel>
```

---

## Advantages Over ListBox/RadioButton

| Feature | ListBox | RadioButton | StatusIndicatorPanel |
|---------|---------|-------------|---------------------|
| **Purpose** | Selection | Selection | Display Only |
| **Interaction** | Click, Keyboard | Click, Keyboard | None |
| **Focus** | Yes | Yes | No |
| **Selection Logic** | Complex | Medium | None |
| **Performance** | Moderate | Moderate | High |
| **Semantic Clarity** | Low (when read-only) | Low (when read-only) | High |
| **Event Overhead** | High | Medium | Minimal |
| **Code Maintainability** | Confusing intent | Confusing intent | Clear intent |

---

## Implementation Checklist

- [ ] Create `StatusIndicatorPanel.cs` class
- [ ] Create `StatusIndicatorItem.cs` internal class
- [ ] Create `IndicatorDisplayMode` enum
- [ ] Create `StatusIndicatorPanelTheme.axaml`
- [ ] Add unit tests for property changes
- [ ] Add visual tests for different display modes
- [ ] Create showcase examples
- [ ] Document in README
- [ ] Add XML documentation comments
- [ ] Performance testing with large item counts

---

## Future Enhancements

### Potential Features
- **Animation**: Smooth transitions when active item changes
- **Custom Shapes**: Support for custom indicator shapes (beyond borders)
- **Tooltip Support**: Show additional info on hover
- **Grouping**: Support for grouped indicators
- **Highlighting Effects**: Glow, pulse, or flash effects for active item
- **Size Modes**: Compact, Normal, Large presets
- **Orientation Auto**: Automatically adjust based on available space

### Example Animation Support

```csharp
public static readonly StyledProperty<bool> AnimateTransitionsProperty =
    AvaloniaProperty.Register<StatusIndicatorPanel, bool>(
        nameof(AnimateTransitions),
        defaultValue: true);

public static readonly StyledProperty<TimeSpan> TransitionDurationProperty =
    AvaloniaProperty.Register<StatusIndicatorPanel, TimeSpan>(
        nameof(TransitionDuration),
        defaultValue: TimeSpan.FromMilliseconds(200));
```

---

## Related Controls

- **ItemsControl**: Base class consideration
- **ListBox**: Comparison for selection scenarios
- **TabControl**: Similar active item concept
- **StatusBar**: Related status display control

---

## References

- [Avalonia ItemsControl Documentation](https://docs.avaloniAui.net/docs/controls/itemscontrol)
- [WPF Custom Control Development](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/controls/control-authoring-overview)
- Inspired by aviation instrument panel indicators

---

*Document Version: 1.0*  
*Last Updated: 2024*  
*Author: AI Assistant*
