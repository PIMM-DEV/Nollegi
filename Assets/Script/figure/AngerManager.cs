using UnityEngine;
using TMPro;  // TextMeshPro 관련 기능을 사용하기 위해 추가

public class AngerManager : MonoBehaviour
{
    public float anger = 0.0f;  // 현재 Anger 지수
    public float maxAnger = 100.0f;  // 최대 Anger 지수
    public TextMeshProUGUI angerText;  // TextMeshProUGUI 컴포넌트 참조

    void Start()
    {
        UpdateAngerUI();  // 게임 시작 시 UI 업데이트
    }

    // Anger 지수를 증가시키는 함수
    public void IncreaseAnger(float amount)
    {
        anger += amount;
        if (anger > maxAnger)
        {
            anger = maxAnger;  // Anger 지수는 최대값을 넘지 않도록 설정
        }

        Debug.Log("현재 Anger 지수: " + anger);
        UpdateAngerUI();  // Anger 지수가 변경될 때마다 UI 업데이트
    }

    // Anger 지수를 UI에 업데이트하는 함수
    void UpdateAngerUI()
    {
        if (angerText != null)
        {
            angerText.text = "Anger: " + anger.ToString("F1");  // 소수점 1자리까지 표시
        }
    }
}