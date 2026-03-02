# Metric Tags 활용 가이드

## 개요

SCADA 및 산업 모니터링 시스템에서 `Tags` 속성은 **메트릭의 컨텍스트와 메타데이터를 저장**하여 다차원 분석과 필터링을 가능하게 합니다.

```csharp
public class MetricVm : ViewModelBase
{
    // 메트릭의 "신분증" 역할
    public Dictionary<string, string> Tags { get; }
}
```

## Tags의 핵심 역할

- 🏷️ **식별**: 어떤 장비, 어디서, 누가 담당
- 📊 **분석**: 그룹별, 위치별, 시간별 집계
- 🔔 **알람**: 컨텍스트 기반 라우팅
- 🔍 **필터링**: 특정 조건의 메트릭 검색
- 📈 **추적**: 이력 관리 및 트렌드 분석

## 주요 사용 용도

### 1. 장비/위치 식별

메트릭이 어느 장비에서 어느 위치에 있는지 식별합니다.

```csharp
var temperature = new MetricVm
{
    Label = "Temperature",
    Value = 45.3,
    Unit = "°C",
    Tags = 
    {
        { "device_id", "Generator-A1" },
        { "location", "Plant-North" },
        { "building", "Building-3" },
        { "floor", "2" },
        { "zone", "Production-Area-1" }
    }
};
```

**활용 예시:**
- 같은 종류의 센서가 여러 장소에 있을 때 구분
- 계층적 구조로 집계 (건물별 → 층별 → 구역별)
- 지리적 대시보드 구성

### 2. 센서/채널 정보

하드웨어 센서와 통신 채널 정보를 저장합니다.

```csharp
Tags = 
{
    { "sensor_type", "PT100" },           // 센서 유형
    { "sensor_serial", "SN123456" },      // 시리얼 번호
    { "manufacturer", "Siemens" },        // 제조사
    { "channel", "AI-01" },               // 아날로그 입력 채널
    { "plc_address", "DB1.DBD100" },      // PLC 메모리 주소
    { "modbus_register", "40001" }        // Modbus 레지스터
}
```

**활용 예시:**
- 센서 교체 추적
- 유지보수 이력 관리
- 캘리브레이션 스케줄
- 통신 장애 디버깅

### 3. 공정/프로세스 컨텍스트

생산 공정 및 프로세스 정보를 저장합니다.

```csharp
Tags = 
{
    { "process", "Cooling" },
    { "stage", "Stage-2" },
    { "batch_id", "BATCH-2024-001" },
    { "product_line", "Line-A" },
    { "work_order", "WO-2024-0115" },
    { "product_sku", "SKU-12345" },
    { "shift", "Day-Shift" }
}
```

**활용 예시:**
- 배치별 품질 분석
- 라인별 성능 비교
- 교대조별 생산성 측정
- 제품별 공정 파라미터 추적

### 4. 환경/운영 조건

운영 환경 및 책임 정보를 저장합니다.

```csharp
Tags = 
{
    { "environment", "production" },      // production, staging, test
    { "criticality", "high" },            // 중요도: low, medium, high
    { "maintenance_group", "MECH-01" },   // 유지보수 그룹
    { "responsible_engineer", "John.Doe" },
    { "department", "Operations" },
    { "cost_center", "CC-1001" }
}
```

**활용 예시:**
- 중요도별 알람 우선순위 결정
- 담당자별 알람 라우팅
- 환경별 임계값 조정
- 비용 센터별 리소스 분석

### 5. 데이터 품질/상태

데이터의 품질과 신뢰성 정보를 저장합니다.

```csharp
Tags = 
{
    { "quality", "good" },                // good, bad, uncertain
    { "source", "real-time" },            // real-time, calculated, estimated
    { "calibration_date", "2024-01-15" },
    { "calibration_status", "valid" },
    { "last_maintenance", "2024-01-10" },
    { "next_maintenance", "2024-07-10" }
}
```

**활용 예시:**
- 데이터 신뢰성 평가
- 캘리브레이션 만료 알림
- 유지보수 스케줄 관리
- 품질 보증 (QA) 리포트

## Prometheus/Grafana 스타일

산업 표준 모니터링 시스템에서 사용하는 레이블 형태입니다.

