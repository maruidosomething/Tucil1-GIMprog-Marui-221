using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dashing : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerCam;
    private Rigidbody rb;
    private PlayerMovement pm;

    [Header("Dashing")]
    public float dashForce = 20f;
    public float dashUpwardForce = 2f;
    public float maxDashYSpeed = 0f;
    public float dashDuration = 0.25f;

    [Header("Charges & Cooldown")]
    public int maxDashCharges = 2;
    private int dashesLeft;
    public float dashCd = 2.5f;
    private float dashCdTimer;

    [Header("Camera Effects")]
    public PlayerCam cam;
    public float dashFov = 95f;

    [Header("UI & Visuals")]
    public Image[] dashChargeIcons; 
    public CanvasGroup dashWooshOverlay; 
    public float wooshFadeSpeed = 5f;

    [Header("Settings")]
    public bool useCameraForward = true;
    public bool allowAllDirections = true;
    public bool disableGravity = false;
    public bool resetVel = true;

    [Header("Input")]
    public KeyCode dashKey = KeyCode.E;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();

        dashesLeft = maxDashCharges;
        
        if (dashWooshOverlay != null)
            dashWooshOverlay.alpha = 0f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(dashKey))
            Dash();

        RegenerateCharges();
        UpdateUI();
    }

    private void Dash()
    {
        if (dashesLeft <= 0) return;

        dashesLeft--;

        pm.dashing = true;
        pm.maxYSpeed = maxDashYSpeed;

        cam.DoFov(dashFov);

        if (dashWooshOverlay != null)
        {
            StopCoroutine(FadeWooshEffect());
            StartCoroutine(FadeWooshEffect());
        }

        Transform forwardT = useCameraForward ? playerCam : orientation;
        Vector3 direction = GetDirection(forwardT);
        Vector3 forceToApply = direction * dashForce + orientation.up * dashUpwardForce;

        if (disableGravity)
            rb.useGravity = false;

        delayedForceToApply = forceToApply;
        Invoke(nameof(DelayedDashForce), 0.025f);
        Invoke(nameof(ResetDash), dashDuration);
    }

    private Vector3 delayedForceToApply;
    private void DelayedDashForce()
    {
        if (resetVel)
            rb.linearVelocity = Vector3.zero;

        rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }

    private void ResetDash()
    {
        pm.dashing = false;
        pm.maxYSpeed = 0;
        cam.DoFov(85f);

        if (disableGravity)
            rb.useGravity = true;
    }

    private void RegenerateCharges()
    {
        if (dashesLeft < maxDashCharges)
        {
            dashCdTimer += Time.deltaTime;
            if (dashCdTimer >= dashCd)
            {
                dashesLeft++;
                dashCdTimer = 0f;
            }
        }
        else
        {
            dashCdTimer = 0f;
        }
    }

    private void UpdateUI()
    {
        for (int i = 0; i < dashChargeIcons.Length; i++)
        {
            if (dashChargeIcons[i] != null)
            {
                dashChargeIcons[i].enabled = (i < dashesLeft);
            }
        }
    }

    private IEnumerator FadeWooshEffect()
    {
        dashWooshOverlay.alpha = 1f;
        dashWooshOverlay.transform.localScale = Vector3.one; 

        float timer = 0f;
        
        while (timer < dashDuration)
        {
            timer += Time.deltaTime;
            float currentScale = Mathf.Lerp(1f, 1.5f, timer / dashDuration);
            dashWooshOverlay.transform.localScale = new Vector3(currentScale, currentScale, 1f);
            yield return null;
        }

        while (dashWooshOverlay.alpha > 0)
        {
            dashWooshOverlay.alpha -= Time.deltaTime * wooshFadeSpeed;
            yield return null;
        }
        
        dashWooshOverlay.transform.localScale = Vector3.one; 
    }

    private Vector3 GetDirection(Transform forwardT)
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 direction = Vector3.zero;

        if (allowAllDirections)
            direction = forwardT.forward * verticalInput + forwardT.right * horizontalInput;
        else
            direction = forwardT.forward;

        if (verticalInput == 0 && horizontalInput == 0)
            direction = forwardT.forward;

        return direction.normalized;
    }
}