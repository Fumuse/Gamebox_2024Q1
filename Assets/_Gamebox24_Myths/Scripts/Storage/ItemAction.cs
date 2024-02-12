using System;

public enum ItemActionEnum
{
    Sell = 1,
    Eat = 2,
}

[Serializable]
public class ItemAction
{
    public ItemActionEnum actionType;

    public Item Item { get; set; }

    public void Action()
    {
        if (actionType == ItemActionEnum.Sell) SellAction();

        if (actionType == ItemActionEnum.Eat) EatAction();
    }

    private void SellAction()
    {
        Storage playerStorage = PlayerController.PlayerStorage;
        playerStorage.Remove(Item, Item.Price * -1);
        playerStorage.Add(playerStorage.soulStonesItem, Item.Price);
    }

    private void EatAction()
    {
        Storage playerStorage = PlayerController.PlayerStorage;
        playerStorage.Remove(Item, Item.Price * -1);
        PlayerIndicators.Instance.CurrentQi += Item.Price;
    }
}