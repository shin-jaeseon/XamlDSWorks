# CompressedTimelineChart Design Document

## Overview

**CompressedTimelineChart** is a specialized chart control that displays time-series data with variable time compression rates across different regions of the horizontal axis. This allows viewing both recent high-resolution data and historical compressed data in a single, continuous chart view.

## Concept

### Problem Statement
Traditional time-series charts have a fixed relationship between pixel width and time duration. This constraint limits the amount of historical data that can be displayed simultaneously with real-time data in limited screen space.

### Solution
Implement a timeline chart with multiple time compression zones:
- **Realtime Zone** (Right): Recent data (0-5 seconds) displayed at high temporal resolution
- **Compressed Zone** (Left): Historical data (5 seconds - 10 minutes) displayed with time compression
- **Continuous Line**: Chart data flows seamlessly across both zones as a single connected line

### Visual Representation

```
┌─────────────────────────────────────────────────────────────┐
│  Compressed Zone (10min ~ 5sec)  │  Realtime Zone (5sec ~ 0)│
│         Slow Flow                │      Fast Flow           │
│  ▁▂▁▂▁▂▃▂▃▁▂▁▃▂▃▄▃▂▄▃▄▃▂▁▂▃     │     ▃▅▆█▆▅▃▁            │
│  (Background: Gray 20% Opacity)  │  (Background: Default)   │
└─────────────────────────────────────────────────────────────┘
```

## Use Cases

### Primary: Medical Patient Monitoring
- Monitor vital signs (ECG, SpO2, respiration) in real-time
- Review recent trends while maintaining awareness of longer-term patterns
- Critical for detecting acute changes while understanding baseline context

### Secondary Applications
- Financial trading platforms
- System monitoring dashboards
- Network traffic analysis
- Industrial process control

## Architecture

### Control Hierarchy

```
AvaloniaUI TemplatedControl
    ↓
CompressedTimelineChart
    ├── PART_CompressedCanvas (Left zone)
    ├── PART_RealtimeCanvas (Right zone)
    ├── PART_TimeAxis (Custom time labels)
    └── PART_ZoomThumb (Interactive zoom control)
```

### Key Properties

```csharp
// Time Range Configuration
public TimeSpan TotalDuration { get; set; }           // Total time window (default: 10 minutes)
public TimeSpan RealtimeDuration { get; set; }        // Realtime zone duration (default: 5 seconds)
public TimeSpan CompressedDuration { get; set; }      // Compressed zone duration (calculated)

// Compression Settings
public double RealtimeScale { get; set; }             // Pixels per second in realtime zone (default: 20)
public double CompressedScale { get; set; }           // Pixels per second in compressed zone (default: 2)
public double CompressionRatio { get; set; }          // Compression ratio (CompressedScale / RealtimeScale)

// Visual Configuration
public double RealtimeZoneWidth { get; set; }         // Width of realtime zone in pixels (default: 200)
public double CompressedZoneWidth { get; set; }       // Width of compressed zone in pixels (default: 400)
public Brush CompressedBackground { get; set; }       // Background brush for compressed zone
public double CompressedBackgroundOpacity { get; set; } // Opacity for visual distinction (default: 0.2)

// Data Binding
public IEnumerable<TimeSeriesPoint> DataPoints { get; set; }  // Time-series data source
public double MinValue { get; set; }                  // Y-axis minimum
public double MaxValue { get; set; }                  // Y-axis maximum
public Brush LineBrush { get; set; }                  // Chart line color
public double LineThickness { get; set; }             // Chart line thickness

// Interaction
public bool IsZoomEnabled { get; set; }               // Enable zoom thumb interaction
public double ZoomThumbPosition { get; set; }         // Position of zoom boundary (0.0 to 1.0)
public double ZoomThumbWidth { get; set; }            // Width of zoom control handle

// Grid and Axis
public bool ShowTimeGrid { get; set; }                // Display vertical time grid lines
public bool ShowValueGrid { get; set; }               // Display horizontal value grid lines
public Brush GridBrush { get; set; }                  // Grid line color
public TimeSpan TimeGridInterval { get; set; }        // Interval between time grid lines
```

### Data Model

```csharp
public class TimeSeriesPoint
{
    public DateTime Timestamp { get; set; }
    public double Value { get; set; }
}

public class CompressedTimelineChartConfiguration
{
    public TimeZoneConfig RealtimeZone { get; set; }
    public TimeZoneConfig CompressedZone { get; set; }
}

public class TimeZoneConfig
{
    public TimeSpan Duration { get; set; }
    public double PixelsPerSecond { get; set; }
    public Brush BackgroundBrush { get; set; }
}
```

## Implementation Phases

### Phase 1: Basic Rendering (MVP)
- [x] Define control structure
- [ ] Implement dual-zone canvas rendering
- [ ] Implement time-to-pixel coordinate conversion
- [ ] Render continuous line across zones
- [ ] Add background color distinction
- [ ] Implement basic time axis labels

### Phase 2: Data Management
- [ ] Implement efficient data point buffering
- [ ] Add real-time data streaming support
- [ ] Implement data decimation for compressed zone
- [ ] Add data update optimization (only redraw changed regions)

### Phase 3: Interactivity
- [ ] Implement zoom thumb control
- [ ] Add drag-to-adjust zoom boundary
- [ ] Implement tooltip with timestamp and value
- [ ] Add click-to-focus functionality

