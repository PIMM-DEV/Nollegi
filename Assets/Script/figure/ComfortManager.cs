using UnityEngine;
using TMPro;  // TextMeshPro 관련 기능을 사용하기 위해 추가

public class ComfortManager : MonoBehaviour
{
    public float comfort = 0.0f;  // 현재 Comfort 지수
    public float maxComfort = 100.0f;  // 최대 Comfort 지수
    public TextMeshProUGUI comfortText;  // TextMeshProUGUI 컴포넌트 참조

    void Start()
    {
        InitializeComfort();  // 초기값 설정
        UpdateComfortUI();    // 게임 시작 시 UI 업데이트
    }

    // 초기값을 난수로 설정하는 함수
    void InitializeComfort()
    {
        comfort = Random.Range(40.0f, 60.0f);  // 40~60 사이의 난수값 설정
    }

    // Comfort 지수를 증가시키는 함수
    public void IncreaseComfort(float amount)
    {
        comfort += amount;
        if (comfort > maxComfort)
        {
            comfort = maxComfort;  // Comfort 지수는 최대값을 넘지 않도록 설정
        }

        Debug.Log("현재 Comfort 지수: " + comfort);
        UpdateComfortUI();  // Comfort 지수가 변경될 때마다 UI 업데이트
    }

    // Comfort 지수를 감소시키는 함수
    public void DecreaseComfort(float amount)
    {
        comfort -= amount;
        if (comfort < 0)
        {
            comfort = 0;  // Comfort 지수는 0 이하로 떨어지지 않도록 설정
        }

        Debug.Log("현재 Comfort 지수: " + comfort);
        UpdateComfortUI();  // Comfort 지수가 변경될 때마다 UI 업데이트
    }

    // Comfort 지수를 UI에 업데이트하는 함수
    void UpdateComfortUI()
    {
        if (comfortText != null)
        {
            comfortText.text = "Comfort: " + comfort.ToString("F1");  // 소수점 1자리까지 표시
        }
    }
}