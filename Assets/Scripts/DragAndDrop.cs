using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Threading.Tasks;

public class DragAndDrop : MonoBehaviour,  IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    public  GameObject pet;
    private RectTransform rectTransform;
    private Vector2 initialPosition;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        initialPosition = rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        Vector2 v = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(v), Vector2.zero);
        if (hit)
        {
            if (hit.transform.gameObject.tag == "pupil")
            { 
                StartCoroutine(eating());
            }  
        }
        else
        {
            rectTransform.anchoredPosition = initialPosition;
        }
    }

    IEnumerator eating()
    {
        PlayerPrefs.SetInt("eating", 1);
        FindObjectOfType<AudioManager>().Play("eating");
        yield return new WaitForSeconds(2);
        FindObjectOfType<AudioManager>().Play("yay");
        rectTransform.anchoredPosition = initialPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnDrop(PointerEventData eventData)
    {

    }
}