```csharp
// Prometheus Label 패턴
var cpuTemp = new MetricVm
{
    Label = "cpu_temperature_celsius",
    Value = 68.5,
    Tags = 
    {
        { "hostname", "server01" },
        { "datacenter", "dc-west" },
        { "rack", "rack-15" },
        { "env", "production" },
        { "job", "node-exporter" }
    }
};

// Grafana 쿼리 예시:
// cpu_temperature_celsius{datacenter="dc-west", env="production"}
// avg(cpu_temperature_celsius{rack=~"rack-1.*"})
```

## 실전 SCADA 시나리오

### 발전소 (Power Plant)

```csharp
public class PowerPlantMetrics
{
    public MetricVm GeneratorTemperature { get; } = new()
    {
        Label = "Generator Temperature",
        Value = 85.0,
        Unit = "°C",
        Min = 0,
        Max = 120,
        HighWarningThreshold = 95,
        HighCriticalThreshold = 105,
        Tags = 
        {
            // 장비 식별
            { "asset_id", "GEN-TURB-01" },
            { "asset_type", "gas_turbine" },
            { "unit", "Unit-1" },
            { "manufacturer", "Siemens" },
            { "model", "SGT-800" },
            { "serial_number", "SN-GT-001" },
            
            // 위치
            { "plant", "Plant-North" },
            { "site", "Site-A" },
            { "building", "Turbine-Hall" },
            { "area", "Generator-Floor" },
            
            // 운영 정보
            { "commissioned_date", "2020-05-15" },
            { "rated_capacity", "50MW" },
            { "current_load", "42MW" },
            { "operating_hours", "35000" },
            
            // 유지보수
            { "maintenance_group", "TURB-MAINT" },
            { "last_inspection", "2024-01-10" },
            { "next_inspection", "2024-07-10" },
            { "responsible_engineer", "eng.smith" },
            
            // 시스템
            { "scada_tag", "GEN01.TEMP.BEARING" },
            { "plc_id", "PLC-01" },
            { "io_module", "AI-16-CH-01" }
        }
    };
}
```

### 제조업 (Manufacturing)

```csharp
public class ManufacturingMetrics
{
    public MetricVm ConveyorSpeed { get; } = new()
    {
        Label = "Conveyor Speed",
        Value = 1.5,
        Unit = "m/s",
        Min = 0,
        Max = 3.0,
        Tags = 
        {
            // 장비
            { "equipment_id", "CONV-LINE-A-01" },
            { "equipment_type", "belt_conveyor" },
            { "equipment_class", "material_handling" },
            
            // 공정
            { "production_line", "Line-A" },
            { "station", "Station-05" },
            { "work_order", "WO-2024-0115" },
            { "product_sku", "SKU-12345" },
            { "batch_id", "BATCH-2024-001" },
            
            // 운영
            { "operator", "Operator-05" },
            { "operator_id", "EMP-12345" },
            { "shift", "Morning" },
            { "shift_start", "06:00" },
            { "shift_supervisor", "Supervisor-A" },
            
            // 품질
            { "quality_check", "passed" },
            { "inspection_time", "2024-01-15T10:30:00Z" },
            
            // 위치
            { "factory", "Factory-Main" },
            { "building", "Production-Hall-1" },
            { "zone", "Assembly-Zone" }
        }
    };
    
    public MetricVm ProductionCount { get; } = new()
    {
        Label = "Production Count",
        Value = 1250,
        Unit = "units",
        Tags = 
        {
            { "production_line", "Line-A" },
            { "shift", "Morning" },
            { "product_sku", "SKU-12345" },
            { "quality_status", "good" },
            { "target", "1500" },              // 목표치
            { "efficiency", "83.3" }           // 효율 (%)
        }
    };
}
```

### 빌딩 관리 시스템 (BMS - Building Management System)

```csharp
public class BuildingMetrics
{
    public MetricVm RoomTemperature { get; } = new()
    {
        Label = "Room Temperature",
        Value = 22.5,
        Unit = "°C",
        Tags = 
        {
            // 위치
            { "building", "HQ-Building" },
            { "floor", "3" },
            { "room", "Conference-Room-A" },
            { "zone", "North-Wing" },
            
            // HVAC
            { "hvac_zone", "Zone-3A" },
            { "ahu_id", "AHU-03" },
            { "vav_box", "VAV-3A-05" },
            
            // 사용
            { "room_type", "conference" },
            { "occupancy", "12" },
            { "capacity", "20" },
            { "booking_status", "occupied" }
        }
    };
}
```

