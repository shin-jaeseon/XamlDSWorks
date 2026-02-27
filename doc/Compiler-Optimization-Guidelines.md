# Compiler Optimization

## 개요

C# 컴파일러와 JIT(Just-In-Time) 컴파일러는 코드를 최적화하여 성능을 향상시킵니다. 하지만 개발자가 특정 패턴과 키워드를 사용하면 **컴파일러가 더 공격적으로 최적화**할 수 있습니다.

### 이 문서의 목적

- ✅ 런타임 성능 향상
- ✅ 메모리 사용량 감소
- ✅ CPU 캐시 효율성 증가
- ✅ GC(Garbage Collection) 압력 감소

:::warning

⚠️ **조기 최적화는 만악의 근원(Premature optimization is the root of all evil)**  
먼저 **정확하고 읽기 쉬운 코드**를 작성하고, 성능 병목이 확인된 **핫 패스(Hot Path)**에만 최적화를 적용하세요.

:::

## sealed 키워드 활용

### 왜 sealed가 최적화에 도움이 되나?

`sealed` 키워드는 클래스나 메서드가 상속되지 않음을 컴파일러에게 알립니다. 이를 통해:

1. **Virtual dispatch 제거**: 가상 메서드 테이블(vtable) 조회 불필요
2. **Devirtualization**: 컴파일러가 직접 호출로 변환
3. **Inlining 가능**: 작은 메서드를 인라인화할 수 있음

### 예시

```csharp
// ❌ Not sealed - Virtual dispatch overhead
public class BaseClass
{
    public virtual void Process() { }
}

public class DerivedClass : BaseClass
{
    public override void Process() { }
}

// ✅ Sealed - Direct call, can be inlined
public sealed class OptimizedClass
{
    public void Process() { }
}
```

### Virtual vs Non-virtual 메서드

```csharp
public class MyClass
{
    // ❌ Virtual - Runtime dispatch
    public virtual void DoWork() { }
}

public sealed class MyClass
{
    // ✅ Sealed class - All methods are effectively non-virtual
    public void DoWork() { }
}

public class MyClass
{
    // ✅ Explicitly sealed method
    public sealed override void DoWork() { }
}
```

### 실제 성능 차이

```csharp
// Benchmark results (simplified)
public class VirtualBenchmark
{
    private BaseClass _instance = new DerivedClass();
    
    [Benchmark]
    public void VirtualCall()
    {
        _instance.Process(); // ~2.5ns per call
    }
}

public class SealedBenchmark
{
    private SealedClass _instance = new SealedClass();
    
    [Benchmark]
    public void DirectCall()
    {
        _instance.Process(); // ~0.5ns per call (5x faster!)
    }
}
```

### 권장 사항

```csharp
// ✅ Private nested classes는 항상 sealed
private sealed class Subscription<T> : IDisposable { }

// ✅ 상속이 의도되지 않은 클래스는 sealed
public sealed class Messenger : IMessenger { }

// ✅ ViewModel이 상속되지 않는다면 sealed
public sealed class MainViewModel : ViewModelBase { }

// ⚠️ 라이브러리 public API는 신중하게 - 확장성 고려 필요
public class LibraryClass // sealed 하지 않음 - 사용자가 확장할 수 있음
{
}
```

---

## readonly 필드 사용

### 최적화 효과

1. **Thread-safety**: 멀티스레드 환경에서 안전
2. **Compiler optimization**: 필드가 변경되지 않음을 보장
3. **JIT optimization**: 값이 변경되지 않으므로 최적화 가능

### readonly 예시

```csharp
// ❌ Mutable field
private Dictionary<Type, List<Delegate>> _subscribers = new();

// ✅ Readonly field
private readonly Dictionary<Type, List<Delegate>> _subscribers = new();

// ✅ Readonly struct field (even better)
private readonly struct Point
{
    public readonly int X;
    public readonly int Y;
    
    public Point(int x, int y) => (X, Y) = (x, y);
}
```

### readonly vs const

```csharp
// const - Compile-time constant (inlined everywhere)
public const int MaxRetries = 3;

// readonly - Runtime constant (allocated once)
public readonly DateTime CreatedAt = DateTime.UtcNow;

// static readonly - Shared across all instances
public static readonly IMessenger DefaultMessenger = new Messenger();
```

### Readonly struct (C# 7.2+)

