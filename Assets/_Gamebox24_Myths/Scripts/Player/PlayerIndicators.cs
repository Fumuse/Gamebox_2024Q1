using UnityEngine;

public class PlayerIndicators : MonoBehaviour
{
    [SerializeField] private int _maxQi = 100;

    public delegate void UpdateQi(int amount, bool save = true);

    public static event UpdateQi OnUpdateQi;

    private bool _loaded = false;

    private int _currentQi = 0;

    public int CurrentQi
    {
        get => _currentQi;
        set
        {
            OnUpdateQi?.Invoke(value, _loaded);
            _currentQi = value;
        }
    }

    public int MaxQi => _maxQi;

    private static PlayerIndicators _instance = null;
    public static PlayerIndicators Instance => _instance;

    private void Awake()
    {
        if (_instance == null) _instance = this;
        else if (_instance == this) Destroy(gameObject);
    }

    private void Start()
    {
        CurrentQi = SaveSerial.CurrentQi;

        _loaded = true;
    }
}