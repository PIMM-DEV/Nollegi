using UnityEngine;
using TMPro;  // TextMeshPro 관련 기능을 사용하기 위해 추가

public class AngerManager : MonoBehaviour
{
    public float anger = 0.0f;  // 현재 Anger 지수
    public float maxAnger = 100.0f;  // 최대 Anger 지수
    public TextMeshProUGUI angerText;  // TextMeshProUGUI 컴포넌트 참조
    
    void Start()
    {
        // 0과 20 사이의 난수로 Anger 초기값 설정
        anger = Random.Range(0.0f, 20.0f);
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

    // Anger 지수를 감소시키는 함수
    public void DecreaseAnger(float amount)
    {
        anger -= amount;
        if (anger < 0)
        {
            anger = 0;  // Comfort 지수는 0 이하로 떨어지지 않도록 설정
        }

        Debug.Log("현재 Anger 지수: " + anger);
        UpdateAngerUI();  // Comfort 지수가 변경될 때마다 UI 업데이트
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