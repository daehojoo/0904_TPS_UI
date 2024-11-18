using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager S_instance;

    public float SoundVolumn = 1.0f; //���� ����
    public bool isSoundMute = false; // ���Ұ� ���� 
   
    void Awake()
    {
        if (S_instance == null) S_instance = this;
        else if (S_instance != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    public void PlaySound(Vector3 pos ,AudioClip clip)
    {
        if (isSoundMute) return; // ���Ұ� �϶� �� �Լ��� �������� 

        GameObject soundObj = new GameObject("Sound!");
                  //������Ʈ ���� ����
        soundObj.transform.position = pos; //�Ҹ��� ��ġ�� ���� ����
        AudioSource audioSource = soundObj.AddComponent<AudioSource>();
        // ������ҽ�  ���۳�Ʈ �� ���ٸ�.
        // AddComponent<AudioSource>()�� ����� �ҽ��� ���� �� �� �ִ�.
        audioSource.clip = clip;
        audioSource.minDistance = 20f;
        audioSource.maxDistance = 50f;
        audioSource.volume = SoundVolumn;
        audioSource.Play();
        Destroy(soundObj,clip.length);
    }

}