### Phase 4: Advanced Features
- [ ] Multiple data series support
- [ ] Configurable compression algorithms (linear, logarithmic)
- [ ] Alarm zones and threshold indicators
- [ ] Export and print functionality
- [ ] Animation for smooth transitions

## Design Considerations

### Advantages
✅ **Space Efficiency**: Display extended time periods in limited screen space  
✅ **Context Preservation**: Maintain awareness of both recent and historical data  
✅ **Continuous Visualization**: Single connected line maintains data continuity  
✅ **Medical Domain Fit**: Ideal for vital signs monitoring scenarios  

### Challenges
⚠️ **Visual Distortion**: Slope and patterns appear different across zones  
⚠️ **User Confusion**: Requires clear visual cues to indicate scale changes  
⚠️ **Data Interpretation**: Users must understand compression effects on data appearance  

### Mitigation Strategies
- Use distinct background colors for each zone
- Display clear time scale indicators
- Add visual transition markers at zone boundaries
- Provide interactive tooltips with precise values
- Include user education/onboarding for first-time users

## UI/UX Guidelines

### Visual Distinction
```
Realtime Zone:
- Background: Transparent or default
- Grid: Dense (every 1 second)
- Labels: Precise (HH:mm:ss)

Compressed Zone:
- Background: Dark gray with 20% opacity
- Grid: Sparse (every 1 minute)
- Labels: Approximate (HH:mm)
```

### Time Axis Labeling
```
|-------------|-------|---|--|-|Now
10min        5min    1min 30s 5s

Compressed    →    Realtime
```

### Interaction Patterns
1. **Hover**: Display tooltip with exact timestamp and value
2. **Click**: Focus/highlight specific time point
3. **Drag Zoom Thumb**: Adjust boundary between zones
4. **Scroll**: Shift time window (optional)

## Technical Specifications

### Performance Targets
- Support 60 FPS rendering for real-time updates
- Handle 10,000+ data points efficiently
- Memory footprint < 100 MB for typical use case
- Smooth zoom thumb dragging (< 16ms response time)

### Rendering Strategy
```csharp
// Pseudo-code for point-to-pixel conversion
double TimeToPixelX(DateTime timestamp)
{
    var age = DateTime.Now - timestamp;
    
    if (age <= RealtimeDuration)
    {
        // Realtime zone: right side
        var secondsAgo = age.TotalSeconds;
        return Width - (secondsAgo * RealtimeScale);
    }
    else
    {
        // Compressed zone: left side
        var compressedSeconds = age.TotalSeconds - RealtimeDuration.TotalSeconds;
        var compressedPixels = compressedSeconds * CompressedScale;
        return RealtimeZoneWidth - compressedPixels;
    }
}
```

## File Structure

```
📁 XamlDS.Itk.Aui
  📁 Controls
    📄 CompressedTimelineChart.cs           // Main control logic
    📄 CompressedTimelineChart.axaml        // Default template
    📄 TimeSeriesPoint.cs                   // Data model
    📄 TimeZoneConfig.cs                    // Configuration model
  📁 Themes
    📁 Controls
      📄 CompressedTimelineChartThemes.axaml  // Styling themes
  📁 Helpers
    📄 TimelineRenderer.cs                  // Rendering logic
    📄 CoordinateConverter.cs               // Time-to-pixel conversion
```

## Example Usage

```xml
<itk:CompressedTimelineChart
    TotalDuration="00:10:00"
    RealtimeDuration="00:00:05"
    RealtimeScale="20"
    CompressedScale="2"
    DataPoints="{Binding VitalSignsData}"
    MinValue="0"
    MaxValue="200"
    LineBrush="LimeGreen"
    LineThickness="2"
    CompressedBackground="#333333"
    CompressedBackgroundOpacity="0.2"
    IsZoomEnabled="True"
    ShowTimeGrid="True"
    ShowValueGrid="True"/>
```

```csharp
// ViewModel example
public ObservableCollection<TimeSeriesPoint> VitalSignsData { get; } = new();

private void OnNewDataReceived(double value)
{
    VitalSignsData.Add(new TimeSeriesPoint 
    { 
        Timestamp = DateTime.Now, 
        Value = value 
    });
    
    // Keep only last 10 minutes of data
    var cutoff = DateTime.Now.AddMinutes(-10);
    while (VitalSignsData.Count > 0 && VitalSignsData[0].Timestamp < cutoff)
    {
        VitalSignsData.RemoveAt(0);
    }
}
```

## Future Enhancements

### Version 2.0
- Multiple compression zones (3+ zones with different scales)
- Logarithmic time compression
- Configurable transition smoothing algorithms

### Version 3.0
- Touch gesture support for mobile devices
- Real-time zoom level adjustment via pinch gesture
- Export to PNG/SVG with preserved scale information

## References

### Similar Concepts in Industry
- **Philips IntelliVue**: Multi-scale ECG displays
- **Bloomberg Terminal**: Focus+Context financial charts
- **Google Analytics**: Variable time-scale traffic graphs

### Academic Research
- Focus+Context visualization techniques
- Time-series data compression algorithms
- Medical monitoring UI/UX best practices

---

**Document Version**: 1.0  
**Created**: 2024  
**Last Updated**: 2024  
**Status**: Design Phase  
**Next Review**: After Phase 1 prototype completion
