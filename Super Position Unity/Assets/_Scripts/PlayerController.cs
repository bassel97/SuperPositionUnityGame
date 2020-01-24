using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    #region Variables

    private Rigidbody rb;
    private Vector3 rbVelocity;
    private Vector3 savedVelocity;

    [SerializeField] private float playerDimension = 1.0f;

    [Header("Movement Data")]
    [SerializeField] private float playerHorzSpeed = 0;
    [SerializeField] private float playerJumpPower = 0;
    [SerializeField] private float wallJumpPower = 0;
    [SerializeField] [Range(0, 1)] private float wallDampingAmount = 0;

    [Header("Is Grounded Data")]
    [SerializeField] float isGroundedRayLength = 0;
    [SerializeField] LayerMask groundLayer = 0;
    private bool isGrounded = false;
    private bool groundJumped = false;      //To prevent corner double Jump power

    [Header("Wall Jump Data")]
    [SerializeField] float wallReadyRayLength = 0;
    [SerializeField] private LayerMask wallLayer = 0;
    private bool isWallJumpReadyLeft = false;
    private bool isWallJumpReadyRight = false;
    [SerializeField] float yVelocityLimiter = 5;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        rbVelocity = rb.velocity;

        groundJumped = false;

        SetIsGrounded();
        SetIsWallJumpReady();

        MovePlayer();
        PlayerJump();
        PlayerWallJump();

        rb.velocity = rbVelocity;
    }
    #endregion

    #region Methods
    public void FreezePlayer()
    {
        //savedVelocity = rb.velocity;
        rb.isKinematic = true;
        enabled = false;
    }

    public void UnfreezePlayer()
    {
        rb.isKinematic = false;
        enabled = true;
        //rb.velocity = savedVelocity;
    }

    private void MovePlayer()
    {
        float horzInput = Input.GetAxis("Horizontal");

        float wallDamping = ((isWallJumpReadyLeft && horzInput < 0) || (isWallJumpReadyRight && horzInput > 0)) ? wallDampingAmount : 1.0f;

        rbVelocity.x = horzInput * playerHorzSpeed * wallDamping;
    }

    private void PlayerJump()
    {
        if (!isGrounded)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(transform.up * playerJumpPower);
            groundJumped = true;
        }
    }

    private void SetIsGrounded()
    {
        Vector3 rayCenterR = transform.position + (transform.right * 0.4f * playerDimension) + (0.1f * playerDimension * transform.up);
        Vector3 rayCenterL = transform.position + (-transform.right * 0.4f * playerDimension) + (0.1f * playerDimension * transform.up);
        Vector3 direction = -transform.up * isGroundedRayLength;

        isGrounded = Physics.Raycast(rayCenterR, direction, out RaycastHit hit, isGroundedRayLength, groundLayer)
            || Physics.Raycast(rayCenterL, direction, out hit, isGroundedRayLength, groundLayer);
    }

    private void SetIsWallJumpReady()
    {
        Vector3 rayCenterLeftUp = transform.position - (0.4f * playerDimension * transform.right) + (0.9f * playerDimension * transform.up);
        Vector3 rayCenterLeftDown = transform.position - (0.4f * playerDimension * transform.right) + (0.1f * playerDimension * transform.up);
        Vector3 directionLeft = -transform.right * wallReadyRayLength;

        Vector3 rayCenterRightUp = transform.position + (0.4f * playerDimension * transform.right) + (0.9f * playerDimension * transform.up);
        Vector3 rayCenterRightDown = transform.position + (0.4f * playerDimension * transform.right) + (0.1f * playerDimension * transform.up);
        Vector3 directionRight = transform.right * wallReadyRayLength;

        isWallJumpReadyRight = Physics.Raycast(rayCenterRightUp, directionRight, out RaycastHit hit, wallReadyRayLength, wallLayer)
            || Physics.Raycast(rayCenterRightDown, directionRight, out hit, wallReadyRayLength, wallLayer);

        isWallJumpReadyLeft = Physics.Raycast(rayCenterLeftUp, directionLeft, out hit, wallReadyRayLength, wallLayer)
            || Physics.Raycast(rayCenterLeftDown, directionLeft, out hit, wallReadyRayLength, wallLayer);
    }

    private void PlayerWallJump()
    {
        if (!(isWallJumpReadyLeft || isWallJumpReadyRight) || groundJumped)
            return;

        if (rbVelocity.y > yVelocityLimiter)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
            rb.AddForce(transform.up * wallJumpPower);
    }
    #endregion
}
