# Metric의 산업 분야 사용

## 개요

"Metric"은 **측정 가능한 정량적 데이터**를 의미하며, 여러 산업 분야에서 표준 용어로 사용됩니다. 이 문서에서는 Metric이 실제 산업 현장에서 어떻게 활용되는지 설명합니다.

## 1. 모니터링 시스템 (Prometheus, Grafana)

### Prometheus의 Metric 타입

Prometheus는 4가지 핵심 Metric 타입을 정의합니다:

| 타입 | 설명 | 사용 예시 |
|------|------|-----------|
| **Counter** | 증가만 하는 값 | HTTP 요청 수, 에러 카운트 |
| **Gauge** | 증가/감소 가능한 값 | 온도, CPU 사용률, 메모리 |
| **Histogram** | 값의 분포 측정 | API 응답 시간 분포 |
| **Summary** | 통계 요약 | 백분위수 계산 |

### 실제 사용 예시

```csharp
// Prometheus .NET Client
var temperature = Metrics.CreateGauge(
    "generator_temperature_celsius",
    "Generator temperature in Celsius",
    new GaugeConfiguration
    {
        LabelNames = new[] { "generator_id" }
    });

temperature.WithLabels("A1").Set(45.3);
```

## 2. Azure Monitor / Application Insights

### Custom Metric 전송

```csharp
var telemetryClient = new TelemetryClient();

telemetryClient.TrackMetric(
    new MetricTelemetry
    {
        Name = "GeneratorA1Temperature",
        Value = 45.3,
        Properties = 
        {
            { "Unit", "Celsius" },
            { "Location", "Plant1" }
        }
    });
```

### Azure Portal 표시 구조

```
Metrics Explorer
├─ Metric: GeneratorA1Temperature
├─ Aggregation: Average, Min, Max
├─ Time Range: Last 24 hours
└─ Chart Type: Line, Area, Bar
```

## 3. SCADA / 산업 자동화

**SCADA (Supervisory Control and Data Acquisition)**에서 Metric은 실시간 계측 데이터를 표현합니다.

### SCADA Metric 구조

```csharp
public class ScadaMetric
{
    public string TagName { get; set; }        // "TANK_01_LEVEL"
    public double Value { get; set; }          // 75.5
    public double MinScale { get; set; }       // 0.0
    public double MaxScale { get; set; }       // 100.0
    public string Unit { get; set; }           // "%"
    public DateTime Timestamp { get; set; }
    public AlarmStatus Status { get; set; }    // Normal, Warning, Critical
}
```

### 일반적인 SCADA Metric 예시

- 탱크 레벨 (Tank Level)
- 압력 센서 (Pressure Sensor)
- 유량계 (Flow Meter)
- 온도계 (Temperature Sensor)
- 회전 속도 (RPM - Rotations Per Minute)

## 4. IoT / Industrial IoT (IIoT)

### MQTT 메시지 형식

```json
{
  "deviceId": "Generator-A1",
  "timestamp": "2024-01-15T10:30:00Z",
  "metrics": [
    {
      "name": "temperature",
      "value": 45.3,
      "unit": "celsius",
      "min": 20.0,
      "max": 80.0
    },
    {
      "name": "vibration",
      "value": 2.1,
      "unit": "mm/s",
      "min": 0.0,
      "max": 10.0
    }
  ]
}
```

## 5. Elastic Stack (ELK)

### Elasticsearch Metric 저장

```json
PUT /device-metrics/_doc/1
{
  "@timestamp": "2024-01-15T10:30:00Z",
  "device": {
    "id": "Generator-A1",
    "type": "generator"
  },
  "metric": {
    "name": "temperature",
    "value": 45.3,
    "unit": "celsius",
    "range": {
      "min": 20.0,
      "max": 80.0
    }
  }
}
```

## 6. DevOps / Performance Monitoring

### StatsD / DataDog 스타일

```csharp
public class PerformanceMetrics
{
    public void RecordMetric(string metricName, double value, Dictionary<string, string> tags)
    {
        // dogstatsd.Gauge("database.query.time", 234.5, 
        //     tags: new[] { "env:prod", "service:api" });
    }
}
```

## 산업별 Metric 예시

### 제조업 (Manufacturing)

**OEE (Overall Equipment Effectiveness) Metrics:**

```
OEE 구성 요소:
├─ Availability (가용성)
│   └─ 운전 시간 / 계획 시간
├─ Performance (성능)
│   └─ 실제 생산량 / 목표 생산량  
└─ Quality (품질)
    └─ 양품 수 / 전체 생산 수
```

### 발전소 (Power Plant)

**Generator Metrics:**

- Active Power (MW) - 유효 전력
- Reactive Power (MVAR) - 무효 전력
- Frequency (Hz) - 주파수
- Voltage (kV) - 전압
- Temperature (°C) - 온도

### 데이터센터

**Server Metrics:**

- CPU Utilization (%) - CPU 사용률
- Memory Usage (GB) - 메모리 사용량
- Disk I/O (IOPS) - 디스크 입출력
- Network Throughput (Mbps) - 네트워크 처리량
- Temperature (°C) - 온도

## Metric의 핵심 특징

### 1. 시계열 데이터 (Time-Series)

Metric은 항상 시간 정보와 함께 저장됩니다.

```csharp
public class TimeSeriesMetric
{
    public string Name { get; set; }
    public double Value { get; set; }
    public DateTime Timestamp { get; set; }  // 시간 정보 필수
    public Dictionary<string, string> Tags { get; set; }  // 메타데이터
}
```

### 2. 집계 가능 (Aggregatable)

