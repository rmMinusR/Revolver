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
