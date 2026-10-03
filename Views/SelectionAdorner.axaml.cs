using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using skiadraw.Models;
using skiadraw.ViewModels;

namespace skiadraw.Views;

public partial class SelectionAdorner : UserControl
{
    private CanvasViewModel? Vm => DataContext as CanvasViewModel;

    public SelectionAdorner()
    {
        InitializeComponent();
    }

    private void BottomRight_DragDelta(object? sender, VectorEventArgs e)
    {
        Vm?.ResizeShape(ResizeHandle.BottomRight,  e.Vector);
    }

    private void BottomLeft_DragDelta(object? sender, VectorEventArgs e)
    {
        Vm?.ResizeShape(ResizeHandle.BottomLeft,  e.Vector);
    }

    private void TopRight_DragDelta(object? sender, VectorEventArgs e)
    {
        Vm?.ResizeShape(ResizeHandle.TopRight,  e.Vector);
    }

    private void TopLeft_DragDelta(object? sender, VectorEventArgs e)
    {
        Vm?.ResizeShape(ResizeHandle.TopLeft,  e.Vector);
    }
    
    private void Left_DragDelta(object? sender, VectorEventArgs e)
    {
        Vm?.ResizeShape(ResizeHandle.Left,  e.Vector);
    }
    
    private void Right_DragDelta(object? sender, VectorEventArgs e)
    {
        Vm?.ResizeShape(ResizeHandle.Right,  e.Vector);
    }
    
    private void Top_DragDelta(object? sender, VectorEventArgs e)
    {
        Vm?.ResizeShape(ResizeHandle.Top,  e.Vector);
    }
    
    private void Bottom_DragDelta(object? sender, VectorEventArgs e)
    {
        Vm?.ResizeShape(ResizeHandle.Bottom,  e.Vector);
    }
    
    private void Rotate_Started(object? sender, VectorEventArgs e) => Vm?.HandleStartRotation();
    private void Rotate_Completed(object? sender, VectorEventArgs e) => Vm?.HandleCompleteRotation();

    private void Rotate_Moved(object? sender, PointerEventArgs e)
    {
        var p = e.GetPosition(SelectionLayer);
        Vm?.HandleRotate(e, p);
    }
}