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
public class UIPatternViewPanel : ModuleUIPanel {
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

    public Button Lock => Element.Q<Button>("Lock");//锁定
    public Button ColorCard => Element.Q<Button>("ColorCard");//色卡

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
    /// <summary> 色卡窗口 </summary>
    public ModuleUIWindow<DataPalette> ColorCardWindow => ModuleCore.ColorCardWindow;
    /// <summary> 图案设计 DataPatternDesign 数据处理器 </summary>
    public ModuleHandle<DataPatternDesign> HandlePatternDesign => ModuleCore.HandlePatternDesign;
    /// <summary> 图案设计项目 DataPatternDesignItem 数据处理器 </summary>
    public ModuleHandle<DataPatternDesignItem> HandlePatternDesignItem => ModuleCore.HandlePatternDesignItem;

    protected override void Awake() {
        Element.generateVisualContent += Element_GenerateVisualContent;

        inputPosition = new UIVectorInput(Position);
        inputPosition.Slider1.SlidingValueChanged += SetPositionX;
        inputPosition.Slider2.SlidingValueChanged += SetPositionY;

        inputScale = new UIVectorInput(Scale);
        inputScale.Slider1.SlidingValueChanged += SetScaleX;
        inputScale.Slider2.SlidingValueChanged += SetScaleY;

        inputRotate = new UIVectorInput(Rotate);
        inputRotate.Slider1.SlidingValueChanged += SetRotate;

        Palette.RegisterCallback<MouseDownEvent>(SetColor);

        Lock.clicked += Lock_clicked;
        ColorCard.clicked += ColorCard_clicked;

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
        inputPosition.Slider1.Value = obj.position.x * 100f + 100f;
        inputPosition.Slider2.Value = obj.position.y * 100f + 100f;
        inputScale.Slider1.Value = obj.scale.x * 100f;
        inputScale.Slider2.Value = obj.scale.y * 100f;
        inputRotate.Slider1.Value = obj.rotate;
        Palette.style.backgroundColor = obj.color;
        Lock.EnableInClassList("pv-button-a-s", obj.isLock);
    }

    #region 设置属性
    private void UpdateDesignItem(Action action) {
        if (!HandlePatternDesignItem.IsValid) { return; }
        action?.Invoke();
        HandlePatternDesignItem.Change();
    }
    private void SetPositionX(float obj) {
        UpdateDesignItem(() => { HandlePatternDesignItem.Current.position.x = obj / 100f; });
    }
    private void SetPositionY(float obj) {
        UpdateDesignItem(() => { HandlePatternDesignItem.Current.position.y = obj / 100f; });
    }
    private void SetScaleX(float obj) {
        UpdateDesignItem(() => {
            HandlePatternDesignItem.Current.scale.x = obj / 100f;
            if (!HandlePatternDesignItem.Current.isLock) { return; }
            HandlePatternDesignItem.Current.scale.y = obj / 100f;
        });
    }
    private void SetScaleY(float obj) {
        UpdateDesignItem(() => {
            HandlePatternDesignItem.Current.scale.y = obj / 100f;
            if (!HandlePatternDesignItem.Current.isLock) { return; }
            HandlePatternDesignItem.Current.scale.x = obj / 100f;
        });
    }
    private void SetRotate(float obj) {
        UpdateDesignItem(() => { HandlePatternDesignItem.Current.rotate = obj; });
    }
    private void SetColor(MouseDownEvent evt) {
        if (!HandlePatternDesignItem.IsValid) { return; }
        //paletteManager.Open(HandlePatternDesignItem.Current.color, (color) => {
        //    HandlePatternDesignItem.Current.color = color;
        //    HandlePatternDesignItem.Change();
        //});
    }
    private void Lock_clicked() {
        UpdateDesignItem(() => {
            bool isLock = HandlePatternDesignItem.Current.isLock;
            HandlePatternDesignItem.Current.isLock = !isLock;
            Lock.EnableInClassList("pv-button-a-s", HandlePatternDesignItem.Current.isLock);
            if (!HandlePatternDesignItem.Current.isLock) { return; }
            float value = HandlePatternDesignItem.Current.scale.x;
            HandlePatternDesignItem.Current.scale.y = value;
        });
    }
    private void ColorCard_clicked() {
        if (!HandlePatternDesignItem.IsValid) { return; }
        DataPalette palette = new DataPalette();
        palette.color = HandlePatternDesignItem.Current.color;
        palette.callback = (color) => {
            HandlePatternDesignItem.Current.color = color;
            HandlePatternDesignItem.Change();
        };
        ColorCardWindow.Open(palette);
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
        public MUSliderHorizontal Slider1 => element.Q<MUSliderHorizontal>("MUSliderHorizontal1");
        public MUSliderHorizontal Slider2 => element.Q<MUSliderHorizontal>("MUSliderHorizontal2");
        public UIVectorInput(VisualElement element) => this.element = element;
    }
}
