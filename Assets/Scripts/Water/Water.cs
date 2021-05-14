using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Water : MonoBehaviour
{
    [SerializeField] private float waterDrag; //물 속 중력.
    private float originDrag;

    [SerializeField] private Color waterColor; //물 속 색깔.
    [SerializeField] private float waterFogDensity; //물 속이 탁한 정도.

    [SerializeField] private Color waterNightColor; //밤 상태의 물 속 색깔.
    [SerializeField] private float waterNightFogDensity;

    private Color originColor; //원래 색깔.
    private float originFogDensity;

    [SerializeField] private Color originNightColor;
    [SerializeField] private float originNightFogDensity;

    [SerializeField] private string sound_WaterOut;
    [SerializeField] private string sound_WaterIn;
    [SerializeField] private string sound_Breathe;

    [SerializeField] private float breatheTime;
    private float currentBreatheTime;

    [SerializeField] private float totalOxygen;
    private float currentOxygen;
    private float temp ;

    [SerializeField] private GameObject go_BaseUi;
    [SerializeField] private Text text_TotalOxygen;
    [SerializeField] private Text text_CurrentOxygen;
    [SerializeField] private Image img_Gauge;

    private StatusController thePlayerStat;

    void Start()
    {
        thePlayerStat = FindObjectOfType<StatusController>();
        currentOxygen = totalOxygen; 
        originColor = RenderSettings.fogColor;
        originFogDensity = RenderSettings.fogDensity;

        originDrag = 0; //플레이어의 Drag값이 0이므로 그대로 0을 준다.
        text_TotalOxygen.text = totalOxygen.ToString();
    }

    void Update()
    {
        if (GameManager.isWater)
        {
            currentBreatheTime += Time.deltaTime;
            if (currentBreatheTime >= breatheTime)
            {
                SoundManager.instance.PlaySE(sound_Breathe);
                currentBreatheTime = 0; 
            }
        }

        DecreaseOxygen();
    }

    private void DecreaseOxygen()
    {
        if (GameManager.isWater)
        {
            currentOxygen -= Time.deltaTime;
            text_CurrentOxygen.text = Mathf.RoundToInt(currentOxygen).ToString();
            img_Gauge.fillAmount = currentOxygen / totalOxygen;

            if (currentOxygen <= 0)
            {
                temp += Time.deltaTime;
                if (temp >= 1)
                {
                    thePlayerStat.DecreaseHP(1);
                    temp = 0;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            GetWater(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            GetOutWater(other);
        }
    }

    private void GetWater(Collider _player)
    {
        go_BaseUi.SetActive(true);
        SoundManager.instance.PlaySE(sound_WaterIn);
        GameManager.isWater = true;
        _player.GetComponent<Rigidbody>().drag = waterDrag;

        if (!GameManager.isNight)
        {
            RenderSettings.fogColor = waterColor;
            RenderSettings.fogDensity = waterFogDensity;
        }
        else
        {
            RenderSettings.fogColor = waterNightColor;
            RenderSettings.fogDensity = waterNightFogDensity;
        }
    }

    private void GetOutWater(Collider _player)
    {
        go_BaseUi.SetActive(false);
        currentOxygen = totalOxygen;
        SoundManager.instance.PlaySE(sound_WaterOut);
        if (GameManager.isWater)
        {
            GameManager.isWater = false;
            _player.GetComponent<Rigidbody>().drag = originDrag;

            if (!GameManager.isNight)
            {
                RenderSettings.fogColor = originColor;
                RenderSettings.fogDensity = originFogDensity; ;
            }
            else
            {
                RenderSettings.fogColor = originNightColor;
                RenderSettings.fogDensity = originNightFogDensity;
            }
        }
    }
}
