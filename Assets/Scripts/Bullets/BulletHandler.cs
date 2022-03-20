using System;
using UnityEngine;

namespace Bullets {
    public class BulletHandler : MonoBehaviour {
        [SerializeField]
        private Bullet masterBulletRef;

        private const int MAXBULLETS = 80;
        
        private Bullet[] _pool;
        private int[] _poolOffset;

//---------------------------------------------------------------------------------------
        private void Awake() {
//We're going to start by populating our pool
            _pool = new Bullet[MAXBULLETS];
            _poolOffset = new int[MAXBULLETS];

            Bullet clone;
            
            int cnt = -1;
            while (++cnt!=MAXBULLETS) {
                clone = Instantiate(masterBulletRef, transform);
                clone.ID = cnt;
                clone.Owner = this;
                
                _pool[cnt] = clone;
                _poolOffset[cnt] = cnt;
                
                clone.gameObject.SetActive(false);
//We don't want to see a million cloned bullets in the Hierarchy, it's super messy
                clone.gameObject.hideFlags = HideFlags.HideInHierarchy;
            }
            
            Destroy(masterBulletRef.gameObject);
        }

//---------------------------------------------------------------------------------------
        public void Init() {
            
        }
        
//---------------------------------------------------------------------------------------
        public void RequestBullet(Vector2 shotPos, Vector2 direction, bool isPlayer) {
            Bullet bull = getFromPool();
            if (bull != null) {
                bull.Init(shotPos,direction,isPlayer);
            }
        }

//---------------------------------------------------------------------------------------
        public void ReturnToPool(int id) {
            _poolOffset[id] = id;
        }
        
//---------------------------------------------------------------------------------------
        private Bullet getFromPool() {
            int cnt = -1;
            while (++cnt!=MAXBULLETS) {
                if (_poolOffset[cnt] != -1) {
//This bullet is available, so lets use it
                    _poolOffset[cnt] = -1;
                    return _pool[cnt];
                }
            }
            
            return null;
        }
    }
}