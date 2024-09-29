using UnityEngine;
using UnityEngine.UI;  // UI 관련 기능을 사용하기 위해 추가
using TMPro;  // TextMeshPro를 사용하기 위해 추가

public class HungerManager : MonoBehaviour
{
    public float hunger = 150.0f;  // 현재 허기 수치
    public float maxHunger = 150.0f;  // 최대 허기 수치
    public TextMeshProUGUI hungerText;  // Text 대신 TextMeshProUGUI로 변경

    public float baseHungerDecreaseRate = 1.0f;  // 기본 허기 감소 속도
    public float movementHungerDecreaseRate = 2.0f;  // 움직일 때 허기 감소 속도
    private float movementTime = 0.0f;  // 누적된 움직임 시간
    private bool isMoving = false;  // 플레이어가 움직이는지 여부
    private bool isDebuffActive = false;  // 디버프 활성화 여부
    public Image screenOverlay;  // 검은색 이미지 (화면을 어둡게 할 UI 이미지)

    void Start()
    {
        hunger = maxHunger;  // 게임 시작 시 허기 수치를 150으로 설정
        UpdateHungerUI();
        screenOverlay.color = new Color(0, 0, 0, 0);  // 처음에는 투명하게 설정
    }

    void Update()
        {
            float decreaseRate = baseHungerDecreaseRate;  // 기본 허기 감소 속도

            // 플레이어가 움직이고 있을 경우
            if (isMoving)
            {
                movementTime += Time.deltaTime;  // 누적된 움직임 시간 추가

                if (movementTime >= 1.0f)  // 누적 시간이 1초 이상일 때
                {
                    decreaseRate = movementHungerDecreaseRate;  // 초당 2씩 허기 감소
                }
            }

            // 허기가 20 이하이면 디버프 발생
            if (hunger <= 20.0f)
            {
                ApplyDebuff();  // 디버프 적용
                decreaseRate *= 0.5f;  // 허기 감소 속도를 50%로 감소
            }
            else
            {
                RemoveDebuff();  // 디버프 해제
            }

            // 허기 감소 처리
            hunger -= decreaseRate * Time.deltaTime;
            hunger = Mathf.Clamp(hunger, 0, maxHunger);  // 허기 수치가 0 아래로 내려가지 않도록 설정

            UpdateHungerUI();
        }

    // 플레이어가 움직일 때 호출되는 함수 (외부에서 호출)
    public void SetMovementState(bool moving)
    {
        isMoving = moving;
    }

   public void IncreaseHunger(float amount)
    {
        if (hunger >= 130.0f)  // 허기가 130 이상일 때
        {
            amount *= 0.5f;  // 섭취 증가량을 50%로 감소
        }

        hunger += amount;

        if (hunger > maxHunger)
        {
            hunger = maxHunger;  // 최대 허기를 초과하지 않도록 제한
        }

        UpdateHungerUI();
    }

    // 디버프 적용 함수
    void ApplyDebuff()
    {
        if (!isDebuffActive)
        {
            isDebuffActive = true;
            Debug.Log("Debuff applied! Hunger is 20 or below.");

            // 화면을 어둡게 만들기 위해 검은색 이미지의 투명도를 증가
            screenOverlay.color = new Color(0, 0, 0, 0.5f);  // 50% 어두운 상태
        }
    }

    // 디버프 해제 함수
    void RemoveDebuff()
    {
        if (isDebuffActive)
        {
            isDebuffActive = false;
            Debug.Log("Debuff removed! Hunger is above 20.");

            // 화면 밝게 하기 - 검은색 이미지의 투명도를 0으로 설정
            screenOverlay.color = new Color(0, 0, 0, 0);  // 투명하게 설정
        }
    }

    void UpdateHungerUI()
    {
        hungerText.text = "Hunger: " + hunger.ToString("F1");
    }
}