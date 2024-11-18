using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[System.Serializable] //��Ʃ����Ʈ  public ����� ��� �ʵ带


public class Player : MonoBehaviour
{
    
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotSpeed = 90f;
    [SerializeField] Rigidbody rbody;
    [SerializeField] CapsuleCollider capCol;
    [SerializeField] Transform tr;
    [SerializeField] Animator animator;
    float h, v, r;
    public bool isRunning;

    private void OnEnable()
    {
        GameManager.OnItemChange += UpdateSetup;
                        //�̺�Ʈ ���
    }
    void UpdateSetup()
    {
        moveSpeed = GameManager.G_instance.gameData.speed;
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        moveSpeed = GameManager.G_instance.gameData.speed;
        //���۳�Ʈ ĳ�� ó��
        rbody = GetComponent<Rigidbody>();
        capCol = GetComponent<CapsuleCollider>();
        tr = GetComponent<Transform>();
 
        isRunning = false;
    }
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        r = Input.GetAxisRaw("Mouse X");
        Vector3 moveDir = (h * Vector3.right) + (v * Vector3.forward);
        tr.Translate(moveDir.normalized * moveSpeed * Time.deltaTime, Space.Self);
        {
            animator.SetFloat("posx", h);
            animator.SetFloat("posy", v);
        }
        tr.Rotate(Vector3.up * r * Time.deltaTime * rotSpeed);

        Sprint();
    }

    private void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            moveSpeed = 10f;
            
            isRunning = true;

        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
            moveSpeed = 5f;

        }
    }

    //private void moveAni()
    //{
    //    if (h > 0.1f)
            
    //    else if (h < -0.1f)
    //        _animation.CrossFade(playerAnimation.runLeft.name, 0.3f);
    //    else if (v > 0.1f)
    //        _animation.CrossFade(playerAnimation.runForward.name, 0.3f);
    //    else if (v < -0.1f)
    //        _animation.CrossFade(playerAnimation.runBackward.name, 0.3f);
    //    else
    //        _animation.CrossFade(playerAnimation.idle.name, 0.3f);
    //}
}
