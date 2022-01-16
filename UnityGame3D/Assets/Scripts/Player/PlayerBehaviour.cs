using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class PlayerBehaviour : MonoBehaviour
{
    [Header("General settings")]
    [SerializeField] private float _speed = 150f;
    [SerializeField] private float _jumpHeight = 1.45f;
    [SerializeField] private float _cancelJumpRate = 10f;
    [SerializeField] private LayerMask _groundLayer;

    [Header("Wall Settings")]
    [SerializeField] private float _wallHugDrag = 25f;
    [SerializeField] private LayerMask _wallLayer;

    private Rigidbody _rigidBody;
    private Collider _collider;
    private AudioSource _jumpSource;

    private bool _isJumping;
    private bool _hasCancelledJump;
    private float _horizontalMovement;
    private float _originalDrag;

    private bool _isGrounded = true;
    private bool _isWallHugging = false;

    public event EventHandler WallHugEnter;
    public event EventHandler WallHugExit;
    public event EventHandler GroundedEnter;
    public event EventHandler GroundedExit;
    public event EventHandler Dead;
    public event EventHandler PauseButtonClicked;
    public event EventHandler<GameObject> PickupTouched;

    // Start is called before the first frame update
    void Start() {
        // Obtain components
        _rigidBody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _jumpSource = GetComponent<AudioSource>();

        // Initialize members
        _originalDrag = _rigidBody.drag;
    }

    private void OnEnable() {
        WallHugEnter += onWallHugEnter;
        WallHugExit += onWallHugExit;
    }

    private void OnDisable() {
        WallHugEnter -= onWallHugEnter;
        WallHugExit -= onWallHugExit;
    }

    // Update is called once per frame
    void Update() {
        // Cache local checks
        bool wasGrounded = _isGrounded;
        bool wasWallHugging = _isWallHugging;

        // Pause input
        if (Input.GetButtonDown("Pause")) {
            PauseButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        // Input checking
        _horizontalMovement = Input.GetAxis("Horizontal");
        if (_horizontalMovement > 0) { // right
            transform.rotation = Quaternion.LookRotation(Vector3.right);
        } else if (_horizontalMovement < 0) { // left
            transform.rotation = Quaternion.LookRotation(-Vector3.right);
        }
        

        // Grounded check
        _isGrounded = Physics.Raycast(new Ray(Position, Vector3.down), 0.1f, _groundLayer);
        if (!wasGrounded) {
            if (_isGrounded) {
                GroundedEnter?.Invoke(this, EventArgs.Empty);
            }
        } else {
            if (!_isGrounded) {
                GroundedExit?.Invoke(this, EventArgs.Empty);
            }
        }

        // Holding a wall
        _isWallHugging = Physics.Raycast(new Ray(CenterOrigin, new Vector2(_horizontalMovement, 0)), 0.15f, _wallLayer) && HasMoveKeyDown;
        if (!wasWallHugging) {
            if (_isWallHugging && !_isGrounded) {
                WallHugEnter?.Invoke(this, EventArgs.Empty);
            }
        } else {
            if (!_isWallHugging || _isGrounded) {
                WallHugExit?.Invoke(this, EventArgs.Empty);
            }
        }

        // Jumping
        if (Input.GetButtonDown("Jump") && !_isJumping) {
            float jumpForce = Mathf.Sqrt(_jumpHeight * -2 * Physics.gravity.y);
            _rigidBody.AddForce(new Vector2(0, jumpForce), ForceMode.Impulse);

            _jumpSource.Play();

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
        _rigidBody.velocity = new Vector2(_horizontalMovement * _speed * Time.deltaTime, _rigidBody.velocity.y);

        // Fall faster after cancelling a jump
        if (_hasCancelledJump && _isJumping && _rigidBody.velocity.y > 0) {
            _rigidBody.AddForce(Vector3.down * _cancelJumpRate);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        // Only reset jumping if the impact is with the floor as opposed to a wall or ceiling
        if (collision.impulse.y > 0 || _isWallHugging) {
            _isJumping = false;
        }
        if (collision.transform.tag == "Enemy") {
            Dead?.Invoke(this, EventArgs.Empty);
        } else if (collision.transform.tag == "Pickup") {
            PickupTouched?.Invoke(this, collision.gameObject);
        }
    }

    private void onWallHugEnter(object sender, EventArgs e) {
        _rigidBody.drag = _wallHugDrag;
    }

    private void onWallHugExit(object sender, EventArgs e) {
        _rigidBody.drag = _originalDrag;
    }

    public bool IsFalling {
        get => _rigidBody.velocity.y < 0;
    }

    public bool HasMoveKeyDown {
        get => _horizontalMovement != 0;
    }

    public Vector3 CenterOrigin {
        get => _collider.bounds.center;
    }

    public Vector3 Position {
        get => transform.position;
    }
}
