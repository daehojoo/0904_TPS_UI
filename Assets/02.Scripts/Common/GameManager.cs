using DataInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager G_instance;
    public bool isGameOver= false;
    public CanvasGroup inventoryCG;
    public Text killTXT;
    public int killCount = 0;
    [Header("DataManager")]
    [SerializeField]
    DataManager dataManager;
    //public GameData gameData;
    public GameDataObject gameData;
    //�κ��丮 �������� ���� �Ǿ��� �� �߻� ��ų �̺�Ʈ ����
    public delegate void ItemChangeDelegate();
    public static event ItemChangeDelegate OnItemChange;
    [SerializeField] private GameObject slotList;
    public GameObject[] itemObjects;

    void Awake()
    {
        if (G_instance == null)
            G_instance = this;
        else if (G_instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
         
        dataManager = GetComponent<DataManager>();
        dataManager.Initialize();

        inventoryCG = GameObject.Find("Inventory").GetComponent<CanvasGroup>();
        killTXT  = GameObject.Find("Canvas-UI").transform.GetChild(7).GetComponent<Text>();
        OnInventoryOpen(false);
        LoadGameData();
    }
    void LoadGameData() //���� �ʱ� ������ �ε�
    {              //�÷��̾� �����۷��� 
                   //killCount = PlayerPrefs.GetInt("KILLCOUNT", 0);
                   //Ű���� ������ �Ѵ�.
        //GameData data = dataManager.Load();
        //gameData.hp = data.hp;
        //gameData.damage = data.damage;
        //gameData.speed = data.speed;
        //gameData.killCount = data.killCount;
        //gameData.equipItem = data.equipItem;
        //ScriptableObject�� ���������� ���� �� �� �ֱ⿡
        //������ �ε��ϴ� ������ �ʿ� ����.
        if (gameData.equipItem.Count > 0)
            InventorySetUp();

        killTXT.text = "<color=#00ff00>KILL:</color> " + "<color=#ff0000>" + gameData.killCount.ToString("0000") + "</color>";
    }
    void InventorySetUp()
    {
        var slots = slotList.GetComponentsInChildren<Transform>();
        for(int i =0; i< gameData.equipItem.Count; i++)
        {
            for(int j =1;j< slots.Length; j++)
            {     //Slot ������ �ٸ� �������� ������ ���� �ε����� �ٷ� �Ѿ
                if (slots[j].childCount > 0) continue;
                int itemIndex = (int)gameData.equipItem[i].itemType;
                //������ �������� ������ ���� �ε����� ����
                itemObjects[itemIndex].GetComponent<Transform>().SetParent(slots[j].transform);
                //�������� �θ�� ���� ���ӿ�����Ʈ �� �ȴ�.
                itemObjects[itemIndex].GetComponent<ItemInfo>().itemData = gameData.equipItem[i];
                //�������� ItemInfo Ŭ������ itemData�� �ε��� gameData.equipItem[i] ������ ���� ����
                break;
                
            }
        }

    }
    void SaveGameData()
    {
        //dataManager.Save(gameData);
        //.asset ���Ͽ� ������ ���� 
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(gameData);
#endif
    }
    //�κ��丮�� �������� �߰� �� ���� ������ ������ ������Ʈ �ϴ� �Լ�
    public void AddItem(Item item)
    {      // ���� �����ۿ� ���� �������̸� �߰����� �ʰ� ��������
        if (gameData.equipItem.Contains(item)) return;
        gameData.equipItem.Add(item);
        //�������� GameData.equipItem �迭�� �߰� 
        switch(item.itemType)
        {
            case Item.ItemType.HP:
                 //������ ��� ��Ŀ� ���� ó�� 
                if (item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.hp += item.value;
                else
                    gameData.hp += gameData.hp * item.value;

                break;
            case Item.ItemType.DAMAGE:
                if (item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.damage += item.value;
                else
                    gameData.damage += gameData.damage * item.value;

                break;
            case Item.ItemType.SPEED:
                if (item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.speed += item.value;
                else
                    gameData.speed += gameData.speed * item.value;
                break;
            case Item.ItemType.GRENADE:

                break;


        }
        OnItemChange();
        //�������� ����� ���� �ǽð����� �ݿ� �ϱ� ����
        //�̺�Ʈ�� �߻� ��Ŵ
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(gameData);
#endif
    }
    public void RemoveItem(Item item) 
    {
        //�κ��丮 UI���� Ư�� �������� ���� �� �������� ����Ǳ� ����
        gameData.equipItem.Remove(item);
        switch (item.itemType)
        {
            case Item.ItemType.HP:
                //������ ��� ��Ŀ� ���� ó�� 
                if (item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.hp -= item.value;
                else
                    gameData.hp = gameData.hp  /(1.0f+item.value);

                break;
            case Item.ItemType.DAMAGE:
                if (item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.damage -= item.value;
                else
                    gameData.damage = gameData.damage / (1.0f+ item.value);

                break;
            case Item.ItemType.SPEED:
                if (item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.speed -= item.value;
                else
                    gameData.speed = gameData.speed /(1.0f+ item.value);
                break;
            case Item.ItemType.GRENADE:

                break;


        }
        OnItemChange();
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(gameData);
#endif

    }


    public bool isPaused =false;
    public void OnPauseClick()
    {
        isPaused = !isPaused;
        Time.timeScale = (isPaused) ? 0.0f : 1f;
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        var scripts = playerObj.GetComponents<MonoBehaviour>();
          //�÷��̾ �ִ� MonoBehaviour ��� �ϴ� ��ũ��Ʈ�� 
        foreach (var script in scripts)
        {
            script.enabled = !isPaused;
        }
        var canvasGroup = GameObject.Find("Panel-Weapon").GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts =!isPaused;
    }
   
    public void OnInventoryOpen(bool isOpened)
    {
       
        Time.timeScale = (isOpened) ? 0f : 1f;
        inventoryCG.alpha = (isOpened) ? 1.0f : 0.0f;
        inventoryCG.interactable = isOpened;
        inventoryCG.blocksRaycasts=isOpened;

    }
    public void KillScore()
    {
        //++killCount;
        ++gameData.killCount;
        killTXT.text = "<color=#00ff00>KILL:</color> " + "<color=#ff0000>" + gameData.killCount.ToString("0000") + "</color>";
        //PlayerPrefs.SetInt("KILLCOUNT", killCount);
        //ų���� ���� �Ѵ�. 
        //PlayerPrefs �� ���ȼ��� ����.
        //�ǹ����� ���ٰ� �Ѵٸ�  ��ȣȭ �ؼ� ���� �ؾ� �Ѵ�.

    }
    private void OnDisable()
    {
       // PlayerPrefs.DeleteKey("KILLCOUNT");
        
    }
    private void OnApplicationQuit()
    {
        SaveGameData();
    }
}
