using UnityEngine;

public class ObjectShiny : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private GameObject _objectBody;

    private Outline _bodyOutline;

    private void Awake()
    {
        _bodyOutline = _objectBody.GetComponent<Outline>();
    }

    public void CanInteract()
    {
        _particleSystem.gameObject.SetActive(true);
        _bodyOutline.enabled = true;
    }

    public void CantInteract()
    {
        _particleSystem.gameObject.SetActive(false);
        _bodyOutline.enabled = false;
    }
}