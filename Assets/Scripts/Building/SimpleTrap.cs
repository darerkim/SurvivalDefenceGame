using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTrap : MonoBehaviour
{
    private Rigidbody[] rigid;
    [SerializeField] private GameObject go_Meat;
    [SerializeField] private int damage;

    private bool isActivated = false;

    private AudioSource theAudio;

    [SerializeField]
    private AudioClip sound_Activated;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponentsInChildren<Rigidbody>();
        theAudio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActivated)
        {
            if (!other.CompareTag("Untagged"))
            {
                theAudio.clip = sound_Activated;
                theAudio.Play();
                isActivated = true;
                Destroy(go_Meat);

                for (int i = 0; i < rigid.Length; i++)
                {
                    rigid[i].isKinematic = false;
                    rigid[i].useGravity = true;
                }

                if (other.transform.name == "Player")
                {
                    other.transform.GetComponent<StatusController>().DecreaseHP(damage);
                }
            }
        }
    }
}
