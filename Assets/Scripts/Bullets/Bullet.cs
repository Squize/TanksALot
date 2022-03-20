using System;
using UnityEngine;

namespace Bullets {
    public class Bullet : MonoBehaviour {
        [HideInInspector]
        public int ID;

        private int playerBullets = 7;
        private int baddieBullets = 8;
        
//Simple flag so we know if this is a bullet the player has fired or not        
        private bool _isPlayer;

        private int _glitchCnt;

        private Vector2 _dir;
        
//Components        
        private Rigidbody2D _rigidbody;
        private SpriteRenderer _renderer;
        
        [HideInInspector]
        public BulletHandler Owner;

//---------------------------------------------------------------------------------------
        private void Awake() {
            _rigidbody = GetComponent<Rigidbody2D>();
            _renderer = GetComponent<SpriteRenderer>();
            _renderer.enabled = false;

            enabled = false;
        }

//---------------------------------------------------------------------------------------
        public void Init(Vector2 pos, Vector2 direction, bool isPlayer) {
            _isPlayer = isPlayer;

            _dir = direction;
//Show it in the Hierarchy when it's active, makes debugging easier            
            gameObject.hideFlags = HideFlags.None;
//Bit of a gotcha, but you have to ensure the gameObject is active before setting
//the rigidBody properties
            gameObject.SetActive(true);

//Also it needs to be in the default layer for a frame or two to avoid colliding with the
//shooter
            gameObject.layer = 0;

            _rigidbody.MovePosition(pos);
            
            _rigidbody.freezeRotation = false;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _rigidbody.MoveRotation(angle);

            
            _rigidbody.AddForce(_dir,ForceMode2D.Impulse);

            _glitchCnt = 0;
            enabled = true;
        }

//---------------------------------------------------------------------------------------
        private void Update() {
            if(++_glitchCnt<2) return;
//There's an ugly glitch when we first trigger this bullet, so wait until the
//a couple of frames before we enable the sprite renderer
            _renderer.enabled = true;

            if (_glitchCnt > 5) {
                if (_isPlayer) {
                    gameObject.layer = playerBullets;
                }
                else {
                    gameObject.layer = baddieBullets;
                }
                
                _rigidbody.freezeRotation = true;

                enabled = false;
            }
        }

//---------------------------------------------------------------------------------------
        private void OnCollisionEnter2D(Collision2D col) {
//We've hit something!
            _rigidbody.velocity=Vector2.zero;
            _rigidbody.angularVelocity = 0;
        
            gameObject.hideFlags = HideFlags.HideInHierarchy;
            gameObject.SetActive(false);
            
            Owner.ReturnToPool(ID);
        }
    }
}