using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Web Post请求数据
/// </summary>
public class UnitWebRequestPost : UnitWebRequest {
    public readonly string url;
    public readonly string json;
    public readonly WWWForm form;
    public readonly WebRequestType type;

    public bool isBackend;
    public Action<string> OnError;
    public Action<string> OnCallback;

    /// <summary> Web Post请求 提交json数据 </summary>
    public UnitWebRequestPost(string url, string json, Action<string> OnCallback = null, bool isBackend = false) {
        this.url = url;
        this.json = json;
        this.isBackend = isBackend;
        this.OnCallback = OnCallback;
        type = WebRequestType.POSTJSON;
    }
    /// <summary> Web Post请求 提交WWWForm数据 </summary>
    public UnitWebRequestPost(string url, WWWForm form, Action<string> OnCallback = null, bool isBackend = false) {
        this.url = url;
        this.form = form;
        this.isBackend = isBackend;
        this.OnCallback = OnCallback;
        type = WebRequestType.POSTFORM;
    }

    public override string Url => url;
    public override bool IsBackend => isBackend;
    public override WebRequestType RequestType => type;
    public override string Json => json;
    public override WWWForm Form => form;
    public override void RequestResultHandle(bool isDone, DownloadHandler downloadHandler) {
        if (!isDone) { OnError?.Invoke(downloadHandler.text); return; }
        OnCallback?.Invoke(downloadHandler.text);
    }
}
