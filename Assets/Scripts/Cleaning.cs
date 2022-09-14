using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Threading.Tasks;

public class Cleaning : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    public GameObject pet;
    private RectTransform rectTransform;
    private Vector2 initialPosition;
    private CanvasGroup canvasGroup;
    public GameObject bubbleRed;
    public GameObject bubbleYellow;
    public GameObject bubbleBlue;
    public GameObject bubblePurple;
    public GameObject bubbleGreen;
    public GameObject sponge;

    public int bubblesCounter;
    public int pos;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        initialPosition = rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;
        if(bubblesCounter == 50)
        {
            clean();
            bubblesCounter = 0;
            pos += 1;
            PlayerPrefs.SetInt("cleaning", 1);
            if (pos == 10)
                pos = 0;
        }
        else
        {
            bubblesCounter += 1;
            
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        bubblesCounter = 0;
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
            }
        }
        else
        {
            rectTransform.anchoredPosition = initialPosition;
        }

    }

    public void clean()
    {
        bubbleRed = Instantiate(bubbleRed, new Vector3(GameObject.FindGameObjectWithTag("pupil").transform.position.x - 0.3f*pos, GameObject.FindGameObjectWithTag("pupil").transform.position.y + 0.1f*pos, 0f), Quaternion.identity) as GameObject;
        bubbleRed.SetActive(true);
        bubbleYellow = Instantiate(bubbleYellow, new Vector3(GameObject.FindGameObjectWithTag("pupil").transform.position.x + 0.2f*pos, GameObject.FindGameObjectWithTag("pupil").transform.position.y + 0.8f*pos, 0f), Quaternion.identity) as GameObject;
        bubbleYellow.SetActive(true);
        bubbleGreen = Instantiate(bubbleGreen, new Vector3(GameObject.FindGameObjectWithTag("pupil").transform.position.x + 0.7f*pos, GameObject.FindGameObjectWithTag("pupil").transform.position.y + 0.4f*pos, 0f), Quaternion.identity) as GameObject;
        bubbleGreen.SetActive(true);
        bubblePurple = Instantiate(bubblePurple, new Vector3(GameObject.FindGameObjectWithTag("pupil").transform.position.x - 0.2f * pos, GameObject.FindGameObjectWithTag("pupil").transform.position.y + 0.1f * pos, 0f), Quaternion.identity) as GameObject;
        bubblePurple.SetActive(true);
        bubbleBlue = Instantiate(bubbleBlue, new Vector3(GameObject.FindGameObjectWithTag("pupil").transform.position.x + 0.4f * pos, GameObject.FindGameObjectWithTag("pupil").transform.position.y - 0.2f * pos, 0f), Quaternion.identity) as GameObject;
        bubbleBlue.SetActive(true);
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnDrop(PointerEventData eventData)
    {

    }
}
