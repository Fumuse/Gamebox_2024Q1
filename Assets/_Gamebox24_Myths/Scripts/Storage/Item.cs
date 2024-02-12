using UnityEngine;

[CreateAssetMenu(fileName = "StorageItem", menuName = "ScriptableObjects/Storage Item", order = 51)]
[System.Serializable]
public class Item : ScriptableObject
{
    [SerializeField] private Sprite _icon;
    [SerializeField] private string _desc;
    [SerializeField] private int _price = 1;

    public ItemAction[] ItemActions;

    public ItemData item = new();

    public string Name => item.itemName;
    public Sprite Icon => _icon;
    public string Desc => _desc;

    public int Price => _price;
}

[System.Serializable]
public class ItemData
{
    public string itemName;
    public string assetName;
}