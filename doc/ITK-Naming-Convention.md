# ITK Naming Convention

## ViewModel 타입 Suffix

- ViewModel: `*Vm`
- CommandViewModel: `*CommandVm`
- FieldViewModel: `*FieldVm`

## 속성명 규칙

### CommandVm 타입
- Suffix: `Cvm`
- 타입: `*CommandVm` → 속성명: `*Cvm`

```csharp
public QuitApplicationCommandVm QuitApplicationCvm { get; }
public OpenHomePanelCommandVm OpenHomePanelCvm { get; }
```

### FieldVm 타입
- Suffix: `Fvm`
- 타입: `*FieldVm` → 속성명: `*Fvm`

```csharp
public UserNameFieldVm UserNameFvm { get; }
public PasswordFieldVm PasswordFvm { get; }
```

### 일반 Vm 타입
- Suffix: 없음 (타입의 Vm 제거)
- 타입: `*Vm` → 속성명: `*`

```csharp
public ThemeSettingsVm ThemeSettings { get; }
public AppMainPanelVm AppMainPanel { get; }
```

## XAML 바인딩 예시

```xaml
<!-- CommandVm: .Command로 내부 ICommand 접근 -->
<Button Command="{Binding QuitApplicationCvm.Command}"/>
<Button DataContext="{Binding OpenHomePanelCvm}" 
        Content="{Binding DisplayName}" 
        Command="{Binding Command}"/>

<!-- FieldVm: .Value로 내부 값 접근 -->
<TextBox Text="{Binding UserNameFvm.Value}"/>

<!-- 일반 Vm: 직접 접근 -->
<ContentControl Content="{Binding ThemeSettings}"/>
```

## 요약

| 타입 | 속성 Suffix | 예시 |
|------|------------|------|
| `*CommandVm` | `Cvm` | `SaveCvm`, `LoadCvm` |
| `*FieldVm` | `Fvm` | `NameFvm`, `EmailFvm` |
| `*Vm` | 없음 | `Settings`, `State` |
