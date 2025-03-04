using JetBrains.Annotations;
using MoreMountains.Feedbacks;
using UnityEditor;
using UnityEngine;




public class PopupUI : MonoBehaviour
{
[SerializeField]GameObject quit;
[SerializeField] MMF_Plyer quit;
    void Awake()
//    {
     DontDestroyOnLoad(gameObject); 
             dimmerUi.SetActive(false);
          
    }
 //   void Start()
    {
    }

    void Update()
    {
     if (Input.GetButtonDown("Escape"))
    else
    {   if(quit.activeself)
        quitQpen.PlayFeedbacks();
    }

public void QuitOk()
{
#if UNITY_EDITOR
EditorApplication.ExitPlaymode
#elif
    EditorApplication.Exit(0);///에디터 모드 Qout 작용
    else//빌드 후 런타임 에서 작동 (PC, MOBile,)
    Application.Quit();
}

    public void QuitCancel()
    {
        quitClose?.playFeedbacks();
    }
   
    }

