using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour

{
    private AudioSource cameraShake;
    Vector3 cameraInitialPosition;
    private const float shakeMagnetude = 0.6f, shakeTime = 0.3f;
    public Camera mainCamera;

    void Awake()
    {
        mainCamera = this.gameObject.GetComponent<Camera>();
        cameraShake = GetComponent<AudioSource>();
    }

    public void ShakeIt()
    {
        cameraInitialPosition = this.transform.position;
        InvokeRepeating("StartCameraShaking", 0f, 0.005f);
        Invoke("StopCameraShaking", shakeTime);
        cameraShake.Play();
    }

    void StartCameraShaking()
    {
        float cameraShakingOffsetX = Random.Range(0, 20) * shakeMagnetude - shakeMagnetude * 10;
        float cameraShakingOffsetY = Random.Range(0, 20) * shakeMagnetude - shakeMagnetude * 10;
        Vector3 cameraIntermadiatePosition = mainCamera.transform.position;
        cameraIntermadiatePosition.x += cameraShakingOffsetX;
        cameraIntermadiatePosition.y += cameraShakingOffsetY;
        mainCamera.transform.position = cameraIntermadiatePosition;
    }

    void StopCameraShaking()
    {
        CancelInvoke("StartCameraShaking");
        mainCamera.transform.position = cameraInitialPosition;
    }

}
