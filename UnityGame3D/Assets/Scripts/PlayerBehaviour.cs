using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _jumpHeight = 5f;
    [SerializeField] private float _cancelRate = 100f;

    private Rigidbody _rigidBody;
    private bool _isJumping;
    private bool _hasCancelledJump;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Jumping
        if (Input.GetButtonDown("Jump") && !_isJumping) {
            float jumpForce = Mathf.Sqrt(_jumpHeight * -2 * (Physics.gravity.y));
            _rigidBody.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);

            _isJumping = true;
            _hasCancelledJump = false;
        }
        if (_isJumping) {
            if (Input.GetButtonUp("Jump")) {
                _hasCancelledJump = true;
            }
        }
    }

    // Fixed Update is called every fixed frame
    private void FixedUpdate() {
        // Movement
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        _rigidBody.MovePosition(transform.position + input * Time.deltaTime * _speed);

        // Jumping
        if (_hasCancelledJump && _isJumping && _rigidBody.velocity.y > 0) {
            _rigidBody.AddForce(Vector3.down * _cancelRate);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        _isJumping = false;
    }
}
