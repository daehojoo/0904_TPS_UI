using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmos : MonoBehaviour
{
    public enum Type { NORMAL,WAYPOINT}
    private const string wayPointFile = "Enemy";
    public Type type = Type.NORMAL;
    public Color _color;
    public float _radius;
    void Start()
    {
       
    }
    private void OnDrawGizmos() //��ȭ�鿡�� �����̳� ���� �׷��ִ� �Լ�
    {                           //����Ƽ ���� �Լ� 
        if (type == Type.NORMAL)
        {
            Gizmos.color = _color;
            Gizmos.DrawSphere(transform.position, _radius);
        }
        else
        {
            Gizmos.color = _color;
            Gizmos.DrawIcon(transform.position + Vector3.up * 1.0f, wayPointFile, true);
                                      // ��ġ                      , ���ϸ�    , ������ ���뿩��
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }

}
