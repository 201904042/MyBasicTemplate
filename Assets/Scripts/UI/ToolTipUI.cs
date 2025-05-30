using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipUI : UIBase
{
    private Image _backgroundImage;
    private TextMeshProUGUI _titleText;
    private TextMeshProUGUI _contentText;

    public override void Init()
    {
        SetComponent();
    }

    public override void SetComponent()
    {
        _backgroundImage = transform.Find("Image").GetComponent<Image>();
        _titleText = transform.Find("Title").GetComponent<TextMeshProUGUI>();
        _contentText = transform.Find("Content").GetComponent<TextMeshProUGUI>();
    }
}
