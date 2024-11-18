using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private readonly string bulletTag = "BULLET";
    [SerializeField] private GameObject bloodEffect;
    public float hp = 100f;
    public Vector3 hpBarOffset = new Vector3(0f, 2.2f, 0f);
    public Image hpBarImage;
    public GameObject hpBar;
    private void OnEnable()
    {
        StartCoroutine(SetHpBar());
    }
        
    IEnumerator SetHpBar()
    {
        yield return new WaitForSeconds(0.1f);
        
         hpBar = ObjectPoolingManager.poolingManager.GetEnemyHpBarPool();
        hpBarImage = hpBar.GetComponentsInChildren<Image>()[1];
        
        var _hpBar = hpBar.GetComponent<EnemyHpBar>();
        _hpBar.targetTr = this.gameObject.transform;
        _hpBar.offSet = hpBarOffset;
        //hpBarImage.gameObject.transform.parent.gameObject.SetActive(true);
        hpBar.gameObject.SetActive(true);
    }

    void Start()
    {
        bloodEffect = Resources.Load("Effects/BulletImpactFleshBigEffect") as GameObject;
    }
    #region ������Ÿ�� ����� �浹 ���� isTrigger üũ�� ��� OnTriggerEnter
    //private void OnCollisionEnter(Collision col)
    //{
    //    if(col.collider.CompareTag(bulletTag))
    //    {
    //        col.gameObject.SetActive(false);
    //        // ���� ��ġ�� �ѱ�  Collision ����ü�ȿ� contacts��� �迭�� �ִ�.
    //        ShowBloodEffect(col);
    //        curhp -= col.gameObject.GetComponent<BulletCtrl>().damage;
    //        curhp = Mathf.Clamp(curhp, 0f, 100f);
    //        if (curhp <= 0f)
    //            Die();
    //    }

    //}
    #endregion

    void OnDamage(object[] _params)
    {
        

        ShowBloodEffect((Vector3)_params[0]);
        hp -= (float)_params[1];
        hp = Mathf.Clamp(hp, 0f, 100f);
        hpBarImage.fillAmount = (float)hp * 0.01f;
        if (hp <= 0f)
        {
            Die();
            //hpBarImage.GetComponentsInChildren<Image>()[1].color = Color.clear;
        }
    }
    void Die()
    {
       // Debug.Log("���!");
        GetComponent<EnemyAI>().state = EnemyAI.State.DIE;
        GameManager.G_instance.KillScore();





    }
    private void ShowBloodEffect(Vector3 col)
    {
        Vector3 pos = col; //��ġ
        Vector3 _normal = col.normalized; //����
        Quaternion rot = Quaternion.FromToRotation(Vector3.forward, _normal);
        GameObject blood = Instantiate(bloodEffect, pos, rot);
        Destroy(blood, 1.0f);
    }
}
