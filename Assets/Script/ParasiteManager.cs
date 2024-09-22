using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParasiteManager : MonoBehaviour
{
    public GameObject parasitePrefab;  // 기생충 프리팹
    public GameObject Fish;  // 손님 물고기
    public int minParasiteCount = 3;  // 최소 기생충 수
    public int maxParasiteCount = 8;  // 최대 기생충 수

    private List<GameObject> parasites = new List<GameObject>();  // 기생충 리스트
    private Mesh fishMesh;  // 물고기 Mesh 데이터

    void Start()
    {
        // 손님 물고기 Mesh 가져오기
        MeshFilter fishMeshFilter = Fish.GetComponent<MeshFilter>();
        if (fishMeshFilter != null)
        {
            fishMesh = fishMeshFilter.mesh;

            // 삼각형과 정점 데이터가 있는지 확인
            if (fishMesh == null || fishMesh.triangles.Length == 0 || fishMesh.vertices.Length == 0)
            {
                Debug.LogError("물고기 Mesh에 삼각형 또는 정점 데이터가 없습니다.");
                return;
            }
        }
        else
        {
            Debug.LogError("물고기 MeshFilter가 없습니다.");
            return;
        }

        // 게임 시작 시, 기생충을 물고기 표면에 바로 부착
        SpawnParasites();
    }

    // 기생충 생성 및 물고기 표면에 부착
    void SpawnParasites()
    {
        if (fishMesh == null)
        {
            Debug.LogError("물고기 Mesh가 없습니다.");
            return;
        }

        int parasiteCount = Random.Range(minParasiteCount, maxParasiteCount);  // 랜덤한 기생충 수 생성

        for (int i = 0; i < parasiteCount; i++)
        {
            // 물고기 Mesh에서 임의의 표면 위치를 계산
            Vector3 spawnPosition = GetRandomSurfacePointOnFish();

            if (spawnPosition != Vector3.zero)
            {
                // 기생충 프리팹 생성
                GameObject parasite = Instantiate(parasitePrefab, spawnPosition, Quaternion.identity);

                // 기생충을 물고기 표면에 부착
                parasite.transform.SetParent(Fish.transform);  // 물고기의 자식으로 설정하여 고정

                // 기생충 리스트에 추가
                parasites.Add(parasite);
            }
            else
            {
                Debug.LogWarning("물고기 표면을 찾지 못했습니다.");
            }
        }
    }

    // 물고기의 Mesh에서 임의의 표면 위치 찾기
    Vector3 GetRandomSurfacePointOnFish()
    {
        if (fishMesh == null) return Vector3.zero;

        // Mesh의 삼각형 배열에서 임의의 삼각형 선택
        int[] triangles = fishMesh.triangles;
        Vector3[] vertices = fishMesh.vertices;

        if (triangles.Length == 0 || vertices.Length == 0)
        {
            Debug.LogError("삼각형 또는 정점 데이터가 없습니다.");
            return Vector3.zero;
        }

        // 삼각형 개수 계산
        int triangleCount = triangles.Length / 3;

        // 임의의 삼각형 선택 (각 삼각형은 3개의 Vertex로 구성됨)
        int triangleIndex = Random.Range(0, triangleCount) * 3;

        // 삼각형의 각 Vertex 가져오기
        Vector3 vertex1 = vertices[triangles[triangleIndex]];
        Vector3 vertex2 = vertices[triangles[triangleIndex + 1]];
        Vector3 vertex3 = vertices[triangles[triangleIndex + 2]];

        // 삼각형 내부의 임의의 점을 선택 (Barycentric 좌표계를 사용)
        Vector3 randomPointInTriangle = GetRandomPointInTriangle(vertex1, vertex2, vertex3);

        // Local 좌표계를 World 좌표계로 변환
        return Fish.transform.TransformPoint(randomPointInTriangle);
    }

    // 삼각형 내부에서 임의의 점을 선택하는 함수 (Barycentric 좌표계)
    Vector3 GetRandomPointInTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        float u = Random.value;
        float v = Random.value;

        // 두 랜덤 값이 삼각형 내부에 있는지 보장
        if (u + v > 1)
        {
            u = 1 - u;
            v = 1 - v;
        }

        // Barycentric 좌표계를 사용하여 삼각형 내부의 임의의 점 계산
        return v1 + u * (v2 - v1) + v * (v3 - v1);
    }
}