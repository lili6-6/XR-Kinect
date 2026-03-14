using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Michsky.MUIP;

public class UIManager : MonoBehaviour
{
    [Header("主界面按钮")]
    [SerializeField] private ButtonManager startGame;

    [Header("设置菜单面板")]
    [SerializeField] private Image menuPanel;

    [Header("场景 UI")]
    [SerializeField] private Image scene1;
    [SerializeField] private Image scene2;

    [Header("位置信息（使用 UI 空物体指定）")]
    [SerializeField] private RectTransform targetPos;      // 目标位置点
    [SerializeField] private RectTransform originalPos;    // 原始位置点

    [Header("动画参数")]
    [SerializeField] private float moveDuration = 1f;      // 移动时间
    [SerializeField] private Ease moveEase = Ease.OutCubic; // 缓动方式

    // -----------------------------
    // 设置菜单
    // -----------------------------
    public void ShowSetting()
    {
        menuPanel.gameObject?.SetActive(true);
    }

    public void CloseSetting()
    {
        menuPanel.gameObject?.SetActive(false);
    }

    // -----------------------------
    // 场景切换动画
    // -----------------------------
    public void ShowScene(Image scene)
    {
        RectTransform rect = scene.rectTransform;
        rect.DOKill(); // 清除当前补间，防止冲突
        rect.DOMove(targetPos.position, moveDuration).SetEase(moveEase);
    }

    public void CloseScene(Image scene)
    {
        RectTransform rect = scene.rectTransform;
        rect.DOKill();
        rect.DOMove(originalPos.position, moveDuration).SetEase(moveEase);
    }
}
