using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuHua;

public class AssetsPatternDesign : ModuleAssets<DataPatternDesign> {
    private List<DataPatternDesign> patternDesigns = new List<DataPatternDesign>();

    public string Directory => $"{SaveTool.PATH}/SaveFile";
    public string FileName => $"/PatternDesign.{SaveTool.EXTENSION}";
    public override event Action OnChange;
    public override int Count => patternDesigns.Count;
    public override List<DataPatternDesign> Datas => patternDesigns;

    /// <summary> 图案素材库 </summary>
    public ModuleAssets<DataPatternMaterials> AssetsPatternMaterials => ModuleCore.AssetsPatternMaterials;
    /// <summary> Web请求模块 </summary>
    public ModuleSingle<UnitWebRequest> SingleWebRequest => ModuleCore.SingleWebRequest;

    protected override void Awake() => ModuleCore.AssetsPatternDesign = this;
    protected void Start() => Load();

    public override void Add(DataPatternDesign data) {
        patternDesigns.Add(data);
        OnChange?.Invoke();
    }
    public override void Remove(DataPatternDesign data) {
        patternDesigns.Remove(data);
        OnChange?.Invoke();
    }
    public override DataPatternDesign Find(int index) {
        return patternDesigns[index];
    }
    public override void ForEach(Action<DataPatternDesign> action) {
        patternDesigns.ForEach(action);
    }

    #region 保存和加载
    private List<Texture2D> textures;
    public override void Save() {
        SaveData saveData = new SaveData();
        for (int i = 0; i < patternDesigns.Count; i++) {
            saveData.patternDesigns.Add(To(patternDesigns[i]));
        }
        SaveTool.SaveObjectToJson(Directory, FileName, saveData);
    }
    public override void Load() {
        string fileName = Directory + FileName;
        SaveData saveData = SaveTool.LoadJsonToObject<SaveData>(fileName);
        if (saveData == null) { return; }

        List<DataPatternMaterials> list = AssetsPatternMaterials.Datas;
        textures = new List<Texture2D>();
        for (int i = 0; i < list.Count; i++) {
            textures.AddRange(list[i].textures);
        }

        patternDesigns = new List<DataPatternDesign>();
        for (int i = 0; i < saveData.patternDesigns.Count; i++) {
            patternDesigns.Add(To(saveData.patternDesigns[i]));
        }
        OnChange?.Invoke();
    }
    #endregion

    #region 数据转换
    private SavePatternDesign To(DataPatternDesign data) {
        SavePatternDesign save = new SavePatternDesign();
        save.id = data.id;
        for (int i = 0; i < data.items.Count; i++) {
            save.items.Add(To(data.items[i]));
        }
        return save;
    }
    private SavePatternDesignItem To(DataPatternDesignItem data) {
        SavePatternDesignItem save = new SavePatternDesignItem();
        save.id = data.texture.name;
        save.position = data.position;
        save.scale = data.scale;
        save.rotate = data.rotate;
        save.color = data.color;
        save.isReverseX = data.isReverseX;
        save.isReverseY = data.isReverseY;
        return save;
    }
    private DataPatternDesign To(SavePatternDesign save) {
        DataPatternDesign data = new DataPatternDesign();

        string path = $"{SaveTool.PATH}/Pattern/{save.id}.png";
        UnitWebRequestTexture uwq = new UnitWebRequestTexture(path, (texture) => {
            data.texture = texture;
            data.OnUpdateTexture?.Invoke(texture);
        });
        SingleWebRequest.Open(uwq);

        data.id = save.id;
        for (int i = 0; i < save.items.Count; i++) {
            data.items.Add(To(data, save.items[i]));
        }
        return data;
    }
    private DataPatternDesignItem To(DataPatternDesign patternDesign, SavePatternDesignItem save) {
        DataPatternDesignItem data = new DataPatternDesignItem(patternDesign);

        for (int i = 0; i < textures.Count; i++) {
            if (save.id == textures[i].name) { data.texture = textures[i]; }
        }

        data.position = save.position;
        data.scale = save.scale;
        data.rotate = save.rotate;
        data.color = save.color;
        data.isReverseX = save.isReverseX;
        data.isReverseY = save.isReverseY;
        return data;
    }
    #endregion

    #region 序列化类
    [Serializable]
    public class SaveData {
        /// <summary> 图案设计列表 </summary>
        public List<SavePatternDesign> patternDesigns = new List<SavePatternDesign>();
    }
    [Serializable]
    public class SavePatternDesign {
        /// <summary> 唯一标识符 </summary>
        public string id;
        /// <summary> 图案设计项目列表 </summary>
        public List<SavePatternDesignItem> items = new List<SavePatternDesignItem>();
    }
    [Serializable]
    public class SavePatternDesignItem {
        /// <summary> 唯一标识符 </summary>
        public string id;
        /// <summary> 位置 </summary>
        public Vector2 position = new Vector2(0, 0);
        /// <summary> 规模 </summary>
        public Vector2 scale = new Vector2(0.5f, 0.5f);
        /// <summary> 旋转 </summary>
        public float rotate = 0;
        /// <summary> 颜色 </summary>
        public Color color = new Color(1, 1, 1, 1);
        /// <summary> 镜像X </summary>
        public bool isReverseX = false;
        /// <summary> 镜像Y </summary>
        public bool isReverseY = false;
    }
    #endregion

}