## Tags 활용 패턴

### 1. 집계 및 그룹화 (Aggregation)

```csharp
// 패턴: 계층적 집계
public class MetricAggregator
{
    public Dictionary<string, double> GetAverageByBuilding(List<MetricVm> metrics)
    {
        return metrics
            .Where(m => m.Tags.ContainsKey("building"))
            .GroupBy(m => m.Tags["building"])
            .ToDictionary(
                g => g.Key,
                g => g.Average(m => m.Value)
            );
    }
    
    public Dictionary<string, double> GetTotalByProductLine(List<MetricVm> metrics)
    {
        return metrics
            .Where(m => m.Label == "Production Count")
            .Where(m => m.Tags.ContainsKey("product_line"))
            .GroupBy(m => m.Tags["product_line"])
            .ToDictionary(
                g => g.Key,
                g => g.Sum(m => m.Value)
            );
    }
}

// SQL 쿼리 예시
/*
-- 건물별 평균 온도
SELECT Tags->>'building' as building, AVG(value) as avg_temp
FROM metrics
WHERE label = 'temperature'
GROUP BY Tags->>'building'

-- 제품 라인별 생산량
SELECT Tags->>'product_line' as line, SUM(value) as total_production
FROM metrics
WHERE label = 'production_count' AND timestamp > NOW() - INTERVAL '1 day'
GROUP BY Tags->>'product_line'
*/
```

### 2. 필터링 및 검색 (Filtering)

```csharp
public class MetricFilter
{
    // 특정 건물의 Critical 상태 메트릭 찾기
    public List<MetricVm> GetCriticalMetricsByBuilding(
        List<MetricVm> metrics, 
        string buildingName)
    {
        return metrics
            .Where(m => m.Status == MetricStatus.Critical)
            .Where(m => m.Tags.GetValueOrDefault("building") == buildingName)
            .ToList();
    }
    
    // 특정 교대조의 메트릭만
    public List<MetricVm> GetMetricsByShift(
        List<MetricVm> metrics, 
        string shift)
    {
        return metrics
            .Where(m => m.Tags.GetValueOrDefault("shift") == shift)
            .ToList();
    }
    
    // 높은 중요도 장비만
    public List<MetricVm> GetHighCriticalityMetrics(List<MetricVm> metrics)
    {
        return metrics
            .Where(m => m.Tags.GetValueOrDefault("criticality") == "high")
            .ToList();
    }
    
    // 유지보수 기한이 임박한 장비
    public List<MetricVm> GetMetricsNeedingMaintenance(
        List<MetricVm> metrics, 
        int daysThreshold = 7)
    {
        var threshold = DateTime.Now.AddDays(daysThreshold);
        
        return metrics
            .Where(m => m.Tags.ContainsKey("next_maintenance"))
            .Where(m => DateTime.TryParse(m.Tags["next_maintenance"], out var date) 
                        && date <= threshold)
            .ToList();
    }
}
```

### 3. 알람 라우팅 (Alarm Routing)

컨텍스트 기반으로 적절한 담당자에게 알람을 전송합니다.

