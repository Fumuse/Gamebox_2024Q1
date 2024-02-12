using TMPro;
using UnityEngine;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private TextMeshProUGUI _itemDescText;
    [SerializeField] private TextMeshProUGUI _itemCountText;

    public void ShowTooltip(StorageSlot slot, Vector3 position)
    {
        _itemNameText.text = slot.Item.Name;
        _itemDescText.text = slot.Item.Desc;
        _itemCountText.text = "Количество: " + slot.Amount; //todo: вынести текст в другое место

        gameObject.transform.position = position;
        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}