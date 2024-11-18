using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    public float viewRange = 15f; // 적 캐릭터의 추적 사정거리
    [Range(0, 360)]
    public float viewAngle = 120f; // 적 캐릭터의 시야각
    [SerializeField] private Transform enemyTr;
    [SerializeField] public Transform[] playerTrs; // 여러 플레이어를 위한 배열
    [SerializeField] private int playerLayer;
    [SerializeField] private int boxLayer;
    [SerializeField] private int berrelLayer;
    [SerializeField] private int layerMask;

    void Start()
    {
        enemyTr = GetComponent<Transform>();
        // 모든 플레이어 태그가 붙은 오브젝트를 찾습니다
        playerTrs = GameObject.FindGameObjectsWithTag("Player").Select(p => p.transform).ToArray();

        playerLayer = LayerMask.NameToLayer("Player");
        berrelLayer = LayerMask.NameToLayer("BARREL");
        boxLayer = LayerMask.NameToLayer("Obstacle");
        layerMask = 1 << playerLayer | 1 << boxLayer | 1 << berrelLayer;
    }

    public Vector3 CirclePoint(float angle)
    {
        // 로컬 좌표계 기준으로 설정하기 위해서 적 캐릭터의 Y 회전값을 더함
        angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0f, Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    public bool IsTracePlayer() // 플레이어 추적 여부 판단
    {
        bool isTrace = false;
        // 범위 내에서 플레이어가 있는지 검사
        foreach (var playerTr in playerTrs)
        {
            Collider[] colls = Physics.OverlapSphere(enemyTr.position, viewRange, 1 << playerLayer);
            foreach (var coll in colls)
            {
                // 플레이어가 범위 내에 있고 시야각에 들어오는지 확인
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

    public bool IsViewPlayer() // 플레이어를 보았는지
    {
        bool isView = false;
        foreach (var playerTr in playerTrs)
        {
            RaycastHit hit;
            Vector3 dir = (playerTr.position - enemyTr.position).normalized;
            // 레이캐스트를 쏘아서 장애물이 있는지 판단
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
