using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using skiadraw.Models;
using skiadraw.States;

namespace skiadraw.ViewModels;

public class DebugPanelViewModel
{
    public ObservableCollection<DebugItem> Info { get; } = [];

    public void ShowAll(ShapeModel? shape, CanvasViewModel? vm)
    {
        Info.Clear();
        
        if (vm != null)
        {
            AddSeparator("Canvas");
            ShowCanvasViewModel(vm, false);
        }

        if (shape != null)
        {
            AddSeparator("Shape");
            ShowShape(shape, false);
        }
    }
    
    public void ShowShape(ShapeModel shape, bool clear = true)
    {
        if (clear) Info.Clear();

        Info.Add(new DebugInfo
        {
            Name = "Name",
            Type = "string",
            GetValue = () => shape.GetType().Name
        });

        Info.Add(new DebugInfo
        {
            Name = "X",
            Type = "double",
            GetValue = () => shape.X
        });

        Info.Add(new DebugInfo
        {
            Name = "Y",
            Type = "double",
            GetValue = () => shape.Y
        });

        Info.Add(new DebugInfo
        {
            Name = "Width",
            Type = "double",
            GetValue = () => shape.Width
        });

        Info.Add(new DebugInfo
        {
            Name = "Height",
            Type = "double",
            GetValue = () => shape.Height
        });

        Info.Add(new DebugInfo
        {
            Name = "Fill",
            Type = "string",
            GetValue = () => shape.Fill
        });

        Info.Add(new DebugInfo
        {
            Name = "Stroke",
            Type = "string",
            GetValue = () => shape.Stroke
        });

        Info.Add(new DebugInfo
        {
            Name = "Stroke Dash",
            Type = "string",
            GetValue = () => shape.StrokeDash
        });

        Info.Add(new DebugInfo
        {
            Name = "Stroke Thickness",
            Type = "double",
            GetValue = () => shape.StrokeThickness
        });

        Info.Add(new DebugInfo
        {
            Name = "Selected",
            Type = "bool",
            GetValue = () => shape.IsSelected
        });

        Info.Add(new DebugInfo
        {
            Name = "Visible",
            Type = "bool",
            GetValue = () => shape.IsVisible
        });

        if (shape is RectangleShape rectangleShape) ShowRectangle(rectangleShape);
    }

    public void ShowRectangle(RectangleShape shape)
    {
        Info.Add(new DebugInfo
        {
            Name = "Radius",
            Type = "double",
            GetValue = () => shape.Radius
        });
    }
    
    public void ShowCanvasViewModel(CanvasViewModel vm, bool clear = true)
    {
        if (clear) Info.Clear();

        Info.Add(new DebugInfo
        {
            Name = "Name",
            Type = "string",
            GetValue = () => vm.GetType().Name
        });

        Info.Add(new DebugInfo
        {
            Name = "SelectedTool",
            Type = "Tool",
            GetValue = () => vm.SelectedTool
        });

        Info.Add(new DebugInfo
        {
            Name = "SelectedShape",
            Type = "ShapeModel",
            GetValue = () => vm.SelectedShape
        });

        Info.Add(new DebugInfo
        {
            Name = "HasSelection",
            Type = vm.HasSelection.GetType().Name,
            GetValue = () => vm.HasSelection
        });

        Info.Add(new DebugInfo
        {
            Name = "Shapes",
            Type = vm.Shapes.GetType().Name,
            GetValue = () => vm.Shapes.Where(s => s.IsVisible).ToList().Count
        });
    }
    
    private void AddSeparator(string title)
    {
        Info.Add(new DebugSeparator
        {
            Title = title
        });
    }
    
    public void Clear()
    {
        Info.Clear();
    }
}