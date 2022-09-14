using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject pet;

    public GameObject hungerText;
    public GameObject happinessText;

    public GameObject namePanel;
    public GameObject nameInput;
    public GameObject nameText;
    public GameObject[] petList;
    public GameObject petPanel;

    public GameObject homePanel;
    public Sprite[] homeTilesSprites;
    public GameObject[] homeTiles;

    public GameObject background;
    public Sprite[] backgroundOptions;

    public GameObject foodPanel;
    public Sprite[] foodIcons;

    public GameObject happyBubble;
    public GameObject angryBubble;
    public GameObject loveBubble;
    public GameObject hungryBubble;

    public GameObject[] dirtStains;
    public GameObject[] hearts;
    private int _clickCount;
    private int _touchCount;
    private int mute;
    public GameObject ball;


    private int petHunger;
    private int petDirt;
    private int petHappiness;

    void Start()
    {

        PlayerPrefs.Save();

        if (!PlayerPrefs.HasKey("looks"))
            PlayerPrefs.SetInt("looks", 0);

        createPet(PlayerPrefs.GetInt("looks"));


        if (!PlayerPrefs.HasKey("tiles"))
            PlayerPrefs.SetInt("tiles", 0);

        changeTiles(PlayerPrefs.GetInt("tiles"));

        if (!PlayerPrefs.HasKey("background"))
            PlayerPrefs.SetInt("background", 0);

        changeBackground(PlayerPrefs.GetInt("background"));
        _touchCount = 0;
        FindObjectOfType<AudioManager>().Play("hi");

    }
    void Update()
    {

        happinessText.GetComponent<Text> ().text = pet.GetComponent<Pet>().happiness.ToString();
        hungerText.GetComponent<Text> ().text = pet.GetComponent<Pet>().hunger.ToString();
        nameText.GetComponent<Text>().text = pet.GetComponent<Pet>().name;
        toggleDirt();
        showEmotions();

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 v = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(v), Vector2.zero);
            if (hit)
            {
                if (hit.transform.gameObject.tag == "ball")
                {
                    _clickCount++;
                    if (_clickCount >= 3)
                    {
                        _clickCount = 0;
                        pet.GetComponent<Pet>().updateHappiness(1);
                        ball.GetComponent<Rigidbody2D>().AddForce(new Vector2(1f, 1f), ForceMode2D.Impulse);
                    }
                }
            }
        }
        if (pet.GetComponent<Collider2D>().IsTouching(ball.GetComponent<Collider2D>()))
        {
            if(_touchCount == 20)
            {
                pet.GetComponent<Pet>().updateHappiness(1);
                _touchCount = 0;
            }
            else
            {
                _touchCount += 1;
            }
            
        }

        for(int i=0; i<hearts.Length; i++)
        {
            
            if (pet.GetComponent<Collider2D>().IsTouching(hearts[i].GetComponent<Collider2D>()))
            {
                pet.GetComponent<Pet>().updateHappiness(5);
                hearts[i].SetActive(false);
                FindObjectOfType<AudioManager>().Play("heart");

            }
        }


    }


    public void triggerNamePanel(bool b)
    {
        namePanel.SetActive(!namePanel.activeInHierarchy);

        if (b)
        {
            pet.GetComponent<Pet>().name = nameInput.GetComponent<InputField>().text;
            PlayerPrefs.SetString("name", pet.GetComponent<Pet>().name);
        }
    }

    public void buttonBehavior(int i)
    {
        switch (i)
        {
            case (0):
            default:
                petPanel.SetActive(!petPanel.activeInHierarchy);
                break;
            case (1):
                homePanel.SetActive(!homePanel.activeInHierarchy);
                break;
            case (2):
                foodPanel.SetActive(!foodPanel.activeInHierarchy);
                break;
            case (3):
                if (mute == 1)
                {
                    AudioListener.volume = 0;
                    mute = 0;
                    GameObject.FindGameObjectWithTag("sound").GetComponent<CanvasGroup>().alpha = 0.6f;
                }
                else
                {
                    AudioListener.volume = 1;
                    mute = 1;
                    GameObject.FindGameObjectWithTag("sound").GetComponent<CanvasGroup>().alpha = 1f;
                }
                break;
            case (4):
                pet.GetComponent<Pet>().savePet();
                Application.Quit();
                break;
        }
    }

    public void createPet(int i)
    {
        if (pet)
        {
            Destroy(pet);
        }

        pet = Instantiate(petList[i], Vector3.zero, Quaternion.identity) as GameObject;

        FindObjectOfType<AudioManager>().Play("hi");

        toggle(petPanel);

        PlayerPrefs.SetInt("looks", i);
    }

    public void changePet(int i)
    {
        if (pet)
        {
            pet.GetComponent<Pet>().savePet();
            Destroy(pet);
        }

        pet = Instantiate(petList[i], Vector3.zero, Quaternion.identity) as GameObject;

        FindObjectOfType<AudioManager>().Play("hi");

        toggle(petPanel);

        PlayerPrefs.SetInt("looks", i);
    }

    public void changeTiles(int t)
    {
        for(int i = 0; i < homeTiles.Length; i++)
        {
            homeTiles[i].GetComponent<SpriteRenderer>().sprite = homeTilesSprites[t];
        }

        toggle(homePanel);

        PlayerPrefs.SetInt("tiles", t);
        FindObjectOfType<AudioManager>().Play("UI");
    }

    public void changeBackground(int i)
    {

        background.GetComponent<SpriteRenderer>().sprite = backgroundOptions[i];

        toggle(homePanel);

        PlayerPrefs.SetInt("background", i);
        FindObjectOfType<AudioManager>().Play("UI");
    }

    public void selectFood(int i)
    {
        //foodPanel.GetComponent<SpriteRenderer>().sprite = foodIcons[i];
    }

    public void toggle(GameObject g)
    {
        if (g.activeInHierarchy)
            g.SetActive(false);
    }

    public void showEmotions()
    {

        if (pet.GetComponent<Pet>().hunger < 10)
        {
            hungryBubble.SetActive(true);
        }
        else
            hungryBubble.SetActive(false);

        if (pet.GetComponent<Pet>().happiness < 10)
        {
            angryBubble.SetActive(true);
        }
        else
            angryBubble.SetActive(false);

        if (pet.GetComponent<Pet>().happiness > 90)
        {
            loveBubble.SetActive(true);
            if (pet.GetComponent<Pet>().happiness == 100)
                FindObjectOfType<AudioManager>().Play("yeah");            
        }
        else
            loveBubble.SetActive(false);
    }

    public void toggleDirt()
    {
        if (pet.GetComponent<Pet>().dirt < 100)
        {
            for (int i = pet.GetComponent<Pet>().dirt; i < 100; i+=10)
            {
                dirtStains[i/10].SetActive(true);
            }
            for (int i = 0; i <= pet.GetComponent<Pet>().dirt; i+=10)
            {
                dirtStains[i/10].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < pet.GetComponent<Pet>().dirt; i+=10)
            {
                dirtStains[i/10].SetActive(false);
            }
        }
    }

}
