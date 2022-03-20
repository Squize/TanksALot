using UnityEngine;

namespace Explosions {
    public class ExplosionHandler : MonoBehaviour{
        [SerializeField]
        private SmallExplosion smallExplosionRef;

        private const int MAXSMALLEXPLOSIONS = 50;

        private const int MAXSLARGEEXPLOSIONS = 5;

        private SmallExplosion[] _pool;
        private int[] _poolOffset;
        
//---------------------------------------------------------------------------------------
        private void Awake() {
//We're going to start by populating our pool
            _pool = new SmallExplosion[MAXSMALLEXPLOSIONS];
            _poolOffset = new int[MAXSMALLEXPLOSIONS];

            SmallExplosion clone;
            
            int cnt = -1;
            while (++cnt!=MAXSMALLEXPLOSIONS) {
                clone = Instantiate(smallExplosionRef, transform);
                clone.ID = cnt;
                clone.Owner = this;
                
                _pool[cnt] = clone;
                _poolOffset[cnt] = cnt;
                
                clone.gameObject.SetActive(false);
//We don't want to see a million cloned bullets in the Hierarchy, it's super messy
                clone.gameObject.hideFlags = HideFlags.HideInHierarchy;
            }
            
            Destroy(smallExplosionRef.gameObject);
        }

//---------------------------------------------------------------------------------------
        public void Init() {
            
        }        
    }
}