```csharp
public class AlarmRouter
{
    public void RouteAlarm(MetricVm metric)
    {
        if (metric.Status == MetricStatus.Critical)
        {
            var criticality = metric.Tags.GetValueOrDefault("criticality", "medium");
            var responsibleEngineer = metric.Tags.GetValueOrDefault("responsible_engineer");
            var maintenanceGroup = metric.Tags.GetValueOrDefault("maintenance_group");
            var department = metric.Tags.GetValueOrDefault("department");
            
            // 중요도에 따른 알람 수준 결정
            var alarmLevel = criticality switch
            {
                "high" => AlarmLevel.Emergency,    // 즉시 전화 + SMS + 이메일
                "medium" => AlarmLevel.High,       // SMS + 이메일
                "low" => AlarmLevel.Normal,        // 이메일만
                _ => AlarmLevel.Normal
            };
            
            // 담당 엔지니어에게 알람 전송
            if (!string.IsNullOrEmpty(responsibleEngineer))
            {
                SendAlert(responsibleEngineer, metric, alarmLevel);
            }
            
            // 유지보수 그룹에 티켓 생성
            if (!string.IsNullOrEmpty(maintenanceGroup))
            {
                CreateMaintenanceTicket(maintenanceGroup, metric);
            }
            
            // 부서 관리자에게 보고
            if (!string.IsNullOrEmpty(department))
            {
                NotifyDepartmentManager(department, metric);
            }
            
            // 위치 기반 알람 (현장 직원)
            var building = metric.Tags.GetValueOrDefault("building");
            if (!string.IsNullOrEmpty(building))
            {
                NotifyOnSitePersonnel(building, metric);
            }
        }
    }
    
    private void SendAlert(string engineer, MetricVm metric, AlarmLevel level)
    {
        // 알람 전송 로직
        var message = $"[{level}] {metric.Label}: {metric.Value} {metric.Unit}\n" +
                      $"Location: {metric.Tags.GetValueOrDefault("building")} / " +
                      $"{metric.Tags.GetValueOrDefault("floor")}\n" +
                      $"Equipment: {metric.Tags.GetValueOrDefault("asset_id")}";
        
        // 알람 레벨에 따라 다른 채널 사용
        // ...
    }
}

public enum AlarmLevel
{
    Normal,     // 이메일
    High,       // SMS + 이메일
    Emergency   // 전화 + SMS + 이메일
}
```

### 4. 컨텍스트 기반 임계값 (Dynamic Thresholds)

환경과 조건에 따라 임계값을 동적으로 조정합니다.

```csharp
public class DynamicThresholdCalculator
{
    public void UpdateThresholds(MetricVm metric)
    {
        var environment = metric.Tags.GetValueOrDefault("environment");
        var criticality = metric.Tags.GetValueOrDefault("criticality");
        var assetType = metric.Tags.GetValueOrDefault("asset_type");
        
        // 환경별 임계값
        if (environment == "production")
        {
            // 프로덕션 환경: 엄격한 임계값
            if (criticality == "high")
            {
                metric.HighWarningThreshold = 75.0;
                metric.HighCriticalThreshold = 85.0;
            }
            else
            {
                metric.HighWarningThreshold = 80.0;
                metric.HighCriticalThreshold = 90.0;
            }
        }
        else if (environment == "test")
        {
            // 테스트 환경: 느슨한 임계값
            metric.HighWarningThreshold = 90.0;
            metric.HighCriticalThreshold = 95.0;
        }
        
        // 장비 유형별 임계값
        if (assetType == "gas_turbine")
        {
            metric.HighCriticalThreshold = 105.0;  // 터빈은 높은 온도 허용
        }
        else if (assetType == "electric_motor")
        {
            metric.HighCriticalThreshold = 80.0;   // 모터는 낮은 온도 제한
        }
        
        // 시간대별 임계값 (예: 야간에는 느슨하게)
        var shift = metric.Tags.GetValueOrDefault("shift");
        if (shift == "Night")
        {
            metric.HighWarningThreshold += 5.0;
        }
    }
    
    // 계절별 임계값 조정
    public void ApplySeasonalAdjustment(MetricVm metric)
    {
        var location = metric.Tags.GetValueOrDefault("location");
        var building = metric.Tags.GetValueOrDefault("building");
        
        var currentMonth = DateTime.Now.Month;
        var isSummer = currentMonth >= 6 && currentMonth <= 8;
        
        if (isSummer && building?.Contains("outdoor") == true)
        {
            // 여름철 실외 장비는 임계값 상향
            metric.HighWarningThreshold += 10.0;
            metric.HighCriticalThreshold += 10.0;
        }
    }
}
```

### 5. 이력 추적 및 분석 (Historical Analysis)

시계열 데이터와 Tags를 조합하여 트렌드를 분석합니다.

