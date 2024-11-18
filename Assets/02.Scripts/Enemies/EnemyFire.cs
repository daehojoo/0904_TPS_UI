using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    [SerializeField] private AudioClip fireClip;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform playerTr;//������ ���ϱ� ���ؼ�
    [SerializeField] private Transform enemyTr;//������ ���ϱ� ���ؼ�
    [SerializeField] private Transform firePos;
    private readonly int hashFire = Animator.StringToHash("FireTrigger");
    private readonly int hashReload = Animator.StringToHash("ReloadTrigger");
    private float nextFire = 0.0f; // ���� �ð��� �߻��� �ð� ���� ����
    private readonly float fireRate = 0.1f; // �Ѿ� �߻� ����
    private readonly float damping = 10.0f; // �÷��̾ ���� ȸ���� �ӵ� 
    public bool isFire = false;
    [Header("Reload")]
    [SerializeField] private readonly float reloadTime = 2.0f; //������ �ð�
    [SerializeField] private readonly int maxBullet = 10; //10���϶� ������ �ϱ����� Max��
    [SerializeField] private int curBullet = 0; //���� �Ѿ� �� 
    [SerializeField] private bool isReload = false; // ������ ����
    [SerializeField] private WaitForSeconds reloadWs; // ��ŸƮ �ڷ�ƾ���� �ð� ���� ����
    [SerializeField] private AudioClip reloadClip; //������ ���� 
    public MeshRenderer muzzleFlash;
    void Start()
    {
        firePos = transform.GetChild(4).GetChild(0).GetChild(0).transform;
        muzzleFlash = transform.GetChild(4).GetChild(0).GetChild(0).GetChild(0).GetComponent<MeshRenderer>();
        muzzleFlash.enabled = false;
        animator = GetComponent<Animator>();
        enemyTr = GetComponent<Transform>();
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
        fireClip = Resources.Load("Sounds/p_m4_1_1") as AudioClip;
        reloadClip = Resources.Load<AudioClip>("Sounds/p_reload 1_1");
        curBullet = maxBullet;
        reloadWs = new WaitForSeconds(reloadTime);
    }
    void Update()
    {
        if (GameManager.G_instance.isGameOver) return;
        if(isFire && !isReload)
        {
            if(Time.time >= nextFire)
            {
                Fire();
                nextFire = 2;
            }
            Vector3 playerNormal = playerTr.position - enemyTr.position;
            Quaternion rot = Quaternion.LookRotation(playerNormal.normalized);
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, damping * Time.deltaTime);
        }


    }
    void Fire()
    {
        var E_bullet = ObjectPoolingManager.poolingManager.E_GetBulletPool();

        E_bullet.transform.position = firePos.position;
        E_bullet.transform.rotation = firePos.rotation;

        E_bullet.SetActive(true);

        animator.SetTrigger(hashFire);
        SoundManager.S_instance.PlaySound(firePos.position, fireClip);

        isReload = (--curBullet % maxBullet) == 0;
        if(isReload)
        {
            StartCoroutine(Reloading());
        }
        StartCoroutine(ShowMuzzleFlash() );
    }
    IEnumerator Reloading()
    {
        animator.SetTrigger(hashReload);//������ �ִϸ��̼� ����
        SoundManager.S_instance.PlaySound(transform.position, reloadClip);
                                  //������ ���� ����
        yield return reloadWs; //������ �ð���ŭ ����ϴ� ���� ����� �纸

        curBullet = maxBullet; // ���� �Ѿ��� �ٽ� 10�߷� 
        isReload = false;   //������ �Һ����� false�� 

    }
    IEnumerator ShowMuzzleFlash()
    {
        muzzleFlash.enabled = true;
       
        muzzleFlash.transform.localScale = Vector3.one * Random.Range(1.5f,2.3f);
        Quaternion rot = Quaternion.Euler(0f, 0f, Random.Range(0f,360f));
        muzzleFlash.transform.localRotation = rot;

        yield return new WaitForSeconds(Random.Range(0.05f,0.2f));

        muzzleFlash.enabled = false;

    }
}
