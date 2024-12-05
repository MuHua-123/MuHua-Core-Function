using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 全局不销毁模块
/// </summary>
public class ModuleGlobal : MonoBehaviour {
    private static ModuleGlobal instance;
    public static ModuleGlobal Instance { get { return instance; } }
    public static bool isInstance { get { return instance != null; } }
    protected virtual void Awake() {
        if (instance != null) { Destroy(gameObject); }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
