using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StorageItemActionPanel : MonoBehaviour, IPointerExitHandler
{
    [SerializeField] private Transform _container;
    [SerializeField] private GameObject _buttonPrefab;

    public void AddButton(ItemAction action)
    {
        GameObject button = Instantiate(_buttonPrefab, _container);
        button.GetComponent<Button>().onClick.AddListener(action.Action);
        button.GetComponentInChildren<TextMeshProUGUI>().text = action.actionType.ToString();
    }

    public void Toggle(bool active)
    {
        if (active) RemoveOldButtons();
        gameObject.SetActive(active);
    }

    protected void RemoveOldButtons()
    {
        foreach (Transform childObjectTrans in _container)
        {
            Destroy(childObjectTrans.gameObject);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Toggle(false);
    }
}
