using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : UIBase
{
    [SerializeField] private Slider progressBar;
    [SerializeField] private Image loadingImage;
    [SerializeField] private TextMeshProUGUI tooltipText;

    public override void SetComponent()
    {
        // �ʿ��� ������Ʈ�� ã�� ������ �Ҵ�
        if (progressBar == null)
            progressBar = GetComponentInChildren<Slider>();

        if (loadingImage == null)
            loadingImage = transform.Find("LoadingImage")?.GetComponent<Image>();

        if (tooltipText == null)
            tooltipText = transform.Find("TooltipText")?.GetComponent<TextMeshProUGUI>();
    }

    public override void Init()
    {
        // �ʱ� ���� ����
        if (progressBar != null)
            progressBar.value = 0f;

        if (loadingImage != null)
            loadingImage.gameObject.SetActive(false);

        if (tooltipText != null)
        {
            tooltipText.text = "";
            tooltipText.gameObject.SetActive(false);
        }
    }

    public void SetProgress(float progress)
    {
        if (progressBar != null)
            progressBar.value = Mathf.Clamp01(progress);
    }

    public void SetLoadingImage(Sprite sprite)
    {
        if (loadingImage != null)
        {
            loadingImage.sprite = sprite;
            loadingImage.gameObject.SetActive(sprite != null);
        }
    }

    public void SetTooltip(string message)
    {
        if (tooltipText != null)
        {
            tooltipText.text = message;
            tooltipText.gameObject.SetActive(!string.IsNullOrEmpty(message));
        }
    }
}
