using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    // Player, origin and destination objects
    public GameObject player;
    public GameObject projectile;
    [FormerlySerializedAs("_destination")] public GameObject destination;

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

    private float _timer;
    private bool _timerStart;

    private AudioSource[] _objectAudio;
    private Rigidbody2D _projectileRb;
    private PolygonCollider2D _destinationCollider;
    private TextMeshProUGUI _timeText;
    private TextMeshProUGUI _yourTimeText;
    private Canvas _winUI;
    private Image _ratingStar3;
    private TextMeshProUGUI _parTime3Text;
    private TextMeshProUGUI _parTime2Text;
    private Image _ratingStar2;
    private TextMeshProUGUI _parTime1Text;
    private Image _ratingStar1;
    private static readonly int MainTex = Shader.PropertyToID("_MainTex");


    void Start()
    {
        _ratingStar1 = ratingStar1.GetComponent<Image>();
        _parTime1Text = parTime1Text.GetComponent<TextMeshProUGUI>();
        _ratingStar2 = ratingStar2.GetComponent<Image>();
        _parTime2Text = parTime2Text.GetComponent<TextMeshProUGUI>();
        _parTime3Text = parTime3Text.GetComponent<TextMeshProUGUI>();
        _ratingStar3 = ratingStar3.GetComponent<Image>();
        _winUI = winUI.GetComponent<Canvas>();
        _yourTimeText = yourTimeText.GetComponent<TextMeshProUGUI>();
        _timeText = timeText.GetComponent<TextMeshProUGUI>();
        _destinationCollider = destination.GetComponent<PolygonCollider2D>();
        _projectileRb = projectile.GetComponent<Rigidbody2D>();

        winUI.GetComponent<Canvas>().enabled = false;
        winText.GetComponent<TextMeshProUGUI>().text = winnerText[Random.Range(0, winnerText.Length)];
        parTime1Text.GetComponent<TextMeshProUGUI>().text = "< " + parTime1.ToString("F2") + "s";
        parTime2Text.GetComponent<TextMeshProUGUI>().text = "< " + parTime2.ToString("F2") + "s";
        parTime3Text.GetComponent<TextMeshProUGUI>().text = "< " + parTime3.ToString("F2") + "s";

        destination = GameObject.FindGameObjectWithTag("Destination");

        _objectAudio = GetComponents<AudioSource>();

        GameObject[] backgrounds = GameObject.FindGameObjectsWithTag("Background");

        foreach(GameObject background in backgrounds)
        {
             if (background.GetComponent<Renderer>())
             {
                 background.GetComponent<Renderer>().sharedMaterial.SetTextureOffset(MainTex, new Vector2(0, 0));
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
            _timeText.text = timerString;
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
        _timeText.text = timerString;
    }
    
    public void Win()
    {
        _timerStart = false;
        _yourTimeText.text = _timer.ToString("F2") + "s";

        if (_timer < parTime1)
        {
            _ratingStar1.sprite = star;
            _parTime1Text.color = new Color(255, 255, 255);
        }
        if (_timer < parTime2)
        {
            _ratingStar2.sprite = star;
            _parTime2Text.color = new Color(255, 255, 255);
        }
        if (_timer < parTime3)
        {
            _ratingStar3.sprite = star;
            _parTime3Text.color = new Color(255, 255, 255);
        }
        
        _winUI.enabled = true;
        _objectAudio[0].Play();
    }

    public void ResetLevel()
    {
        _winUI.enabled = false;

        ResetTimer();
        _ratingStar1.sprite = starEmpty;
        _ratingStar2.sprite = starEmpty;
        _ratingStar3.sprite = starEmpty;

        Color dimmed = new Color(190, 190, 190);
        _parTime1Text.color = dimmed;
        _parTime2Text.color = dimmed;
        _parTime3Text.color = dimmed;
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
