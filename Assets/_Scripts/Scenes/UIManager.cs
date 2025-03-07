using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI tmversion;

    void OnValidate()
    {
        if ( tmversion != null)
        tmversion.text = $"v{Application.version}";
    }

    public void OnStartButtonClick()
    {
        SceneManager.LoadScene("1Ingame"); // "1Ingame" 씬으로 이동
    }
}