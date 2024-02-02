using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Avatar : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Character character { get; set; }
    private Outline avatarOutline;
    private Image avatarImage;

    private void Start()
    {
        avatarOutline = GetComponent<Outline>();
        avatarImage = GetComponent<Image>();

        if (avatarOutline == null || avatarImage == null)
        {
            Debug.LogError("Outline or Image component not found. Make sure they are attached to the GameObject.");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        character.Highlight(0);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        character.Highlight(1);
    }
    public void EnableHighlight()
    {
        if (avatarOutline != null)
        {
            avatarOutline.enabled = true;
        }
    }

    public void DisableHighlight()
    {
        if (avatarOutline != null)
        {
            avatarOutline.enabled = false;
        }
    }

    public void ChangeAlpha(float value)
    {
        if (avatarImage != null)
        {
            var tempColor = avatarImage.color;
            tempColor.a = value;
            avatarImage.color = tempColor;
        }
    }
}
