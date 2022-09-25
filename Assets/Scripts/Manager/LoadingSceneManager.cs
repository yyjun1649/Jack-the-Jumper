using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    public static string nextScene;
    [SerializeField] Image progressBar;
    [SerializeField] Image Icon;
    [SerializeField] SpriteRenderer Character;

    private void Start()
    {
        StartCoroutine(LoadScene());
        if (Managers.Game.Character == 0)
            Character.gameObject.GetOrAddComponent<BaseController>().SetAnim("Animation/Jack");
        else if (Managers.Game.Character == 1)
            Character.gameObject.GetOrAddComponent<BaseController>().SetAnim("Animation/Builder");
        else if (Managers.Game.Character == 2)
            Character.gameObject.GetOrAddComponent<BaseController>().SetAnim("Animation/Alien");
        else if (Managers.Game.Character == 3)
            Character.gameObject.GetOrAddComponent<BaseController>().SetAnim("Animation/BigBoy");
    }

    private void Update()
    {
        Icon.sprite = Character.sprite;
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return new WaitForSeconds(0.02f);
            timer += Time.deltaTime;
            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);
                if (progressBar.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);
                if (progressBar.fillAmount == 1.0f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}