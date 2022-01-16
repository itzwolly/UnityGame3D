using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Game : MonoBehaviour
{
    [Header("Gameplay")]
    [SerializeField] private bool _pauseOnStart;
    [SerializeField] private PlayerBehaviour _player;
    [SerializeField] private Transform _currentRespawnPoint;

    [Header("Canvases")]
    [SerializeField] private GameObject _hudCanvas;
    [SerializeField] private GameObject _mainMenuCanvas;
    [SerializeField] private GameObject _settingsCanvas;
    [SerializeField] private GameObject _pauseMenuCanvas;

    [Header("Cameras")]
    [SerializeField] private MainCameraBehaviour _mainCamera;
    [SerializeField] private GameObject _minimapCamera;
    [SerializeField] private MenuCameraBehaviour _menuCamera;

    [Header("Events")]
    [SerializeField] private MainMenuEvents _mainMenuEvents;
    [SerializeField] private InGameEvents _inGameEvents;

    private bool _paused;

    public event EventHandler GameStarting;
    public event EventHandler GameStarted;
    public event EventHandler GamePaused;
    public event EventHandler GameUnpaused;
    public event EventHandler GameQuittingToMenu;
    public event EventHandler GameQuitToMenu;

    private void Awake() {
        _hudCanvas.SetActive(false);
        _mainMenuCanvas.SetActive(true);
    }

    private void Start() {
        if (_pauseOnStart) {
            // Pause the game without calling pause, because
            // we're not interested in the paused event here.
            pauseInternal();
        }
    }

    private void OnEnable() {
        GameStarting += onGameStarting;
        GamePaused += onGamePaused;
        GameUnpaused += onGameUnpaused;
        GameQuittingToMenu += onGameQuitting;

        _player.PauseButtonClicked += onPlayerClickedPauseButton;
        _player.Dead += onPlayerDead;
        _player.PickupTouched += onPlayerPickupTouched;

        _mainMenuEvents.PlayClicked += onGamePlayClicked;
        _mainMenuEvents.SettingsClicked += onGameSettingsClicked;
        _mainMenuEvents.SaveSettingsClicked += onSaveGameSettingsClicked;
        _mainMenuEvents.QuitClicked += onGameQuitClicked;

        _inGameEvents.ResumeGameClicked += onResumeGameClicked;
        _inGameEvents.SettingsClicked += onGameSettingsClicked;
        _inGameEvents.SaveSettingsClicked += onSaveGameSettingsClicked;
        _inGameEvents.QuitToMenuClicked += onQuitToMenuClicked;

        _mainCamera.TransitionFinished += onMainCameraTransitionFinished;

        _menuCamera.TransitionFinished += onMenuCameraTransitionFinished;
    }

    

    private void OnDisable() {
        GameStarting -= onGameStarting;
        GamePaused -= onGamePaused;
        GameUnpaused -= onGameUnpaused;
        GameQuittingToMenu -= onGameQuitting;

        _player.PauseButtonClicked -= onPlayerClickedPauseButton;
        _player.Dead -= onPlayerDead;
        _player.PickupTouched -= onPlayerPickupTouched;

        _mainMenuEvents.PlayClicked -= onGamePlayClicked;
        _mainMenuEvents.SettingsClicked -= onGameSettingsClicked;
        _mainMenuEvents.SaveSettingsClicked -= onSaveGameSettingsClicked;
        _mainMenuEvents.QuitClicked -= onGameQuitClicked;

        _inGameEvents.ResumeGameClicked -= onResumeGameClicked;
        _inGameEvents.SettingsClicked -= onGameSettingsClicked;
        _inGameEvents.SaveSettingsClicked -= onSaveGameSettingsClicked;
        _inGameEvents.QuitToMenuClicked -= onQuitToMenuClicked;

        _mainCamera.TransitionFinished -= onMainCameraTransitionFinished;

        _menuCamera.TransitionFinished -= onMenuCameraTransitionFinished;
    }

    private void startGame() {
        // Resume the game without calling unpause, because we're not
        // interested in the unpaused event here.
        unpauseInternal();

        // Enable the in-game hud
        _hudCanvas.SetActive(true);

        // Alert listeners the game has actually started.
        GameStarted?.Invoke(this, EventArgs.Empty);
    }

    private void quitToMenu() {
        // pause the game without calling pause, because we're not
        // interested in the unpaused event here.
        pauseInternal();

        // Enable the menu hud
        _mainMenuCanvas.SetActive(true);

        // Alert listeners the game has actually quit to menu.
        GameQuitToMenu?.Invoke(this, EventArgs.Empty);
    }

    private void pauseInternal() {
        _paused = true;
        Time.timeScale = 0;
    }

    private void unpauseInternal() {
        _paused = false;
        Time.timeScale = 1;
    }

    public void Pause() {
        if (!_paused) {
            _paused = true;
            _pauseMenuCanvas.SetActive(true);
            GamePaused?.Invoke(this, EventArgs.Empty);
        }
    }

    public void Unpause() {
        if (_paused) {
            _paused = false;
            _pauseMenuCanvas.SetActive(false);
            GameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }

    private void onPlayerClickedPauseButton(object sender, EventArgs e) {
        if (!_paused) {
            Pause();
        } else {
            Unpause();
        }
    }

    private void onPlayerDead(object sender, EventArgs e) {
        _player.transform.position = _currentRespawnPoint.position;
        _player.transform.rotation = _currentRespawnPoint.rotation;
    }

    private void onPlayerPickupTouched(object sender, GameObject e) {
        Destroy(e);
    }

    private void onGamePaused(object sender, EventArgs e) {
        Time.timeScale = 0;
    }

    private void onGameUnpaused(object sender, EventArgs e) {
        Time.timeScale = 1;
    }

    
    private void onGameStarting(object sender, EventArgs e) {
        startGame();
    }

    private void onGameQuitting(object sender, EventArgs e) {
        quitToMenu();
    }

    private void onGamePlayClicked(object sender, EventArgs e) {
        // Disable current canvas
        _mainMenuCanvas.SetActive(false);
        // Start menu camera transition to game
        _menuCamera.GameStartTransition();
    }

    private void onMainCameraTransitionFinished(object sender, EventArgs e) {
        GameQuittingToMenu?.Invoke(this, EventArgs.Empty);
    }

    private void onMenuCameraTransitionFinished(object sender, EventArgs e) {
        GameStarting?.Invoke(this, EventArgs.Empty);
    }

    private void onGameSettingsClicked(object sender, EventArgs e) {
        // Enable Settings Canvas
        _settingsCanvas.SetActive(true);
    }

    private void onSaveGameSettingsClicked(object sender, EventArgs e) {
        // Disable Settings canvas
        _settingsCanvas.SetActive(false);
    }

    private void onGameQuitClicked(object sender, EventArgs e) {
        Application.Quit();
    }

    private void onResumeGameClicked(object sender, EventArgs e) {
        Unpause();
    }

    private void onQuitToMenuClicked(object sender, EventArgs e) {
        // Disable current canvas
        _hudCanvas.SetActive(false);
        _pauseMenuCanvas.SetActive(false);

        // Start transition
        _mainCamera.GameQuitTransition();
    }

    public bool Paused {
        get => _paused;
    }
}
