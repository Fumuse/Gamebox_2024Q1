using TMPro;
using UnityEngine;

public class SimpleEnemy : MonoBehaviour, IDamagable
{
    [SerializeField] private GameObject _damageTextPrefab;
    [SerializeField] private Transform _textPoint;

    public void Damage(int damageAmount)
    {
        //TODO: можно сделать через пул объектов
        GameObject text = Instantiate(_damageTextPrefab, _textPoint);
        text.GetComponentInChildren<TextMeshProUGUI>().text = damageAmount.ToString();
    }
}