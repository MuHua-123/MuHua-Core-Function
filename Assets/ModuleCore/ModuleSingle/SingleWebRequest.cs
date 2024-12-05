using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Web请求模块
/// </summary>
public class SingleWebRequest : ModuleSingle<UnitWebRequest> {

    protected override void Awake() {
        if (ModuleCore.SingleWebRequest != null) { Destroy(gameObject); }
        ModuleCore.SingleWebRequest = this;
    }

    public override void Open(UnitWebRequest request) {
        if (request.RequestType == WebRequestType.GET) { StartCoroutine(IWebRequestGet(request)); }
        if (request.RequestType == WebRequestType.POSTFORM) { StartCoroutine(IWebRequestPOSTFORM(request)); }
        if (request.RequestType == WebRequestType.POSTJSON) { StartCoroutine(IWebRequestPOSTJSON(request)); }
        if (request.RequestType == WebRequestType.Texture) { StartCoroutine(IWebRequestTexture(request)); }
    }
    public override void Complete() {
        throw new System.NotImplementedException();
    }
    public override void Close() {
        throw new System.NotImplementedException();
    }

    public IEnumerator IWebRequestGet(UnitWebRequest request) {
        string url = request.Url;
        using (UnityWebRequest web = UnityWebRequest.Get(url)) {
            yield return web.SendWebRequest();
            bool isDone = web.isDone && web.result == UnityWebRequest.Result.Success;
            request.RequestResultHandle(isDone, web.downloadHandler);
        }
    }
    public IEnumerator IWebRequestPOSTFORM(UnitWebRequest request) {
        string url = request.Url;
        WWWForm form = request.Form;
        using (UnityWebRequest web = UnityWebRequest.Post(url, form)) {
            yield return web.SendWebRequest();
            bool isDone = web.isDone && web.result == UnityWebRequest.Result.Success;
            request.RequestResultHandle(isDone, web.downloadHandler);
        }
    }
    public IEnumerator IWebRequestPOSTJSON(UnitWebRequest request) {
        string url = request.Url;
        string json = request.Json;
        byte[] postBytes = System.Text.Encoding.Default.GetBytes(json);
#if UNITY_2022
        using (UnityWebRequest web = UnityWebRequest.PostWwwForm(url, "POST")) {
#else
        using (UnityWebRequest web = UnityWebRequest.Post(url, "POST")) {
#endif
            web.uploadHandler.Dispose();
            web.uploadHandler = new UploadHandlerRaw(postBytes);
            web.SetRequestHeader("Content-Type", "application/json");
            yield return web.SendWebRequest();
            bool isDone = web.isDone && web.result == UnityWebRequest.Result.Success;
            request.RequestResultHandle(isDone, web.downloadHandler);
        }
    }
    public IEnumerator IWebRequestTexture(UnitWebRequest request) {
        string url = request.Url;
        using (UnityWebRequest web = UnityWebRequestTexture.GetTexture(url)) {
            yield return web.SendWebRequest();
            bool isDone = web.isDone && web.result == UnityWebRequest.Result.Success;
            request.RequestResultHandle(isDone, web.downloadHandler);
        }
    }
}
