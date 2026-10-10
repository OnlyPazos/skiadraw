using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace skiadraw.Controls;

public partial class ToolButton : UserControl
{
    public static readonly StyledProperty<Geometry?> IconProperty =
        AvaloniaProperty.Register<ToolButton, Geometry?>(nameof(Icon));

    public static readonly StyledProperty<string?> ToolTipProperty =
        AvaloniaProperty.Register<ToolButton, string?>(nameof(ToolTip));

    public static readonly StyledProperty<ICommand?> CommandProperty =
        AvaloniaProperty.Register<ToolButton, ICommand?>(nameof(Command));

    public static readonly StyledProperty<object?> CommandParameterProperty =
        AvaloniaProperty.Register<ToolButton, object?>(nameof(CommandParameter));

    public static readonly StyledProperty<bool> IsSelectedProperty =
        AvaloniaProperty.Register<ToolButton, bool>(nameof(IsSelected));

    public ToolButton()
    {
        InitializeComponent();
    }

    public Geometry? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public string? ToolTip
    {
        get => GetValue(ToolTipProperty);
        set => SetValue(ToolTipProperty, value);
    }

    public ICommand? Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public bool IsSelected
    {
        get => GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }
}