using skiadraw.States;

namespace skiadraw.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly DrawingState _state = new();

    public ToolbarViewModel Toolbar { get; }
    public CanvasViewModel Canvas { get; }
    public DebugPanelViewModel Debug { get; }
    
    public MainViewModel()
    {
        Toolbar = new ToolbarViewModel(_state);
        Debug = new DebugPanelViewModel();
        Canvas = new CanvasViewModel(Debug, _state);
    }
}