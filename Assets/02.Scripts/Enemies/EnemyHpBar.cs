using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//������ǥ�� �о ��ũ����ǥ�� ǥ���ؾ���
public class EnemyHpBar : MonoBehaviour
{

    [SerializeField] private Camera uiCamera;
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform rectParent;
    [SerializeField] private RectTransform rectHp;
    public Vector3 offSet = Vector3.zero;
    public Transform targetTr;




    void Start()
    {
        canvas = GameObject.Find("Ui_Canvas").GetComponent<Canvas>();
        uiCamera = canvas.worldCamera;

        rectHp = this.gameObject.GetComponent<RectTransform>();

        rectParent = GameObject.Find("Ui_Canvas").GetComponent<RectTransform>();
    }
   


void LateUpdate()
    {
        //Debug.Log(targetTr.position);
        var screenPos = Camera.main.WorldToScreenPoint(targetTr.position+offSet);
        if (screenPos.z < 0.0f)
        {
            screenPos *= -1.0f;//ī�޶�� z���� ������ �ȵ�        
        }
        var localPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, uiCamera, out localPos);
        rectHp.localPosition = localPos;//��������� �̹����� ��ġ�� ����


    }
}
