

// 전역 , 지역 , 멤버  => 전역 > 멤버 > 지역

// static : 정적인 <-> dynamic ( new Vector)
// 전역 클래스
public static class GameManager
{
    // 전역 변수
    public static bool IsPlaying = false;
    public static bool IsGameover = false;
    public static bool IsUiOpened = false;

    // 이동 거리
    public static double mileage; //누적거리
        public static uint mileageFinish; //누적거리


    // 획득 코인 (int -21억 ~ 21억 : 4byte , unit 0 ~ 42억 : 4byte )
    public static uint coins;

    // 
    public static int life = 3;
}


