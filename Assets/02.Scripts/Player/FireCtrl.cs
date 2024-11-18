using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
[System.Serializable]
public struct PlayerSound
{
    public AudioClip[] fire;
    public AudioClip[] reload;
}

public class FireCtrl : MonoBehaviour
{
    public enum WeaponType
    {
        RIFLE = 1,SHOTGUN = 2
    }
    public Animator animator;
    public PlayerSound playerSound;
    public WeaponType curType = WeaponType.SHOTGUN;
    public Transform firePos;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip fireclip;
    [SerializeField] private Player player;
    [SerializeField] private ParticleSystem muzzleFlash;
    private readonly string EnemyTag = "ENEMY";
    private readonly string BarrelTag = "BARREL";
    private readonly string WallTag = "WALL";
    private const float DIST = 20f;
    public Image magazineImg; //źâ �̹���
    public Text magazineText; // ���� �Ѿ� �� text UI
    public float reloadTime = 2.0f; // ������ �ð�
    public bool isReloading = false;  // ������ ���� 
    public int maxBullet = 10; //�Ѿ� �ִ��
    public int curBullet = 10; //���� �Ѿ� �� 
    public Sprite[] weaponIcons;
    public Image weaponImg;
   
    public bool isFire = false;
    private float nextFire; // ���� �߻� �ð��� ���� �� ����
    public float fireRate = 0.3f; //�Ѿ� �߻� ����
    private int enemyLayer; //�� ���̾� ��ȣ�� ���� ����
    private int boxLayer;
    private int barrelLayer;
    private int layerMask;
    void Start()
    {
        magazineImg = GameObject.Find("Canvas-UI").transform.GetChild(1)
            .GetChild(2).GetComponent<Image>();
        magazineText = GameObject.Find("Canvas-UI").transform.GetChild(1).
            GetChild(0).GetComponent<Text>();
        weaponImg = GameObject.Find("Canvas-UI").transform.GetChild(3)
            .GetChild(0).GetComponent<Image>();
        player = GetComponent<Player>();
        source = GetComponent<AudioSource>();
        fireclip = Resources.Load("Sounds/p_ak_1_1") as AudioClip;
        weaponIcons = Resources.LoadAll<Sprite>("WeaponIcons");
        muzzleFlash.Stop();
        enemyLayer = LayerMask.NameToLayer("ENEMY");
        barrelLayer = LayerMask.NameToLayer("BARREL");
        boxLayer = LayerMask.NameToLayer("BOXES");
        layerMask = 1<< enemyLayer| 1<< barrelLayer|1 << boxLayer;
                       animator = GetComponent<Animator>();
       
    }

    
    void Update()
    {
        RaycastHit hit;
        // ������ ���� ������Ʈ�� ��ġ�� �Ÿ� ������ �ִ� ����ü
        if (Physics.Raycast(firePos.position, firePos.forward, out hit, 25f, layerMask))
            isFire = (hit.collider.CompareTag("ENEMY"));
        else
            isFire = false;


       //if (EventSystem.current.IsPointerOverGameObject())
          //  return;

         if(!isReloading && isFire && !player.isRunning)
        {
            if(Time.time >nextFire)
            {
                --curBullet;
                Fire();
                
                if(curBullet==0)
                {
                    StartCoroutine(Reloading());
                }
                nextFire = Time.time + fireRate;
            }
        }



        //UI�� Ư�� �̺�Ʈ�� �߻� �Ǹ� ���� ������.
        //Debug.DrawRay(firePos.position, firePos.forward * 100f, Color.red);

        #region ���콺 ���� ��ư���� �߻� 
        if (Input.GetMouseButtonDown(0)&&!isReloading)
        {   
            if(!player.isRunning)
            {
                animator.SetTrigger("fireTrigger");
                --curBullet; // �Ѿ� ���� ���� 
                Fire();
                muzzleFlash.Play();
                if (curBullet == 0)
                    StartCoroutine(Reloading());
            }
              
        }
        else if (Input.GetMouseButtonUp(0))
        {
            muzzleFlash.Stop();
        }
        #endregion
    }
    void Fire()
    {
        #region projectile movement  ���

        ////������Ʈ Ǯ�� ����� �ƴ� �� 
        ////Instantiate(bulletPrefab,firePos.position,firePos.rotation);
        ////������Ʈ Ǯ�� ��� �϶� ���� 
        //var _bullet = ObjectPoolingManager.poolingManager.GetBulletPool();
        //if(_bullet != null)
        //{
        //    _bullet.transform.position = firePos.position;
        //    _bullet.transform.rotation = firePos.rotation;
        //    _bullet.SetActive(true);
        //}
        #endregion
        RaycastHit hit; //������ ������Ʈ ������ �浹�����̳�
                        // �Ÿ����� �˷��ִ� ���� ����ü
                        //������ ����� �� �¾Ҵ� �� ����
        if (Physics.Raycast(firePos.position, firePos.forward, out hit, DIST))
        {       //�±װ˻� 
            if (hit.collider.gameObject.tag == EnemyTag)
            {
                
                object[] _params = new object[2];
                _params[0] = hit.point; //ù��° �迭���� ���� ��ġ�� ����
                _params[1] = 25f; // �ι�° �迭���� ������ ���� ����
                // ������ ���� ������Ʈ�� �Լ��� ȣ�� �ϸ鼭 �Ű����� ���� ����
                
                hit.collider.gameObject.SendMessage("OnDamage", _params,
                       SendMessageOptions.DontRequireReceiver);
            }
            //if (hit.collider.gameObject.tag == WallTag)
            //{
            //    //object[] _params = new object[1];
            //    //_params[0] = hit.point;
              

            //    //hit.collider.gameObject.SendMessage("OnDamage", _params,
            //    //       SendMessageOptions.DontRequireReceiver);

            //}
            if (hit.collider.gameObject.tag == BarrelTag)
            {
                object[] _params = new object[2];
                _params[0] = firePos.position; //�߻���ġ
                _params[1] = hit.point; // ���� ��ġ 
                hit.collider.gameObject.SendMessage("OnDamage", _params,
                       SendMessageOptions.DontRequireReceiver);

            }

        }
        source.PlayOneShot(fireclip, 1.0f);
        magazineImg.fillAmount = (float)curBullet / (float)maxBullet;
        UpdateBulletText();
    }

    private void UpdateBulletText()
    {
        //(���� �Ѿ� �� /�ִ� �Ѿ� ��) ǥ�� 
        magazineText.text = string.Format("<color=#ff0000>{0}</color> / {1} ",
            curBullet, maxBullet);
    }
    public void OnChangeWeapon()
    {
        curType = (WeaponType)((int)++curType % 2);
         
        weaponImg.sprite = weaponIcons[(int)curType];
         //��������Ʈ �̹��� ��ü 

    }

    IEnumerator Reloading()
    {
        isReloading = true;

        SoundManager.S_instance.PlaySound(transform.position, playerSound.reload[(int)curType]);
        yield return
            new WaitForSeconds(playerSound.reload[(int)curType].length + 0.3f);
                                   //���ε� ���� ���̺��� 0.3 ��ŭ
        isReloading = false;
        magazineImg.fillAmount = 1.0f; //�� ��� ���� ����¸� �ٽ� ä��
        curBullet = maxBullet;
        UpdateBulletText();

    }
}
