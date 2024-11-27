using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public float timeRemaining = 120;
    public bool timerIsRunning = false;
    public TextMeshProUGUI timeText;
    public bool allPlanetsCollected = false;
    [SerializeField] private AudioClip timerSound;
    private AudioSource audioSource;
    private bool timerSoundStarted = false;

    /// <summary>
    /// Initializes the timer, loading any saved state and setting up the AudioSource.
    /// </summary>
    private void Start()
    {
        LoadTimer();

        // Ensure timer is paused initially
        timerIsRunning = false;
        UpdateTimerDisplay();

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = timerSound;
        audioSource.playOnAwake = false;
        audioSource.loop = true;
    }

    /// <summary>
    /// Handles the timer countdown and checks game conditions in every frame.
    /// </summary>
    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0 && !allPlanetsCollected)
            {
                if (timeRemaining <= 17f && !timerSoundStarted)
                {
                    PlayTimerSound();
                    timerSoundStarted = true;
                }

                // Continue the countdown if the player hasn't won yet
                timeRemaining -= Time.unscaledDeltaTime;
                UpdateTimerDisplay();
            }
            else
            {
                // Stop the timer and call GameOver with the appropriate outcome
                timeRemaining = Mathf.Max(timeRemaining, 0);
                timerIsRunning = false;
                UpdateTimerDisplay();
                SaveTimer();

                if (allPlanetsCollected)
                {
                    Debug.Log("All planets collected - You won!");
                    GameManager.instance.GameOver(true);
                }
                else
                {
                    Debug.Log("Time's up - You lost!");
                    StopTimerSound();
                    GameManager.instance.GameOver(false);
                }
            }
        }
    }

    /// <summary>
    /// Updates the timer display on the UI.
    /// </summary>
    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    /// <summary>
    /// Stops the timer.
    /// </summary>
    public void StopTimer()
    {
        timerIsRunning = false;
    }

    /// <summary>
    /// Starts the timer.
    /// </summary>
    public void StartTimer()
    {
        timerIsRunning = true;
    }

    /// <summary>
    /// Resets the timer to the initial value and pauses it.
    /// </summary>
    public void ResetTimer()
    {
        timeRemaining = 120;
        timerIsRunning = false;
        UpdateTimerDisplay();
        SaveTimer();
        StopTimerSound();
    }

    /// <summary>
    /// Pauses the timer without resetting it.
    /// </summary>
    public void PauseTimer()
    {
        timerIsRunning = false;
    }

    /// <summary>
    /// Saves the current timer value to PlayerPrefs.
    /// </summary>
    public void SaveTimer()
    {
        PlayerPrefs.SetFloat("SavedTimer", timeRemaining);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Loads the timer value from PlayerPrefs or sets the default if none is saved.
    /// </summary>
    public void LoadTimer()
    {
        if (PlayerPrefs.HasKey("SavedTimer"))
        {
            timeRemaining = PlayerPrefs.GetFloat("SavedTimer");
            if (timeRemaining <= 0)
            {
                timeRemaining = 121;
            }
        }
        else
        {
            timeRemaining = 120;
        }
    }

    /// <summary>
    /// Plays the timer sound during the final countdown.
    /// </summary>
    private void PlayTimerSound()
    {
        if (audioSource != null && timerSound != null)
        {
            audioSource.volume = 0.8f;
            audioSource.Play();
        }
    }

    /// <summary>
    /// Stops the timer sound.
    /// </summary>
    private void StopTimerSound()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
