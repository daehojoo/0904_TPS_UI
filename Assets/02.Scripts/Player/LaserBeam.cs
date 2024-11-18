using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class LaserBeam : MonoBehaviour
{
    private Transform tr;
    private LineRenderer line;
    [SerializeField] private Transform firePos;
    [SerializeField] private FireCtrl fireCtrl;
    [SerializeField] private Player player;
    void Start()
    {
        tr = transform;
        line = GetComponent<LineRenderer>();
        line.useWorldSpace = false;
        line.enabled = false;
        firePos = transform.GetComponentsInParent<Transform>()[1];
        fireCtrl = GetComponentInParent<FireCtrl>();
        player = GetComponentInParent<Player>();
       
    }
    void Update()
    {
        if(Time.timeScale <= 0f) return;
        //if (EventSystem.current.IsPointerOverGameObject()) return;
           // UI ���� ���콺 �����ͳ� �հ����� �÷��� �ִٸ� ���� ������.
        //������ �̸� ����
        Ray ray = new Ray(firePos.position, tr.forward);
        RaycastHit hit;
      
        Debug.DrawRay(ray.origin, ray.direction*100f ,Color.blue );
        if (fireCtrl.isFire && !fireCtrl.isReloading&& !player.isRunning )
        {
            line.SetPosition(0, tr.InverseTransformPoint(ray.origin));

            if (Physics.Raycast(ray, out hit, 100f))
            {
                line.SetPosition(1, tr.InverseTransformPoint(hit.point));
            }
            else //���� �ʾ��� �� ������ 100���� ��´�. 
            {
                line.SetPosition(1, tr.InverseTransformPoint(ray.GetPoint(100f)));

            }
            StartCoroutine(ShowRaserBeam());
        }
        #region ���콺 ���ʹ�ư ������ ���� ������ �������ϱ� 
        if (Input.GetMouseButtonDown(0) && !fireCtrl.isReloading)
        {    //���� �������� ù��° ���� ��ġ ����
            line.SetPosition(0, tr.InverseTransformPoint(ray.origin));
                                    //������ǥ �������� ������ǥ ���������� ����
            // � ��ü�� ������ �¾��� ���� ��ġ�� Line Renderer�� �������� ����
            if(Physics.Raycast(ray,out hit,100f))
            {
                line.SetPosition(1,tr.InverseTransformPoint(hit.point));
            }
            else //���� �ʾ��� �� ������ 100���� ��´�. 
            {                                                
                line.SetPosition(1, tr.InverseTransformPoint(ray.GetPoint(100f)));
                                    
            }
            StartCoroutine(ShowRaserBeam());
        }
        #endregion

    }
    IEnumerator ShowRaserBeam()
    {
        line.enabled = true;
        yield return new WaitForSeconds(Random.Range(0.01f,0.2f));
        line.enabled = false;
    }
}
