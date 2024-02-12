using UnityEngine;

public class DestroyOnAnimationEnd : MonoBehaviour
{
    public void DestroyParent()
    {
        GameObject parent = transform.parent.gameObject;
        Destroy(parent);
    }

    public void DestroyCurrent()
    {
        Destroy(gameObject);
    }
}