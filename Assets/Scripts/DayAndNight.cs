using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    [SerializeField] private float secondPerRealTimeSecond; //게임 세계의 100초는 현실세계의 1초
    [SerializeField] private float nightFogDensity; //밤 상태의 Fog 밀도
    [SerializeField] private float fogDensityCalc; //증감량 계산.

    private float dayFogDensity; //낮 상태의 Fog 밀도
    private float currentFogDensity;

    // Start is called before the first frame update
    void Start()
    {
        dayFogDensity = RenderSettings.fogDensity;       
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.right, 0.1f * secondPerRealTimeSecond * Time.deltaTime);

        if (transform.eulerAngles.x >= 170)
            GameManager.isNight = true;
        if (transform.eulerAngles.x >= 340)
            GameManager.isNight = false;

        if (GameManager.isNight)
        {
            if (currentFogDensity <= nightFogDensity )
            {
                currentFogDensity += 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
        else
        {
            if (currentFogDensity >= dayFogDensity)
            {
                currentFogDensity -= 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
    }
}
