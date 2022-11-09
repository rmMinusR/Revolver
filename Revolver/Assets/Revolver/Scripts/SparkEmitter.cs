using UnityEngine;

public sealed class SparkEmitter : MonoBehaviour
{
    [SerializeField] [Min(0)] private float sparkRange = 0.1f;
    [SerializeField] private GameObject sparkFX;

    public void Fire()
    {
        SparkReciever.SparkNearby(transform.position, sparkRange);
        Instantiate(sparkFX, transform.position, transform.rotation);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, sparkRange);
    }
}