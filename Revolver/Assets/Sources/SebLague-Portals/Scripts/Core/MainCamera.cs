using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MainCamera : MonoBehaviour {

    Portal[] portals;

    void Awake () {
        portals = FindObjectsOfType<Portal> ();

        RenderPipelineManager.beginCameraRendering += Render;
    }

    //private void LateUpdate()
    //{
    //    Render();
    //}
    //void Render()

    void Render(ScriptableRenderContext ctx, Camera cam)
    {

        for (int i = 0; i < portals.Length; i++) {
            portals[i].PrePortalRender ();
        }
        for (int i = 0; i < portals.Length; i++) {
            portals[i].Render (ctx);
        }

        for (int i = 0; i < portals.Length; i++) {
            portals[i].PostPortalRender ();
        }

    }

    private void OnDestroy()
    {
        portals = new Portal[0];
    }

}