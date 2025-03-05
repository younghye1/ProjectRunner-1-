

// 전역 , 지역 , 멤버  => 전역 > 멤버 > 지역

// static : 정적인 <-> dynamic ( new Vector)
// 전역 클래스
public static class GameManager
{
    // 전역 변수
    public static bool IsPlaying = false;
    public static bool IsGameover = true;
    public static bool IsUiOpened = false;

    
    public static double mileage; //누적 거리
    public static uint mileageFinish; // Finish 거리

    // 획득 코인 (int -21억 ~ 21억 : 4byte , unit 0 ~ 42억 : 4byte )
    public static uint coins;

    public static int life = 3;


    // 플레이어 상태 플래그
    public static PlayerState playerstate;


    public static void Reset()
    {
        IsPlaying = false;
        IsGameover = true;
        IsUiOpened = false;

        mileage = 0;
        mileageFinish = 0;
        coins = 0;
        life = 3;
        
        playerstate = 0;
    }

}