```csharp
public class MetricHistoryAnalyzer
{
    // 배치별 품질 추적
    public BatchQualityReport AnalyzeBatchQuality(
        List<MetricHistoryEntry> history, 
        string batchId)
    {
        var batchMetrics = history
            .Where(h => h.Snapshot.Tags.GetValueOrDefault("batch_id") == batchId)
            .OrderBy(h => h.Timestamp)
            .ToList();
        
        return new BatchQualityReport
        {
            BatchId = batchId,
            StartTime = batchMetrics.First().Timestamp,
            EndTime = batchMetrics.Last().Timestamp,
            AverageValue = batchMetrics.Average(m => m.Snapshot.Value),
            MinValue = batchMetrics.Min(m => m.Snapshot.Value),
            MaxValue = batchMetrics.Max(m => m.Snapshot.Value),
            OutOfSpecCount = batchMetrics.Count(m => 
                m.Snapshot.Status == MetricStatus.Warning || 
                m.Snapshot.Status == MetricStatus.Critical)
        };
    }
    
    // 장비별 가동 시간 분석
    public EquipmentUptime AnalyzeEquipmentUptime(
        List<MetricHistoryEntry> history,
        string equipmentId,
        TimeSpan period)
    {
        var startTime = DateTime.Now - period;
        var equipmentMetrics = history
            .Where(h => h.Timestamp >= startTime)
            .Where(h => h.Snapshot.Tags.GetValueOrDefault("equipment_id") == equipmentId)
            .ToList();
        
        var totalTime = period;
        var downtime = equipmentMetrics
            .Where(m => m.Snapshot.Status == MetricStatus.NoData || 
                        m.Snapshot.Value == 0)
            .Count() * TimeSpan.FromMinutes(1);  // 1분 간격 샘플링 가정
        
        return new EquipmentUptime
        {
            EquipmentId = equipmentId,
            Period = period,
            TotalTime = totalTime,
            Uptime = totalTime - downtime,
            UptimePercentage = ((totalTime - downtime).TotalSeconds / totalTime.TotalSeconds) * 100
        };
    }
    
    // 교대조별 생산성 비교
    public Dictionary<string, double> CompareShiftProductivity(
        List<MetricHistoryEntry> history,
        DateTime date)
    {
        return history
            .Where(h => h.Timestamp.Date == date.Date)
            .Where(h => h.Snapshot.Label == "Production Count")
            .Where(h => h.Snapshot.Tags.ContainsKey("shift"))
            .GroupBy(h => h.Snapshot.Tags["shift"])
            .ToDictionary(
                g => g.Key,
                g => g.Sum(h => h.Snapshot.Value)
            );
    }
}

public class MetricHistoryEntry
{
    public DateTime Timestamp { get; set; }
    public MetricVm Snapshot { get; set; }
}

public class BatchQualityReport
{
    public string BatchId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public double AverageValue { get; set; }
    public double MinValue { get; set; }
    public double MaxValue { get; set; }
    public int OutOfSpecCount { get; set; }
}

public class EquipmentUptime
{
    public string EquipmentId { get; set; }
    public TimeSpan Period { get; set; }
    public TimeSpan TotalTime { get; set; }
    public TimeSpan Uptime { get; set; }
    public double UptimePercentage { get; set; }
}
```

## 모범 사례 (Best Practices)

### 1. Tags 명명 규칙

일관된 명명 규칙을 사용하여 가독성과 유지보수성을 높입니다.

```csharp
// ✅ 좋은 예: 소문자, 언더스코어, 일관성
var goodTags = new Dictionary<string, string>
{
    { "device_id", "GEN-01" },
    { "production_line", "Line-A" },
    { "sensor_type", "PT100" },
    { "asset_type", "gas_turbine" },
    { "environment", "production" }
};

// ❌ 나쁜 예: 불일치, 공백, 대소문자 혼용
var badTags = new Dictionary<string, string>
{
    { "DeviceID", "GEN-01" },           // 카멜케이스
    { "Production Line", "Line-A" },     // 공백
    { "sensorType", "PT100" },          // 카멜케이스
    { "AssetType", "gas_turbine" },     // 파스칼케이스
    { "ENV", "production" }             // 약어
};

// 권장 네이밍 패턴
// - 모두 소문자
// - 단어 구분은 언더스코어(_)
// - 약어보다는 전체 단어 사용
// - 계층적 구조는 점(.)으로 구분 (예: "location.building.floor")
```

