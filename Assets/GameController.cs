using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // Planets and gravity
    GameObject[] _planets;

    // Player, origin and destination objects
    public GameObject player;
    public GameObject projectile;
    public GameObject _destination;

    // UI bits
    public GameObject winUI;
    public GameObject winText;
    public GameObject parTime1Text;
    public GameObject parTime2Text;
    public GameObject parTime3Text;
    public GameObject yourTimeText;
    public GameObject timeText;
    public string[] winnerText;

    // Timer and level-specific stuff
    public float parTime1 = 20.0f;
    public float parTime2 = 15.0f;
    public float parTime3 = 10.0f;

    public Sprite star;
    public Sprite starEmpty;

    public GameObject ratingStar1;
    public GameObject ratingStar2;
    public GameObject ratingStar3;

    private float _timer = 0.0f;
    private bool _timerStart = false;

    private AudioSource[] _objectAudio;
    private Rigidbody2D _projectileRb;
    private PolygonCollider2D _destinationCollider;


    void Start()
    {
        _destinationCollider = _destination.GetComponent<PolygonCollider2D>();
        _projectileRb = projectile.GetComponent<Rigidbody2D>();

        winUI.GetComponent<Canvas>().enabled = false;
        winText.GetComponent<TextMeshProUGUI>().text = winnerText[Random.Range(0, winnerText.Length)].ToString();
        parTime1Text.GetComponent<TextMeshProUGUI>().text = "< " + parTime1.ToString("F2") + "s";
        parTime2Text.GetComponent<TextMeshProUGUI>().text = "< " + parTime2.ToString("F2") + "s";
        parTime3Text.GetComponent<TextMeshProUGUI>().text = "< " + parTime3.ToString("F2") + "s";

        _destination = GameObject.FindGameObjectWithTag("Destination");
        _planets = GameObject.FindGameObjectsWithTag("Planet");

        _objectAudio = GetComponents<AudioSource>();

        GameObject[] backgrounds = GameObject.FindGameObjectsWithTag("Background");

        foreach(GameObject background in backgrounds)
        {
             if (background.GetComponent<Renderer>())
             {
                 background.GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", new Vector2(0, 0));
             }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if(_timerStart)
        {
            _timer += Time.deltaTime;
            var timerString = _timer.ToString("F2") + "s";
            timeText.GetComponent<TextMeshProUGUI>().text = timerString;
        }

        if (_projectileRb.IsTouching(_destinationCollider) && (_projectileRb.IsSleeping() || _projectileRb.inertia < 0.075f) && _timerStart)
        {
            Win();
        }

        if(Input.GetAxis("Restart") > 0.0f)
        {
            ReplayLevel();
        }

        if (Input.GetAxis("Quit") > 0.0f)
        {
            Quit();
        }
    }

    public void StartTimer()
    {
        _timerStart = true;   
    }

    public void StopTimer()
    {
        _timerStart = false;
    }

    public void ResetTimer()
    {
        StopTimer();
        _timer = 0.0f;
        var timerString = _timer.ToString("F2") + "s";
        timeText.GetComponent<TextMeshProUGUI>().text = timerString;
    }
    
    public void Win()
    {
        _timerStart = false;
        yourTimeText.GetComponent<TextMeshProUGUI>().text = _timer.ToString("F2") + "s";

        if (_timer < parTime1)
        {
            ratingStar1.GetComponent<Image>().sprite = star;
            parTime1Text.GetComponent<TextMeshProUGUI>().color = new Color(255, 255, 255);
        }
        if (_timer < parTime2)
        {
            ratingStar2.GetComponent<Image>().sprite = star;
            parTime2Text.GetComponent<TextMeshProUGUI>().color = new Color(255, 255, 255);
        }
        if (_timer < parTime3)
        {
            ratingStar3.GetComponent<Image>().sprite = star;
            parTime3Text.GetComponent<TextMeshProUGUI>().color = new Color(255, 255, 255);
        }
        
        winUI.GetComponent<Canvas>().enabled = true;
        _objectAudio[0].Play();
    }

    public void ResetLevel()
    {
        winUI.GetComponent<Canvas>().enabled = false;

        ResetTimer();
        ratingStar1.GetComponent<Image>().sprite = starEmpty;
        ratingStar2.GetComponent<Image>().sprite = starEmpty;
        ratingStar3.GetComponent<Image>().sprite = starEmpty;

        Color dimmed = new Color(190, 190, 190);

        parTime1Text.GetComponent<TextMeshProUGUI>().color = dimmed;
        parTime2Text.GetComponent<TextMeshProUGUI>().color = dimmed;
        parTime3Text.GetComponent<TextMeshProUGUI>().color = dimmed;
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadLevel(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void ReplayLevel()
    {
        _timer = 0.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Menu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
