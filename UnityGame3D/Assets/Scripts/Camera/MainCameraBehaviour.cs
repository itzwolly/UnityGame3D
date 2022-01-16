using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraBehaviour : MonoBehaviour
{
    [SerializeField] private FollowCamera _followCamera;
    [SerializeField] private DepthOfFieldEffect _disableDepthOfField;
    [SerializeField] private CameraTransition _cameraTransition;

    private int _routinesRunning;

    public event EventHandler TransitionFinished;

    private void Start() {
        _disableDepthOfField.EffectStarted += onEffectStarted;
        _cameraTransition.TransitionStarted += onTransitionStarted;

        _disableDepthOfField.EffectFinished += onEffectFinished;
        _cameraTransition.TransitionFinished += onTransitionFinished;
    }

    public void GameQuitTransition() {
        StartCoroutine(startTransition());
    }

    public void SetFollowEnabled(bool enabled) {
        _followCamera.enabled = enabled; // Disable the follow camera script
    }

    private IEnumerator startTransition() {
        SetFollowEnabled(false);
        _disableDepthOfField.StartTransition();
        _cameraTransition.StartTransition();

        yield return new WaitUntil(() => _routinesRunning == 0);

        TransitionFinished?.Invoke(this, EventArgs.Empty);
        gameObject.SetActive(false);
    }

    private void onTransitionStarted(object sender, System.EventArgs e) {
        _routinesRunning++;
    }

    private void onEffectStarted(object sender, System.EventArgs e) {
        _routinesRunning++;
    }

    private void onTransitionFinished(object sender, System.EventArgs e) {
        _routinesRunning--;
    }

    private void onEffectFinished(object sender, System.EventArgs e) {
        _routinesRunning--;
    }
}
