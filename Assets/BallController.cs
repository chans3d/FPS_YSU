using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BallController : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveForce = 10f;
    public float jumpForce = 5f;
    public float maxSpeed = 8f;

    [Header("카메라 설정")]
    public float cameraDistance = 6f;
    public float cameraHeight = 3f;
    public float cameraSmooth = 2f;
    public float maxCameraTurnAngle = 125f;
    public float reverseRotationBoost = 3f;

    [Header("기타")]
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.6f;

    private Rigidbody rb;
    private Transform cam;
    private Vector3 moveDirection = Vector3.forward;

    private bool isGrounded = false;
    private bool canJump = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            canJump = false;
        }
    }

    void FixedUpdate()
    {
        HandleMovement();
        CheckGrounded();
        UpdateCamera();
    }

    void CheckGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
        if (isGrounded)
        {
            canJump = true;
        }
    }

    void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 input = new Vector3(h, 0f, v);

        if (input.magnitude > 0.1f)
        {
            Quaternion camRot = Quaternion.Euler(0, cam.eulerAngles.y, 0);
            moveDirection = camRot * input.normalized;

            rb.AddForce(moveDirection * moveForce);

            Vector3 horizontalVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            if (horizontalVel.magnitude > maxSpeed)
            {
                horizontalVel = horizontalVel.normalized * maxSpeed;
                rb.velocity = new Vector3(horizontalVel.x, rb.velocity.y, horizontalVel.z);
            }
        }
    }

    void UpdateCamera()
    {
        if (!isGrounded) return;

        Vector3 flatVelocity = rb.velocity;
        flatVelocity.y = 0;

        float inputH = Input.GetAxis("Horizontal");
        float inputV = Input.GetAxis("Vertical");

        if (flatVelocity.magnitude < 0.5f && Mathf.Abs(inputH) < 0.1f && Mathf.Abs(inputV) < 0.1f)
            return;

        Vector3 velocityDirection = flatVelocity.normalized;

        Vector3 targetPosition = transform.position - velocityDirection * cameraDistance + Vector3.up * cameraHeight;
        cam.position = Vector3.Lerp(cam.position, targetPosition, Time.deltaTime * cameraSmooth);

        Quaternion desiredRotation;

        if (Input.GetKey(KeyCode.S))
        {
            // 뒤로 갈 때 즉시 공을 바라보게 (LookAt 방식)
            desiredRotation = Quaternion.LookRotation(transform.position - cam.position);
            cam.rotation = Quaternion.Slerp(cam.rotation, desiredRotation, Time.deltaTime * cameraSmooth * reverseRotationBoost);
        }
        else
        {
            // 일반 이동 시 회전 제한 적용
            desiredRotation = Quaternion.LookRotation(transform.position - cam.position);
            float angleDiff = Quaternion.Angle(cam.rotation, desiredRotation);

            if (angleDiff <= maxCameraTurnAngle)
            {
                cam.rotation = Quaternion.Slerp(cam.rotation, desiredRotation, Time.deltaTime * cameraSmooth);
            }
        }
    }
}
