using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject _firstScreen;
    private GameObject _currentScreen;

    // Start is called before the first frame update
    void Start()
    {
        ChangeScreenStateActivity(_firstScreen);
    }

    public void ChangeScreenStateActivity(GameObject anotherScreen)
    {
        if (_currentScreen)
        {
            _currentScreen.SetActive(false);
        }

        anotherScreen.SetActive(true);
        _currentScreen = anotherScreen;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}