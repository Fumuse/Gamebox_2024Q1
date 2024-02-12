using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] private int _storageSlotsCount = 40;
    [SerializeField] private GameObject _storageGUIContainer;
    [SerializeField] public GameObject _storageSlotPrefab;
    [SerializeField] private List<StorageSlot> _storageSlots = new();
    [SerializeField] public Item soulStonesItem;


    public List<StorageSlot> StorageSlots => _storageSlots;

    private GameObject[] _storageSlotsObjects;
    private bool _storageIsLoaded = false;

    private void Awake()
    {
        DrawStorageInventory();

        foreach (SerializeSlot slot in SaveSerial.Storages)
        {
            if (slot.owner != gameObject.name) continue;

            Item item = Resources.Load<Item>(slot.item.assetName);
            Add(item, slot.amount);
        }

        _storageIsLoaded = true;
    }

    private void DrawStorageInventory()
    {
        if (_storageGUIContainer != null)
        {
            _storageSlotsObjects = new GameObject[_storageSlotsCount];
            for (int s = 0; s < _storageSlotsCount; s++)
            {
                GameObject slot = Instantiate(_storageSlotPrefab, _storageGUIContainer.transform);
                _storageSlotsObjects[s] = slot;
            }
        }
    }

    public bool Add(Item item, int amount = 1)
    {
        StorageSlot storageSlot = Contains(item);
        if (storageSlot != null)
        {
            storageSlot.Amount += amount;
        }
        else
        {
            storageSlot = new StorageSlot(item, amount, gameObject.name);
            if (_storageSlotsObjects.Length > 0)
            {
                int slotIndex = EmptyStorageSlotIndex();
                storageSlot.guiItem = _storageSlotsObjects[slotIndex].GetComponent<GUIStorageSlot>();
                storageSlot.guiItem.SetItem(item, amount);
                storageSlot.guiItem.Slot = storageSlot;
            }

            _storageSlots.Add(storageSlot);
        }

        if (_storageIsLoaded)
            SaveSerial.AddStoragesSlot(storageSlot.SerializedSlot());

        return true;
    }

    public bool Remove(Item item, int amount = -1)
    {
        bool result = false;
        StorageSlot storageSlot = Contains(item);
        if (storageSlot != null)
        {
            storageSlot.Amount += amount;
            SaveSerial.AddStoragesSlot(storageSlot.SerializedSlot());

            if (storageSlot.Amount <= 0)
            {
                if (storageSlot.guiItem != null)
                {
                    storageSlot.guiItem.ClearItem();
                }

                SaveSerial.RemoveStoragesSlot(storageSlot.SerializedSlot());
                _storageSlots.Remove(storageSlot);
            }

            result = true;
        }

        return result;
    }

    public int EmptyStorageSlotIndex()
    {
        int counterIndex = 0;
        foreach (var slot in _storageSlotsObjects)
        {
            if (slot.TryGetComponent(out GUIStorageSlot guiSlot))
            {
                if (guiSlot.Slot == null)
                {
                    return counterIndex;
                }
            }

            counterIndex++;
        }

        return -1;
    }

    [CanBeNull]
    public StorageSlot Contains(Item item)
    {
        StorageSlot desiredSlot = null;
        foreach (StorageSlot slot in _storageSlots)
        {
            if (slot.Item == item)
            {
                desiredSlot = slot;
                break;
            }
        }

        return desiredSlot;
    }

#if UNITY_EDITOR
    public override string ToString()
    {
        string storage = "";

        foreach (StorageSlot slot in _storageSlots)
        {
            storage += slot.Item.Name + " " + slot.Amount + "\n";
        }

        return storage;
    }
#endif
}