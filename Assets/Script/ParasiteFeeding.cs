using UnityEngine;

public class ParasiteFeeding : MonoBehaviour
{
    public float detectionRadius = 2.0f;  // 탐지 반경 
    public HungerManager hungerManager;  // HungerManager 참조
    public TrustManager trustManager;  // TrustManager 참조
    public AngerManager angerManager;  // AngerManager 참조

    public float comfortIncreaseAmount = 5.0f;  // 기생충을 먹을 때 증가하는 호감 지수
    public float comfortAdaptationRate = 1.0f;  // 초기 쾌락 적응률 (조정이 필요할 수 있음)

    public ComfortManager comfortManager;  // comfortManager 참조
    public float hungerIncreaseAmount = 10.0f;  // 기생충을 먹을 때 증가할 허기 수치
    public LayerMask parasiteLayer;  // LayerMask를 사용하여 기생충만 탐지    
    public AudioClip eatSound;  // 기생충을 먹을 때 재생될 사운드
    private AudioSource audioSource;  // 오디오 소스 참조

    private GameObject lastDetectedParasite = null;  // 마지막으로 탐지된 기생충
    void Start()
    {
        // AudioSource 컴포넌트를 가져옵니다.
        audioSource = GetComponent<AudioSource>();
    }    
    
    void Update()
    {

        // 물고기 주변에 있는 기생충 탐지
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, parasiteLayer);

        GameObject detectedParasite = null;

        if (hitColliders.Length > 0)
        {
            // 탐지된 기생충 중 첫 번째 기생충 탐지
            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Parasite"))
                {
                    detectedParasite = hitCollider.gameObject;
                    break;  // 첫 번째 탐지된 기생충만 처리
                }
            }
        }

        // 탐지된 기생충이 있으면 색상 변경
        if (detectedParasite != null)
        {
            // 탐지 중인 기생충의 색상을 빨간색으로 변경
            ChangeParasiteColor(detectedParasite, Color.red);

            // 이전에 탐지된 기생충과 다른 기생충이면 색상 복구
            if (lastDetectedParasite != null && lastDetectedParasite != detectedParasite)
            {
                ChangeParasiteColor(lastDetectedParasite, Color.white);
            }

            // 'e' 키를 눌렀을 때 기생충을 먹기
            if (Input.GetKeyDown(KeyCode.E))
            {
                EatParasite(detectedParasite);
            }

            // 현재 탐지된 기생충을 저장
            lastDetectedParasite = detectedParasite;
        }
        else
        {
            // 탐지된 기생충이 없을 때 이전에 탐지된 기생충의 색상을 복구
            if (lastDetectedParasite != null)
            {
                ChangeParasiteColor(lastDetectedParasite, Color.white);
                lastDetectedParasite = null;  // 탐지된 기생충 초기화
            }
        }
    }

    // 기생충을 먹는 함수
    void EatParasite(GameObject parasite)
    {
        Debug.Log(parasite.name + " 기생충을 먹었습니다.");

        // 허기 수치 증가
        hungerManager.IncreaseHunger(15.0f);

        // 호감 지수 증가 (적응도 적용)
        float comfortIncreaseAmount = Random.Range(0.5f, 5.0f) * trustManager.GetComfortAdaptationRate();
        comfortManager.IncreaseComfort(comfortIncreaseAmount);

        // 불쾌 지수 감소
        angerManager.DecreaseAnger(comfortIncreaseAmount / 2.0f);

        // 쾌락적응 적용 (호감 행동으로 처리)
        trustManager.ApplyAdaptation(true);  // true = 호감 행동

        // 신뢰 시스템 적용
        trustManager.ApplyTrustSystem();


                // 효과음 재생
        if (eatSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(eatSound);
        }

        // 탐지된 기생충 오브젝트 삭제
        Destroy(parasite);
    }

    // 기생충의 색상을 변경하는 함수
    void ChangeParasiteColor(GameObject parasite, Color color)
    {
        Renderer parasiteRenderer = parasite.GetComponent<Renderer>();
        if (parasiteRenderer != null)
        {
            parasiteRenderer.material.color = color;
        }
    }

    // OverlapSphere 탐지 범위를 디버깅하기 위한 Gizmos 표시
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);  // OverlapSphere 탐지 범위 표시
    }
}