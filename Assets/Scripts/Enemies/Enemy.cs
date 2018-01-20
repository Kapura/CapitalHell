using UnityEngine;
using BulletHell.Shooting;
using BulletHell.Player;

namespace BulletHell.Enemies
{
    public class Enemy : MonoBehaviour, IPoolable
    {
        public int maxHP;
        public float downwardVelocity = 3f;
        public float despawnY;  // Enemy proceeds downward and despawns when they hit this Y

        public SpriteRenderer itemRenderer;
        public SpriteMask flashMask;
        public SpriteRenderer flashRenderer;
        public Rigidbody2D rb;
        [Space]
        public ParticleSystem hearticles;

        [Header("Audio")]
        public AudioSource src;
        public AudioClip hitClip;

        private int _currentHP;
        private bool _hitThisFrame = false;

        private SpawnPool pool;
        private GameManager mgr;

        private void OnEnable()
        {
            itemRenderer.enabled = true;
            _currentHP = maxHP;
            rb.simulated = true;
            rb.velocity = new Vector2(0, -1 * downwardVelocity);
        }

        private void OnDisable()
        {
            itemRenderer.enabled = false;
            flashRenderer.enabled = false;
            rb.simulated = false;
            transform.position = new Vector3(0, 20, 0);
        }

        private void LateUpdate()
        {
            flashRenderer.enabled = _hitThisFrame;
            if (_hitThisFrame) src.PlayOneShot(hitClip);
            _hitThisFrame = false;  // Reset for next frame

            if (transform.position.y <= despawnY)
                pool.Reclaim(this);
        }

        public void SetPool(SpawnPool pool)
        {
            this.pool = pool;
        }

        public void SetGameManager(GameManager mgr)
        {
            this.mgr = mgr;
        }

        public void SetEnemyDefinition(EnemyDefinition def)
        {
            maxHP = def.HP;
            _currentHP = maxHP;
            itemRenderer.sprite = def.imageSprite;
            flashMask.sprite = def.imageSprite;
        }

        public void SetDownwardVelocity(float velocity)
        {
            downwardVelocity = velocity;
            rb.velocity = new Vector2(0, -1 * downwardVelocity);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            BulletScript bullet = other.GetComponent<BulletScript>();
            if (bullet != null) OnBulletHit(bullet);

            PlayerShooter player = other.GetComponent<PlayerShooter>();
            if (player != null) OnPlayerEnter(player);
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            PlayerShooter player = other.GetComponent<PlayerShooter>();
            if (player != null) OnPlayerExit(player);
        }

        private void OnBulletHit(BulletScript bullet)
        {
            bullet.OnHit();
            _hitThisFrame = true;

            _currentHP--;

            if (_currentHP == 0)
                OnDeath();
        }

        private void OnPlayerEnter(PlayerShooter player)
        {
            if (mgr.NumKills < 1)
                hearticles.Play();
        }

        private void OnPlayerExit(PlayerShooter player)
        {
            if (hearticles.isPlaying)
                hearticles.Stop();
        }

        private void OnDeath()
        {
            pool.Reclaim(this);
            mgr.OnEnemyDeath();

            if (hearticles.isPlaying)
                hearticles.Stop();
        }
    }
}