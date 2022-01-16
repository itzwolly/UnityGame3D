using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveBehaviour : MonoBehaviour
{
    [SerializeField] private Transform[] _points;
    [SerializeField] private float[] _delaysAtPoint;
    [SerializeField] private float _speed;
    [SerializeField] private bool _hasInitialDelay;
     
    private int _pointIndex;
    private Transform _currentPoint;
    private float _currentDelay;
    private bool _shouldMove;

    // Start is called before the first frame update
    private void Start() {
        _currentPoint = _points[_pointIndex];
        _currentDelay = _delaysAtPoint[_pointIndex];
        _shouldMove = !_hasInitialDelay;

        if (!_shouldMove) {
            StartDelayAtPoint(_currentDelay);
        }
    }

    // Update is called once per frame
    private void Update() {
        if (!_shouldMove) {
            return;
        }

        // Move our position a step closer to the target point
        float step = _speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, _currentPoint.position, step);

        // Check if the position of the enemy and the current point are approximately equal
        if (Vector3.Distance(transform.position, _currentPoint.position) < 0.001f) {
            // Checks if the point index is equal to or bigger than length-1
            // and if this is the case sets it to negative length-1.
            if (_pointIndex >= _points.Length - 1) {
                _pointIndex = -(_points.Length - 1);
            }

            // We use the absolute value when we do the lookup so
            // it results in a pattern like this: 0, 1, 2, 1, 0, 1, 2, etc.
            _currentPoint = _points[Mathf.Abs(_pointIndex)];
            _currentDelay = _delaysAtPoint[Mathf.Abs(_pointIndex)];
            _pointIndex++;

            StartDelayAtPoint(_currentDelay);
        }
    }

    private Coroutine StartDelayAtPoint(float pSeconds) {
        // Stop enemy from moving
        _shouldMove = false;
        return StartCoroutine(waitForSeconds(pSeconds));
    }

    private IEnumerator waitForSeconds(float pSeconds) {
        yield return new WaitForSeconds(pSeconds);
        // Start enemy from moving
        _shouldMove = true;
    }
}
