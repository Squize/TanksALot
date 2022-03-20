using System;
using Bullets;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;

namespace Player {
    public class Player : MonoBehaviour {
        public enum State {
            Nop,
            Active,
            Killed,
            Dead
        }

        public State CurrentState;

        [Space(10)]
        [SerializeField]
        private Camera camera;
        
        [Header("Visual Elements")] 
        [SerializeField]
        private Transform turretHolder;

        private Rigidbody2D _rigidbody;

        [Header("Shooting Properties")] 
        [SerializeField] 
        private Transform shootPoint;

        [SerializeField] 
        [Range(0.1f,2)]
        private float reloadTime;

        private float _currentReloadTime;
        
//Movement properties
        [Header("Movement Properties")] 
        [SerializeField]
        private float speed;
        [SerializeField]
        private float reverseSpeed;
        
        [SerializeField]
        private float turnSpeed;

        private float _movementInputValue;
        private float _turnInputValue;

//Default pos / rot values so we can reset the game correctly        
        private Vector2 _defaultPos;
        private Vector2 _defaultRot;
        private Vector2 _defaultTurretRot;

//External class refs        
        private BulletHandler _bulletHandler;
        
//---------------------------------------------------------------------------------------
        private void Awake() {
            _rigidbody = GetComponent<Rigidbody2D>();

//Store the default pos / rot values            
            _defaultPos = _rigidbody.position;
            _defaultRot = transform.localEulerAngles;
            _defaultTurretRot = turretHolder.localEulerAngles;

        }

//---------------------------------------------------------------------------------------
        private void Start() {
//Get our references here, it gives Unity time to have called all the Awake methods
            _bulletHandler = GameController.Instance.BulletHandler;
        }

//---------------------------------------------------------------------------------------
        public void Init() {
//Reset all our positions / rotations
            _rigidbody.position = _defaultPos;
            transform.localEulerAngles = _defaultRot;
            turretHolder.localEulerAngles = _defaultTurretRot;

            CurrentState = State.Active;
        }

//---------------------------------------------------------------------------------------
        private void Update() {
            if (CurrentState == State.Nop) return;

            if (CurrentState == State.Active) {
                handleInput();
                rotateTowardsMouse();
                if (--_currentReloadTime < 0) {
                    testForShooting();
                }
            }
        }

//---------------------------------------------------------------------------------------
        private void FixedUpdate() {
            if (CurrentState == State.Active) {
                movePlayer();
                turnPlayer();
            }
        }

//---------------------------------------------------------------------------------------
        private void handleInput() {
//Horizontal / Vertical are set up by Unity, map to WASD / Arrow keys        
            _movementInputValue = Input.GetAxis("Vertical");
            _turnInputValue = Input.GetAxis("Horizontal");
        }

//---------------------------------------------------------------------------------------
        private void rotateTowardsMouse() {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 5.23f;
 
            Vector3 objectPos = camera.WorldToScreenPoint (transform.position);
            mousePos.x -= objectPos.x;
            mousePos.y -= objectPos.y;
 
            float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
            turretHolder.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        
//---------------------------------------------------------------------------------------
        private void testForShooting() {
            bool isMouseDown = Input.GetMouseButtonDown(0);
            
//Jump is the default rather that Fire          
            if (Input.GetButtonDown("Jump") || isMouseDown) {
//Reset our reload time so we're not firing a constant stream of bullets
                _currentReloadTime = reloadTime;
                _bulletHandler.RequestBullet(shootPoint.position, shootPoint.right, true);
            }
        }
        
//---------------------------------------------------------------------------------------
        private void movePlayer() {
//Create a vector in the direction the tank is facing with a magnitude based on the input,
//speed and the time between frames.
            float currentSpeed = speed;
            if (_movementInputValue < 0) {
                currentSpeed = reverseSpeed;
            }

            Vector2 movement = transform.right * (_movementInputValue * currentSpeed) * Time.deltaTime;

            _rigidbody.MovePosition(_rigidbody.position + movement);
        }

//---------------------------------------------------------------------------------------
        private void turnPlayer() {
//The _turnInputValue is set to minus so right is clockwise, feels more natural
            float turn = -_turnInputValue * turnSpeed * Time.deltaTime;
            _rigidbody.MoveRotation(_rigidbody.rotation +turn);
        }

    }
}