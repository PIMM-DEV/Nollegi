using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NollegiMoveController : MonoBehaviour {
    public float moveSpeed = 3.0f;           // 물고기의 이동 속도
    public float verticalSpeed = 3.0f;       // 물고기의 상승/하강 속도
    public float smoothness = 0.95f;         // 미끄러지는 효과를 위한 감속 비율
    public float rotationSpeed = 2.0f;       // 물고기의 회전 속도
    public float tiltAmount = 10.0f;         // 물고기의 상하 기울기 정도
    public float bobbingSpeed = 0.5f;        // 물에 둥실둥실 떠 있는 효과의 속도
    public float bobbingAmount = 0.2f;       // 물에 둥실둥실 떠 있는 효과의 범위

    private Rigidbody rb;                    // 물고기의 Rigidbody 컴포넌트
    private Quaternion initialRotationOffset; // 물고기의 초기 회전 보정 값
    private Vector3 lastMoveDirection;       // 마지막 이동 방향을 기억
    private bool isBobbing = false;          // 둥실둥실 상태인지 여부
    private float bobbingTime = 0.0f;        // 둥실둥실 시간

    void Start() {
        Initialize();
    }
    void FixedUpdate()
    {
        HandleRotation();
        HandleMovement();
        HandleBobbingEffect();
    }

    void Update() {
        
    }

    // 초기화 메소드
    private void Initialize() {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;               // 물고기는 물 속에 있으므로 중력을 사용하지 않음
        initialRotationOffset = Quaternion.Euler(0, 90, 0);  // 초기 회전 보정 값
    }

    // 물고기 회전 처리 메소드
    private void HandleRotation() {
        Transform cameraTransform = Camera.main.transform;
        float tilt = CalculateTilt();

        // 최종 회전값 계산 및 적용
        Quaternion targetRotation = cameraTransform.rotation * Quaternion.Euler(tilt, 0, 0) * initialRotationOffset;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * 0.01f);
    }

    // 상하 기울기 계산 메소드
    private float CalculateTilt() {
        if (Input.GetKey(KeyCode.Space)) {
            return -tiltAmount;  // 상승 시 위로 기울임
        }
        else if (Input.GetKey(KeyCode.LeftShift)) {
            return tiltAmount;   // 하강 시 아래로 기울임
        }
        return 0.0f;
    }

    // 물고기 이동 처리 메소드
    private void HandleMovement() {
        Vector3 moveDirection = CalculateMoveDirection();

        if (moveDirection != Vector3.zero) {
            ApplyMovement(moveDirection);
        } else {
            ApplySlidingEffect();
        }
    }

    // 이동 방향 계산 메소드
    private Vector3 CalculateMoveDirection() {
        Transform cameraTransform = Camera.main.transform;

        Vector3 moveDirection = cameraTransform.forward * Input.GetAxis("Vertical") * moveSpeed;
        moveDirection += cameraTransform.right * Input.GetAxis("Horizontal") * moveSpeed;

        // 상승/하강 추가
        if (Input.GetKey(KeyCode.Space)) {
            moveDirection += Vector3.up * verticalSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftShift)) {
            moveDirection += Vector3.down * verticalSpeed;
        }

        return moveDirection;
    }

    // 이동 속도를 적용하는 메소드
    private void ApplyMovement(Vector3 moveDirection) {
        rb.velocity = Vector3.Lerp(rb.velocity, moveDirection, smoothness * 0.01f);
        lastMoveDirection = moveDirection;   // 마지막 이동 방향 저장
        isBobbing = false;                   // 이동 중이므로 둥실둥실 상태 비활성화
    }

    // 미끄러지는 효과를 적용하는 메소드
    private void ApplySlidingEffect() {
        rb.velocity = Vector3.Lerp(rb.velocity, lastMoveDirection * 0.1f, smoothness * 0.01f);

        // 속도가 거의 0이 되면 둥실둥실 효과 활성화
        if (rb.velocity.magnitude < 0.1f) {
            isBobbing = true;
        }
    }

    // 둥실둥실 떠 있는 효과 처리 메소드
    private void HandleBobbingEffect() {
        if (isBobbing) {
            bobbingTime += 0.01f * bobbingSpeed;
            Vector3 bobbing = new Vector3(0, Mathf.Sin(bobbingTime) * bobbingAmount, 0);
            transform.position += bobbing * 0.01f;
        }
    }
}