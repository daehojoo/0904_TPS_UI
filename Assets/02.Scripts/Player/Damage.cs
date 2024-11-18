using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage : MonoBehaviour
{
    private readonly string E_bulletTag = "E_BULLET";
    [SerializeField] private GameObject bloodEffect;
    [SerializeField] private Image bloodScreen;
    [SerializeField] private Image HpBar;
    public int curhp = 0;
    public int InitHp = 100;
    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;
    private void OnEnable()
    {
        GameManager.OnItemChange += UpdateSetup;
    }
    void UpdateSetup()
    {
        InitHp = (int)GameManager.G_instance.gameData.hp;
        curhp +=(int) GameManager.G_instance.gameData.hp - curhp;
    }
    void Start()
    {
        InitHp =(int)GameManager.G_instance.gameData.hp;
        curhp = InitHp;
        HpBar = GameObject.Find("Canvas-UI").transform.GetChild(2).GetChild(2).GetComponent<Image>();
        bloodScreen = GameObject.Find("Canvas-UI").transform.GetChild(0).GetComponent<Image>();
        bloodEffect = Resources.Load("Effects/BulletImpactFleshBigEffect") as GameObject;
       
        HpBar.color = Color.green;
    }
    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag(E_bulletTag))
        {
            col.gameObject.SetActive(false);
            // ���� ��ġ�� �ѱ�  Collision ����ü�ȿ� contacts��� �迭�� �ִ�.
            ShowBloodEffect(col);
            curhp -= 5;
            curhp = Mathf.Clamp(curhp, 0, InitHp);
            ShowHpBar();
            if (curhp <= 0)
                PlayerDie();

            StartCoroutine(ShowBloodScreen());
        }
    }

    private void ShowHpBar()
    {
        HpBar.fillAmount = (float)curhp / (float)InitHp;
        if (HpBar.fillAmount <= 0.3f)
            HpBar.color = Color.red;
        else if (HpBar.fillAmount <= 0.5f)
            HpBar.color = Color.yellow;
    }

    IEnumerator ShowBloodScreen()
    {
        bloodScreen.color = new Color(1, 0, 0,Random.Range(0.25f,0.35f));
        yield return new WaitForSeconds(0.1f);
        bloodScreen.color = Color.clear; 
        //�ؽ�ó�� ������ ���� 0 r g b  a  ���� 0
    }
    public void PlayerDie()
    {
        //GameObject[] enemies = GameObject.FindGameObjectsWithTag("ENEMY");
        //for(int i =0; i < enemies.Length; i++)
        //{
        //    enemies[i].gameObject.SendMessage("OnPlayerDie", 
        //        SendMessageOptions.DontRequireReceiver);
        //}
        //���� ������ ���� mmorpg ���ӿ� ���� ��Ÿ
        OnPlayerDie();

    }
    private void ShowBloodEffect(Collision col)
    {
        Vector3 pos = col.contacts[0].point; //��ġ
        Vector3 _normal = col.contacts[0].normal;//����

        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);
        GameObject blood = Instantiate(bloodEffect, pos, rot);
        Destroy(blood, 1.0f);
    }
}
