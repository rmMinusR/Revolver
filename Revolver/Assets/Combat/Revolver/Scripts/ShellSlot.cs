using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ShellSlot : MonoBehaviour
{
    internal Shell contents;

    public void Place(Shell newContents)
    {
        if (contents) Destroy(contents.gameObject);
        contents = Instantiate(newContents, transform);
        contents.slot = this;

        contents.transform.localPosition = Vector3.zero;
        contents.transform.localRotation = Quaternion.identity;
        contents.transform.localScale = Vector3.one;
    }

    [SerializeField] [Min(0)] private float unloadScatter = 0.5f;

    public void Unload()
    {
        if (contents)
        {
            contents.transform.parent = null;

            contents.gameObject.AddComponent<Rigidbody>().velocity += Random.insideUnitSphere * unloadScatter;
            contents.gameObject.AddComponent<MeshCollider>().convex = true;

            TimedDestroy destroyer = contents.gameObject.AddComponent<TimedDestroy>();
            destroyer.timeToLive = contents.casingPersistTime;

            contents = null;
        }
    }

#if UNITY_EDITOR
    [SerializeField] private Shell dbgLoad;
    private void Update()
    {
        if (dbgLoad != null)
        {
            Place(dbgLoad);
            dbgLoad = null;
        }
    }
#endif
}
