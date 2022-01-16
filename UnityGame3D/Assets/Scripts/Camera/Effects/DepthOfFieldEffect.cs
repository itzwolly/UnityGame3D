using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DepthOfFieldEffect : MonoBehaviour
{
    [SerializeField] private PostProcessVolume _menuVolume;
    [SerializeField] private float _valueToLerp;

    private bool _shouldLerp = false;
    private bool _finishedLerping = false;
    private float _startingFocalLength;

    private Coroutine _effectRoutine = null;
    private DepthOfField _depthOfField = null;

    public event EventHandler EffectStarted;
    public event EventHandler EffectFinished;

    private void Start() {
        _depthOfField = _menuVolume.profile.GetSetting<DepthOfField>();
    }

    private void Update() {
        if (_shouldLerp) {
            // Update focal length
            _depthOfField.focalLength.value = Mathf.Lerp(_depthOfField.focalLength.value, _valueToLerp, Time.unscaledDeltaTime);

            // Check if we're done lerping
            if (_valueToLerp < _startingFocalLength) {
                if (_depthOfField.focalLength.value <= _valueToLerp + 1) {
                    _finishedLerping = true;
                }
            } else if (_valueToLerp > _startingFocalLength) {
                if (_depthOfField.focalLength.value >= _valueToLerp - 1) {
                    _finishedLerping = true;
                }
            }
        }
    }

    public void StartTransition() {
        if (_effectRoutine == null) {
            _effectRoutine = StartCoroutine(lerpEffect());
        }
    }

    private IEnumerator lerpEffect() {
        _shouldLerp = true;
        _finishedLerping = false;
        _startingFocalLength = _depthOfField.focalLength.value;

        EffectStarted?.Invoke(this, EventArgs.Empty);
        yield return new WaitUntil(() => _finishedLerping);
        reset();
        EffectFinished?.Invoke(this, EventArgs.Empty);
    }

    private void reset() {
        _shouldLerp = false;
        _effectRoutine = null;
    }
}
