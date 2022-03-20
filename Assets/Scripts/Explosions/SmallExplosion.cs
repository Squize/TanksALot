using System;
using UnityEngine;

namespace Explosions {
    public class SmallExplosion : MonoBehaviour{
        [HideInInspector]
        public int ID;

        [SerializeField]
        private Sprite[] frames;

        private int _animOffset;
        private int _animFlipFlop;
        
        private SpriteRenderer _spriteRenderer;
        
        [HideInInspector]
        public ExplosionHandler Owner;

//---------------------------------------------------------------------------------------
        private void Awake() {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            gameObject.SetActive(false);
        }

//---------------------------------------------------------------------------------------
        public void Init(Vector2 pos) {
            transform.position = pos;
            _animOffset = 0;
            _animFlipFlop = 0;

            _spriteRenderer.sprite = frames[0];
            
            gameObject.SetActive(true);
        }
    }
}