```csharp
// ✅ Readonly struct - Immutable, defensive copy avoided
public readonly struct CultureInfo
{
    public readonly string Name { get; }
    public readonly string DisplayName { get; }
    
    public CultureInfo(string name, string displayName)
    {
        Name = name;
        DisplayName = displayName;
    }
}

// ❌ Regular struct - Defensive copies on readonly access
public struct MutablePoint
{
    public int X { get; set; }
    public int Y { get; set; }
}

readonly MutablePoint point = new MutablePoint { X = 1, Y = 2 };
var x = point.X; // Defensive copy created!
```

---

## struct vs class 선택

### 언제 struct를 사용할까?

struct를 사용하는 경우:

- ✅ 크기가 작음 (일반적으로 16 bytes 이하)
- ✅ 불변(immutable)
- ✅ 자주 생성되고 짧은 수명
- ✅ 값 의미론이 적합

class를 사용하는 경우:

- ✅ 크기가 큼 (>16 bytes)
- ✅ 가변(mutable)
- ✅ 참조 의미론이 필요
- ✅ 상속이 필요

### 성능 비교

```csharp
// ✅ Good use of struct - Small, immutable
public readonly struct Point
{
    public int X { get; }
    public int Y { get; }
    
    public Point(int x, int y) => (X, Y) = (x, y);
}

// ❌ Bad use of struct - Too large, causes stack overflow risk
public struct LargeStruct
{
    public double[] Data; // 8 bytes (reference)
    public Matrix4x4 Transform; // 64 bytes
    public string Name; // 8 bytes (reference)
    // Total: 80+ bytes - Too large for stack!
}

// ✅ Use class instead
public class LargeData
{
    public double[] Data { get; set; }
    public Matrix4x4 Transform { get; set; }
    public string Name { get; set; }
}
```

### ref struct (C# 7.2+)

Stack-only 타입 - 힙 할당 불가:

```csharp
// ✅ ref struct - Can only live on stack
public ref struct SpanBasedParser
{
    private readonly Span<char> _buffer;
    
    public SpanBasedParser(Span<char> buffer)
    {
        _buffer = buffer;
    }
    
    public void Parse()
    {
        // Zero allocation parsing
    }
}

// ❌ Cannot do this - ref struct cannot be heap-allocated
// List<SpanBasedParser> list = new(); // Compiler error!
```

---

## ref, in, out 매개변수

### ref - 참조로 전달

```csharp
// ❌ Copy on pass (large struct)
public void ProcessLargeStruct(LargeStruct data)
{
    // 'data' is a copy - expensive!
}

// ✅ Pass by reference - No copy
public void ProcessLargeStruct(ref LargeStruct data)
{
    // 'data' is a reference - fast!
}
```

### in - 읽기 전용 참조 (C# 7.2+)

```csharp
// ✅ Pass large struct by readonly reference
public int Calculate(in Matrix4x4 matrix)
{
    // Cannot modify 'matrix'
    // No defensive copy for readonly members
    return matrix.M11 + matrix.M22;
}

// Usage
Matrix4x4 transform = Matrix4x4.Identity;
int result = Calculate(in transform);
```

### ref return (C# 7.0+)

```csharp
// ✅ Return reference to avoid copy
public ref int GetElement(int index)
{
    return ref _array[index];
}

// Usage - Can modify in-place
ref int element = ref GetElement(5);
element = 42; // Modifies array directly
```

---

## Span&lt;T&gt;와 Memory&lt;T&gt;

### Span&lt;T&gt; - Stack-based slice

```csharp
// ❌ Creates substring - Allocates new string
string text = "Hello, World!";
string hello = text.Substring(0, 5); // Allocation!

// ✅ Span<char> - Zero allocation
ReadOnlySpan<char> span = text.AsSpan();
ReadOnlySpan<char> hello = span.Slice(0, 5); // No allocation!
```

### 실제 사용 예시

```csharp
// String parsing without allocation
public static bool TryParseLanguageCode(ReadOnlySpan<char> input, out string language, out string region)
{
    int dashIndex = input.IndexOf('-');
    if (dashIndex == -1)
    {
        language = string.Empty;
        region = string.Empty;
        return false;
    }
    
    // No string allocation until we actually need it
    language = input.Slice(0, dashIndex).ToString();
    region = input.Slice(dashIndex + 1).ToString();
    return true;
}

// Usage
if (TryParseLanguageCode("ko-KR".AsSpan(), out var lang, out var reg))
{
    Console.WriteLine($"{lang}, {reg}");
}
```

### Memory&lt;T&gt; - Heap-based slice

