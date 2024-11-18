using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DataInfo;
public class Drop : MonoBehaviour,IDropHandler
{
   // public int isDropCount = 0;
    public void OnDrop(PointerEventData eventData)
    {        //�ڽ� ������Ʈ�� ���ٸ� 
        if(transform.childCount == 0)
            Drag.draggingItem.transform.SetParent(transform,false);
            //�巡�� ������ �θ� ������Ʈ�� Slot �ڱ� �ڽ��� �ȴ�.
        Item item = Drag.draggingItem.GetComponent<ItemInfo>().itemData;
        //�巡�� �ǰ� �ִ� ������Ʈ�� ItemInfo Ŭ������ itemData�� ����
        // �κ��丮 UI���� �ö� �������� item �� ����
        //Drag.draggingItem.GetComponent<Drag>().isDropping = true;
        GameManager.G_instance.AddItem(item);
        //++isDropCount;
        
    }

}
