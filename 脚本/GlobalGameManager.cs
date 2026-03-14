using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class GlobalGameManager : MonoBehaviour
{
    [Header("转场遮罩（可选）")]
    [SerializeField] private Image fadeImage;          // 转场时的遮罩（通常是全屏黑图）
    [SerializeField] private float fadeDuration = 0.8f;// 渐变时间

    public static GlobalGameManager instance;
    private string targetScene;                        // 保存当前选中的场景

    void Awake()
    {
        // 保证单例 + 切换场景不销毁
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject); // 保留本物体和子级
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // -----------------------------
    // 退出游戏
    // -----------------------------
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 在编辑器中退出播放模式
#else
        Application.Quit(); // 在打包后的游戏中退出
#endif
    }

    // -----------------------------
    // 点击按钮设置目标场景
    // -----------------------------
    public void SetTargetScene(string sceneName)
    {
        targetScene =sceneName;
        Debug.Log("目标场景已设置为: " + targetScene);
    }

    // -----------------------------
    // 点击开始游戏后触发转场
    // -----------------------------
    public void StartGame()
    {
        if (!string.IsNullOrEmpty(targetScene))
        {
            StartCoroutine(DoSceneTransition(targetScene));
        }
        else
        {
            Debug.LogWarning("未设置目标场景！");
        }
    }

    // -----------------------------
    // 转场协程
    // -----------------------------
    private IEnumerator DoSceneTransition(string scene)
    {
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            fadeImage.color = new Color(0, 0, 0, 0);
            yield return fadeImage.DOFade(1f, fadeDuration).WaitForCompletion();
        }

        AsyncOperation async = SceneManager.LoadSceneAsync(scene);
        async.allowSceneActivation = true;

        while (!async.isDone)
        {
            yield return null;
        }

        if (fadeImage != null)
        {
            yield return fadeImage.DOFade(0f, fadeDuration).WaitForCompletion();
            fadeImage.gameObject.SetActive(false);
        }
    }
}
