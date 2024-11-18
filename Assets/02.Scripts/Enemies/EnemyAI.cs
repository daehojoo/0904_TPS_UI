using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]
public class EnemyAI : MonoBehaviour
{    
    public enum State //������ ��� 
    {
        PTROL=0,TRACE,ATTACK,DIE
    }
    public State state = State.PTROL;
    [SerializeField] private Transform playerTr; // �Ÿ��� ������� ����
    [SerializeField] private Transform enemyTr; //�Ÿ��� ������� ����
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyFOV enemyFOV;
    // ���� �Ÿ� ���� �Ÿ� 
    public float attackDist = 5.0f;
    public float traceDist = 10f;
    public bool isDie = false; //��� ���� �Ǵ�
    private WaitForSeconds ws;
    private EnemyMoveAgent moveAgent;
    private EnemyFire enemyFire;
    // �ִϸ����� ��Ʈ�ѷ��� ���� �� �Ķ������ �ؽð��� ������ �̸� ����
    private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashSpeed = Animator.StringToHash("moveSpeed");
    private readonly int hashDie = Animator.StringToHash("DieTrigger");
    private readonly int hashDieIdx = Animator.StringToHash("DieIdx");
    private readonly int hashOffset = Animator.StringToHash("Offset");
    private readonly int hashWalkSpeed = Animator.StringToHash("WalkSpeed");
    private readonly int hashPlayerDie = Animator.StringToHash("PlayerDieTrigger");
    private readonly int hashexpDie = Animator.StringToHash("ExpDieTrigger");
    void Awake()
    {
        animator = GetComponent<Animator>();
        moveAgent = GetComponent<EnemyMoveAgent>();
        var player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        playerTr = player.GetComponent<Transform>();
        enemyTr = GetComponent<Transform>();
        enemyFire = GetComponent<EnemyFire>();
        ws = new WaitForSeconds(0.3f);
        enemyFOV = GetComponent<EnemyFOV>();
    }
    private void OnEnable() //������Ʈ Ȱ��ȭ �ɶ� ���� ȣ��
    {
        Damage.OnPlayerDie += OnPlayerDie;
        // �̺�Ʈ ���� 
        animator.SetFloat(hashOffset, Random.Range(0.2f, 1.0f));
        animator.SetFloat(hashWalkSpeed, Random.Range(1.0f, 2.0f));
        StartCoroutine(CheckState());
        StartCoroutine(Action());
    }
    IEnumerator CheckState()
    {
        yield return new WaitForSeconds(1.0f);
        // ������Ʈ Ǯ�� ������ �ٸ� ��ũ��Ʈ�� �ʱ�ȭ�� ���� ��� 
        while(!isDie)
        {
            if(state == State.DIE) yield break;
            //��� �����̸� �ڷ�ƾ �Լ��� ���� ��Ŵ
            float dist = (playerTr.position - enemyTr.position).magnitude;
            if (dist <= attackDist)
            {
                if (enemyFOV.IsViewPlayer())
                    state = State.ATTACK; // ��ֹ��� ������ ���� ��� 
                else 
                    state = State.TRACE;
            }
                
            else if (enemyFOV.IsTracePlayer())
                state = State.TRACE;
            else
                state = State.PTROL;

            yield return ws;
        }
    }
    IEnumerator Action()
    {
        while (!isDie)
        {
            yield return ws;
            switch(state)
            {
                case State.PTROL:
                    enemyFire.isFire = false;
                    moveAgent.patrolling = true;
                    animator.SetBool(hashMove, true);
                    break;
                case State.ATTACK:
                    if(enemyFire.isFire==false)
                      enemyFire.isFire = true;
                    moveAgent.Stop();
                    animator.SetBool(hashMove, false);
                    break;
                case State.TRACE:
                    enemyFire.isFire =false;
                    moveAgent.traceTarget = playerTr.position;
                    animator.SetBool(hashMove, true);
                    break;
                case State.DIE:
                    EnemyDie();
                    break;
            }

        }


    }

    private void EnemyDie()
    {
        isDie = true;
        moveAgent.Stop();
        enemyFire.isFire = false;
        animator.SetTrigger(hashDie);
        animator.SetInteger(hashDieIdx, Random.Range(0, 2));
        GetComponent<Rigidbody>().isKinematic = true;//��������
        GetComponent<CapsuleCollider>().enabled = false;//ĸ�� �ݶ��̴� ��Ȱ��ȭ
        gameObject.tag = "Untagged"; //��� �ߴٸ� �±׻���
        state = State.DIE;
        StartCoroutine(ObjectPoolPush());
    }
    void ExpDie()
    {
        if (isDie) return;
        GetComponent<EnemyDamage>().hpBarImage.fillAmount = 0;
        GetComponent<EnemyDamage>().hpBarImage.color = Color.clear;
        GetComponent<EnemyDamage>().hp = 0f;


        GameManager.G_instance.KillScore();
        moveAgent.Stop();
        enemyFire.isFire = false;
        GetComponent<Rigidbody>().isKinematic = true;//��������
        GetComponent<CapsuleCollider>().enabled = false;//ĸ�� �ݶ��̴� ��Ȱ��ȭ
        gameObject.tag = "Untagged"; //��� �ߴٸ� �±׻���
        state = State.DIE;
        isDie = true;
        animator.SetTrigger(hashexpDie);
        StartCoroutine(ObjectPoolPush());
        
    }
    IEnumerator ObjectPoolPush()
    {
        yield return new WaitForSeconds(3f);
        GetComponent<EnemyDamage>().hpBar.gameObject.SetActive(false);
        isDie = false;
        GetComponent<Rigidbody>().isKinematic = false;//������ �ְ�
        GetComponent<CapsuleCollider>().enabled = true;//ĸ�� �ݶ��̴� Ȱ��ȭ
        gameObject.tag = "ENEMY"; // ������Ʈ�� Ȱ��ȭ �Ǳ��� �±��̸��� �������
     
        gameObject.SetActive(false);
        state = State.PTROL;
        GetComponent<EnemyDamage>().hpBarImage.fillAmount = 1f;
        GetComponent<EnemyDamage>().hpBarImage.color = Color.red;
        GetComponent<EnemyDamage>().hp = 100f;


    }
    void OnPlayerDie()
    {
        StopAllCoroutines(); //��� �ڷ�ƾ ����
        GetComponent<Rigidbody>().isKinematic = true;
        //�ִϸ��̼� ���
        animator.SetTrigger(hashPlayerDie);
        GameManager.G_instance.isGameOver = true;
    }


    void Update()
    {
        animator.SetFloat(hashSpeed,moveAgent.speed);
    }
    private void OnDisable()
    {
        Damage.OnPlayerDie -= OnPlayerDie;
                   // �̺�Ʈ ���� ���� 
    }
}
