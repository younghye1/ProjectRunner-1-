using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void OnStartButtonClick()
    {
        SceneManager.LoadScene("1Ingame"); // "1Ingame" 씬으로 이동
    }
}