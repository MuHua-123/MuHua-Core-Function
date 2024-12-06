using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using MuHua;

public class UIColorCardWindow : ModuleUIWindow<DataPalette> {
    public VisualTreeAsset ColorCardTemplateAsset;
    public List<Color> colors;
    private DataPalette palette;
    private List<UIColor> uIColors = new List<UIColor>();

    public override VisualElement Element => ModuleUIPage.Q<VisualElement>("ColorCardWindow");
    public Button CloseButton => Element.Q<Button>("Close");
    public MUScrollViewVertical ColorCardList => Element.Q<MUScrollViewVertical>("MUScrollViewVertical");

    public override void Awake() => ModuleCore.ColorCardWindow = this;
    protected void Start() => CloseButton.clicked += Close;
    protected void OnDestroy() => uIColors.ForEach(obj => obj.Release());

    public override void Open(DataPalette palette) {
        this.palette = palette;
        Element.style.display = DisplayStyle.Flex;
        ColorCardList.ClearContainer();
        uIColors.ForEach(obj => obj.Release());
        uIColors = new List<UIColor>();
        colors.ForEach(CreateUIColor);
    }
    public override void Close() => Element.style.display = DisplayStyle.None;

    public void SetColor(Color color) => palette.callback?.Invoke(color);

    private void CreateUIColor(Color color) {
        VisualElement element = ColorCardTemplateAsset.Instantiate();
        UIColor uIColor = new UIColor(color, element, this);
        ColorCardList.AddContainer(uIColor.element);
        uIColors.Add(uIColor);
    }

    /// <summary> 颜色项目 </summary>
    public class UIColor : UIItem<Color, UIColor> {
        public readonly UIColorCardWindow parent;
        public VisualElement Color => element.Q<VisualElement>("Color");
        public UIColor(Color value, VisualElement element, UIColorCardWindow parent) : base(value, element) {
            this.parent = parent;
            Color.style.backgroundColor = value;
            Color.RegisterCallback<MouseDownEvent>(MouseDown);
        }
        private void MouseDown(MouseDownEvent evt) {
            parent.SetColor(value);
            parent.Close();
        }
    }
}
