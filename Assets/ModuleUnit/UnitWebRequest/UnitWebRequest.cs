using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Web请求类型
/// </summary>
public enum WebRequestType {
    /// <summary> GET </summary>
    GET = 0,
    /// <summary> POST 表单 </summary>
    POSTFORM = 1,
    /// <summary> POST Json </summary>
    POSTJSON = 2,
    /// <summary> GET 获取图片 </summary>
    Texture = 3
}
/// <summary>
/// Web请求数据
/// </summary>
public abstract class UnitWebRequest {
    /// <summary> Web请求地址 </summary>
    public abstract string Url { get; }
    /// <summary> 是否后台处理 </summary>
    public abstract bool IsBackend { get; }
    /// <summary> Web请求类型 </summary>
    public abstract WebRequestType RequestType { get; }
    /// <summary> 提交json数据 </summary>
    public virtual string Json { get; }
    /// <summary> 提交Form表单数据 </summary>
    public virtual WWWForm Form { get; }

    /// <summary> Web请求结果处理 </summary>
    public abstract void RequestResultHandle(bool isDone, DownloadHandler downloadHandler);
}