### 2. 카디널리티 주의 (Cardinality)

Tags의 고유 값 조합 수(카디널리티)가 너무 높으면 성능 문제가 발생합니다.

```csharp
// ✅ 낮은 카디널리티 (권장)
var lowCardinalityTags = new Dictionary<string, string>
{
    { "location", "Plant-North" },      // 10개 공장
    { "shift", "Day-Shift" },           // 3개 교대조
    { "status", "running" },            // 5개 상태
    { "environment", "production" },    // 3개 환경
    { "criticality", "high" }           // 3개 수준
};

// ⚠️ 높은 카디널리티 (주의)
var highCardinalityTags = new Dictionary<string, string>
{
    { "batch_id", "BATCH-2024-001" },       // 수천 개 가능
    { "work_order", "WO-2024-0115" },       // 수천 개 가능
    { "operator_id", "EMP-12345" }          // 수백~수천 명
};

// ❌ 매우 높은 카디널리티 (피해야 함)
var veryHighCardinalityTags = new Dictionary<string, string>
{
    { "timestamp", "2024-01-15T10:30:45" }, // 무한대 - 절대 금지!
    { "uuid", "550e8400-e29b-41d4-a716-446655440000" }, // 무한대
    { "request_id", "req-123456789" }       // 무한대
};
```

**카디널리티 관리 팁:**

1. **타임스탬프는 Tags에 넣지 않기** - `MetricVm.Timestamp` 속성 사용
2. **UUID/GUID는 Tags에 넣지 않기** - 필요시 별도 속성 사용
3. **높은 카디널리티 값은 제한적으로 사용** - 배치 ID 등은 단기간만 보관
4. **인덱싱 전략 수립** - 자주 쿼리하는 Tag만 인덱싱

### 3. Tags 계층 구조

계층적 구조를 사용하면 다양한 레벨의 집계가 가능합니다.

```csharp
// 권장: 계층적 구조
var hierarchicalTags = new Dictionary<string, string>
{
    // 위치 계층
    { "site", "Site-A" },
    { "plant", "Plant-North" },
    { "building", "Building-3" },
    { "floor", "2" },
    { "zone", "Production-Area-1" },
    
    // 조직 계층
    { "company", "ACME-Corp" },
    { "division", "Manufacturing" },
    { "department", "Operations" },
    { "team", "Production-Team-1" },
    
    // 장비 계층
    { "asset_class", "rotating_equipment" },
    { "asset_type", "gas_turbine" },
    { "asset_id", "GEN-TURB-01" },
    { "component", "bearing" },
    { "sensor_id", "TEMP-01" }
};

// 집계 예시
// - Site 레벨: 전체 사이트 평균
// - Plant 레벨: 공장별 평균
// - Building 레벨: 건물별 평균
// - Floor 레벨: 층별 평균
```

### 4. 표준 Tag 세트 정의

조직 내에서 표준 Tag 세트를 정의하여 일관성을 유지합니다.

```csharp
// 표준 Tag 클래스
public static class StandardTags
{
    // 필수 Tags
    public const string AssetId = "asset_id";
    public const string Location = "location";
    public const string Environment = "environment";
    
    // 선택 Tags
    public const string Criticality = "criticality";
    public const string MaintenanceGroup = "maintenance_group";
    public const string ResponsibleEngineer = "responsible_engineer";
    
    // 값 제약
    public static readonly string[] ValidEnvironments = { "production", "staging", "test", "development" };
    public static readonly string[] ValidCriticalities = { "low", "medium", "high", "critical" };
}

// 사용
public class MetricBuilder
{
    public MetricVm BuildMetric(string label, double value)
    {
        var metric = new MetricVm
        {
            Label = label,
            Value = value
        };
        
        // 필수 Tags 검증
        if (!metric.Tags.ContainsKey(StandardTags.AssetId))
        {
            throw new InvalidOperationException("Asset ID is required");
        }
        
        // 값 검증
        if (metric.Tags.TryGetValue(StandardTags.Environment, out var env))
        {
            if (!StandardTags.ValidEnvironments.Contains(env))
            {
                throw new ArgumentException($"Invalid environment: {env}");
            }
        }
        
        return metric;
    }
}
```

