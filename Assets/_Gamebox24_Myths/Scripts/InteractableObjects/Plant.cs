public class Plant : InteractableObject
{
    public static event CollectItemEvent OnCollectItem;

    public override async void CollectItem(int amount = 1)
    {
        if (HasCollect) return;
        CancelInteract();

        OnCollectItem?.Invoke(true, _collectTime);

        bool collect = await CollectItemTimer();
        if (collect)
        {
            AddItemToPlayerStorage(amount);
            CancelInteract();

            if (ItemsCurrentCount > 0) CollectItem();
        }
        else
        {
            CancelInteract();
        }
    }

    protected new void CancelInteract()
    {
        base.CancelInteract();
        OnCollectItem?.Invoke(false, 0);
    }

    protected override void AfterPlayerMove()
    {
        if (HasCollect)
        {
            CancelInteract();
        }
    }
}