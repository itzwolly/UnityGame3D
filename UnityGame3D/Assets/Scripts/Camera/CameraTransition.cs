using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraTransition : MonoBehaviour
{
    [SerializeField] private Camera _cameraToTransitionTo;

    [Header("General Settings")]
    [SerializeField] private bool _lerpRotation;
    [SerializeField] private bool _resetPositionAfterTransition;
    [SerializeField] private bool _resetRotationAfterTransition;

    [Header("Orthographic settings")]
    [SerializeField] private bool _lerpOrthographicSize;
    [SerializeField] private bool _resetOrthographicSizeAfterTransition;

    private Camera _currentCamera;
    private Vector3 _startingPosition;
    private Quaternion _startingRotation;
    private float _startingOrthographicSize;

    private bool _shouldLerp;
    private Coroutine _transitionRoutine;
    private Transform _transitionTransform;

    public event EventHandler TransitionStarted;
    public event EventHandler TransitionFinished;

    private void Start() {
        _currentCamera = GetComponent<Camera>();
        _startingPosition = transform.position;
        _startingRotation = transform.rotation;
        _startingOrthographicSize = _currentCamera.orthographicSize;

        _transitionTransform = _cameraToTransitionTo.transform;
    }

    private void Update() {
        if (_shouldLerp) {
            transform.position = Vector3.Lerp(transform.position, _transitionTransform.position, Time.unscaledDeltaTime);
            if (_lerpRotation) {
                transform.rotation = Quaternion.Lerp(transform.rotation, _transitionTransform.rotation, Time.unscaledDeltaTime);
            }
            if (_lerpOrthographicSize) {
                _currentCamera.orthographicSize = Mathf.Lerp(_currentCamera.orthographicSize, _cameraToTransitionTo.orthographicSize, Time.unscaledDeltaTime);
            }
        }
    }

    public void StartTransition() {
        if (_transitionRoutine == null) {
            _transitionRoutine = StartCoroutine(transitionCamera());
        }
    }

    private IEnumerator transitionCamera() {
        _shouldLerp = true;
        TransitionStarted?.Invoke(this, EventArgs.Empty);
        yield return new WaitUntil(() => Vector3.Distance(transform.position, _transitionTransform.position) <= 0.1f);
        reset();
        TransitionFinished?.Invoke(this, EventArgs.Empty);
    }

    private void reset() {
        _shouldLerp = false;
        _transitionRoutine = null;
        _cameraToTransitionTo.gameObject.SetActive(true);

        // Reset position
        if (_resetPositionAfterTransition) {
            transform.position = _startingPosition;
        }
        if (_resetRotationAfterTransition) {
            transform.rotation = _startingRotation;
        }
        if (_resetOrthographicSizeAfterTransition) {
            _currentCamera.orthographicSize = _startingOrthographicSize;
        }
    }
}
