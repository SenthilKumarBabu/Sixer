using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GraphicColorChanger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Graphic graphic;
    [SerializeField] private Color pressedColor = Color.grey;
    [SerializeField] private Color normalColor = Color.white;
    private Toggle toggle;
    private Selectable selectable;
    private void Start()
    {
        toggle = GetComponent<Toggle>();
        selectable = GetComponent<Selectable>();
    }
    private void OnEnable()
    {
        if (selectable != null && graphic != null && selectable.interactable)
        {
            if (toggle == null || toggle.isOn == false)
            {
                graphic.color = normalColor;
            }
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Assign Reference")]
    void AssignReferences()
    {
        graphic = transform.GetChild(0).GetComponent<Graphic>();
    }
#endif

    public void OnPointerDown(PointerEventData eventData)
    {
        if (selectable != null && graphic != null && selectable.interactable)
            graphic.color = pressedColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (selectable != null && graphic != null && selectable.interactable)
        {
            if (toggle == null || toggle.isOn == false)
            {
                graphic.color = normalColor;
            }
        }
    }
}
