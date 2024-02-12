using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGUI : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryContainer;
    [SerializeField] private Image _qiProgressBar;

    private PlayerIndicators _playerIndicators;

    private void Awake()
    {
        PlayerIndicators.OnUpdateQi += OnUpdateQi;
    }

    public void ToggleInventory()
    {
        _inventoryContainer.SetActive(!_inventoryContainer.activeInHierarchy);
    }

    private void OnUpdateQi(int amount, bool save)
    {
        if (_playerIndicators == null)
            _playerIndicators = PlayerIndicators.Instance;
        float width = Convert.ToSingle(amount) / Convert.ToSingle(_playerIndicators.MaxQi);

        DOTween.To(
            () => _qiProgressBar.fillAmount,
            x => _qiProgressBar.fillAmount = x,
            width,
            1f
        ).SetEase(Ease.Linear);
    }
}