```csharp
// Span<T> cannot be stored in fields (ref struct limitation)
// Use Memory<T> for async scenarios

public class BufferedReader
{
    private readonly Memory<byte> _buffer;
    
    public BufferedReader(Memory<byte> buffer)
    {
        _buffer = buffer;
    }
    
    public async Task<int> ReadAsync()
    {
        // Can use Memory<T> in async methods
        return await ReadFromStreamAsync(_buffer);
    }
}
```

---

## ValueTask vs Task

### 언제 ValueTask를 사용할까?

```csharp
// ❌ Task - Always allocates
public async Task<int> GetCachedValueAsync()
{
    if (_cache.TryGetValue(key, out var value))
        return value; // Still allocates Task!
    
    return await FetchFromDatabaseAsync();
}

// ✅ ValueTask - No allocation for synchronous path
public async ValueTask<int> GetCachedValueAsync()
{
    if (_cache.TryGetValue(key, out var value))
        return value; // No allocation!
    
    return await FetchFromDatabaseAsync();
}
```

### ValueTask 성능 비교

```csharp
// Benchmark: 1,000,000 cache hits

// Task version: ~40 MB allocated
public async Task<int> TaskVersion()
{
    return await GetCachedValueAsync(); // Each call allocates Task
}

// ValueTask version: ~0 MB allocated
public async ValueTask<int> ValueTaskVersion()
{
    return await GetCachedValueAsync(); // No allocation for cache hits!
}
```

### 주의사항

```csharp
// ⚠️ Don't do this with ValueTask!
ValueTask<int> task = GetValueAsync();
await task;
await task; // ❌ Don't await twice!

// ✅ Convert to Task if you need to await multiple times
Task<int> task = GetValueAsync().AsTask();
await task;
await task; // ✅ OK
```

---

## 문자열 최적화

### String interpolation

```csharp
// ❌ String interpolation - Allocates
string message = $"Hello, {name}!";

// ✅ String.Concat - Slightly better
string message = string.Concat("Hello, ", name, "!");

// ✅ StringBuilder - Best for loops
var sb = new StringBuilder();
for (int i = 0; i < 1000; i++)
{
    sb.Append($"Item {i}");
}
string result = sb.ToString();
```

### DefaultInterpolatedStringHandler (C# 10+)

```csharp
// Modern C# automatically uses DefaultInterpolatedStringHandler
string message = $"Culture: {culture.Name}, Display: {culture.DisplayName}";

// Behind the scenes (optimized by compiler):
var handler = new DefaultInterpolatedStringHandler(/* ... */);
handler.AppendLiteral("Culture: ");
handler.AppendFormatted(culture.Name);
handler.AppendLiteral(", Display: ");
handler.AppendFormatted(culture.DisplayName);
string message = handler.ToStringAndClear();
```

### String comparison

```csharp
// ❌ Case-insensitive comparison - Slow
if (name.ToLower() == "admin") { }

// ✅ Ordinal comparison - Fast
if (name.Equals("admin", StringComparison.OrdinalIgnoreCase)) { }

// ✅ Span-based comparison - Fastest
if (name.AsSpan().Equals("admin", StringComparison.OrdinalIgnoreCase)) { }
```

---

## LINQ 최적化

### 피해야 할 패턴

```csharp
// ❌ Multiple enumerations
var items = GetItems().Where(x => x.IsActive);
var count = items.Count(); // Enumeration 1
var first = items.First();  // Enumeration 2 - Queries DB again!

// ✅ Materialize once
var items = GetItems().Where(x => x.IsActive).ToList();
var count = items.Count; // Property access
var first = items[0];    // Index access
```

### Any() vs Count()

```csharp
// ❌ Count() - Enumerates entire sequence
if (collection.Count() > 0) { }

// ✅ Any() - Stops at first element
if (collection.Any()) { }

// ✅ Count property for collections
if (list.Count > 0) { }
```

### First() vs FirstOrDefault()

```csharp
// ❌ FirstOrDefault() + null check - Two operations
var item = items.FirstOrDefault(x => x.Id == id);
if (item != null) { }

// ✅ TryGetFirst (C# 12+) - Coming soon
if (items.TryGetFirst(x => x.Id == id, out var item)) { }

// ✅ Or use Any + First
if (items.Any(x => x.Id == id))
{
    var item = items.First(x => x.Id == id);
}
```

### Avoid closure allocations

