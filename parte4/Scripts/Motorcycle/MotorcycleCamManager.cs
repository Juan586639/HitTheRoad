using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MotorcycleCamManager : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    MotorcycleMovement movement;

    [Header("Movement")]
    public float dutchMultiplier = 3;
    float lerpTimer = 0;

    [Header("Noise")]
    CinemachineBasicMultiChannelPerlin noise;

    // Start is called before the first frame update
    void Awake()
    {
        movement = GetComponent<MotorcycleMovement>();
        //
        if (virtualCamera)
        {
            noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            noise.m_AmplitudeGain = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        lerpTimer += Time.deltaTime;
        lerpTimer = Mathf.Clamp01(lerpTimer);
        //
        virtualCamera.m_Lens.Dutch = Mathf.Lerp(virtualCamera.m_Lens.Dutch , - movement.actualSteering * dutchMultiplier, lerpTimer);
    }

    public void ResetLerpTimer()
    {
        lerpTimer = 0;
    }

    bool canShake = true;
    public void ShakeCam()
    {
        if(canShake)
        {
            StartCoroutine(ShakeCoroutine());
            canShake = false;
        }
    }

    IEnumerator ShakeCoroutine()
    {
        noise.m_AmplitudeGain = 10;
        while (noise.m_AmplitudeGain > 0)
        {
            noise.m_AmplitudeGain -= Time.unscaledDeltaTime * 20;
            yield return null;
        }
        noise.m_AmplitudeGain = 0;
        yield return new WaitForSecondsRealtime(.25f);
        canShake = true;
    }
}
