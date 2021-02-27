using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public abstract class ButtonBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public AudioSource hoverSound;
    public Vector3 defaultScale;

    private Image image;
    private void Awake()
    {
        defaultScale = this.transform.localScale;
        image = GetComponent<Image>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Enlarge();
        hoverSound.Play();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = defaultScale;
    }
    public abstract void OnClick();
    protected void Enlarge(){
        transform.DOScale(defaultScale + new Vector3(0.2f,0.2f,0.2f), 0.01f);
    }
    protected void FadeOut()
    {
        image.DOFade(0f, 1f);
    }
}
