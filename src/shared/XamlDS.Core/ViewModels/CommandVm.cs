using System.Windows.Input;
using XamlDS.Input;

namespace XamlDS.ViewModels;

/// <summary>
/// Command ViewModel base class
/// </summary>
public abstract class CommandVm : ViewModelBase
{
    protected CommandVm(string name)
    {
        Name = name;
        _command = new RelayCommand(Execute, CanExecute);
    }

    /// <summary>
    /// The programmatic identifier for this object.
    /// </summary>
    /// <remarks>
    /// It's used as an identifier in logs and for debugging purposes.
    /// </remarks>
    public string Name { get; }

    private string _displayName = string.Empty;

    /// <summary>
    /// Gets or sets the user-friendly name displayed in the UI.
    /// </summary>
    /// <remarks>
    /// This property is used for displaying the name of the functionality or device
    /// to the end user in the interface, such as in usage history charts or component labels.
    /// </remarks>
    public virtual string DisplayName
    {
        get => _displayName;
        set => SetProperty(ref _displayName, value);
    }

    private string _description = string.Empty;

    /// <summary>
    /// Detailed explanation of this object's purpose or functionality.
    /// </summary>
    /// <remarks>
    /// This property provides a more detailed description of the object's role or function.
    /// For Command ViewModels (Cvm), this is typically used as tooltip text for buttons.
    /// </remarks>
    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    public RelayCommand _command { get; }

    public ICommand Command => _command;

    public abstract void Execute(object? parameter);

    public virtual bool CanExecute(object? parameter)
    {
        return true;
    }

    public void RaiseCanExecuteChanged()
    {
        _command.RaiseCanExecuteChanged();
    }
}
