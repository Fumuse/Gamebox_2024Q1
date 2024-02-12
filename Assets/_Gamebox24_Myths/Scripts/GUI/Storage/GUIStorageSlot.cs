using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GUIStorageSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TextMeshProUGUI _itemAmountText;
    [SerializeField] private ItemTooltip _tooltip;
    [SerializeField] private StorageItemActionPanel _itemActionPanel;

    public Image ItemIcon => _itemIcon;

    public TextMeshProUGUI ItemAmountText => _itemAmountText;

    public StorageSlot Slot { get; set; } = null;

    private void Validate()
    {
        if (_tooltip == null)
        {
            _tooltip = FindObjectOfType<ItemTooltip>(true);
        }

        if (_itemActionPanel == null)
        {
            _itemActionPanel = FindObjectOfType<StorageItemActionPanel>(true);
        }
    }

    private void Start()
    {
        Validate();
    }

    private void OnValidate()
    {
        Validate();
    }

    public void SetItem(Item item, int count = 1)
    {
        if (item.Icon != null)
        {
            _itemIcon.sprite = item.Icon;
            _itemIcon.gameObject.SetActive(true);
        }

        _itemAmountText.text = count.ToString();
        _itemAmountText.gameObject.SetActive(true);
    }

    public void ClearItem()
    {
        _itemIcon.gameObject.SetActive(false);
        _itemAmountText.gameObject.SetActive(false);
        
        _tooltip.HideTooltip();
        _itemActionPanel.Toggle(false);

        Slot = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Slot == null) return;
        _tooltip.ShowTooltip(Slot, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _tooltip.HideTooltip();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Slot == null) return;
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            _itemActionPanel.Toggle(true);
            
            foreach (var action in Slot.Item.ItemActions)
            {
                action.Item = Slot.Item;
                _itemActionPanel.AddButton(action);
            }
            
            _itemActionPanel.transform.position = transform.position;
        }
    }
}