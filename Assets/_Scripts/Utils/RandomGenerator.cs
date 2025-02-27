using System.Collections.Generic;
using UnityEngine;


public abstract class RandomItem
{
    public string name;
    public int weight;  

    public abstract object GetItem();
}

public class RandomGenerator
{
    public List<RandomItem> items = new List<RandomItem>();

    public int totalweight; // 모든 아이템안의 비중 총 합


    // 비중 총 합 구하기
    private void CalcTotalWeight()
    {
        totalweight = 0;

        foreach(var item in items)
        {
            totalweight += item.weight;
        }
    }

    // 비중에 맞게 랜덤 발생하기

    // 3 , 4 , 7 , 1 = 4개 : 총합 15
    public RandomItem GetRandom()
    {
        int rnd = Random.Range(0, totalweight);
        int weightSum = 0;
        // 0 ~ 14 : 15

        foreach ( var i in items )
        {
            weightSum += i.weight;
            // 1루프 : 14 < 3 => 실패
            // 2루프 : 14 < 3 + 4 => 실패
            // 3루프 : 14 < 3 + 4 + 7 => 실패
            // 4루프 : 14 < 3 + 4 + 7 + 1 => 성공
            if (rnd < weightSum )
                return i;
        }

        return null;
    }

    // 아이템을 등록하고 , 비중 총 합을 다시 계산한다.
    public void AddItem(RandomItem item)
    {
        items.Add(item);

        CalcTotalWeight();
    }

}
