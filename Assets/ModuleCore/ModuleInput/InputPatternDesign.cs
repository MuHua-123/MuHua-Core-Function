using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InputPatternDesign : ModuleInput<UnitMouseInput> {
    private bool isDownMouseLeft;
    private UnitMouseInput leftInputUnit = new PatternDesignLefts();
    private UnitMouseInput rightInputUnit;

    /// <summary> 视图相机模块 </summary>
    public ModuleCamera CameraView => ModuleCore.CameraView;

    public override Vector2 MousePosition => Input.mousePosition;
    public override UnitMouseInput Current => leftInputUnit;
    public override event Action<UnitMouseInput> OnChangeInput;
    public override void ChangeInput(UnitMouseInput input) => OnChangeInput?.Invoke(input);

    protected override void Awake() => ModuleCore.InputPatternDesign = this;

    public override void Binding(VisualElement element) {
        element.RegisterCallback<MouseDownEvent>(MouseDown);
        element.RegisterCallback<MouseMoveEvent>(MouseMove);
        element.RegisterCallback<MouseUpEvent>(MouseRelease);
        element.RegisterCallback<MouseOutEvent>(MouseRelease);
    }

    private DataMouseInput To(Vector2 localMousePosition, float scrollWheel) {
        DataMouseInput data = new DataMouseInput();
        data.ScrollWheel = scrollWheel;
        data.ViewPosition = CameraView.ScreenToViewPosition(localMousePosition);
        data.WorldPosition = CameraView.ScreenToWorldPosition(localMousePosition);
        data.ScreenPosition = localMousePosition;
        return data;
    }
    private void MouseDown(MouseDownEvent evt) {
        DataMouseInput data = To(evt.localMousePosition, 0);
        if (evt.button == 0) { leftInputUnit.MouseDown(data); isDownMouseLeft = true; }
        //if (evt.button == 1) { rightInputUnit.MouseDown(data); isDownMouseRight = true; }
    }
    private void MouseMove(MouseMoveEvent evt) {
        DataMouseInput data = To(evt.localMousePosition, 0);
        if (isDownMouseLeft) { leftInputUnit.MouseDrag(data); }
        //if (isDownMouseRight) { rightInputUnit.MouseDrag(data); }
        leftInputUnit.MouseMove(data);
        //rightInputUnit.MouseMove(data);
    }
    private void MouseRelease(MouseUpEvent evt) {
        DataMouseInput data = To(evt.localMousePosition, 0);
        leftInputUnit.MouseRelease(data); isDownMouseLeft = false;
        //rightInputUnit.MouseRelease(data); isDownMouseRight = false;
    }
    private void MouseRelease(MouseOutEvent evt) {
        DataMouseInput data = To(evt.localMousePosition, 0);
        leftInputUnit.MouseRelease(data); isDownMouseLeft = false;
        //rightInputUnit.MouseRelease(data); isDownMouseRight = false;
    }
}