시간 범위별로 통계 집계가 가능합니다.

```sql
-- 시간대별 평균, 최소, 최대
SELECT 
    AVG(value) as avg_temp,
    MIN(value) as min_temp,
    MAX(value) as max_temp
FROM metrics
WHERE name = 'temperature'
GROUP BY time_bucket('1 hour', timestamp)
```

### 3. 임계값 기반 알람

Metric 값에 따라 자동으로 알람을 발생시킬 수 있습니다.

```csharp
public class MetricAlarmRule
{
    public string MetricName { get; set; }
    public double WarningThreshold { get; set; }   // 70.0
    public double CriticalThreshold { get; set; }  // 85.0
    public TimeSpan Duration { get; set; }         // 5분 이상 지속 시
}
```

## MetricVm 구현

### 기본 구조

```csharp
public class MetricVm : ViewModelBase
{
    // 핵심 Gauge Metric 속성
    public double Value { get; set; }   // 현재 값
    public double Min { get; set; }     // 스케일 최소
    public double Max { get; set; }     // 스케일 최대
    
    // 메타데이터
    public string Label { get; set; }   // Metric 이름
    public string Unit { get; set; }    // 단위
    
    // 시각화
    public double Step { get; set; }    // 눈금 간격
}
```

### 산업 표준 확장

```csharp
public class MetricVm : ViewModelBase
{
    // 기존 속성...
    
    // 산업 표준 추가 속성
    public DateTime Timestamp { get; set; }           // 측정 시간
    public MetricStatus Status { get; set; }          // Normal, Warning, Critical
    public double? LowWarningThreshold { get; set; }  // 하한 경고 임계값
    public double? HighWarningThreshold { get; set; } // 상한 경고 임계값
    public double? LowCriticalThreshold { get; set; } // 하한 위험 임계값
    public double? HighCriticalThreshold { get; set; }// 상한 위험 임계값
    public Dictionary<string, string> Tags { get; set; }  // 추가 메타데이터
}

public enum MetricStatus
{
    Normal,
    Warning,
    Critical,
    NoData
}
```

## 임계값 평가 로직

### 자동 Status 업데이트

```csharp
public class MetricVm : ViewModelBase
{
    partial void OnValueChanged(double value)
    {
        UpdateStatus();
    }

    private void UpdateStatus()
    {
        if (double.IsNaN(Value))
        {
            Status = MetricStatus.NoData;
            return;
        }

        // Critical 범위 체크
        if (HighCriticalThreshold.HasValue && Value >= HighCriticalThreshold.Value)
        {
            Status = MetricStatus.Critical;
            return;
        }
        if (LowCriticalThreshold.HasValue && Value <= LowCriticalThreshold.Value)
        {
            Status = MetricStatus.Critical;
            return;
        }

        // Warning 범위 체크
        if (HighWarningThreshold.HasValue && Value >= HighWarningThreshold.Value)
        {
            Status = MetricStatus.Warning;
            return;
        }
        if (LowWarningThreshold.HasValue && Value <= LowWarningThreshold.Value)
        {
            Status = MetricStatus.Warning;
            return;
        }

        Status = MetricStatus.Normal;
    }
}
```

## 사용 예시

### ViewModel 레이어

```csharp
public class DeviceViewModel
{
    public DeviceViewModel()
    {
        // Metric 초기화
        Temperature = new MetricVm
        {
            Label = "Generator A1 Temperature",
            Unit = "°C",
            Min = 0,
            Max = 100,
            Step = 10,
            LowWarningThreshold = 70,
            HighWarningThreshold = 85,
            HighCriticalThreshold = 95
        };
    }

    public MetricVm Temperature { get; }

    public void UpdateSensorData(double temperature)
    {
        Temperature.Value = temperature;
        Temperature.Timestamp = DateTime.UtcNow;
        // Status는 자동으로 업데이트됨
    }
}
```

### View 레이어

```xml
<!-- XAML -->
<StackPanel>
    <!-- Linear Gauge 표시 -->
    <controls:LinearGauge 
        MinValue="{Binding Temperature.Min}"
        MaxValue="{Binding Temperature.Max}"
        Step="{Binding Temperature.Step}"
        CurrentValue="{Binding Temperature.Value}"
        Label="{Binding Temperature.Label}"
        Unit="{Binding Temperature.Unit}" />
    
    <!-- Status 표시 -->
    <TextBlock Text="{Binding Temperature.Status}" 
               Foreground="{Binding Temperature.Status, Converter={StaticResource StatusColorConverter}}" />
</StackPanel>
```

## 결론

"Metric"은 다음과 같은 특징을 가진 산업 표준 용어입니다:

- ✅ **시계열 데이터**: 시간에 따른 값의 변화 추적
- ✅ **범위 기반**: Min/Max 스케일로 정규화된 표현
- ✅ **집계 가능**: 통계적 분석 및 트렌드 파악
- ✅ **임계값 기반 알람**: 자동화된 모니터링
- ✅ **메타데이터**: Tags를 통한 다차원 분석

이러한 특성으로 인해 **모니터링, 제어, 계측** 시스템에서 보편적으로 사용되는 핵심 개념입니다.

## 참고 자료

- [Prometheus Metric Types](https://prometheus.io/docs/concepts/metric_types/)
- [Azure Monitor Metrics](https://learn.microsoft.com/en-us/azure/azure-monitor/essentials/data-platform-metrics)
- [Grafana Documentation](https://grafana.com/docs/)
- [OpenTelemetry Metrics](https://opentelemetry.io/docs/concepts/signals/metrics/)
