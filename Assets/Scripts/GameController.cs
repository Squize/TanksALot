using System;
using Bullets;
using Explosions;
using UnityEngine;

namespace DefaultNamespace {
    public class GameController : MonoBehaviour {
        [SerializeField]
        private int score;

        [SerializeField] 
        private float health;
        
        [Header("Class references")]
        public Player.Player Player;

        public BulletHandler BulletHandler;
        
        public ExplosionHandler ExplosionHandler;
        
        public static GameController Instance;

//---------------------------------------------------------------------------------------
        private void Awake() {
            if (Instance == null) {
                Instance = this;
            }
            else {
                Destroy(gameObject);
            }
        }

//---------------------------------------------------------------------------------------
        private void Start() {
            StartGame();
        }

//---------------------------------------------------------------------------------------
        public void StartGame() {
//Reset all our gameplay vars
            score = 0;
            health = 100;
            
//Start all our game classes            
            Player.Init();
            BulletHandler.Init();
        }
        
    }
}