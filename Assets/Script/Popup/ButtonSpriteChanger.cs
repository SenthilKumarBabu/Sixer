using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSpriteChanger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image image;
    [SerializeField] private Sprite pressedSprite;
    [SerializeField] private Sprite normalSprite;

#if UNITY_EDITOR
    [ContextMenu("Assign References")]
    void AssignReferences()
    {
        image = transform.GetChild(0).GetComponent<Image>();
    }
#endif

    public void OnPointerDown(PointerEventData eventData)
    {
        image.sprite = pressedSprite;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        image.sprite = normalSprite;
    }
}
