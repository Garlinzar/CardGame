using UnityEngine;
using UnityEngine.EventSystems;

public class CardSelector : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public static GameObject selectedCard;

    [Header("Zielobjekt für die Verschiebung")]
    public Transform visualTarget;

    private Vector3 originalLocalPosition;
    private bool isSelected = false;
    private bool isHovered = false;

    void Start()
    {
        // Falls kein Ziel gesetzt wurde, benutze das eigene Objekt
        if (visualTarget == null)
        {
            visualTarget = transform;
        }

        originalLocalPosition = visualTarget.localPosition;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (selectedCard == gameObject)
        {
            Deselect();
            selectedCard = null;
        }
        else
        {
            if (selectedCard != null)
            {
                selectedCard.GetComponent<CardSelector>()?.Deselect();
            }

            selectedCard = gameObject;
            Select();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        UpdateVisual();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        UpdateVisual();
    }

    public void Select()
    {
        isSelected = true;
        UpdateVisual();
    }

    public void Deselect()
    {
        isSelected = false;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        // Priorität: ausgewählt > hover > normal
        if (isSelected)
        {
            visualTarget.localPosition = originalLocalPosition + new Vector3(0, 40, 0);
        }
        else if (isHovered)
        {
            visualTarget.localPosition = originalLocalPosition + new Vector3(0, 20, 0);
        }
        else
        {
            visualTarget.localPosition = originalLocalPosition;
        }
    }
}
