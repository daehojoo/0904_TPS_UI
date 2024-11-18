using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    public float viewRange = 15f; // �� ĳ������ ���� �����Ÿ�
    [Range(0, 360)]
    public float viewAngle = 120f; // �� ĳ������ �þ߰�
    [SerializeField] private Transform enemyTr;
    [SerializeField] public Transform[] playerTrs; // ���� �÷��̾ ���� �迭
    [SerializeField] private int playerLayer;
    [SerializeField] private int boxLayer;
    [SerializeField] private int berrelLayer;
    [SerializeField] private int layerMask;

    void Start()
    {
        enemyTr = GetComponent<Transform>();
        // ��� �÷��̾� �±װ� ���� ������Ʈ�� ã���ϴ�
        playerTrs = GameObject.FindGameObjectsWithTag("Player").Select(p => p.transform).ToArray();

        playerLayer = LayerMask.NameToLayer("Player");
        berrelLayer = LayerMask.NameToLayer("BARREL");
        boxLayer = LayerMask.NameToLayer("Obstacle");
        layerMask = 1 << playerLayer | 1 << boxLayer | 1 << berrelLayer;
    }

    public Vector3 CirclePoint(float angle)
    {
        // ���� ��ǥ�� �������� �����ϱ� ���ؼ� �� ĳ������ Y ȸ������ ����
        angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0f, Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    public bool IsTracePlayer() // �÷��̾� ���� ���� �Ǵ�
    {
        bool isTrace = false;
        // ���� ������ �÷��̾ �ִ��� �˻�
        foreach (var playerTr in playerTrs)
        {
            Collider[] colls = Physics.OverlapSphere(enemyTr.position, viewRange, 1 << playerLayer);
            foreach (var coll in colls)
            {
                // �÷��̾ ���� ���� �ְ� �þ߰��� �������� Ȯ��
                Vector3 dir = (playerTr.position - enemyTr.position).normalized;
                if (Vector3.Angle(enemyTr.forward, dir) < viewAngle * 0.5f)
                {
                    isTrace = true;
                    break;
                }
            }
            if (isTrace) break;
        }
        return isTrace;
    }

    public bool IsViewPlayer() // �÷��̾ ���Ҵ���
    {
        bool isView = false;
        foreach (var playerTr in playerTrs)
        {
            RaycastHit hit;
            Vector3 dir = (playerTr.position - enemyTr.position).normalized;
            // ����ĳ��Ʈ�� ��Ƽ� ��ֹ��� �ִ��� �Ǵ�
            if (Physics.Raycast(enemyTr.position, dir, out hit, viewRange, layerMask))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    isView = true;
                    break;
                }
            }
        }
        return isView;
    }
}
