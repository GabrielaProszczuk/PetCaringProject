using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class Pet : MonoBehaviour
{
    [SerializeField]
    private int _hunger;

    [SerializeField]
    private int _happiness;

    [SerializeField]
    private int _dirt;

    [SerializeField]
    private string _name;

    private bool _serverTime;
    private int _clickCount;

    //movement
    private Rigidbody2D rb2d;
    private float moveSpeed;
    private float jumpForce;
    private float moveHorizontal;
    private float moveVertical;
    private int stepsCounter;
    public GameObject[] dirtStains;



    void Start()
    {
        moveSpeed = 8f;
        jumpForce = 20f;
        rb2d = gameObject.GetComponent<Rigidbody2D>();

        PlayerPrefs.SetInt("cleaning", 0);

       updateStats();

        if (!PlayerPrefs.HasKey("name"))
        {
            PlayerPrefs.SetString("name", "Pupil");
        }
        _name = PlayerPrefs.GetString("name");
        stepsCounter = 0;
    }

    void Update()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        GetComponent<Animator>().SetBool("jump", gameObject.transform.position.y > -1.9f);
        GetComponent<Animator>().SetBool("left", moveHorizontal < 0);
        GetComponent<Animator>().SetBool("right", moveHorizontal > 0);
        

        if (PlayerPrefs.GetInt("eating") == 1)
        {
            StartCoroutine(eating());
        }
        if (PlayerPrefs.GetInt("cleaning") == 1)
        {
            updateDirt(10);
            
            if (_dirt < 100)
                updateHappiness(5);
        }

        if (_happiness < 10 || _hunger < 10)
            GetComponent<Animator>().SetBool("hurt", true);
        else
            GetComponent<Animator>().SetBool("hurt", false);
    }

    IEnumerator eating()
    {
        PlayerPrefs.SetInt("eating", 0);
        updateHunger(5);
        GetComponent<Animator>().SetBool("eats", true);
        yield return new WaitForSeconds(2);
        
        GetComponent<Animator>().SetBool("eats", false);
    }

    void FixedUpdate()
    {
        
        if (moveHorizontal > 0.1f || moveHorizontal < -0.1f)
        {
            rb2d.AddForce(new Vector2(moveHorizontal*moveSpeed, 0f), ForceMode2D.Impulse);
            if (stepsCounter == 0 && moveVertical == 0)
            {
                FindObjectOfType<AudioManager>().Play("steps");
                stepsCounter += 1;
            }
            else if (stepsCounter == 20)
                stepsCounter = 0;
            else
                stepsCounter += 1;
        }
        if (moveVertical > 0.1f || moveVertical < -0.1f)
        {
            rb2d.AddForce(new Vector2(0f, moveVertical * jumpForce), ForceMode2D.Impulse);
            
            if (stepsCounter == 0)
            {
                FindObjectOfType<AudioManager>().Play("jump");
                stepsCounter += 1;
            }
            else if (stepsCounter == 20)
                stepsCounter = 0;
            else
                stepsCounter += 1;
        }
    }

    void updateStats()
    {
        if (!PlayerPrefs.HasKey("_hunger"))
        {
            _hunger = 100;
            PlayerPrefs.SetInt("_hunger", _hunger);
        }
        else
        {
            _hunger = PlayerPrefs.GetInt("_hunger");

        }

        if (!PlayerPrefs.HasKey("_happiness"))
        {
            _happiness = 100;
            PlayerPrefs.SetInt("_happiness", _happiness);
        }
        else
        {
            _happiness = PlayerPrefs.GetInt("_happiness");

        }
        if (!PlayerPrefs.HasKey("_dirt"))
        {
            _dirt = 100;
            PlayerPrefs.SetInt("_dirt", _dirt);
        }
        else
        {
            _dirt = PlayerPrefs.GetInt("_dirt");

        }
        if (!PlayerPrefs.HasKey("then"))
            PlayerPrefs.SetString("then", getStringtime());

        TimeSpan ts = getTimeSpan();

        _hunger -= (int)(ts.TotalHours * 2);

        if (_hunger < 0)
            _hunger = 0;

        _happiness -= (int)((100 - _hunger)*(ts.TotalHours / 5));

        if (_happiness < 0)
            _happiness = 0;

        _dirt -= (int)(ts.TotalHours * 2 / 100) * 100;

        if (_dirt < 0)
            _dirt = 0;


        if (_serverTime)
            updateServer();
        else
            InvokeRepeating("updateDevice", 0f, 30f);
    }

    void updateServer()
    {
        PlayerPrefs.SetString("then", getStringtime());
    }

    void updateDevice()
    {
        
    }

    TimeSpan getTimeSpan()
    {
        if (_serverTime)
        {
            return new TimeSpan();
        }
        else
        {
            return DateTime.Now - Convert.ToDateTime(PlayerPrefs.GetString("then"));
        }
    }

    string getStringtime()
    {
        DateTime now = DateTime.Now;
        return now.Day + "/" + now.Month + "/" + now.Year + " " + now.Hour + ":" + now.Minute + ":" + now.Second;
    }

    public int hunger
    {
        get { return _hunger; }
        set { _hunger = value; }
    }

    public int happiness
    {
        get { return _happiness; }
        set { _happiness = value; }
    }

    public int dirt
    {
        get { return _dirt; }
        set { _dirt = value; }
    }

    public string name
    {
        get { return _name; }
        set { _name = value; }
    }
    public float moveHor
    {
        get { return moveHorizontal; }
        set { moveHorizontal = value; }
    }

    public float moveVer
    {
        get { return moveVertical; }
        set { moveVertical = value; }
    }

    public void updateHappiness(int i)
    {
        happiness += i;
        if(happiness > 100)
        {
            happiness = 100;
        }
    }

    public void updateHunger(int i)
    {
        hunger += i;
        if (hunger > 100)
        {
            hunger = 100;
        }
    }

    public void updateDirt(int i)
    {
        dirt += i;
        if (dirt > 100)
        {
            dirt = 100;
        }
        PlayerPrefs.SetInt("cleaning", 0);
    }

    public void savePet()
    {
        if (!_serverTime)
            updateDevice();

        PlayerPrefs.SetString("then", getStringtime());
        PlayerPrefs.SetInt("_hunger", _hunger);
        PlayerPrefs.SetInt("_happiness", _happiness);
        PlayerPrefs.SetInt("_dirt", _dirt);
        PlayerPrefs.SetInt("cleaning", 0);
        PlayerPrefs.Save();
    }

}
