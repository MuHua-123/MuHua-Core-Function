using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using MuHua;


public class UIPagePatternDesign : ModuleUIPage {
    [CustomLabel("上一个场景")][SceneName] public string scene1;
    [CustomLabel("下一个场景")][SceneName] public string scene2;

    public VisualElement TopMenu => Q<VisualElement>("TopMenu");
    public Button Button1 => TopMenu.Q<Button>("Button1");
    public Button Button2 => TopMenu.Q<Button>("Button2");

    /// <summary> 图案设计库 </summary>
    public ModuleAssets<DataPatternDesign> AssetsPatternDesign => ModuleCore.AssetsPatternDesign;

    protected override void Awake() => ModuleCore.CurrentPage = this;
    protected void Start() {
        Button1.clicked += Button1_clicked;
        Button2.clicked += Button2_clicked;
    }

    private void Button1_clicked() {
        SavePattern();
        SceneLoading.Jump(scene1);
    }
    private void Button2_clicked() {
        SavePattern();
        SceneLoading.Jump(scene2);
    }

    public void SavePattern() {
        List<DataPatternDesign> datas = AssetsPatternDesign.Datas;
        string directory = $"{SaveTool.PATH}/Pattern";
        if (Directory.Exists(directory)) { Directory.Delete(directory, true); }
        Directory.CreateDirectory(directory);
        for (int i = 0; i < datas.Count; i++) {
            string path = $"{directory}/{datas[i].id}.png";
            File.WriteAllBytes(path, datas[i].texture.EncodeToPNG());
        }
        AssetsPatternDesign.Save();
    }
}
