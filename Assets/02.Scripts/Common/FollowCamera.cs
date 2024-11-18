using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform target; //���� �ٴ� ���
    [SerializeField] private float Height = 5.0f; //ī�޶� ����
    [SerializeField] private float distance = 7.0f; // Ÿ�ٰ��� �Ÿ�
    [SerializeField] private float movedamping = 10f; // ī�޶� �̵� ȸ����  ������ �ε巴�� ��ȭ �ϴ� ��
    [SerializeField] private float rotdamping = 15f;
    [SerializeField] private Transform CamTr; // �ڱ��ڽ� Ʈ������
    [SerializeField] private float targetOffset = 2.0f; // Ÿ�Ͽ����� ī�޶� ���̰�
    [Header("Camera Obstacle Move")]
    public float maxHeight = 12f; // ī�޶� ��ֹ��� �������� �ö� ����
    public float castOffset = 2.0f;// ���ΰ� �Ӹ� ���� ���� 
    public float originHeight;
    IEnumerator Start()
    {
        CamTr = GetComponent<Transform>();
        yield return new WaitForSeconds(0.5f);
        Height = 5f;
        originHeight = Height; 
        // ī�޶� ���� �̵� �� ���� ���̷� ���ƿ��� ����
    }
    void Update()
    {
        if(target ==null) return;
        Vector3 castTarget = target.position + (target.up * castOffset);
                              //Ÿ��(�÷��̾�)��ġ���� 1��ŭ �ø�
        Vector3 castDir = (castTarget - CamTr.position).normalized;
                           //Ÿ�� ��ġ��  ī�޶� ��ġ�� ���� ����� �Ÿ��� ������
        RaycastHit hit;
        if(Physics.Raycast(CamTr.position, castDir, out hit,Mathf.Infinity))
        {
            if(!hit.collider.CompareTag("Player")) //�÷��̾ ������ ���� �ʾҴٸ� 
            {
                Height = Mathf.Lerp(Height,maxHeight,Time.deltaTime *5f);
                         //�������� �������� �Լ� 
            }
           else //  �¾Ҵٸ� 
            {
                Height = Mathf.Lerp(Height, originHeight, Time.deltaTime * 5f);
            }
        }


    }
    void LateUpdate()
    {               
       
                //Ÿ�� �����ǿ���  distance��ŭ �ڿ� ��ġ      +  Height ���� ��ŭ ���� ��ġ 
        var CamPos = target.position - (target.forward * distance) + (target.up * Height);
        CamTr.position = Vector3.Slerp(CamTr.position, CamPos, Time.deltaTime * movedamping);
                //��� ����  (�ڱ��ڽ� ��ġ���� , Campos ���� ,damping �ð� ��ŭ �ε巴�� ������
        CamTr.rotation = Quaternion.Slerp(CamTr.rotation,target.rotation, Time.deltaTime * rotdamping);
        //��� �������� ȸ�� (�ڱ��ڽŷ����̼ǿ��� ,Ÿ�� �����̼� ���� , damping �ð� ��ŭ �ε巴�� ȸ����
        CamTr.LookAt(target.position +(target.up *targetOffset));
                     //Ÿ�� �����ǿ���  2��ŭ ���� �ø�
    }
    private void OnDrawGizmos() // ��ȭ�鿡�� �����̳� ���� �׷��ִ� �Լ� �ݹ��Լ�
    {
        //CamTr.position = transform.position;
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(target.position + (target.up * targetOffset), 0.1f);
                 //����
        //Gizmos.DrawLine(target.position +(target.up *targetOffset),CamTr.position);
               //��
    }
}