```csharp
// ❌ Closure allocation in loop
for (int i = 0; i < 1000; i++)
{
    var item = items.FirstOrDefault(x => x.Id == i); // Allocates closure!
}

// ✅ Use local variable
for (int i = 0; i < 1000; i++)
{
    int localId = i;
    var item = items.FirstOrDefault(x => x.Id == localId);
}

// ✅ Even better - Use loop
for (int i = 0; i < 1000; i++)
{
    T? item = null;
    foreach (var x in items)
    {
        if (x.Id == i)
        {
            item = x;
            break;
        }
    }
}
```

---

## Boxing/Unboxing 방지

### Boxing이란?

값 타입(struct)을 참조 타입(object)으로 변환하면 힙 할당이 발생합니다.

```csharp
// ❌ Boxing - Allocates on heap
int value = 42;
object boxed = value; // Boxing allocation!

// ❌ Implicit boxing in collections
ArrayList list = new ArrayList();
list.Add(42); // Boxing!
list.Add(43); // Boxing!

// ✅ Generic collections - No boxing
List<int> list = new List<int>();
list.Add(42); // No boxing!
list.Add(43); // No boxing!
```

### ToString() on value types

```csharp
// ❌ Implicit boxing
int number = 42;
Console.WriteLine($"Number: {number}"); // Boxing + allocation

// ✅ Explicit ToString() - Still allocates string, but no boxing
Console.WriteLine($"Number: {number.ToString()}");

// ✅ Interpolated string handler - Optimized (C# 10+)
Console.WriteLine($"Number: {number}"); // Compiler optimizes this now!
```

### Generic constraints

```csharp
// ❌ No constraint - Boxing for value types
public void Process<T>(T value)
{
    if (value.Equals(default)) // Boxing if T is value type!
    {
    }
}

// ✅ struct constraint - No boxing
public void Process<T>(T value) where T : struct
{
    if (value.Equals(default)) // No boxing!
    {
    }
}

// ✅ Generic comparer - No boxing
public void Process<T>(T value, IEqualityComparer<T> comparer)
{
    if (comparer.Equals(value, default)) // No boxing!
    {
    }
}
```

---

## Modern C# 기능 활용

### Collection expressions (C# 12+)

```csharp
// ❌ Old way
var list = new List<string> { "A", "B", "C" };

// ✅ Collection expression
List<string> list = ["A", "B", "C"];

// ✅ Spread operator
List<string> combined = [..list1, ..list2];
```

### Primary constructors (C# 12+)

```csharp
// ❌ Old way
public class Messenger : IMessenger
{
    private readonly ILogger _logger;
    
    public Messenger(ILogger logger)
    {
        _logger = logger;
    }
}

// ✅ Primary constructor
public class Messenger(ILogger logger) : IMessenger
{
    public void Send<TMessage>(TMessage message)
    {
        logger.LogInformation($"Sending {message}");
    }
}
```

### Required members (C# 11+)

```csharp
// ✅ Required properties
public class Configuration
{
    public required string ConnectionString { get; init; }
    public required int MaxRetries { get; init; }
}

// Usage - Compiler enforces initialization
var config = new Configuration
{
    ConnectionString = "...",
    MaxRetries = 3
}; // ✅ All required members set
```

### Raw string literals (C# 11+)

```csharp
// ❌ Escaped strings
string json = "{\n  \"name\": \"value\"\n}";

// ✅ Raw string literals
string json = """
    {
      "name": "value"
    }
    """;
```

---

## 컬렉션 최적화

### Capacity 지정

```csharp
// ❌ Default capacity - Multiple reallocations
var list = new List<int>();
for (int i = 0; i < 1000; i++)
{
    list.Add(i); // May reallocate multiple times
}

// ✅ Specify capacity - Single allocation
var list = new List<int>(1000);
for (int i = 0; i < 1000; i++)
{
    list.Add(i); // No reallocation
}
```

### HashSet vs List for lookups

```csharp
// ❌ List.Contains - O(n)
var list = new List<int> { 1, 2, 3, /* ... 1000 items */ };
bool exists = list.Contains(999); // Scans entire list

// ✅ HashSet.Contains - O(1)
var set = new HashSet<int> { 1, 2, 3, /* ... 1000 items */ };
bool exists = set.Contains(999); // Instant lookup
```

### Dictionary initialization

```csharp
// ❌ Multiple allocations
var dict = new Dictionary<string, int>();
dict.Add("one", 1);
dict.Add("two", 2);
dict.Add("three", 3);

// ✅ Collection initializer - Single allocation
var dict = new Dictionary<string, int>
{
    ["one"] = 1,
    ["two"] = 2,
    ["three"] = 3
};

// ✅ With capacity
var dict = new Dictionary<string, int>(100);
```

