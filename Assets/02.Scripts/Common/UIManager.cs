using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class UIManager : MonoBehaviour
{
    public void OnClickPlayBtn()
    {
        //SceneManager.LoadScene("Level_1");
        //SceneManager.LoadScene("BattleFieldScene", LoadSceneMode.Additive);
        SceneManager.LoadScene("SceneLoader");
        //LoadSceneMode.Additive �������� ���� ���� �ʰ� �߰� �ؼ� ���ο� ���� �ε��Ѵ�.
        //LoadSceneMode.Single ������ �ε�� ���� ��� ������ ���ο� ���� �ε� �Ѵ�.
    }
    public void OnClickQuitBtn()
    {
#if UNITY_EDITOR //����Ƽ �����Ϳ����� 
        //�������� ���ø����̼� ����
        EditorApplication.isPlaying = true; //����Ƽ���� ���� 
#else
    Application.Quit(); // ��� �Ϸ��� ���� �� ȭ�鿡�� ���� �������� ���� 
#endif

    }

}
