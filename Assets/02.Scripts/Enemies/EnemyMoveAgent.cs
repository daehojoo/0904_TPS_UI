using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMoveAgent : MonoBehaviour
{
    //��Ʈ�� ������ ��� ���� List ���׸�(�Ϲ���) ���� 
    public List<Transform> wayPointList;
    public int nexIdx = 0;// ���� ���� ������ �迭 �ε��� ��
    [SerializeField] private NavMeshAgent agent;
    private readonly float patrolSpeed = 1.5f;
    private readonly float traceSpeed = 4.0f;
    private float damping = 1.0f; //ȸ�� �� ���� �ӵ��� ���� �ϴ� ���
    private Transform enemyTr;
    private bool _patrolling;
    public bool patrolling //���� ��Ƽ 
    {
        get { return _patrolling; } 
        set 
        { 
            _patrolling = value; 
              if (_patrolling)
              {
                agent.speed = patrolSpeed;
                damping = 1.0f;
                MovewayPoint();
              }
         }
    }
    private Vector3 _traceTarget;
    public Vector3 traceTarget
    {
        get { return _traceTarget; }
        set
        {
            _traceTarget = value;
            agent.speed = traceSpeed;
            damping = 7.0f;
            TraceTarget(_traceTarget);
        }
    }
    public float speed
    {                 //NavmeshAgent �ӵ� 
        get { return agent.velocity.magnitude; }
    }
    void Start()
    {
        enemyTr = GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        agent.updateRotation = false;
        //������Ʈ�� �̿��ؼ� ȸ���ϴ� ����� ��Ȱ��ȭ ���� �ε巴���� ����
        var group = GameObject.Find("WayPointGroup");
        // ���̶�Ű�� �ִ� ������Ʈ�� �� WayPointGroup �� ã�Ƽ� ����
        if (group != null )
        {         // ���� ������Ʈ�� Ʈ�������� wayPointList�� �� ��´�.
            group.GetComponentsInChildren<Transform>(wayPointList);
            wayPointList.RemoveAt(0); //ù��° �ε����� ���� 
        }
        nexIdx = Random.Range(0,wayPointList.Count);
        MovewayPoint();

    }
    void Update()
    {
        if (agent.isStopped == false)
        {
            //NavMeshAgent ���� �� ���� ���͸� ���ʹϾ� Ÿ���� ������ ��ȯ
            if (agent.desiredVelocity != Vector3.zero)
            {
                Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);
                enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot,
                                              Time.deltaTime * damping);
                //���� �Լ��� �̿��ؼ� ���������� �ε巴�� ȸ�� ��Ŵ
            }
        }
        if (_patrolling==false) return;
        //���� ���������� 0.5���� �۰ų� ���ٸ� 
        if(agent.remainingDistance <=0.5f)
        {

            //nexIdx = ++nexIdx % wayPointList.Count;
            nexIdx = Random.Range(0, wayPointList.Count);
            MovewayPoint();
        }

    }
    void MovewayPoint()
    {        //�ִ� ��� ����� ������ �ʰų� ���� �Ҿ�������
        if (agent.isPathStale) return;
        // ��� ������ ������� =  ����Ʈ�� ��Ҵ� Ʈ������
        agent.destination = wayPointList[nexIdx].position;
        agent.isStopped = false;
    }
    private void TraceTarget(Vector3 pos)
    {
        if(agent.isPathStale) return;
        agent.destination = pos;
        agent.isStopped = false;
        
    }
    public void Stop()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        _patrolling = false;
    }
}
