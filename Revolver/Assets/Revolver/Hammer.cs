using UnityEngine;

[RequireComponent(typeof(Collider))]
public sealed class Hammer : MonoBehaviour
{
    [SerializeField] private GameObject sparks;
    [SerializeField] [Min(0)] private float sparkThreshold = 0.1f;

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > sparkThreshold)
        {
            Instantiate(sparks, transform);
            if (collision.gameObject.TryGetComponent(out Shell shell)) shell.Ignite();
        }
    }
}