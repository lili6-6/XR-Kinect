using DG.Tweening;
using RenderHeads.Media.AVProMovieCapture;
using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneManager : MonoBehaviour
{
    public Image fadeimage;
    public float fadeDuration = 0.8f;
    public Animator _startAnimator;
    public CanvasGroup tips;
    //[Header("设置菜单面板")]
    //[SerializeField] private Image menuPanel;

    [SerializeField] float duration = 5f;
    [SerializeField]public  KinectManager Kinectmanager;
    [SerializeField]public AudioManager audioManager;
    [SerializeField] public TextMeshProUGUI tipsText;
    private bool isRecording = false;
    private bool isTransitioning = false;
    // private RecorderController recorderController;
    //private RecorderControllerSettings recorderSettings;
    private CaptureFromScreen capture;
    private Coroutine stopAfterTimeRoutine;
    private EventSystem cachedEventSystem;
    private bool eventSystemWasEnabled = false;
    public static GameSceneManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    //Start is called before the first frame update
    void Start()
    {

       // StartRecording();
        fadeImage();
        tips.alpha = 0f;
        _startAnimator.SetTrigger("start");
       


    }

    public void StartCapture()
    {
        if (isRecording || isTransitioning) return;
        isRecording = true;

        tipsText.text = "开始录制";
        tips.alpha = 0f;
        ShowAndHideTips();

        GameObject go = new GameObject("Recorder");
        capture = go.AddComponent<CaptureFromScreen>();

        // 关键配置
        capture.IsRealTime = true; // 用真实时间
        capture.FrameRate = 60f;

        capture.OutputFolderPath = Application.persistentDataPath;
        capture.StopMode = StopMode.None;

        capture.StartCapture();

        stopAfterTimeRoutine = StartCoroutine(StopAfterTime());
    }

    private IEnumerator StopAfterTime()
    {
        yield return new WaitForSeconds(duration);
        StopCaptureInternal(true);
    }


    public void ShowAndHideTips()
    {
        // 停止残留动画，避免冲突
        tips.DOKill();

        // 方案1：用DOTween序列（Sequence）实现，逻辑更清晰，推荐
        Sequence tipSequence = DOTween.Sequence();

        // 第一步：渐显（alpha从0到1）
        tipSequence.Append(tips.DOFade(1, fadeDuration)
            .OnStart(() =>
            {
                // 渐显开始时开启交互
                tips.interactable = true;
                tips.blocksRaycasts = true;
            }));

        // 第二步：停留2秒（无动画，仅等待）
        tipSequence.AppendInterval(2f);

        // 第三步：渐隐（alpha从1到0）
        tipSequence.Append(tips.DOFade(0, fadeDuration)
            .OnComplete(() =>
            {
                // 渐隐完成后关闭交互
                tips.interactable = false;
                tips.blocksRaycasts = false;
            }));

        // 可选：设置序列的缓动曲线（也可给单个动画单独设置）
        tipSequence.SetEase(Ease.OutQuad);
    }
    public void StopCapture()
    {
        if (!isRecording) return;
        StopCaptureInternal(false);
    }
    //在编辑器模式下启动录制
    //public void StartRecording()
    //{
    //    Debug.Log("配置录制设置");
    //    // 创建 RecorderControllerSettings 对象
    //    recorderSettings = ScriptableObject.CreateInstance<RecorderControllerSettings>();

    //    // 创建 MovieRecorderSettings 实例
    //    var videoRecorderSettings = ScriptableObject.CreateInstance<MovieRecorderSettings>();


    //    // 使用当前时间戳作为文件名的一部分，避免覆盖
    //    string timeStamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
    //    // 设置输出文件路径
    //    videoRecorderSettings.OutputFile = $"Assets/Recordings/recording_{timeStamp}.mp4";  // 使用时间戳生成唯一文件名


    //    // 设置输出格式为 MP4
    //    videoRecorderSettings.OutputFormat = MovieRecorderSettings.VideoRecorderOutputFormat.MP4;

    //    // 设置帧率
    //    videoRecorderSettings.FrameRate = 30;

    //    // 设置输入来源为主摄像头
    //    var cameraInputSettings = new CameraInputSettings
    //    {
    //        Source = ImageSource.MainCamera // 使用主摄像头作为输入源
    //    };
    //    videoRecorderSettings.ImageInputSettings = cameraInputSettings;

    //    // 设置是否捕捉 alpha 通道（透明度）
    //    videoRecorderSettings.CaptureAlpha = false;

    //    // 将设置添加到 RecorderControllerSettings 中
    //    recorderSettings.AddRecorderSettings(videoRecorderSettings);

    //    // 创建并启动 RecorderController
    //    recorderController = new RecorderController(recorderSettings);
    //    recorderController.PrepareRecording();
    //    recorderController.StartRecording();
    //    Debug.Log("开始录制");
    //}
    //// 停止录制
    //public void StopRecording()
    //{
    //    if (isRecording)
    //    {
    //        recorderController.StopRecording();
    //        Debug.Log("录制停止");
    //    }

    //}


    public  void fadeImage()
    {
     fadeimage.gameObject.SetActive(true);
        // 场景加载完成后，淡入效果
        DOTween.ToAlpha(() => fadeimage.color, x => fadeimage.color = x, 0, fadeDuration / 2);
       
    }

    //public void recorder()
    //{
    //    if (!isRecording)
    //    {
    //        StartCapture();
    //        //Debug.Log("开始录制");
            
    //    }
    //    else
    //    {
    //        StopCapture();
    //        //Debug.Log("停止录制");
    //        isRecording = false;
    //    }
    //}
    //public void ShowSetting()
    //{
    //    menuPanel.gameObject?.SetActive(true);
    //}

    //public void CloseSetting()
    //{
    //    menuPanel.gameObject?.SetActive(false);
    //}
    public void ExitGame()
    {
        //StopRecording();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 在编辑器中退出播放模式
#else
        Application.Quit(); // 在打包后的游戏中退出
#endif
    }
    public void Return(string sceneName)
    {
        if (isTransitioning)
            return;

        string targetScene = sceneName;
        if (!string.IsNullOrEmpty(targetScene))
        {
            StartCoroutine(ShutdownAndTransition(targetScene));

        }
        else
        {
            Debug.LogWarning("未设置目标场景！");
        }

    }

    private IEnumerator ShutdownAndTransition(string scene)
    {
        isTransitioning = true;
        SetInputEnabled(false);

        StopCaptureInternal(false);

        if (Kinectmanager != null)
        {
            Kinectmanager.StopKinect();
        }

        yield return DoSceneTransition(scene);

        SetInputEnabled(true);
        isTransitioning = false;
    }
    private IEnumerator DoSceneTransition(string scene)
    {
        if (fadeimage != null)
        {
            fadeimage.gameObject.SetActive(true);
            fadeimage.color = new Color(0, 0, 0, 0);
            yield return fadeimage.DOFade(1f, fadeDuration).WaitForCompletion();
        }

        AsyncOperation async = SceneManager.LoadSceneAsync(scene);
        async.allowSceneActivation = true;

        while (!async.isDone)
        {
            yield return null;
        }

        if (fadeimage != null)
        {
            yield return fadeimage.DOFade(0f, fadeDuration).WaitForCompletion();
            fadeimage.gameObject.SetActive(false);
        }
    }

    public void Reset()
    {
        Kinectmanager.ResetFilters();
    }

    private void StopCaptureInternal(bool showTips)
    {
        if (!isRecording)
            return;

        if (stopAfterTimeRoutine != null)
        {
            StopCoroutine(stopAfterTimeRoutine);
            stopAfterTimeRoutine = null;
        }

        if (capture != null)
        {
            capture.StopCapture();
            Destroy(capture.gameObject);
            capture = null;
        }

        isRecording = false;

        if (showTips && tipsText != null)
        {
            tipsText.text = "录制结束";
            ShowAndHideTips();
        }
    }

    private void SetInputEnabled(bool enabled)
    {
        if (!enabled)
        {
            cachedEventSystem = EventSystem.current;
            eventSystemWasEnabled = cachedEventSystem != null && cachedEventSystem.enabled;

            if (cachedEventSystem != null)
            {
                cachedEventSystem.enabled = false;
            }
        }
        else if (cachedEventSystem != null && eventSystemWasEnabled)
        {
            cachedEventSystem.enabled = true;
        }
    }
}

