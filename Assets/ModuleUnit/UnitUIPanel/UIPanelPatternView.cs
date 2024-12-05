using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using MuHua;
//using Palette;

/// <summary>
/// 图案编辑视图
/// </summary>
public class UIPanelPatternView : UnitUIPanel {
    //public PaletteManager paletteManager;
    private UIVectorInput inputPosition;
    private UIVectorInput inputScale;
    private UIVectorInput inputRotate;

    public override VisualElement Element => ModuleUIPage.Q<VisualElement>("PatternView");
    public VisualElement Rendering => Element.Q<VisualElement>("Rendering");
    public VisualElement Position => Element.Q<VisualElement>("Position");
    public VisualElement Scale => Element.Q<VisualElement>("Scale");
    public VisualElement Rotate => Element.Q<VisualElement>("Rotate");
    public VisualElement Colour => Element.Q<VisualElement>("Colour");
    public VisualElement Palette => Colour.Q<VisualElement>("Palette");

    public Button Button1 => Element.Q<Button>("Button1");//X镜像
    public Button Button2 => Element.Q<Button>("Button2");//重置
    public Button Button3 => Element.Q<Button>("Button3");//Y镜像
    public Button Button4 => Element.Q<Button>("Button4");//保存

    /// <summary> 视图相机模块 </summary>
    public ModuleCamera CameraView => ModuleCore.CameraView;
    /// <summary> 图案设计库 </summary>
    public ModuleAssets<DataPatternDesign> AssetsPatternDesign => ModuleCore.AssetsPatternDesign;
    /// <summary> 图案设计 输入模块 </summary>
    public ModuleInput<UnitMouseInput> InputPatternDesign => ModuleCore.InputPatternDesign;
    /// <summary> 图案设计 DataPatternDesign 数据处理器 </summary>
    public ModuleHandle<DataPatternDesign> HandlePatternDesign => ModuleCore.HandlePatternDesign;
    /// <summary> 图案设计项目 DataPatternDesignItem 数据处理器 </summary>
    public ModuleHandle<DataPatternDesignItem> HandlePatternDesignItem => ModuleCore.HandlePatternDesignItem;

    protected override void Awake() {
        Element.generateVisualContent += Element_GenerateVisualContent;

        inputPosition = new UIVectorInput(Position);
        inputPosition.MUFloatField1.RegisterCallback<ChangeEvent<float>>(SetPositionX);
        inputPosition.MUFloatField2.RegisterCallback<ChangeEvent<float>>(SetPositionY);

        inputScale = new UIVectorInput(Scale);
        inputScale.MUFloatField1.RegisterCallback<ChangeEvent<float>>(SetScaleX);
        inputScale.MUFloatField2.RegisterCallback<ChangeEvent<float>>(SetScaleY);

        inputRotate = new UIVectorInput(Rotate);
        inputRotate.MUFloatField1.RegisterCallback<ChangeEvent<float>>(SetRotate);

        Palette.RegisterCallback<MouseDownEvent>(SetColor);

        Button1.clicked += Button1_clicked;
        Button2.clicked += Button2_clicked;
        Button3.clicked += Button3_clicked;
        Button4.clicked += Button4_clicked;
    }

    protected void Start() {
        InputPatternDesign.Binding(Rendering);
        HandlePatternDesignItem.OnChange += HandlePatternDesignItem_OnChange;
    }
    protected void OnDestroy() {
        HandlePatternDesignItem.OnChange -= HandlePatternDesignItem_OnChange;
    }

    private void HandlePatternDesignItem_OnChange(DataPatternDesignItem obj) {
        if (obj == null) { return; }
        inputPosition.MUFloatField1.SetValueWithoutNotify(obj.position.x * 100f);
        inputPosition.MUFloatField2.SetValueWithoutNotify(obj.position.y * 100f);
        inputScale.MUFloatField1.SetValueWithoutNotify(obj.scale.x * 100f);
        inputScale.MUFloatField2.SetValueWithoutNotify(obj.scale.y * 100f);
        inputRotate.MUFloatField1.SetValueWithoutNotify(obj.rotate);
        Palette.style.backgroundColor = obj.color;
    }

