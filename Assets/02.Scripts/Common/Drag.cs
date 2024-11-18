using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IDragHandler,IBeginDragHandler,IEndDragHandler
{
    [SerializeField]
    private Transform itemTr;
    [SerializeField]
    private Transform inventoryTr;
    [SerializeField]
    private Transform itemList;
    [SerializeField]
    private CanvasGroup canvasGroup;
    public static GameObject draggingItem = null;
    DragDirction dragDirction;
    IEnumerator Start()
    {
        inventoryTr = GameObject.Find("Inventory").transform;
        itemTr = GetComponent<Transform>();
        yield return new WaitForSeconds(0.5f);
        itemList = GameObject.Find("ItemList ").transform;
        canvasGroup = GetComponent<CanvasGroup>();
        dragDirction = GetComponent<DragDirction>();
    }
    public void OnDrag(PointerEventData eventData)
    {        //�巡�� �̺�Ʈ
         itemTr.position = Input.mousePosition;
         // �巡�� �̺�Ʈ�� �߻��ϸ� �������� ��ġ�� 
         //���콺 Ŀ���� ��ġ�� ����
    }
    // �巡�׸� ���� �� �� �� �� ȣ�� �Ǵ� �̺�Ʈ 
    public void OnBeginDrag(PointerEventData eventData)
    {
       this.transform.SetParent(inventoryTr);
        //�巡�װ� ���۵Ǹ�  �θ� ������Ʈ�� �κ��丮�� �ȴ�.
        draggingItem = this.gameObject;
        //�巡�װ� ���۵Ǹ� �巡�� �Ǵ� ������ ������ ����
        canvasGroup.blocksRaycasts = false;
        //�巡�װ� ���� �Ǿ��� �� �ٸ� UI�̺�Ʈ�� ���� �ʰ� �ϱ� 
    }
    //�巡�װ� �����ٸ� ȣ�� �Ǵ� �̺�Ʈ �Լ� 
    public void OnEndDrag(PointerEventData eventData)
    {   
        //�巡�װ� ���� �Ǹ� �巡�� �������� null�� ����
        draggingItem = null;
        canvasGroup.blocksRaycasts = true;
        //�θ� �κ��丮��� 
        if(itemTr.parent == inventoryTr )
        {        //�������� �����۸���Ʈ�� �θ� �ȴ�.
            
            itemTr.SetParent(itemList.transform);
           
           GameManager.G_instance.RemoveItem(GetComponent<ItemInfo>().itemData);
               //�κ��丮 ���Կ� �ö� �������� ���ٸ� 
          
          
        }

    }
}
