using UnityEngine;

namespace BulletHell.Shooting
{
    public class BulletScript : MonoBehaviour, IPoolable
    {
        public SpriteRenderer spriteRenderer;
        public Collider2D bulletCollider;

        [Space]
        public float speed;
        // Bullets are assumed to move towards some Max Y then re-add themselves to the pool
        public float maxY;
        
        private SpawnPool pool;

        private void OnEnable()
        {
            spriteRenderer.enabled = true;
        }

        private void OnDisable()
        {
            this.transform.position = new Vector3(0, -20, 0);
            spriteRenderer.enabled = false;
        }

        private void Update()
        {
            float moveDistance = speed * Time.deltaTime;

            transform.Translate(0, moveDistance, 0);

            if (transform.position.y > maxY)
                pool.Reclaim(this);
        }

        public void SetPool(SpawnPool pool)
        {
            this.pool = pool;
        }

        public void OnHit()
        {
            pool.Reclaim(this);
        }
    }
}