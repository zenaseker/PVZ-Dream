using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TextClick : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI m_TextMeshPro;
    public Canvas m_Canvas;


    public void OnPointerClick(PointerEventData eventData)
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(m_TextMeshPro, Input.mousePosition, Camera.main);
        if (linkIndex != -1)
        {
            TMP_LinkInfo linkInfo = m_TextMeshPro.textInfo.linkInfo[linkIndex];
            DebugShow.Instance.Init(Attribute.Instance.GetBuffDescribe(linkInfo.GetLinkText()), 5);
        }
    }
}