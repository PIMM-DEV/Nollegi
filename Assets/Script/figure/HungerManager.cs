using UnityEngine;
using TMPro;  // TextMeshPro를 사용하기 위해 추가

public class HungerManager : MonoBehaviour
{
    public float hunger = 0.0f;  // 현재 허기 수치
    public float maxHunger = 100.0f;  // 최대 허기 수치
    public TextMeshProUGUI hungerText;  // Text 대신 TextMeshProUGUI로 변경

    void Start()
    {
        UpdateHungerUI();
    }

    public void IncreaseHunger(float amount)
    {
        hunger += amount;
        if (hunger > maxHunger)
        {
            hunger = maxHunger;
        }

        UpdateHungerUI();
    }

    void UpdateHungerUI()
    {
        hungerText.text = "Hunger: " + hunger.ToString("F1");
    }
}