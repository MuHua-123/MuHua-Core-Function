using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Web Get请求数据
/// </summary>
public class UnitWebRequestGet : UnitWebRequest {
    public readonly string url;

    public bool isBackend;
    public Action<string> OnError;
    public Action<string> OnCallback;

    /// <summary> Web Get请求数据 </summary>
    public UnitWebRequestGet(string url, Action<string> OnCallback = null, bool isBackend = false) {
        this.url = url;
        this.isBackend = isBackend;
        this.OnCallback = OnCallback;
    }

    public override string Url => url;
    public override bool IsBackend => isBackend;
    public override WebRequestType RequestType => WebRequestType.GET;
    public override void RequestResultHandle(bool isDone, DownloadHandler downloadHandler) {
        if (!isDone) { OnError?.Invoke(downloadHandler.text); return; }
        OnCallback?.Invoke(downloadHandler.text);
    }
}