### Array vs List

```csharp
// ✅ Array - Fixed size, fastest access
int[] array = new int[1000];

// ✅ List - Dynamic size, slightly slower
List<int> list = new List<int>(1000);

// ✅ Span - Zero-copy slice
Span<int> span = array.AsSpan();
```

---

## 실전 예제: 최적화 전/후

### 예제 1: Message dispatcher

```csharp
// ❌ Before optimization
public class Messenger
{
    private Dictionary<Type, List<Delegate>> _subscribers = new();
    
    public void Send<TMessage>(TMessage message)
    {
        var type = typeof(TMessage);
        if (_subscribers.ContainsKey(type))
        {
            foreach (var handler in _subscribers[type])
            {
                ((Action<TMessage>)handler)(message); // Repeated cast
            }
        }
    }
}

// ✅ After optimization
public sealed class Messenger
{
    private readonly Dictionary<Type, List<Delegate>> _subscribers = new();
    private readonly object _lock = new();
    
    public void Send<TMessage>(TMessage message)
    {
        List<Delegate>? handlers;
        
        lock (_lock)
        {
            var messageType = typeof(TMessage);
            if (!_subscribers.TryGetValue(messageType, out handlers))
                return;
            
            handlers = handlers.ToList(); // Copy to avoid lock during iteration
        }
        
        foreach (var handler in handlers.Cast<Action<TMessage>>()) // Single cast
        {
            try
            {
                handler(message);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
```

### 예제 2: String processing

```csharp
// ❌ Before optimization - Multiple allocations
public string FormatCultureName(string cultureName)
{
    var parts = cultureName.Split('-');
    var language = parts[0].ToLower();
    var region = parts[1].ToUpper();
    return $"{language}-{region}";
}

// ✅ After optimization - Zero allocations
public string FormatCultureName(ReadOnlySpan<char> cultureName)
{
    int dashIndex = cultureName.IndexOf('-');
    if (dashIndex == -1)
        return cultureName.ToString();
    
    var language = cultureName.Slice(0, dashIndex);
    var region = cultureName.Slice(dashIndex + 1);
    
    return string.Create(cultureName.Length, (language, region), (span, state) =>
    {
        state.language.ToLowerInvariant(span);
        span[dashIndex] = '-';
        state.region.ToUpperInvariant(span.Slice(dashIndex + 1));
    });
}
```

---

## 성능 측정 도구

### BenchmarkDotNet

```bash
dotnet add package BenchmarkDotNet
```

```csharp
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

[MemoryDiagnoser]
public class StringBenchmarks
{
    [Benchmark]
    public string StringInterpolation()
    {
        return $"Hello, World!";
    }
    
    [Benchmark]
    public string StringConcat()
    {
        return string.Concat("Hello, ", "World!");
    }
}

// Run benchmarks
BenchmarkRunner.Run<StringBenchmarks>();
```

### Performance Profiler (Visual Studio)

1. **CPU Usage**: 핫 패스 식별
2. **Memory Usage**: 할당 패턴 분석
3. **.NET Object Allocation**: GC 압력 측정

---

## 체크리스트

### 코드 리뷰 시 확인 사항

- [ ] `sealed` 키워드를 적절히 사용했는가?
- [ ] 필드가 `readonly`로 선언되어 있는가?
- [ ] 큰 struct를 값으로 전달하는가? (`ref`/`in` 사용)
- [ ] 문자열 처리에 `Span<T>` 사용을 고려했는가?
- [ ] 자주 호출되는 async 메서드에 `ValueTask` 사용을 고려했는가?
- [ ] LINQ 쿼리가 여러 번 열거되는가?
- [ ] Boxing이 발생하는 곳은 없는가?
- [ ] 컬렉션에 capacity를 지정했는가?
- [ ] `Any()` 대신 `Count()` > 0을 사용하는가?

---

## 결론

컴파일러 최적화는 **측정 가능한 성능 개선**을 가져올 수 있습니다. 하지만:

1. **먼저 정확성과 가독성을 확보하세요**
2. **성능 병목을 측정하세요** (BenchmarkDotNet, Profiler)
3. **핫 패스에만 최적화를 적용하세요**
4. **최적화 후 다시 측정하세요**

> "측정하지 않으면 개선할 수 없다" - Peter Drucker

Happy optimizing! 🚀
