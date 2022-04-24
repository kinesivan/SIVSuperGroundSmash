 using System;
 using System.Collections;
using System.Collections.Generic;
 using Cinemachine;
 using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;
    public Camera camera;
    public CinemachineFreeLook cameraFreeLook;

    public float speed;
    public float dashSpeed;
    public float jumpForce;
    public float superJumpForce;
    public float superJumpChargeSpeed;
    public float smashingSpeed;
    public float smashingAccel;

    public float groundCheckDist;
    public Transform groundCheck;
    public LayerMask groundCheckMask;

    public GameObject smashMarker;
    public Transform playerMesh;
    
    
    private const float DashChargeSpeed = 3;
    private const float MovementDrag = 0.96f;

    private Rigidbody _rb;
    private bool _canJump;
    private bool _frozen;
    private bool _didSuperJump;
    private float _dashCharge;
    private float _superJumpCharge;
    private float _t;
    private float _lastSuperJumpTick;
    private float _lastJumpTick;
    private Vector3 _smashing;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        _t++;
        
        // Update dashing state when active.
        if (Input.GetKey(KeyCode.LeftShift) && _canJump)
        {
            _dashCharge = Mathf.Lerp(_dashCharge, 1f, DashChargeSpeed * Time.deltaTime);
        }
        else
        {
            _dashCharge = Mathf.Lerp(_dashCharge, 0f, DashChargeSpeed * Time.deltaTime);
        }
        
        // Update jump charging state when active.
        if (Input.GetButton("Jump") && _canJump)
        {
            animator.SetBool("JumpCharging", true);
            _superJumpCharge = Mathf.Lerp(_superJumpCharge, 1f, superJumpChargeSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("JumpCharging", false);
        }
        
        // Enter smash-aiming state.
        if (Input.GetMouseButton(0) && _didSuperJump && _smashing == Vector3.zero)
        {
            cameraFreeLook.m_Orbits[0].m_Radius = 1f;
            cameraFreeLook.m_Orbits[1].m_Height = 4;
            cameraFreeLook.m_Orbits[1].m_Radius = 1;
            cameraFreeLook.m_Orbits[2].m_Height = 4;
            smashMarker.SetActive(true);
            var ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundCheckMask))
            {
                var newVel2 = _rb.velocity;
                newVel2.Scale(new Vector3(0.5f, 0.5f, 0.5f));
                _rb.velocity = newVel2;
                smashMarker.transform.position = hit.point;
            }
        }

        cameraFreeLook.m_Lens.FieldOfView = Mathf.Lerp(40, 60, _dashCharge);
        var currentSpeed = Mathf.Lerp(speed, dashSpeed, _dashCharge);
        
        // Retrieve keyboard input.
        var horizInput = Input.GetAxis("Horizontal");
        var vertInput = Input.GetAxis("Vertical");

        // Retrieve normalized camera basis vectors.
        var forward = camera.transform.forward;
        var right = camera.transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        var moveDir = Vector3.zero;
        // Calculate and apply movement velocity.
        if (!_frozen)
        {
            moveDir = ((forward * vertInput) + (right * horizInput)).normalized;
            if (moveDir.magnitude >= 0.05)
                _rb.AddForce(new Vector3(moveDir.x * currentSpeed, 0, moveDir.z * currentSpeed));
        }

        // Apply custom drag.
        var newVel = _rb.velocity;
        newVel.x *= MovementDrag;
        newVel.z *= MovementDrag;
        _rb.velocity = newVel;
        
        // Apply flying movement
        if (_smashing != Vector3.zero)
        {
            var newVel3 = (_smashing - _rb.position).normalized * smashingSpeed;
            _rb.velocity = Vector3.Lerp(_rb.velocity, newVel3, smashingAccel * Time.deltaTime);
        }
        
        // Apply animation.
        var animVelZ = Vector3.Dot(moveDir, camera.transform.forward);
        var animVelX = Vector3.Dot(moveDir, camera.transform.right);
        animator.SetFloat("VelocityZ", animVelZ, 0.2f, Time.deltaTime);
        animator.SetFloat("VelocityX", animVelX, 0.2f, Time.deltaTime);

        // Update inner mesh rotation (we don't rotate root to preserve forward direction).
        if (moveDir != Vector3.zero)
        {
            playerMesh.rotation = Quaternion.LookRotation(moveDir);
        }
    }

    private void Update()
    {
        // Check whether player can jump and handle accordingly.
        _canJump = Physics.CheckSphere(groundCheck.position, groundCheckDist, groundCheckMask);
        if (_canJump && Input.GetButtonUp("Jump"))
        {
            animator.SetBool("Falling", true);
            _lastJumpTick = _t;

            if (_superJumpCharge > 0.25f)
            {
                _rb.AddForce(Vector3.up * (superJumpForce * _superJumpCharge));
                _didSuperJump = true;
                _lastSuperJumpTick = _t;
            }
            else
            {
                _rb.AddForce(Vector3.up * jumpForce);
            }

            return;
        }

        if (_canJump && _t >= _lastJumpTick+10)
        {
            _smashing = Vector3.zero;
            _rb.useGravity = true;
            _frozen = false;
            animator.SetBool("Falling", false);
            animator.SetBool("Flying", false);
        }

        if (_canJump && _didSuperJump && _t >= _lastSuperJumpTick+2)
        {
            smashMarker.SetActive(false);
            _superJumpCharge = 0;
            _didSuperJump = false;
        }
        
        // Transition to smashing state.
        if (Input.GetMouseButtonUp(0) && (_didSuperJump || !_canJump) && _smashing == Vector3.zero)
        {
            cameraFreeLook.m_Orbits[0].m_Radius = 3f;
            cameraFreeLook.m_Orbits[1].m_Height = 2.5f;
            cameraFreeLook.m_Orbits[1].m_Radius = 4f;
            cameraFreeLook.m_Orbits[2].m_Height = 0.4f;
            smashMarker.SetActive(false);
            animator.SetBool("Flying", true);
            _smashing = smashMarker.transform.position;
            _frozen = true;
            _rb.useGravity = false;
        }
    }
}
