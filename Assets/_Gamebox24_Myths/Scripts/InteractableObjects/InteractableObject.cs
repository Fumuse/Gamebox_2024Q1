using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ObjectShiny))]
public abstract class InteractableObject : MonoBehaviour, IInteractableObject
{
    [SerializeField] private int _itemsCount = 3;
    [SerializeField] private Item _item;
    [SerializeField] private Image _interactTimer;
    [SerializeField] protected float _collectTime = 3f;
    [SerializeField] private float _respawnTime = 120f;

    [SerializeField] private GameObject _guiCanvas;

    public delegate void CollectItemEvent(bool startCollect, float timeCollect);

    public static bool HasCollect = false;
    private CancellationTokenSource _cts;

    private float _currentCollectTime = 0f;
    private float _currentRespawnTime = 0f;
    private int _itemsCurrentCount = 0;
    private GameObject _interactTimerWrapper;
    private ObjectShiny _shiny;

    private bool _respawnStarted = false;

    public int ItemsCurrentCount
    {
        get => _itemsCurrentCount;
        protected set
        {
            _itemsCurrentCount = value;
            if (_itemsCurrentCount <= 0)
            {
                NotShownToItem();
                _shiny.CantInteract();
            }
            else _shiny.CanInteract();

            if (_itemsCurrentCount < _itemsCount) RespawnItem();
        }
    }

    private void OnValidate()
    {
        if (_itemsCount <= 0) _itemsCount = 1;
    }

    private void Start()
    {
        _shiny = GetComponent<ObjectShiny>();
        ItemsCurrentCount = _itemsCount;
        if (_interactTimer != null) _interactTimerWrapper = _interactTimer.gameObject.transform.parent.gameObject;

        PlayerController.AfterPlayerMove += AfterPlayerMove;
    }

    public abstract void CollectItem(int amount = 1);

    protected async UniTask<bool> CollectItemTimer()
    {
        if (ItemsCurrentCount <= 0) return false;

        _cts = new CancellationTokenSource();
        HasCollect = true;

        _currentCollectTime = 0f;
        _interactTimer.fillAmount = 0;
        _interactTimerWrapper.SetActive(true);
        while (_currentCollectTime < _collectTime)
        {
            bool isCanceled = await UniTask.WaitForEndOfFrame(this, _cts.Token).SuppressCancellationThrow();
            if (isCanceled || !HasCollect) return false;
            _currentCollectTime += Time.deltaTime;
            _interactTimer.fillAmount = _currentCollectTime / _collectTime;
        }

        return true;
    }

    protected void AddItemToPlayerStorage(int amount)
    {
        Storage storage = PlayerController.PlayerStorage;
        storage.Add(_item, amount);
        ItemsCurrentCount -= amount;
    }

    public void ShownToItem()
    {
        if (_guiCanvas == null) return;
        if (ItemsCurrentCount <= 0) return;
        _guiCanvas.SetActive(true);
    }

    public void NotShownToItem()
    {
        if (_guiCanvas == null) return;
        _guiCanvas.SetActive(false);
        CancelInteract();
    }

    protected void CancelInteract()
    {
        if (_cts != null) _cts.Cancel();

        if (_interactTimerWrapper != null)
            _interactTimerWrapper.SetActive(false);

        HasCollect = false;
    }

    private async void RespawnItem()
    {
        if (_respawnStarted) return;

        _respawnStarted = true;
        _currentRespawnTime = 0f;
        while (_currentRespawnTime < _respawnTime)
        {
            await UniTask.WaitForEndOfFrame(this);
            _currentRespawnTime += Time.deltaTime;
        }

        _respawnStarted = false;
        ItemsCurrentCount += 1;

        if (ItemsCurrentCount < _itemsCount)
            RespawnItem();
    }

    protected abstract void AfterPlayerMove();
}