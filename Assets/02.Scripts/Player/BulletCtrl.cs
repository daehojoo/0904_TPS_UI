using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform tr;
    [SerializeField] private TrailRenderer trailRenderer;
    public float Speed = 1500f;
    public float damage = 25f;
    void Awake()
    {
        tr = transform;
        rb = GetComponent<Rigidbody>();
        trailRenderer = GetComponent<TrailRenderer>();

        //Destroy(this.gameObject,3.0f);
        StartCoroutine(BulletDisable());
    }
    IEnumerator BulletDisable()
    {
        yield return new WaitForSeconds(3);
        this.gameObject.SetActive(false);
    }
    private void OnEnable() //������Ʈ�� Ȱ��ȭ �ɶ� �ڵ�ȣ��
    {
        damage = GameManager.G_instance.gameData.damage;
        GameManager.OnItemChange += UpdateSetup;
        rb.AddForce(tr.forward * Speed);
    }
    void  UpdateSetup()
    {
        damage = GameManager.G_instance.gameData.damage;
    }
    private void OnDisable()//������Ʈ�� ��Ȱ��ȭ �ɶ� �ڵ�ȣ��
    {
        trailRenderer.Clear();
       //tr.position  = Vector3.zero; 
        tr.rotation = Quaternion.identity;
        rb.Sleep(); // ������ �ٵ� �۵� ���� 
    }
}
