using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DataInfo;
public class DragDirction : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    private Vector2 startPosition;
    public Transform ItemListTr;
    public Transform ItemTr;
    public bool isDown = false;
    void Start()
    {
        ItemListTr = GameObject.Find("ItemList ").transform;
        ItemTr = GetComponent<Transform>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        startPosition = Input.mousePosition;
        //startPosition = Input.touches[0].position; //����� �÷����̸� 
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Vector2 endPosition = Input.mousePosition;
        Vector2 direction = endPosition - startPosition;

        // �Ʒ������� �巡���ߴ��� �Ǻ�
        if (direction.y < 0 )
        {
          
            Debug.Log("�Ʒ������� �巡���߽��ϴ�.");
           
            isDown = true;
        }
        else
        {
            Debug.Log("�ٸ� �������� �巡���߽��ϴ�.");
            isDown = false;
        }
    }
}
