using UnityEngine;

public abstract class SparkReciever : MonoBehaviour
{
    public static void SparkNearby(Vector3 pos, float range)
    {
        foreach (SparkReciever i in FindObjectsOfType<SparkReciever>()) if (Vector3.Distance(i.transform.position, pos) < range) i.OnSparked();
    }

    protected internal abstract void OnSparked();
}