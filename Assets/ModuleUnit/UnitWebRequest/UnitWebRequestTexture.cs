using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Web Get请求 Texture
/// </summary>
public class UnitWebRequestTexture : UnitWebRequest {
    public readonly string url;

    public bool isBackend;
    public Action<string> OnError;
    public Action<Texture2D> OnCallback;

    /// <summary> Web Get请求 Texture </summary>
    public UnitWebRequestTexture(string url, Action<Texture2D> OnCallback = null, bool isBackend = false) {
        this.url = url;
        this.isBackend = isBackend;
        this.OnCallback = OnCallback;
    }

    public override string Url => url;
    public override bool IsBackend => isBackend;
    public override WebRequestType RequestType => WebRequestType.Texture;
    public override void RequestResultHandle(bool isDone, DownloadHandler downloadHandler) {
        if (!isDone) { OnError?.Invoke(downloadHandler.text); return; }
        DownloadHandlerTexture dht = downloadHandler as DownloadHandlerTexture;
        OnCallback?.Invoke(dht.texture);
    }
}
