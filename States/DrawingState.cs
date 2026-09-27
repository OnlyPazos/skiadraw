using CommunityToolkit.Mvvm.ComponentModel;
using skiadraw.Models;

namespace skiadraw.States;

public partial class DrawingState : ObservableObject
{
    [ObservableProperty]
    private Tool _selectedTool = Tool.Selection;
    [ObservableProperty]
    private object _selectedObject = null;
}