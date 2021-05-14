using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//데이터 직렬화?
[System.Serializable]
public class Sound
{
    public string name; //곡 이
    public AudioClip clip; //곡
}

public class SoundManager : MonoBehaviour
{
    //이 스크립트를 어디서든지 접근 가능하도록 공유자원으로 만들어준다.
    static public SoundManager instance;
    #region singleton
    //개체 생성시 최초 실행
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject); //다른 신으로 이동할 때에 삭제되지 않도록? 해준다?
        }
        else
            Destroy(gameObject); //다른 신에서 다시 이 신으로 돌아왔을때 기존의 사운드메니져는 Awake가 실행되지 않으므로 그대로 살아남게 되고
                                      //또다시 생겨난 사운드메니저는 삭제해준다. 이 말은 신 이동이 일어날때에 해당 신의 모든 객체의 Destroy가 이루어진다는 것을 의미한다.
    }
    #endregion singleton

    public AudioSource[] audioSourceEffects;
    public AudioSource audioSourceBgm;

    public string[] playSoundName;

    public Sound[] effectSounds;
    public Sound[] bgmSounds;

    void Start()
    {
        playSoundName = new string[audioSourceEffects.Length];
        PlayBGM("trimBGM");
    }

    public void PlaySE(string _name)
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if (_name == effectSounds[i].name)
            {
                for (int j = 0;  j < audioSourceEffects.Length;  j++)
                {
                    if (!audioSourceEffects[j].isPlaying)
                    {
                        audioSourceEffects[j].clip = effectSounds[i].clip;
                        audioSourceEffects[j].Play();
                        playSoundName[j] = _name;
                        return;
                    }
                }
                Debug.Log("모든 가용 오디오소스가 사용중입니다.");
                return;
            }
        }
        Debug.Log(_name + " 사운드가 SoundManager에 등록되지 않았습니다.");
    }

    public void StopAllSE()
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
             audioSourceEffects[i].Stop();
    }

    public void StopSE(string _name)
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            if (_name == playSoundName[i])
            {
                audioSourceEffects[i].Stop();
                return;                
            }

        }
        Debug.Log("재생중인 " + _name + " 사운드가 없습니다.");
    }

    public void PlayBGM(string _name)
    {
        for (int i = 0; i < bgmSounds.Length; i++)
        {
            if (_name == bgmSounds[i].name)
            {
                audioSourceBgm.clip = bgmSounds[i].clip;
                audioSourceBgm.volume = 0.2f;
                audioSourceBgm.Play();
                return;
            }
        }
    }

    public void StopBGM()
    {
        audioSourceBgm.Stop();
    }
}
