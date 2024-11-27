using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq; 
using Newtonsoft.Json; 


public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject startButton, scoreButton, restartButton, player, ball, blockPrefab, shadowPrefab;
    [SerializeField]
    TMP_Text scoreText, diamondText, highScoreText, highScoreEndText, restartButtonText;
    [SerializeField]
    private GameObject popupPanel;  
    [SerializeField]
    private TMP_Text questionText;  
    [SerializeField]
    private Button answerButton1, answerButton2, answerButton3;  

    [SerializeField]
    private Vector3 startPos, offset;  

    [System.Serializable]
    public class PlanetQuestion
    {
        public string questionText;
        public string[] answers;
        public int correctAnswerIndex;
    }

    [SerializeField]
    private PlanetCollector planetCollector;

    [SerializeField]
    private TMP_Text collectedPlanetCountText;

    [SerializeField] private GameObject fireworkPrefab; 
    [SerializeField] private Transform fireworkSpawnPoint; 

    [SerializeField] private TMP_Text endText; 

    [SerializeField] private AudioClip fireworkSound;

    [SerializeField] private AudioClip rightAnswerSound; 
    [SerializeField] private AudioClip wrongAnswerSound; 
    [SerializeField] private AudioClip gameOverSound; 

    private int score, diamonds, highScore, planets, combo;
    private bool isCombo, gameHasStarted = false, isPopupActive = false, isSelectingAnswer = false;
    private int selectedAnswerIndex = 0;
    private string upcomingPlanet = "";
    public static GameManager instance;
    public delegate void SetComboAnimation(bool isCombo);
    public event SetComboAnimation updateComboAnimation;
    public bool IsFirstBlock { get; set; } = true; 
    public bool IsCombo
    {
        get { return isCombo; }
        set
        {
            isCombo = value;
            updateComboAnimation?.Invoke(isCombo);  
        }
    }

    /// <summary>
    /// Ensures a single instance of the GameManager exists.
    /// </summary>
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private Dictionary<string, List<PlanetQuestion>> planetQuestions;
    private PlanetQuestion currentQuestion;

    /// <summary>
    /// Initializes the game, loads saved data, and prepares the UI.
    /// </summary>
    private void Start()
    {

        //ClearHighScore();
        LoadAskedQuestionsTracker(); 
        InitializePlanetQuestions();
        scoreButton.SetActive(false);
        restartButton.SetActive(false);

        score = 0;
        combo = 0;
        highScore = PlayerPrefs.HasKey("HighScore") ? PlayerPrefs.GetInt("HighScore") : 0;
        planets = 0;
        diamonds = 0;

        diamondText.text = diamonds.ToString();
        highScoreText.text = "BÄST : " + highScore.ToString();

        for (int i = 0; i < 4; i++)
        {
            SpawnBlock();
        }

        popupPanel.SetActive(false);
        UpdateCollectedPlanetCount(); 
    }

    /// <summary>
    /// Clears the saved high score.
    /// </summary>
    public void ClearHighScore()
    {
        PlayerPrefs.SetInt("HighScore", 0); 
        PlayerPrefs.Save(); 
        highScore = 0; 
        highScoreText.text = "BÄST : " + highScore.ToString(); 
        Debug.Log("High score cleared.");
    }

    /// <summary>
    /// Initializes planet-related questions for each planet.
    /// </summary>
    private void InitializePlanetQuestions()
    {
        planetQuestions = new Dictionary<string, List<PlanetQuestion>>();

        planetQuestions["Mercury"] = new List<PlanetQuestion>
        {
            new PlanetQuestion
            {
                questionText = "Vilken planet är närmast solen?",
                answers = new string[] { "Jorden", "Venus", "Merkurius" },
                correctAnswerIndex = 2
            },
            new PlanetQuestion
            {
                questionText = "Hur lång tid tar en rymdresa till Merkurius",
                answers = new string[] { "10 år", "4 år", "100 år" },
                correctAnswerIndex = 1
            },
            new PlanetQuestion
            {
                questionText = "Vad är medeltemperaturen Merkurius?",
                answers = new string[] { "117 grader", "0 grader", "-50 grader" },
                correctAnswerIndex = 0
            },
            new PlanetQuestion
            {
                questionText = "Hur många Merkurius skulle få plats inuti Jorden?",
                answers = new string[] { "50", "1", "18" },
                correctAnswerIndex = 2
            },
            new PlanetQuestion
            {
                questionText = "Hur lång tid tar det för Merkurius att snurra runt Solen?",
                answers = new string[] { "88 jord-dagar", "2 jord-dagar", "1 år" },
                correctAnswerIndex = 0
            },
            new PlanetQuestion
            {
                questionText = "Merkurius upptäcktes för ___ år sedan?",
                answers = new string[] { "3000", "2", "100" },
                correctAnswerIndex = 0
            },
            new PlanetQuestion
            {
                questionText = "Vilken färg har Merkurius?",
                answers = new string[] { "Blå", "Rosa", "Mörkgrå" },
                correctAnswerIndex = 2
            },
            new PlanetQuestion
            {
                questionText = "Hur många månar har Merkurius?",
                answers = new string[] { "10", "0", "5" },
                correctAnswerIndex = 1
            }
        };
        planetQuestions["Venus"] = new List<PlanetQuestion>
        {
            new PlanetQuestion
            {
                questionText = "Vilken är den andra planeten från solen?",
                answers = new string[] { "Merkurius", "Venus", "Jorden" },
                correctAnswerIndex = 1
            },
            new PlanetQuestion
            {
                questionText = "Vad består Venus himmel av?",
                answers = new string[] { "Syra-moln", "glass", "vattenånga" },
                correctAnswerIndex = 0
            },
            new PlanetQuestion
            {
                questionText = "När upptäcktes Venus?",
                answers = new string[] { "År 110", "År 1999", "År 1610" },
                correctAnswerIndex = 2
            },
            new PlanetQuestion
            {
                questionText = "Hur många jord-dagar är ett dygn på Venus?",
                answers = new string[] { "243 dagar", "3 dagar", "50 dagar" },
                correctAnswerIndex = 0
            },
            new PlanetQuestion
            {
                questionText = "Är ett dygn på Venus längre än ett år på Venus?",
                answers = new string[] { "Nej", "Ja", "lika långa" },
                correctAnswerIndex = 1
            },
            new PlanetQuestion
            {
                questionText = "Hur många månar har Venus? ",
                answers = new string[] { "2", "30", "0" },
                correctAnswerIndex = 2
            },
            new PlanetQuestion
            {
                questionText = "Vilken planet är varmare än Venus?",
                answers = new string[] { "Ingen", "Merkurius", "Jorden" },
                correctAnswerIndex = 0
            },
            new PlanetQuestion
            {
                questionText = "Vilken planet har flest vulkaner?",
                answers = new string[] { "Jorden", "Jupiter", "Venus" },
                correctAnswerIndex = 2
            },
            new PlanetQuestion
            {
                questionText = "Venus är ____ än jorden.",
                answers = new string[] { "kallare", "varmare", "mildare" },
                correctAnswerIndex = 1
            }
        };

        planetQuestions["Earth"] = new List<PlanetQuestion>
        {
            new PlanetQuestion
            {
                questionText = "Vilken planet bor vi på?",
                answers = new string[] { "Jorden", "Saturnus", "Mars" },
                correctAnswerIndex = 0
            },
            new PlanetQuestion
            {
                questionText = "Vilken planet är den tredje planeten från solen?",
                answers = new string[] { "Saturnus", "Uranus", "Jorden" },
                correctAnswerIndex = 2
            },
            new PlanetQuestion
            {
                questionText = "Vad är Jordens vetenskapliga namn?",
                answers = new string[] { "Skibidi", "Tellus", "Telia" },
                correctAnswerIndex = 1
            },
            new PlanetQuestion
            {
                questionText = "Hur gammal är Jorden?",
                answers = new string[] { "4.5 miljarder år", "1000 år", "30000 år" },
                correctAnswerIndex = 0
            },
            new PlanetQuestion
            {
                questionText = "På vilken planet uppfanns pizza?",
                answers = new string[] { "Saturnus", "Jorden", "Jupiter" },
                correctAnswerIndex = 1
            },
            new PlanetQuestion
            {
                questionText = "Vad är Jordens yta mest täckt av? ",
                answers = new string[] { "Is", "Land", "Vatten" },
                correctAnswerIndex = 2
            },
            new PlanetQuestion
            {
                questionText = "Finns det djur på andra planeter än Jorden?",
                answers = new string[] { "Nej", "Ja, på Mars", "Ja, på Venus" },
                correctAnswerIndex = 0
            },
            new PlanetQuestion
            {
                questionText = "Vilka planeter skulle få plats mellan Jorden och Månen?",
                answers = new string[] { "Inga", "Bara Jupiter", "Alla" },
                correctAnswerIndex = 2
            },

            new PlanetQuestion
            {
                questionText = "Hur många dagar tar det för jorden att kretsa runt solen?",
                answers = new string[] { "75 dagar", "365 dagar", "100 dagar" },
                correctAnswerIndex = 1
            }
        };

        planetQuestions["Mars"] = new List<PlanetQuestion>
        {
            new PlanetQuestion
            {
                questionText = "Mars är känd som___?",
                answers = new string[] { "Den röda planeten", "Den blå planeten", "Den fula planeten" },
                correctAnswerIndex = 0
            },
            new PlanetQuestion
            {
                questionText = "Hur många månar har Mars?",
                answers = new string[] { "10", "2", "22" },
                correctAnswerIndex = 1
            },
            new PlanetQuestion
            {
                questionText = "Hur länge kan en sandstorm vara på Mars?",
                answers = new string[] { "Flera veckor", "1 år", "Finns inga" },
                correctAnswerIndex = 0
            },
            new PlanetQuestion
            {
                questionText = "På Mars skulle en 5 kg katt väga ungefär lika mycket som…?",
                answers = new string[] { "Elefant", "Råtta", "Humla" },
                correctAnswerIndex = 1
            },
            new PlanetQuestion
            {
                questionText = "Vilken planet har den högsta vulkanen i solsystemet, högre än något berg på jorden?",
                answers = new string[] { "Mercury", "Uranus", "Mars" },
                correctAnswerIndex = 2
            },
            new PlanetQuestion
            {
                questionText = "Vad ger Mars sin röda färg?",
                answers = new string[] { "Rost", "Svavel", "Jorgubbar" },
                correctAnswerIndex = 0
            },
            new PlanetQuestion
            {
                questionText = "Hur mycket högre kan du hoppa på Mars?",
                answers = new string[] { "7 ggr", "3 ggr", "20 ggr" },
                correctAnswerIndex = 1
            },

            new PlanetQuestion
            {
                questionText = "Vilken chokladkaka har samma namn som en planet?",
                answers = new string[] { "Venus", "Uranus", "Mars" },
                correctAnswerIndex = 2
            }
        };

        planetQuestions["Jupiter"] = new List<PlanetQuestion>
        {
            new PlanetQuestion
            {
                questionText = "Vilken är den största planeten i solsystemet?",
                answers = new string[] { "Jorden", "Jupiter", "Mars" },
                correctAnswerIndex = 1
            },
            new PlanetQuestion
            {
                questionText = "Hur många jordklot skulle få plats i en av Jupiters stormar?",
                answers = new string[] { "3", "20", "1" },
                correctAnswerIndex = 0
            },
            new PlanetQuestion
            {
                questionText = "Vilken planet är så stor att den skulle kunna äta upp alla andra planeter till frukost?",
                answers = new string[] { "Jorden", "Jupiter", "Merkurius" },
                correctAnswerIndex = 1
            },
            new PlanetQuestion
            {
                questionText = "Jupiter är en___ .",
                answers = new string[] { "måne", "liten stenplanet", "gigantisk gasboll" },
                correctAnswerIndex = 2
            },
            new PlanetQuestion
            {
                questionText = "Vad luktar Jupiter som?",
                answers = new string[] { "Ruttna ägg", "Blommor", "köttbullar" },
                correctAnswerIndex = 0
            },
            new PlanetQuestion
            {
                questionText = "Hur mycket mer hade du vägt på Jupiter  än på jorden?",
                answers = new string[] { "10 ggr mer", "2.4 ggr mer", "6 ggr mer" },
                correctAnswerIndex = 1
            },
            new PlanetQuestion
            {
                questionText = "Hur många månar har Jupiter?",
                answers = new string[] { "0", "1", "95" },
                correctAnswerIndex = 2
            },
            new PlanetQuestion
            {
                questionText = "Jupiter är uppkallad efter den romerska guden av ___?",
                answers = new string[] { "Kriget", "Havet", "Himlen" },
                correctAnswerIndex = 2
            }
        };

        planetQuestions["Saturn"] = new List<PlanetQuestion>
        {
            new PlanetQuestion
            {
                questionText = "Vilken planet är känd för sina vackra ringar?",
                answers = new string[] { "Saturnus", "Mars", "Jorden" },
                correctAnswerIndex = 0
            },
            new PlanetQuestion
            {
                questionText = "Vilken planet är så lätt att den skulle kunna flyta i ett gigantiskt badkar?",
                answers = new string[] { "Saturnus", "Mercury", "Jorden" },
                correctAnswerIndex = 0
            },
            new PlanetQuestion
            {
                questionText = "Hur skulle det kännas att ta på Saturnus ringar? Som…",
                answers = new string[] { "en bordsskiva", "snö och sand", "luft" },
                correctAnswerIndex = 1
            },
            new PlanetQuestion
            {
                questionText = "Hur långt är ett dygn på Saturnus jämfört med jorden ?",
                answers = new string[] { "Längre", "Lika långt", "Kortare" },
                correctAnswerIndex = 2
            },
            new PlanetQuestion
            {
                questionText = "Hur många månar har Saturnus?",
                answers = new string[] { "146", "2", "50" },
                correctAnswerIndex = 0
            },
            new PlanetQuestion
            {
                questionText = "Saturnus är ___ än jorden.",
                answers = new string[] { "Mycket mindre", "Mindre", "Större" },
                correctAnswerIndex = 2
            },
            new PlanetQuestion
            {
                questionText = "Hur lång tid tar det för Saturnus att kretsa runt solen?",
                answers = new string[] { "1 år", "29 år", "2 år" },
                correctAnswerIndex = 1
            }
        };

        planetQuestions["Uranus"] = new List<PlanetQuestion>
        {
            new PlanetQuestion
            {
                questionText = "Vad luktar uranus som?",
                answers = new string[] { "Gosedjur", "Fis", "Jordgubbar" },
                correctAnswerIndex = 1
            },
            new PlanetQuestion
            {
                questionText = "Vilken planet snurrar på sidan som en tunna som rullar genom rymden?",
                answers = new string[] { "Uranus", "Jorden", "Venus" },
                correctAnswerIndex = 0
            },
            new PlanetQuestion
            {
                questionText = "Vilken färg har Uranus?",
                answers = new string[] { "Röd", "Blå", "Gul" },
                correctAnswerIndex = 1
            },
            new PlanetQuestion
            {
                questionText = "Hur långt är ett dygn på Uranus? Runt ...",
                answers = new string[] { "2 timmar", "1 dag", "17 timmar" },
                correctAnswerIndex = 2
            },
            new PlanetQuestion
            {
                questionText = "Hur många månader har ett år på Uranus?",
                answers = new string[] { "28", "60", "200" },
                correctAnswerIndex = 0
            },
            new PlanetQuestion
            {
                questionText = "Hur många jordklot får plats i uranus?",
                answers = new string[] { "2", "63", "10" },
                correctAnswerIndex = 1
            },

            new PlanetQuestion
            {
                questionText = "Uranus är en ___ jätte",
                answers = new string[] { "röd", "eld", "is" },
                correctAnswerIndex = 2
            },
            new PlanetQuestion
            {
                questionText = "Vilken planet är den sjunde från solen?",
                answers = new string[] { "Uranus", "Jorden", "Neptunus" },
                correctAnswerIndex = 0
            }
        };

        planetQuestions["Neptune"] = new List<PlanetQuestion>
        {
            new PlanetQuestion
            {
                questionText = "Vilken planet är längst bort från solen?",
                answers = new string[] { "Jorden", "Venus", "Neptunus" },
                correctAnswerIndex = 2
            },
            new PlanetQuestion
            {
                questionText = "Vilket klimat har Neptunus?",
                answers = new string[] { "Vilda vindar", "Soligt och varmt", "Snöigt" },
                correctAnswerIndex = 0
            },
            new PlanetQuestion
            {
                questionText = "Vilken planet har en mystisk mörk fläck som ser ut som ett stort 'rymdblåmärke'?",
                answers = new string[] { "Mars", "Neptunus", "Jorden" },
                correctAnswerIndex = 1
            },
            new PlanetQuestion
            {
                questionText = "Vilken planet är så blå att den ser ut som ett gigantiskt hav som flyter i rymden?",
                answers = new string[] { "Venus", "Merkurius", "Neptunus" },
                correctAnswerIndex = 2
            },
            new PlanetQuestion
            {
                questionText = "Kan du se Neptunus från jorden med blotta ögat? ",
                answers = new string[] { "Nej", "Ja", "Med kikare" },
                correctAnswerIndex = 0
            },
            new PlanetQuestion
            {
                questionText = "Hur många ringar har Neptunus?",
                answers = new string[] { "Inga", "Minst 5", "2" },
                correctAnswerIndex = 1
            },
            new PlanetQuestion
            {
                questionText = "Hur kallt är på Neptunus?",
                answers = new string[] { "-220", "20", "0" },
                correctAnswerIndex = 0
            },
            new PlanetQuestion
            {
                questionText = "Vilken färg har Neptunus?",
                answers = new string[] { "Rosa", "Blå", "Grön" },
                correctAnswerIndex = 1
            }
        };
    }

    /// <summary>
    /// Updates the game state each frame, handling input and checking for game conditions.
    /// </summary>
    private void Update()
    {
        if (isPopupActive && restartButton.activeInHierarchy && !isSelectingAnswer)
        {
            HandlePlayAgainInput(); 
        }

        if (isPopupActive && isSelectingAnswer)
        {
            HandleAnswerSelection();  
        }

        if (!gameHasStarted && Input.GetKeyDown(KeyCode.UpArrow) || DancePadInputManager.instance.moveUp)
        {
            GameStart();
        }
    }

    /// <summary>
    /// Handles player input for selecting answers when a popup is active.
    /// </summary>
    private void HandleAnswerSelection()
    {
        if (DancePadInputManager.instance != null)
        {
            if (DancePadInputManager.instance.IsLeftPressedOnce())
            {
                selectedAnswerIndex = Mathf.Clamp(selectedAnswerIndex - 1, 0, 2);
                HighlightSelectedAnswer();
            }
            else if (DancePadInputManager.instance.IsRightPressedOnce())
            {
                selectedAnswerIndex = Mathf.Clamp(selectedAnswerIndex + 1, 0, 2);
                HighlightSelectedAnswer();
            }

            if (DancePadInputManager.instance.moveUp || DancePadInputManager.instance.moveDown)
            {
                ConfirmAnswer();
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectedAnswerIndex = Mathf.Clamp(selectedAnswerIndex - 1, 0, 2);
            HighlightSelectedAnswer();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectedAnswerIndex = Mathf.Clamp(selectedAnswerIndex + 1, 0, 2);
            HighlightSelectedAnswer();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            ConfirmAnswer();
        }
    }

    private void HighlightSelectedAnswer()
    {
        Vector3 originalScale = new Vector3(2f, 2f, 2f);
        Vector3 enlargedScale = originalScale * 1.2f;

        Color normalColor = Color.white;                          
        Color selectedColor = new Color(1f, 0.75f, 0.8f);         
        Color pressedColor = Color.green;                         

        answerButton1.transform.localScale = originalScale;
        answerButton2.transform.localScale = originalScale;
        answerButton3.transform.localScale = originalScale;

        answerButton1.GetComponent<Image>().color = normalColor;
        answerButton2.GetComponent<Image>().color = normalColor;
        answerButton3.GetComponent<Image>().color = normalColor;

        switch (selectedAnswerIndex)
        {
            case 0:
                answerButton1.transform.localScale = enlargedScale;
                answerButton1.GetComponent<Image>().color = selectedColor; 
                break;
            case 1:
                answerButton2.transform.localScale = enlargedScale;
                answerButton2.GetComponent<Image>().color = selectedColor; 
                break;
            case 2:
                answerButton3.transform.localScale = enlargedScale;
                answerButton3.GetComponent<Image>().color = selectedColor; 
                break;
        }

        if (Input.GetKeyDown(KeyCode.Return)) 
        {
            switch (selectedAnswerIndex)
            {
                case 0:
                    answerButton1.GetComponent<Image>().color = pressedColor;
                    break;
                case 1:
                    answerButton2.GetComponent<Image>().color = pressedColor;
                    break;
                case 2:
                    answerButton3.GetComponent<Image>().color = pressedColor;
                    break;
            }
        }
    }

    /// <summary>
    /// Confirms the selected answer and applies the corresponding result.
    /// </summary>
    private void ConfirmAnswer()
    {
        if (selectedAnswerIndex == currentQuestion.correctAnswerIndex)
        {
            Debug.Log("Correct answer!");

            PlaySound(rightAnswerSound);

            score += 10;
            scoreText.text = score.ToString();

            if (planetCollector != null && !string.IsNullOrEmpty(upcomingPlanet))
            {
                planetCollector.CollectPlanet(upcomingPlanet);
            }
        }
        else
        {
            Debug.Log("Incorrect answer.");

            PlaySound(wrongAnswerSound, playLastSecond: true);
        }

        isSelectingAnswer = false;
        isPopupActive = false;
        Time.timeScale = 1f;
        popupPanel.SetActive(false);
    }

    private void PlaySound(AudioClip clip, bool playLastSecond = false)
    {
        if (clip != null)
        {
            GameObject audioObject = new GameObject("TempAudio");
            AudioSource audioSource = audioObject.AddComponent<AudioSource>();

            audioSource.clip = clip;
            audioSource.playOnAwake = false;

            if (playLastSecond && clip.length > 1f)
            {
                audioSource.time = clip.length - 1f; 
            }

            audioSource.Play();
            Destroy(audioObject, clip.length - audioSource.time);
        }
    }

    public void ShowPlanetPopup(string planetName)
    {
        upcomingPlanet = planetName;
        currentQuestion = GetRandomQuestion(upcomingPlanet);

        if (currentQuestion != null)
        {
            popupPanel.SetActive(true);
            isPopupActive = true;
            isSelectingAnswer = true;
            Time.timeScale = 0f;

            questionText.text = currentQuestion.questionText;
            answerButton1.GetComponentInChildren<TMP_Text>().text = currentQuestion.answers[0];
            answerButton2.GetComponentInChildren<TMP_Text>().text = currentQuestion.answers[1];
            answerButton3.GetComponentInChildren<TMP_Text>().text = currentQuestion.answers[2];

            selectedAnswerIndex = 0;
            HighlightSelectedAnswer();
        }
        else
        {
            Debug.LogWarning("No questions found for planet: " + planetName);
        }
    }

    private HashSet<string> collectedPlanets = new HashSet<string>();

    public void MarkPlanetAsCollected(string planetName)
    {
        if (!collectedPlanets.Contains(planetName))
        {
            collectedPlanets.Add(planetName);
        }
    }

    public bool IsPlanetCollected(string planetName)
    {
        return collectedPlanets.Contains(planetName);
    }

    /// <summary>
    /// Updates the UI to display the number of collected planets.
    /// </summary>
    public void UpdateCollectedPlanetCount()
    {
        collectedPlanetCountText.text = $"{collectedPlanets.Count}/8";
        if (collectedPlanets.Count == 8)
        {
            CountdownTimer countdownTimer = FindObjectOfType<CountdownTimer>();
            if (countdownTimer != null)
            {
                countdownTimer.allPlanetsCollected = true;
            }
        }
    }

    private void SaveAskedQuestionsTracker()
    {
        string trackerJson = JsonConvert.SerializeObject(askedQuestionsTracker);
        PlayerPrefs.SetString("AskedQuestionsTracker", trackerJson);
        PlayerPrefs.Save(); 
    }
    
    private void LoadAskedQuestionsTracker()
    {
        if (PlayerPrefs.HasKey("AskedQuestionsTracker"))
        {
            string trackerJson = PlayerPrefs.GetString("AskedQuestionsTracker");
            askedQuestionsTracker = JsonConvert.DeserializeObject<Dictionary<string, HashSet<int>>>(trackerJson);

            if (askedQuestionsTracker == null)
            {
                askedQuestionsTracker = new Dictionary<string, HashSet<int>>();
            }
        }
        else
        {
            askedQuestionsTracker = new Dictionary<string, HashSet<int>>();
        }
    }

    /// <summary>
    /// Displays the play-again button with an optional message.
    /// </summary>
    /// <param name="message">The message to display on the button.</param>
    private void ShowPlayAgainButton(string message = "Vill du spela igen?")
    {
        restartButton.SetActive(true);
        restartButtonText.text = message; 
        isPopupActive = true;
        Time.timeScale = 0f; 
                            
                            
        if (message == "Du vann!")
        {
            SpawnFireworks();
        }
    }

    private void SpawnFireworks()
    {
        if (fireworkPrefab != null)
        {
            Vector3 fireworkOffset = new Vector3(0, -2, 0); 
            GameObject firework = Instantiate(fireworkPrefab, fireworkSpawnPoint.position + fireworkOffset, Quaternion.identity);

            ParticleSystem fireworkParticles = firework.GetComponent<ParticleSystem>();
            if (fireworkParticles != null)
            {
                var main = fireworkParticles.main;
                main.useUnscaledTime = true;
            }
            AudioSource audioSource = firework.AddComponent<AudioSource>();
            audioSource.clip = fireworkSound;
            audioSource.playOnAwake = false;
            audioSource.volume = 0.7f; 
            audioSource.Play();

            Destroy(firework, 5f);
        }
    }

    /// <summary>
    /// Starts the game, enabling player and ball movement and starting the timer.
    /// </summary>
    public void GameStart()
    {
        gameHasStarted = true;
        scoreButton.SetActive(true);
        startButton.SetActive(false);

        if (player != null)
        {
            Player playerScript = player.GetComponent<Player>();
            if (playerScript != null) playerScript.hasGameStarted = true;
        }
        if (ball != null)
        {
            Ball ballScript = ball.GetComponent<Ball>();
            if (ballScript != null) ballScript.hasGameStarted = true;
        }

        CountdownTimer countdownTimer = FindObjectOfType<CountdownTimer>();
        if (countdownTimer != null)
        {
            countdownTimer.StartTimer();
        }

        for (int i = 0; i < 4; i++)
        {
            SpawnBlock();
        }
    }

    /// <summary>
    /// Ends the game, shows appropriate UI, and handles win/lose conditions.
    /// </summary>
    /// <param name="isWin">Indicates whether the player won.</param>
    public void GameOver(bool isWin = false)
    {
        CountdownTimer countdownTimer = FindObjectOfType<CountdownTimer>();
        if (countdownTimer != null)
        {
            if (isWin)
            {
                countdownTimer.ResetTimer(); 
            }
            else
            {
                countdownTimer.PauseTimer(); 
            }
            countdownTimer.SaveTimer(); 
        }
        if (isPopupActive)
        {
            ClosePopup();
            Time.timeScale = 0f; 
        }
        if (isWin)
        {
            ShowPlayAgainButton("Du vann!");
        }
        else if (countdownTimer != null && countdownTimer.timeRemaining > 0)
        {
            PlayGameOverSound();
            ShowPlayAgainButton("Vill du spela igen?");
        }
        else
        {
            PlayGameOverSound();
            ShowPlayAgainButton(" Tiden är slut! Du förlorade! :(");

        }
    }

    private void ClosePopup()
    {
        if (popupPanel != null && popupPanel.activeSelf)
        {
            popupPanel.SetActive(false); 
            isPopupActive = false; 
            isSelectingAnswer = false;
        }
    }

    private void PlayGameOverSound()
    {
        if (gameOverSound != null)
        {
            GameObject audioObject = new GameObject("GameOverAudio");
            AudioSource audioSource = audioObject.AddComponent<AudioSource>();

            audioSource.clip = gameOverSound;
            audioSource.playOnAwake = false;
            audioSource.volume = 1.0f; 
            audioSource.time = 0f; 

            audioSource.ignoreListenerPause = true;
            audioSource.Play();

            Destroy(audioObject, gameOverSound.length);
        }
    }

    /// <summary>
    /// Restarts the game with options for resetting or returning to the intro scene.
    /// </summary>
    /// <param name="isWin">Indicates if the restart is due to a win.</param>
    /// <param name="restartToIntro">Indicates if the restart should go to the intro scene.</param>
    public void GameRestart(bool isWin = false, bool restartToIntro = false)
    {
        SaveAskedQuestionsTracker(); 
        CountdownTimer countdownTimer = FindObjectOfType<CountdownTimer>();
        if (countdownTimer != null)
        {
            if (isWin)
            {
                countdownTimer.ResetTimer(); 
            }
            else
            {
                countdownTimer.SaveTimer(); 
            }
            countdownTimer.allPlanetsCollected = false; 
        }

        isPopupActive = false;
        isSelectingAnswer = false;
        restartButton.SetActive(false);
        scoreButton.SetActive(false);
        startButton.SetActive(true);
        gameHasStarted = false;

        score = 0;
        planets = 0;
        combo = 0;
        collectedPlanets.Clear();

        scoreText.text = "00";
        collectedPlanetCountText.text = "0/8";

        if (restartToIntro)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("IntroScene");
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay");
        }

        Time.timeScale = 1f;
    }

    private void HandlePlayAgainInput()
    {
        if (DancePadInputManager.instance != null && DancePadInputManager.instance.moveUp)
        {
            CountdownTimer countdownTimer = FindObjectOfType<CountdownTimer>();

            bool isWin = collectedPlanets.Count == 8;
            if (isWin)
            {
                GameRestart(true, restartToIntro: true);
            }
            else if (countdownTimer != null && countdownTimer.timeRemaining <= 0)
            {
                GameRestart(true, restartToIntro: true);
            }
            else
            {
                GameRestart();  
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            CountdownTimer countdownTimer = FindObjectOfType<CountdownTimer>();
            bool isWin = collectedPlanets.Count == 8;

            if (isWin)
            {
                GameRestart(true, restartToIntro: true);
            }
            else if (countdownTimer != null && countdownTimer.timeRemaining <= 0)
            {
                GameRestart(true, restartToIntro: true);
            }
            else
            {
                GameRestart();
            }
        }
    }

    /// <summary>
    /// Called when a planet is destroyed, triggering updates and potential popup display.
    /// </summary>
    public void OnPlanetDestroyed()
    {
        if (!string.IsNullOrEmpty(upcomingPlanet))
        {
            UpdatePlanetInteraction();
            StartCoroutine(ShowPopupAfterDelay(1.0f, upcomingPlanet));
        }
    }

    private IEnumerator ShowPopupAfterDelay(float delay, string planetName)
    {
        yield return new WaitForSecondsRealtime(delay);

        if (isPopupActive) yield break;

        ShowPlanetPopup(planetName);
    }

    private Dictionary<string, HashSet<int>> askedQuestionsTracker = new Dictionary<string, HashSet<int>>();

    /// <summary>
    /// Retrieves a random question for a given planet, ensuring no repetition until all questions are used.
    /// </summary>
    /// <param name="planet">The name of the planet.</param>
    /// <returns>A random question for the planet.</returns>
    private PlanetQuestion GetRandomQuestion(string planet)
    {
        if (planetQuestions.ContainsKey(planet))
        {
            List<PlanetQuestion> questions = planetQuestions[planet];
            if (!askedQuestionsTracker.ContainsKey(planet))
            {
                askedQuestionsTracker[planet] = new HashSet<int>();
            }

            HashSet<int> askedQuestions = askedQuestionsTracker[planet];

            // If all questions are asked, reset the tracker
            if (askedQuestions.Count == questions.Count)
            {
                askedQuestions.Clear();
                Debug.Log($"All questions for {planet} have been asked. Resetting tracker.");
            }

            // Find an unused random question
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, questions.Count);
            } while (askedQuestions.Contains(randomIndex));

            // Mark the question as asked
            askedQuestions.Add(randomIndex);

            return questions[randomIndex];
        }
        Debug.LogWarning($"No questions found for planet: {planet}");
        return null;
    }

    /// <summary>
    /// Spawns a block at the next position in the sequence.
    /// </summary>
    public void SpawnBlock()
    {
        startPos += offset;
        GameObject tempBlock = Instantiate(blockPrefab);
        float xPos = Random.Range(-8f, 8f);
        tempBlock.transform.position = startPos + new Vector3(xPos, 0, 0);
        tempBlock.GetComponent<Block>().SubscribeToMethod();
    }

    /// <summary>
    /// Updates the player's score based on combo and block position.
    /// </summary>
    /// <param name="currentBlock">The block the player interacted with.</param>
    public void UpdateScore(GameObject currentBlock)
    {
        Vector3 playerPos = player.transform.position;
        Vector3 blockPos = currentBlock.transform.position;

        if (Mathf.Abs(playerPos.x - blockPos.x) < 1f)
        {
            combo++;
            IsCombo = true;
        }
        else
        {
            combo = 1;
            IsCombo = false;
        }

        GameObject tempShadow = Instantiate(shadowPrefab);
        tempShadow.transform.position = shadowPrefab.transform.position + currentBlock.transform.position;
        Destroy(tempShadow, 3f);

        score += combo;
        scoreText.text = score.ToString();

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore); 
            highScoreText.text = "BÄST : " + highScore.ToString(); 
            PlayerPrefs.Save(); 
        }
    }

    public void UpdatePlanetInteraction()
    {
        planets++;
        scoreText.text = score.ToString();
    }
}