    #region 设置属性
    private void UpdateDesignItem(Action action) {
        if (!HandlePatternDesignItem.IsValid) { return; }
        action?.Invoke();
        HandlePatternDesignItem.Change();
    }
    private void SetPositionX(ChangeEvent<float> evt) {
        UpdateDesignItem(() => { HandlePatternDesignItem.Current.position.x = evt.newValue / 100f; });
    }
    private void SetPositionY(ChangeEvent<float> evt) {
        UpdateDesignItem(() => { HandlePatternDesignItem.Current.position.y = evt.newValue / 100f; });
    }
    private void SetScaleX(ChangeEvent<float> evt) {
        UpdateDesignItem(() => { HandlePatternDesignItem.Current.scale.x = evt.newValue / 100f; });
    }
    private void SetScaleY(ChangeEvent<float> evt) {
        UpdateDesignItem(() => { HandlePatternDesignItem.Current.scale.y = evt.newValue / 100f; });
    }
    private void SetRotate(ChangeEvent<float> evt) {
        UpdateDesignItem(() => { HandlePatternDesignItem.Current.rotate = evt.newValue; });
    }
    private void SetColor(MouseDownEvent evt) {
        if (!HandlePatternDesignItem.IsValid) { return; }
        //paletteManager.Open(HandlePatternDesignItem.Current.color, (color) => {
        //    HandlePatternDesignItem.Current.color = color;
        //    HandlePatternDesignItem.Change();
        //});
    }
    private void Button1_clicked() {
        UpdateDesignItem(() => {
            bool isReverseX = HandlePatternDesignItem.Current.isReverseX;
            HandlePatternDesignItem.Current.isReverseX = !isReverseX;
        });
    }
    private void Button2_clicked() {
        UpdateDesignItem(() => {
            HandlePatternDesignItem.Current.position = new Vector2(0, 0);
            HandlePatternDesignItem.Current.scale = new Vector2(0.5f, 0.5f);
            HandlePatternDesignItem.Current.rotate = 0;
            HandlePatternDesignItem.Current.color = new Color(1, 1, 1, 1);
            HandlePatternDesignItem.Current.isReverseX = false;
            HandlePatternDesignItem.Current.isReverseY = false;
        });
    }
    private void Button3_clicked() {
        UpdateDesignItem(() => {
            bool isReverseY = HandlePatternDesignItem.Current.isReverseY;
            HandlePatternDesignItem.Current.isReverseY = !isReverseY;
        });
    }
    private void Button4_clicked() {
        DataPatternDesign patternDesign = new DataPatternDesign();
        AssetsPatternDesign.Add(patternDesign);
        HandlePatternDesign.Change(patternDesign);
    }
    #endregion

    #region 更新视图
    private void Element_GenerateVisualContent(MeshGenerationContext context) {
        StartCoroutine(UpdateRenderTexture());
    }
    private IEnumerator UpdateRenderTexture() {
        yield return null;
        //int width = (int)Rendering.resolvedStyle.width;
        //int height = (int)Rendering.resolvedStyle.height;
        CameraView.UpdateRenderTexture(512, 512);
        Background background = Background.FromRenderTexture(CameraView.RenderTexture);
        StyleBackground style = new StyleBackground(background);
        Rendering.style.backgroundImage = style;
    }
    #endregion

    public class UIVectorInput {
        public readonly VisualElement element;
        public MUFloatField MUFloatField1 => element.Q<MUFloatField>("MUFloatField1");
        public MUFloatField MUFloatField2 => element.Q<MUFloatField>("MUFloatField2");
        public UIVectorInput(VisualElement element) => this.element = element;
    }
}
