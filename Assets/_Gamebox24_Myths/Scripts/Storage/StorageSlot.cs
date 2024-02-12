using UnityEngine;

[System.Serializable]
public class StorageSlot
{
    [SerializeField] private Item _item;
    [SerializeField] private int _amount;
    public GUIStorageSlot guiItem;

    public string Owner { get; set; }

    public Item Item
    {
        get => _item;
        private set => _item = value;
    }

    public int Amount
    {
        get => _amount;
        set
        {
            _amount = value;

            if (guiItem != null)
                guiItem.ItemAmountText.text = _amount.ToString();
        }
    }

    public StorageSlot(Item item, int amount, string owner)
    {
        Item = item;
        Amount = amount;
        Owner = owner;
    }

    public SerializeSlot SerializedSlot()
    {
        SerializeSlot slot = new();
        slot.amount = Amount;
        slot.item = Item.item;
        slot.owner = Owner;
        return slot;
    }
}

[System.Serializable]
public class SerializeSlot
{
    public int amount;
    public ItemData item;
    public string owner;

    public override string ToString()
    {
        return $"{item.itemName} {amount} {owner}";
    }
}