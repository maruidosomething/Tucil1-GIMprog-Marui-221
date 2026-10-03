using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCam : MonoBehaviour
{
    public float sensX = 100f;
    public float sensY = 100f;

    public Transform orientation;
    public Transform camHolder;

    float xRotation;
    float yRotation;
    float camTilt;

    private Camera cam;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cam = GetComponent<Camera>();
        if (cam == null) cam = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * sensX * 0.01f;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensY * 0.01f;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        camHolder.rotation = Quaternion.Euler(xRotation, yRotation, camTilt);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void DoFov(float endValue)
    {
        if (cam != null)
            cam.DOFieldOfView(endValue, 0.25f);
    }

    public void DoTilt(float zTilt)
    {
        DOTween.To(() => camTilt, x => camTilt = x, zTilt, 0.25f);
    }
}