using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShotPathResolver : MonoBehaviour
{
    private ShotCore __shot;
    protected ShotCore shot => __shot != null ? __shot : (__shot = GetComponent<ShotCore>());

    protected internal abstract void ResolvePath();
}
