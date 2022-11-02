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
}
