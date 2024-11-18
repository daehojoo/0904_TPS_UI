using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    public static ObjectPoolingManager poolingManager;
    [SerializeField] private GameObject bulletPrefab; //�Ѿ� ������
    [SerializeField] private GameObject E_bulletPrefab; //�Ѿ� ������
    public int maxPool = 10; //������Ʈ Ǯ�� ���� �� ���� 
    public List<GameObject> bulletPoolList;
    public List<GameObject> E_bulletPoolList;
    [Header("EnemyObjectPool")]
    public GameObject EnemyPrefab;
    public List<GameObject> EnemyPoolList;
    public List<Transform> SpawnPointList;
    [Header("EnemyHpBarObjectPool")]
    public GameObject enemyHpbar;
    public List<GameObject> EnemyHpPoolList;
    public Canvas uiCanvas;



    void Awake() //Awake-> OnEnable-> Start
    {
        if (poolingManager == null)
            poolingManager = this;
        else if (poolingManager != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        bulletPrefab = Resources.Load("Bullet") as GameObject;
        E_bulletPrefab = Resources.Load("E_Bullet") as GameObject;
        EnemyPrefab = Resources.Load<GameObject>("Enemy");
        enemyHpbar = Resources.Load<GameObject>("enemyHpBar");




        CreateBulletPool(); //������Ʈ Ǯ�� ���� �Լ� 
        CreateE_BulletPool();
        CreateEnemyPool();
        CreateEnemyHpPool();
    }
    private void Start()
    {
        var spawnPoint = GameObject.Find("SpawnPoints");
        if (spawnPoint != null)
            spawnPoint.GetComponentsInChildren<Transform>(SpawnPointList);

         SpawnPointList.RemoveAt(0);
        if (SpawnPointList.Count > 0)
            StartCoroutine(CreateEnemy());

    }

    private void CreateEnemyPool()
    {
        GameObject EnemyGroup = new GameObject("EnemyGroup");
        for (int i = 0; i < 10; i++)
        {
            var enemyObj = Instantiate(EnemyPrefab, EnemyGroup.transform);
            enemyObj.name = $"{(i + 1).ToString()} ��";
            enemyObj.tag = "ENEMY";
            enemyObj.SetActive(false);
            EnemyPoolList.Add(enemyObj);
        }
    }
    private void CreateEnemyHpPool()
    {
        GameObject EnemyHpBarGroup = new GameObject("EnemyHpBarGroup");
        for (int i = 0; i < 10; i++)
        {
            var enemyObj = Instantiate(enemyHpbar, uiCanvas.transform);
            enemyObj.name = $"HPBar";
            enemyObj.SetActive(false);
            EnemyHpPoolList.Add(enemyObj);
        }
    }


    void CreateBulletPool()
    {               //���� ������Ʈ ���� 
        GameObject playerBulletGroup = new GameObject("PlayerBulletGroup");
        for(int i =0;i< 150;i++)
        {
            var _bullet = Instantiate(bulletPrefab,playerBulletGroup.transform);
            _bullet.name = $"{(i+1).ToString()} ��";
            _bullet.SetActive(false);
            bulletPoolList.Add(_bullet);
        }

    }
    void CreateE_BulletPool()
    {               //���� ������Ʈ ���� 
        GameObject EnemyBulletGroup = new GameObject("Enemy_BulletGroup");
        for (int i = 0; i < 150; i++)
        {
            var E_bullet = Instantiate(E_bulletPrefab, EnemyBulletGroup.transform);
            E_bullet.name = $"{(i + 1).ToString()} ��";
            E_bullet.SetActive(false);
            E_bulletPoolList.Add(E_bullet);
        }

    }
    public GameObject GetBulletPool()
    {
        for(int i = 0; i< bulletPoolList.Count;i++)
        {      
            //��Ȱ�� �Ǿ��ٸ� activeSelf�� Ȱ��ȭ ��Ȱ�� ���θ� �˷���
            if (bulletPoolList[i].activeSelf ==false)
            {
                return bulletPoolList[i];
            }
        }
        return null;
    }
    public GameObject GetEnemyHpBarPool()
    {
        for (int i = 0; i < 10; i++)
        {
            //��Ȱ�� �Ǿ��ٸ� activeSelf�� Ȱ��ȭ ��Ȱ�� ���θ� �˷���
            if (EnemyHpPoolList[i].activeSelf == false)
            {
                return EnemyHpPoolList[i];
            }
        }
        return null;
    }
    public GameObject E_GetBulletPool()
    {
        for (int i = 0; i < E_bulletPoolList.Count; i++)
        {
            //��Ȱ�� �Ǿ��ٸ� activeSelf�� Ȱ��ȭ ��Ȱ�� ���θ� �˷���
            if (E_bulletPoolList[i].activeSelf == false)
            {
                return E_bulletPoolList[i];
            }
        }
        return null;
    }
    IEnumerator CreateEnemy()
    {
        while (!GameManager.G_instance.isGameOver)
        {
            yield return new WaitForSeconds(3f);
            if (GameManager.G_instance.isGameOver) yield break;
              //������ ���� �Ǹ� �ڷ�ƾ�� ���� �ؼ� ���� ��ƾ�� ���� ���� ����
            foreach(GameObject _enemy in EnemyPoolList)
            {
                if(_enemy.activeSelf ==false)
                {
                    int idx = Random.Range(0,SpawnPointList.Count-1);
                    _enemy.transform.position = SpawnPointList[idx].position;

                    _enemy.transform.rotation = SpawnPointList[idx].rotation;
                    _enemy.gameObject.SetActive(true);
                    break;
                }
            }
            
        }
    }
   
}
