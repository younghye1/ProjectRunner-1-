using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CustomInspector;


public class PhaseManager : MonoBehaviour
{

    [HorizontalLine("기본속성"), HideField] public bool _l0;
    [SerializeField] float updateInterval = 1f;


    [HorizontalLine("Phase 속성"), HideField] public bool _l1;
    [SerializeField] List<Phase> mileageList = new List<Phase>();

    private TrackManager trkMgr;
    private IngameUI uiIngame;




    IEnumerator Start()
    {
        trkMgr = FindFirstObjectByType<TrackManager>();
        yield return new WaitUntil( ()=> trkMgr != null );

        uiIngame = FindFirstObjectByType<IngameUI>();
        yield return new WaitUntil( ()=> uiIngame != null );

        GetFinishline();

        yield return new WaitUntil( ()=> GameManager.IsPlaying );
        StartCoroutine(IntervalUpdate());
    }


    IEnumerator IntervalUpdate()
    {
        if(mileageList == null || mileageList.Count <= 0)
            yield break;


        int i = 0;

        while( true )
        {
            Phase phase = mileageList[i];
            if (GameManager.mileage >= phase.Mileage)
            {
                SetPhase(phase);
                i++;
            }

            if (i >= mileageList.Count)
            {
                GameClear(phase);
                yield break;
            }

            yield return new WaitForSeconds(updateInterval);
        }
    }

    void GetFinishline()
    {
        Phase phaseEnd = mileageList.LastOrDefault();       

        GameManager.mileageFinish = phaseEnd.Mileage;
    }



    void SetPhase(Phase phase)
    {
        uiIngame?.ShowInfo(phase.Name);
        trkMgr?.SetPhase(phase);
    }

    void GameClear(Phase phase)
    {
        uiIngame?.ShowInfo(phase.Name);
        trkMgr?.SetPhase(phase);

        GameManager.IsPlaying = false;
        GameManager.IsGameover = true;
    }

}
