using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//�÷��̰� �Ǿ��� �� �跲 ������ ���� �ϰ� ����� 
//5�� �跲�� �Ѿ˿� ������ ���� ���������� �����ϱ� 
public class BarrelCtrl : MonoBehaviour
{
    [SerializeField] private GameObject Effect; //���� ����Ʈ 
    [SerializeField] private Texture[] textures;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private int hitCount = 0;
    [SerializeField] private AudioClip ExpClip;
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private Mesh[] meshes;
    private readonly string bulletTag = "BULLET";
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        meshes = Resources.LoadAll<Mesh>("Meshes");
        textures = Resources.LoadAll<Texture>("BarrelTextures");
        meshRenderer.material.mainTexture = textures[Random.Range(0,textures.Length)];
        Effect = Resources.Load("ExpEffect") as GameObject;
        ExpClip = Resources.Load("Sounds/grenade_exp_1") as AudioClip;
    }
    #region ������Ÿ�� ����� �浹���� 
    //private void OnCollisionEnter(Collision col)
    //{
    //    if(col.gameObject.CompareTag(bulletTag))
    //    {
    //        if(++hitCount ==5)
    //        {

    //            ExplosionBarrel();
    //        }
    //    }

    //}
    #endregion

    void OnDamage(object[] _params)
    {
        Vector3 firePos = (Vector3)_params[0]; //�߻���ġ
        Vector3 hitPos = (Vector3)_params[1]; // ���� ��ġ 
        // ���� ��ǥ���� �߻���ġ ���� = �Ÿ��� ������ ���� �� �ִ�.
        Vector3 incomeVector = hitPos - firePos; //Ray�� ������ ���ϱ� ����
        // ���� ���δ� �Ի� ���Ͷ�� �Ѵ�.
        incomeVector = incomeVector.normalized; //�Ի纤�͸� ����ȭ ���ͷ� ����
        //Ray�� hit ��ǥ�� �Ի纤���� ������ ���� ���� 
        GetComponent<Rigidbody>().AddForceAtPosition(incomeVector * 1500f,
            hitPos);
        // � ������ ���� ��Ƽ� ������ �����ǰ� �Ϸ��� �Ҷ� ȣ�� �Ǵ� �޼���

        if (++hitCount == 5)
            ExplosionBarrel();
        
    }
    void ExplosionBarrel()
    {
        Camera.main.GetComponent<CameraShake>().TurnOn();
        //���� ����Ʈ ȿ��
        GameObject eff = Instantiate(Effect,transform.position,Quaternion.identity);
        //Debug.Log(eff.transform.position);
        Destroy(eff,2.0f);

        Collider[] Cols = Physics.OverlapSphere(transform.position, 20f, 1 << 7 | 1<<8);
             //�跲 �ڱ��ڽ� ��ġ���� 20�ݰ濡 �跲 ���̾ Cols��� �迭�� ��´�
        foreach(Collider col in Cols)
        {
            Rigidbody rigidbody = col.GetComponent<Rigidbody>();
            if(rigidbody != null)
            {
                rigidbody.mass = 1.0f; //�跲 ���Ը� ������ 
                rigidbody.AddExplosionForce(1000, transform.position, 20f, 1200f);
                //������ٵ� Ŭ���� ���� �Լ��� AddExplosionForce(���ķ�, ��ġ ,�ݰ�,���� �ڱ�ġ�� ��)
                col.gameObject.SendMessage("ExpDie",SendMessageOptions.DontRequireReceiver);
            }
            Invoke("BarrelMassChange", 3.0f);
        }
        SoundManager.S_instance.PlaySound(transform.position, ExpClip);
        //��׷��� �޽��� ����
        int idx = Random.Range(0, meshes.Length);
        meshFilter.sharedMesh = meshes[idx];
        GetComponent<MeshCollider>().sharedMesh = meshes[idx];
    }
    void BarrelMassChange()
    {
        Collider[] Cols = Physics.OverlapSphere(transform.position, 20f, 1 << 8);
        //�跲 �ڱ��ڽ� ��ġ���� 20�ݰ濡 �跲 ���̾ Cols��� �迭�� ��´�
        foreach (Collider col in Cols)
        {
            Rigidbody rigidbody = col.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.mass = 60.0f; 
             
            }


        }
    }
}
