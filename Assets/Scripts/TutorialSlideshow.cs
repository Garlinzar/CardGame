using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSlideshow : MonoBehaviour
{
  [Header("UI-References")]
  public Image slideImage;
  public Button prevButton;
  public Button nextButton;
  public Button backButton;         // z.B. BackButtonTutorial

  [Header("Slides")]
  public List<Sprite> slides;       // Größe = 6

  private int currentIndex = 0;

  void Awake()
  {
    // Buttons verknüpfen
    prevButton.onClick.AddListener(Prev);
    nextButton.onClick.AddListener(Next);
    backButton.onClick.AddListener(Close);
  }

  void OnEnable()
  {
    // Start mit erstem Bild
    currentIndex = 0;
    UpdateSlide();
  }

  private void UpdateSlide()
  {
    slideImage.sprite = slides[currentIndex];
    // Buttons deaktivieren, wenn am Ende/Anfang
    prevButton.interactable = (currentIndex > 0);
    nextButton.interactable = (currentIndex < slides.Count - 1);
  }

  public void Next()
  {
    if (currentIndex < slides.Count - 1)
    {
      currentIndex++;
      UpdateSlide();
    }
  }

  public void Prev()
  {
    if (currentIndex > 0)
    {
      currentIndex--;
      UpdateSlide();
    }
  }

  public void Close()
  {
    gameObject.SetActive(false);
  }
}