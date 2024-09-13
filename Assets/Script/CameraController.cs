using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public GameObject Target;               // 카메라가 따라다닐 타겟
    private Vector3 currentVelocity;  // 카메라의 현재 속도를 저장할 변수

    public float offsetX = 0.0f;            // 타깃 기준 카메라의 x좌표
    public float offsetY = 10.0f;           // 타깃 기준 카메라의 y좌표
    public float offsetZ = -10.0f;          // 타깃 기준 카메라의 z좌표

    public float CameraSpeed = 10.0f;       // 카메라의 속도
    public float mouseSensitivity = 100.0f; // 마우스 감도
    private float xRotation = 0.0f;         // 카메라의 x축 회전값
    private float yRotation = 0.0f;         // 카메라의 y축 회전값

    public LayerMask obstacleLayers;        // 충돌을 감지할 레이어 마스크
    public float minDistance = 1.0f;        // 타겟과 카메라 사이의 최소 거리

    Vector3 TargetPos;                      // 타겟의 위치

    void Start() {
        // 커서를 고정하고 숨김
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {
        
    }

    void FixedUpdate()
    {
        HandleCameraRotation();

        Vector3 desiredPosition = CalculateDesiredCameraPosition();
        desiredPosition = HandleCameraCollision(desiredPosition);
        MoveCamera(desiredPosition);
    }

    void LateUpdate() {
        
    }



    // 카메라 회전 처리
    private void HandleCameraRotation() {
        // 마우스 입력을 받아 카메라 회전 계산
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * 0.01f;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * 0.01f;

        xRotation -= mouseY;  // 상하 회전
        yRotation += mouseX;  // 좌우 회전
        xRotation = Mathf.Clamp(xRotation, -80f, 80f); // 위아래 회전을 제한

        // A와 D 키로 카메라의 y축 회전값을 추가로 조정
        if (Input.GetKey(KeyCode.A)) {
            yRotation -= mouseSensitivity * 0.01f; // 왼쪽 회전
        }
        if (Input.GetKey(KeyCode.D)) {
            yRotation += mouseSensitivity * 0.01f; // 오른쪽 회전
        }

        // 카메라의 회전을 바로 적용
        Quaternion targetRotation = Quaternion.Euler(xRotation, yRotation, 0.0f);
        transform.localRotation = targetRotation; // 바로 적용하여 회전과 이동을 동기화
    }

    // 카메라가 목표로 하는 위치 계산
    private Vector3 CalculateDesiredCameraPosition() {
        // 타겟 위치 + 각각 카메라 방향마다 얼마나 떨어질지
        return Target.transform.position + transform.forward * offsetZ + transform.right * offsetX + transform.up * offsetY;
    }

    // 충돌 감지를 통해 카메라 위치 보정
    private Vector3 HandleCameraCollision(Vector3 desiredPosition) {
        RaycastHit hit;
        if (Physics.Raycast(Target.transform.position, (desiredPosition - Target.transform.position).normalized, out hit, -offsetZ, obstacleLayers)) {
            // 충돌 시 카메라 위치 보정
            float distance = Mathf.Clamp(hit.distance, minDistance, -offsetZ);
            desiredPosition = Target.transform.position + transform.forward * -distance + transform.right * offsetX + transform.up * offsetY;
        }

        return desiredPosition;
    }

    // 카메라 이동 처리
    private void MoveCamera(Vector3 desiredPosition) {
        // 카메라의 위치를 부드럽게 이동
        transform.position = Vector3.Lerp(transform.position, desiredPosition, 0.01f * CameraSpeed);
    }
}