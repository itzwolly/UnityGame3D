using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class MenuCameraBehaviour : MonoBehaviour
{
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

    public void GameStartTransition() {
        StartCoroutine(startTransition());
    }

    private IEnumerator startTransition() {
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
