using CommunityToolkit.Mvvm.Input;
using skiadraw.Models;
using skiadraw.States;

namespace skiadraw.ViewModels;

public partial class ToolbarViewModel : ViewModelBase
{
    private readonly DrawingState _state;
    public Tool SelectedTool => _state.SelectedTool;
    
    public ToolbarViewModel(DrawingState state)
    {
        _state = state;
        _state.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(DrawingState.SelectedTool))
                OnPropertyChanged(nameof(SelectedTool));
        };
    }
    
    [RelayCommand]
    private void SelectTool(Tool tool) => _state.SelectedTool = tool;
}