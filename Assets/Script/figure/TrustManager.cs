using UnityEngine;
using TMPro;  // TextMeshPro 관련 기능 추가

public class TrustManager : MonoBehaviour
{
    private AngerManager angerManager;
    private ComfortManager comfortManager;
    private float trustModifier = 1.0f;  // 기본 상태

    public TextMeshProUGUI trustText;  // 신뢰/불신 상태를 표시할 TextMeshProUGUI

    // 적응도 관련 변수
    private float angerAdaptationRate = 1.0f;  // 불쾌 적응도
    private float comfortAdaptationRate = 1.0f;  // 호감 적응도
    private const float adaptationDecreaseRate = 0.1f;  // 적응도 감소율
    private const float minAdaptationRate = 0.5f;  // 적응도의 최소값

    void Start()
    {
        angerManager = GetComponent<AngerManager>();
        comfortManager = GetComponent<ComfortManager>();

        // AngerManager와 ComfortManager가 먼저 초기화된 후 신뢰도 계산
        Invoke("ApplyTrustSystem", 0.1f);  // 0.1초 후 신뢰도 시스템 적용 (초기화가 끝난 후 호출)
    }

    // 신뢰/불신 시스템을 적용하는 함수
    public void ApplyTrustSystem()
    {
        float comfort = comfortManager.comfort;
        float anger = angerManager.anger;

        // 호감 지수가 불쾌 지수의 2배 이상일 경우
        if (comfort > 2 * anger)
        {
            trustModifier = 0.8f;  // 불쾌 지수 상승폭 1.2배 감소 (신뢰)
        }
        // 불쾌 지수가 호감 지수의 2배 이상일 경우
        else if (anger > 2 * comfort)
        {
            trustModifier = 1.2f;  // 불쾌 지수 상승폭 1.2배 증가 (불신)
        }
        else
        {
            trustModifier = 1.0f;  // 기본 상태
        }

        // 신뢰/불신 상태 UI 업데이트
        UpdateTrustUI();
    }

    // 신뢰 지수를 UI에 업데이트하는 함수
    void UpdateTrustUI()
    {
        if (trustText != null)
        {
            if (trustModifier == 0.8f)
            {
                trustText.text = "Status: Trust";  // 신뢰 상태 표시
                trustText.color = Color.blue;  // 신뢰일 때 파란색
            }
            else if (trustModifier == 1.2f)
            {
                trustText.text = "Status: Distrust";  // 불신 상태 표시
                trustText.color = Color.red;  // 불신일 때 빨간색
            }
            else
            {
                trustText.text = "Status: Neutral";  // 기본 상태 표시
                trustText.color = Color.white;  // 중립일 때 흰색
            }
        }
    }

    // 적응도를 적용하는 함수
    public void ApplyAdaptation(bool isComfortAction)
    {
        if (isComfortAction)
        {
            // 호감 증가 액션인 경우
            comfortAdaptationRate -= adaptationDecreaseRate;  // 호감 적응도 감소
            comfortAdaptationRate = Mathf.Max(minAdaptationRate, comfortAdaptationRate);  // 최소값으로 제한
            angerAdaptationRate = 1.0f;  // 불쾌 적응도 초기화
        }
        else
        {
            // 불쾌 증가 액션인 경우
            angerAdaptationRate -= adaptationDecreaseRate;  // 불쾌 적응도 감소
            angerAdaptationRate = Mathf.Max(minAdaptationRate, angerAdaptationRate);  // 최소값으로 제한
            comfortAdaptationRate = 1.0f;  // 호감 적응도 초기화
        }

        // 신뢰 시스템 적용
        ApplyTrustSystem();
    }

    // 현재 신뢰 수치 반환
    public float GetTrustModifier()
    {
        return trustModifier;
    }

    // 현재 불쾌 적응도 반환
    public float GetAngerAdaptationRate()
    {
        return angerAdaptationRate;
    }

    // 현재 호감 적응도 반환
    public float GetComfortAdaptationRate()
    {
        return comfortAdaptationRate;
    }
}