### 5. Tags 문서화

프로젝트 문서에 사용하는 Tags를 명확히 정의합니다.

```markdown
# Metric Tags 표준

## 필수 Tags

| Tag 이름 | 설명 | 예시 값 | 카디널리티 |
|----------|------|---------|-----------|
| asset_id | 장비 고유 식별자 | GEN-TURB-01 | ~1000 |
| location | 물리적 위치 | Plant-North | ~50 |
| environment | 운영 환경 | production, test | 4 |

## 선택 Tags

| Tag 이름 | 설명 | 예시 값 | 카디널리티 |
|----------|------|---------|-----------|
| criticality | 중요도 | low, medium, high | 3 |
| maintenance_group | 유지보수 그룹 | MECH-01 | ~20 |
| shift | 교대조 | Day, Night | 3 |

## Tags 사용 규칙

1. 모든 Metric은 asset_id, location, environment를 포함해야 함
2. 카디널리티가 10,000을 초과하지 않도록 관리
3. Tag 값은 소문자와 하이픈(-) 사용 권장
4. 새로운 Tag 추가 시 아키텍처 팀 승인 필요
```

## 성능 최적화

### 인덱싱 전략

```csharp
// 자주 쿼리되는 Tag에 인덱스 생성
/*
CREATE INDEX idx_metrics_asset_id ON metrics ((Tags->>'asset_id'));
CREATE INDEX idx_metrics_location ON metrics ((Tags->>'location'));
CREATE INDEX idx_metrics_environment ON metrics ((Tags->>'environment'));
CREATE INDEX idx_metrics_status ON metrics (status);

-- 복합 인덱스
CREATE INDEX idx_metrics_location_env 
    ON metrics ((Tags->>'location'), (Tags->>'environment'));
*/
```

### 캐싱

```csharp
public class MetricTagCache
{
    private readonly Dictionary<string, HashSet<string>> _tagValuesCache = new();
    
    // Tag 값들을 캐싱하여 자동완성 등에 활용
    public void CacheTagValues(List<MetricVm> metrics)
    {
        _tagValuesCache.Clear();
        
        foreach (var metric in metrics)
        {
            foreach (var (key, value) in metric.Tags)
            {
                if (!_tagValuesCache.ContainsKey(key))
                {
                    _tagValuesCache[key] = new HashSet<string>();
                }
                _tagValuesCache[key].Add(value);
            }
        }
    }
    
    public IEnumerable<string> GetTagValues(string tagName)
    {
        return _tagValuesCache.GetValueOrDefault(tagName, new HashSet<string>());
    }
}
```

## 결론

SCADA 및 산업 모니터링 시스템에서 **Tags는 단순한 메타데이터가 아닌, 다차원 분석과 지능형 운영의 핵심 요소**입니다.

### Tags의 가치

- 📊 **다차원 분석**: 다양한 관점에서 데이터 집계 및 분석
- 🎯 **정확한 타겟팅**: 컨텍스트 기반 알람 라우팅
- 🔍 **강력한 필터링**: 복잡한 조건으로 메트릭 검색
- 📈 **트렌드 분석**: 시간에 따른 패턴 발견
- 🛠️ **유지보수 최적화**: 예측적 유지보수 지원

### 성공적인 Tags 활용을 위한 핵심

1. **일관된 명명 규칙** 준수
2. **카디널리티** 관리
3. **계층적 구조** 설계
4. **표준화된 Tag 세트** 정의
5. **적절한 문서화**

이러한 원칙을 따르면 확장 가능하고 유지보수하기 쉬운 산업 모니터링 시스템을 구축할 수 있습니다.

## 참고 자료

- [Prometheus Best Practices - Metric and Label Naming](https://prometheus.io/docs/practices/naming/)
- [Grafana Labels Best Practices](https://grafana.com/docs/grafana/latest/fundamentals/timeseries-dimensions/)
- [Azure Monitor Custom Metrics](https://learn.microsoft.com/en-us/azure/azure-monitor/essentials/metrics-custom-overview)
- [OpenTelemetry Semantic Conventions](https://opentelemetry.io/docs/specs/semconv/)
