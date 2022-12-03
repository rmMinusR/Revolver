using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Camera))]
public class MainCamera : MonoBehaviour {

    void Awake () {
        RenderPipelineManager.beginCameraRendering += Render;
    }

    void Render(ScriptableRenderContext ctx, Camera cam)
    {
        if (cam.gameObject != this.gameObject) return;

        foreach (Portal p in Portal.Instances) p.PrePortalRender ();
        foreach (Portal p in Portal.Instances) p.Render(ctx);
        foreach (Portal p in Portal.Instances) p.PostPortalRender ();
    }

    private void OnDestroy()
    {
        RenderPipelineManager.beginCameraRendering -= Render;
    }

}