using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public CanvasGroup fadeCG; //���̵� �� ���� �ϱ� ����
    [Range(0.5f, 2.0f)] public float fadeDuration = 1.0f;
    public Dictionary<string,LoadSceneMode> loadScenes = new Dictionary<string,LoadSceneMode>();
    void InitSceneInfo()
    {
        loadScenes.Add("Level_1", LoadSceneMode.Additive);
        loadScenes.Add("BattleFieldScene",LoadSceneMode.Additive);

    }
    IEnumerator Start()
    {
        //fadeCG = GetComponent<CanvasGroup>();
        fadeCG.alpha = 1.0f;// ó�� ���İ� ������
        InitSceneInfo(); //�� ��ġ�� 
        
        foreach (var loadscene in loadScenes )
        {
            yield return StartCoroutine(LoadScene(loadscene.Key,loadscene.Value));
        }

        StartCoroutine(Fade(0.0f));
    }
    IEnumerator LoadScene(string sceneName,LoadSceneMode mode)
    {
        //�񵿱� ������� ���� �ε� �ϰ� �ε尡 �Ϸ� �� ������ ����� 
        yield return SceneManager.LoadSceneAsync(sceneName,mode);
        //ȣ�� �� ���� Ȱ��ȭ 
        Scene loadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount-1);
        SceneManager.SetActiveScene(loadedScene);

    }
    IEnumerator Fade(float finalAlpha)
    {
        // ����Ʈ ���� ������ ���� ���� �ϱ� ���� �������� ���� Ȱ��ȭ 
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Level_1"));
        fadeCG.blocksRaycasts= true;
        //���밪 �Լ��� ����� ���  
        float fadeSpeed = Mathf.Abs(fadeCG.alpha - finalAlpha) / fadeDuration;
                                   //1.0f         0.0f
        while(!Mathf.Approximately(fadeCG.alpha, finalAlpha))
        {       //���� ������ �ϴٸ� 
            fadeCG.alpha = Mathf.MoveTowards(fadeCG.alpha,finalAlpha,fadeSpeed *Time.deltaTime);
                           //���������� �ε巴�� �����ϰ� ���Ѵ�.
            yield return null; //  �������� ������ 

        }
      
        fadeCG.blocksRaycasts = false;
        // �񵿱���(������)���� FadeIn�� �Ϸ� �� ��  SceneLoader�� ���� (Unload)
        SceneManager.UnloadSceneAsync("SceneLoader");
    }    
}
