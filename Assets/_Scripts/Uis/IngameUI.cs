using UnityEngine;
using TMPro;
using DG.Tweening;
using CustomInspector;
using MoreMountains.Feedbacks;


public class IngameUI : MonoBehaviour
{
    [HorizontalLine]
    [SerializeField] TextMeshProUGUI tmInformation;
    [SerializeField] MMF_Player feedbackInformation;


    [HorizontalLine]
    [SerializeField] TextMeshProUGUI tmMileage;
    [SerializeField] TextMeshProUGUI tmCoin;
    [SerializeField] TextMeshProUGUI tmLife;


    void Awake()
    {
        tmInformation.text = "";        
    }

    void Update()
    {
        UpdateMileage();
        UpdateCoins(); 
        UpdateLife();
    }
    

    public void ShowInfo(string info, float duration = 1f)
    {
        if (feedbackInformation.IsPlaying)
            feedbackInformation.StopFeedbacks();

        // (예) 5초 동안 표시
        // 표시중에 새로운 콜
        // 1. 기존 작업 마무리하고 처리한다 => 스택 쌓아두고 처리
        // 2. 기존 작업 취소하고 새로 바로 처리한다. => 즉시 업데이트 처리
        tmInformation.text = info;   
        feedbackInformation.GetFeedbackOfType<MMF_Pause>().PauseDuration = duration;
        feedbackInformation.PlayFeedbacks();
    }

    void UpdateMileage()
    {
        // 작은수 표현
        if ( GameManager.mileage <= 1000f )
        {
            long intpart = (long)GameManager.mileage;
            int decpart = (int)((GameManager.mileage - intpart) * 10);
            tmMileage.text = $"{intpart}<size=80%>.{decpart}</size><size=60%>m</size>";
        }
        // 큰수 표현
        else
        {
            ((long)GameManager.mileage).ToStringKilo(out string intpart, out string decpart, out string unitpart);        
            tmMileage.text = $"{intpart}<size=80%>{decpart}{unitpart}</size><size=60%>m</size>";
        }
    }

    private uint _lastcoins;
    private Tween _tweencoin;
    void UpdateCoins()
    {
        if (_lastcoins == GameManager.coins) 
            return;

        if (_tweencoin != null)
            _tweencoin.Kill(true);

        // "N0" 역할 12345 => 12,345
        tmCoin.text = GameManager.coins.ToString("N0");
        _lastcoins = GameManager.coins;

        tmCoin.rectTransform.localScale = Vector3.one;
        _tweencoin = tmCoin.rectTransform.DOPunchScale(Vector3.one*0.5f, 0.2f, 10, 1)
                        .OnComplete(()=> tmCoin.rectTransform.localScale = Vector3.one);
    }

    private int lastLife;
    void UpdateLife()
    {
        if (lastLife == GameManager.life)
            return;

        tmLife.text = GameManager.life.ToString();

        if (GameManager.life <= 0)
        {
            ShowInfo("GAME OVER", 5f);
            GameManager.IsGameover = true;
        }

        lastLife = GameManager.life;
    }
}
