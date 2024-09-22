using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBiting : MonoBehaviour
{
    public float detectionRadius = 2.0f;  // 물고기 탐지 반경
    public HungerManager hungerManager;  // HungerManager 참조
    public AngerManager angerManager;  // AngerManager 참조
    public float hungerIncreaseAmount = 10.0f;  // 물고기를 뜯어먹을 때 증가할 허기 수치
    public float angerIncreaseAmount = 10.0f;  // 물고기를 뜯어먹을 때 증가할 Anger 수치
    public LayerMask fishLayer;  // 물고기만 탐지하기 위한 LayerMask

    private GameObject detectedFish = null;  // 탐지된 물고기
    private MaterialPropertyBlock propBlock;  // Material Property Block 선언
    private Color originalColor;  // 원래 색상 저장 변수

    void Start()
    {
        // MaterialPropertyBlock 초기화
        propBlock = new MaterialPropertyBlock();
    }

    void Update()
    {
        // 주변 물고기 탐지
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, fishLayer);

        if (hitColliders.Length > 0)
        {
            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Fish") && hitCollider.gameObject != this.gameObject)
                {
                    detectedFish = hitCollider.gameObject;
                    break;  // 첫 번째 탐지된 물고기만 처리
                }
            }
        }
        else
        {
            detectedFish = null;  // 탐지된 물고기가 없으면 초기화
        }


        // 'Q' 키를 눌렀을 때 물고기를 뜯어먹기
        if (detectedFish != null && Input.GetKeyDown(KeyCode.Q))
        {
            BiteFish(detectedFish);
        }
    }

    // 물고기를 뜯어먹는 함수
    void BiteFish(GameObject fish)
    {
        Debug.Log(fish.name + " 물고기의 살점을 뜯어먹었습니다.");
        
        // 허기 수치 증가
        hungerManager.IncreaseHunger(hungerIncreaseAmount);
        
        // Anger 수치 증가
        angerManager.IncreaseAnger(angerIncreaseAmount);
    }
}