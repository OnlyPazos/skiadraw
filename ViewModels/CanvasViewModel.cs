using System;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using skiadraw.Models;
using skiadraw.States;

namespace skiadraw.ViewModels;

public partial class CanvasViewModel : ViewModelBase
{
    private readonly DrawingState _state;
    private readonly DebugPanelViewModel _inspector;

    public ObservableCollection<ShapeModel> Shapes { get; } = [];

    public Tool SelectedTool => _state.SelectedTool;

    private Point _drawingStartPoint;
    private Point? _dragStartPoint;
    private Point? _shapeStartPosition;

    private Point? _panStart;
    private double _panStartX, _panStartY;

    private ShapeModel? _drawingShape;

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(HasSelection))]
    private ShapeModel? _selectedShape;

    public bool HasSelection => SelectedShape is not null;

    const double MIN_SIZE = 20;

    public CanvasViewModel(DebugPanelViewModel inspector, DrawingState state)
    {
        _inspector = inspector;

        _state = state;
        _state.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(DrawingState.SelectedTool))
                OnPropertyChanged(nameof(SelectedTool));
        };
    }

    // VIEWPORT
    public void HandleViewportPointerPressed(Grid viewport, TranslateTransform panTransform, PointerPressedEventArgs e)
    {
        var point = e.GetCurrentPoint(viewport);

        if (!point.Properties.IsMiddleButtonPressed) return;
        
        _panStart = point.Position;
        _panStartX = panTransform.X;
        _panStartY = panTransform.Y;
        e.Pointer.Capture(viewport);
        e.Handled = true;
    }

    public void HandleViewportPointerMoved(Grid viewport, TranslateTransform panTransform, PointerEventArgs e)
    {
        if (_panStart is not { } start) return;

        var current = e.GetCurrentPoint(viewport).Position;
        panTransform.X = _panStartX + (current.X - start.X);
        panTransform.Y = _panStartY + (current.Y - start.Y);
        e.Handled = true;
    }

    public void HandleViewportPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (_panStart is null) return;
        _panStart = null;
        e.Pointer.Capture(null);
        e.Handled = true;
    }

    public void HandleViewportPointerWheelChanged(Grid viewport,
        TranslateTransform panTransform,
        ScaleTransform zoomTransform,
        PointerWheelEventArgs e)
    {
        var oldZoom = zoomTransform.ScaleX;
        var delta = e.Delta.Y > 0 ? 1.1 : 1 / 1.1;
        var newZoom = Math.Clamp(oldZoom * delta, 0.1, 10);

        var pointerPos = e.GetPosition(viewport);
        var offsetX = pointerPos.X - panTransform.X;
        var offsetY = pointerPos.Y - panTransform.Y;

        panTransform.X -= offsetX * (newZoom / oldZoom - 1);
        panTransform.Y -= offsetY * (newZoom / oldZoom - 1);

        zoomTransform.ScaleX = newZoom;
        zoomTransform.ScaleY = newZoom;

        e.Handled = true;
    }

    // CANVAS
    public void HandlePointerPressed(Point point)
    {
        DeselectShape();

        _drawingStartPoint = point;

        _drawingShape = CreateShape(point);

        if (_drawingShape is not null)
            Shapes.Add(_drawingShape);
    }

    public void HandlePointerMoved(Point point)
    {
        if (_drawingShape is null) return;

        // Normaliza para permitir arrastrar en cualquier dirección
        _drawingShape.X = Math.Min(_drawingStartPoint.X, point.X);
        _drawingShape.Y = Math.Min(_drawingStartPoint.Y, point.Y);
        _drawingShape.Width = Math.Round(Math.Abs(point.X - _drawingStartPoint.X), 2);
        _drawingShape.Height = Math.Round(Math.Abs(point.Y - _drawingStartPoint.Y), 2);

        _drawingShape.IsVisible = !IsShapeTooSmall(_drawingShape);
    }

    public void HandlePointerReleased(Point point)
    {
        FinishShapeCreation();
    }


    // SHAPE INTERACTION
    public void SelectShape(ShapeModel shape)
    {
        if (SelectedShape == shape)
            return;

        SelectedShape?.IsSelected = false;
        SelectedShape = shape;
        SelectedShape.IsSelected = true;

        _inspector.ShowAll(SelectedShape, this);
    }

    public void DeselectShape()
    {
        SelectedShape?.IsSelected = false;
        SelectedShape = null;

        _inspector.ShowAll(SelectedShape, this);
    }

    public void DeleteShape(ShapeModel shape)
    {
        if (SelectedShape == shape) DeselectShape();
        if (_drawingShape == shape) _drawingShape = null;
        Shapes.Remove(shape);
    }

    public void MoveShape(Point point)
    {
        if (SelectedShape is null
            || _dragStartPoint is null
            || _shapeStartPosition is null
            || SelectedTool != Tool.Selection) return;

        var delta = point - (Point)_dragStartPoint;
        SelectedShape.X = Math.Round(((Point)_shapeStartPosition).X + delta.X, 2);
        SelectedShape.Y = Math.Round(((Point)_shapeStartPosition).Y + delta.Y, 2);
    }

    public void ResizeShape(ResizeHandle handle, Vector delta)
    {
        if (SelectedShape is null) return;

        var shape = SelectedShape;
        var newHeight = shape.Height - delta.Y;
        var newWidth = shape.Width - delta.X;

        switch (handle)
        {
            case ResizeHandle.BottomRight:
                shape.Width = Math.Max(MIN_SIZE, shape.Width + delta.X);
                shape.Height = Math.Max(MIN_SIZE, shape.Height + delta.Y);
                break;
            case ResizeHandle.TopRight:
                shape.Width = Math.Max(MIN_SIZE, shape.Width + delta.X);
                if (newHeight >= MIN_SIZE)
                {
                    shape.Y += delta.Y;
                    shape.Height = newHeight;
                }

                break;
            case ResizeHandle.BottomLeft:
                if (newWidth >= MIN_SIZE)
                {
                    shape.X += delta.X;
                    shape.Width = newWidth;
                }

                shape.Height = Math.Max(MIN_SIZE, shape.Height + delta.Y);

                break;
            case ResizeHandle.TopLeft:
                if (newWidth >= MIN_SIZE)
                {
                    shape.X += delta.X;
                    shape.Width = newWidth;
                }

                if (newHeight >= MIN_SIZE)
                {
                    shape.Y += delta.Y;
                    shape.Height = newHeight;
                }

                break;
            case ResizeHandle.Bottom:
                shape.Height = Math.Max(MIN_SIZE, shape.Height + delta.Y);
                break;
            case ResizeHandle.Left:
                if (newWidth >= MIN_SIZE)
                {
                    shape.X += delta.X;
                    shape.Width = newWidth;
                }

                break;
            case ResizeHandle.Right:
                shape.Width = Math.Max(MIN_SIZE, shape.Width + delta.X);
                break;
            case ResizeHandle.Top:
                if (newHeight >= MIN_SIZE)
                {
                    shape.Y += delta.Y;
                    shape.Height = newHeight;
                }

                break;
            default:
                return;
        }

        shape.X = Math.Round(shape.X, 2);
        shape.Y = Math.Round(shape.Y, 2);
        shape.Width = Math.Round(shape.Width, 2);
        shape.Height = Math.Round(shape.Height, 2);
    }

    public void HandleShapePressed(ShapeModel shape, Point? point = null)
    {
        switch (SelectedTool)
        {
            case Tool.Eraser:
                DeleteShape(shape);
                break;

            case Tool.Selection:
                SelectShape(shape);

                if (point is null || SelectedShape is null) return;

                _dragStartPoint = (Point)point;
                _shapeStartPosition = new Point(SelectedShape.X, SelectedShape.Y);
                break;

            case Tool.Hand:
            case Tool.Rectangle:
            case Tool.Diamond:
            case Tool.Ellipse:
            case Tool.Arrow:
            case Tool.Line:
            case Tool.Draw:
            case Tool.Text:
            default:
                SelectShape(shape);
                break;
        }
    }

    public void HandleShapeMoved(Point point)
    {
        MoveShape(point);

        _inspector.ShowAll(SelectedShape, this);
    }

    public void HandleShapeReleased()
    {
        _dragStartPoint = null;
        _shapeStartPosition = null;
    }


    // SHAPE CREATION
    private ShapeModel? CreateShape(Point point)
    {
        return SelectedTool switch
        {
            Tool.Rectangle => new RectangleShape
            {
                X = point.X,
                Y = point.Y,
            },

            Tool.Ellipse => new EllipseShape
            {
                X = point.X,
                Y = point.Y
            },

            _ => null
        };
    }

    private void FinishShapeCreation()
    {
        if (_drawingShape is null)
            return;

        if (IsShapeTooSmall(_drawingShape))
        {
            Shapes.Remove(_drawingShape);
        }
        else
        {
            SelectShape(_drawingShape);
        }

        _drawingShape = null;
    }

    private static bool IsShapeTooSmall(ShapeModel shape)
    {
        return shape.Width < MIN_SIZE || shape.Height < MIN_SIZE;
    }


    // SHAPE HOVER
    public void HandleShapePointerEntered(object? sender, PointerEventArgs e)
    {
        if (SelectedShape != null) return;

        if (e.Source is not Control { DataContext: ShapeModel shape } control) return;

        _inspector.ShowAll(shape, this);
    }

    public void HandleShapePointerExited(object? sender, PointerEventArgs e)
    {
        if (SelectedShape != null) return;

        _inspector.ShowAll(SelectedShape, this);
    }
}