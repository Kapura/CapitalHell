using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using BulletHell.Shooting;

namespace BulletHell.Enemies
{
    public class Midboss : MonoBehaviour
    {
        public int maxHP;

        public GameManager mgr;
        public UnityEvent afterDefeated;

        [Header("Movement")]
        public float offscreenY;
        public float onscreenY;
        public float verticalVelocity;

        [Header("Images")]
        public SpriteRenderer flashRenderer;
        public Rigidbody2D rb;
        public Collider2D col;

        [Header("Sounds")]
        public AudioSource src;
        public AudioClip hitClip;
        public AudioClip introClip;
        public AudioClip midClip;
        public AudioClip exitClip;

        [Header("Explosions")]
        public ParticleSystem explosionParticles;
        public AudioClip explosionClip;
        public float explosionInterval;
        public Transform[] explosionPositions;

        [Header("HiddenVictory")]
        public AudioClip victoryClip;
        public float postVictoryDelay;
        public UnityEvent AfterPostVictoryDelay;
        public float sceneloadDelay;
        public string victoryScene;

        private enum BattlePhases
        {
            PreBoss,
            Entering,
            OnScreen,
            Exiting,
            Over,
            HiddenVictory
        }

        private int _currentHP;
        private BattlePhases _currentPhase = BattlePhases.PreBoss;
        private bool _hitThisFrame = false;

        private float _nextExplostionTime = float.PositiveInfinity;
        private int _lastExplosionIndex = -1;

        private void Awake()
        {
            rb.simulated = false;
            col.enabled = false;
            this.transform.position = new Vector3(0, offscreenY, 0);
        }

        [ContextMenu("StartMidboss")]
        public void StartMidbossPhase()
        {
            rb.simulated = true;
            _currentPhase = BattlePhases.Entering;
            src.PlayOneShot(introClip);
        }

        private void Update()
        {
            switch (_currentPhase)
            {
                case BattlePhases.Entering:
                    if (transform.position.y <= onscreenY)
                    {
                        if (mgr.NumKills > 0)
                        {
                            src.PlayOneShot(midClip);
                            _currentPhase = BattlePhases.OnScreen;
                            _currentHP = maxHP;
                            col.enabled = true;
                        }
                        else
                        {
                            _currentPhase = BattlePhases.HiddenVictory;
                            rb.velocity = Vector2.zero;
                            StartCoroutine(DoHiddenVictory());
                        }
                    }
                    else
                    {
                        rb.velocity = new Vector2(0, -1 * verticalVelocity);
                    }
                    break;

                case BattlePhases.OnScreen:

                    if (_currentHP <= 0)
                    {
                        src.PlayOneShot(exitClip);
                        col.enabled = false;
                        _currentPhase = BattlePhases.Exiting;
                        _nextExplostionTime = Time.time + 0.1f;
                    }
                    else
                    {
                        rb.velocity = new Vector2(Mathf.Sin(Time.time / 0.5f) * 0.2f, 0);
                    }
                    break;

                case BattlePhases.Exiting:

                    if (transform.position.y >= offscreenY)
                    {
                        // Fully offscreen
                        _currentPhase = BattlePhases.Over;
                        _nextExplostionTime = float.PositiveInfinity;
                        rb.simulated = false;
                        afterDefeated.Invoke();
                    }
                    else
                    {
                        rb.velocity = new Vector2(0, 0.5f * verticalVelocity);

                        // Explosions
                        if (Time.time >= _nextExplostionTime)
                        {
                            // Find a new place to explode
                            int newIndex = _lastExplosionIndex;
                            while (newIndex == _lastExplosionIndex)
                            {
                                newIndex = Random.Range(0, explosionPositions.Length);
                            }

                            explosionParticles.transform.position = explosionPositions[newIndex].position;
                            explosionParticles.Play();
                            _lastExplosionIndex = newIndex;
                            src.PlayOneShot(explosionClip);
                            _nextExplostionTime += explosionInterval;
                        }
                    }
                    break;
            }
        }

        private IEnumerator DoHiddenVictory()
        {
            src.PlayOneShot(victoryClip);
            yield return new WaitForSeconds(postVictoryDelay);
            AfterPostVictoryDelay.Invoke();
            yield return new WaitForSeconds(sceneloadDelay);
            SceneManager.LoadScene(victoryScene);
        }

        private void LateUpdate()
        {
            flashRenderer.enabled = _hitThisFrame;
            if (_hitThisFrame) src.PlayOneShot(hitClip);
            _hitThisFrame = false;  // Reset for next frame
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            BulletScript bullet = other.GetComponent<BulletScript>();
            if (bullet != null) OnBulletHit(bullet);
        }

        private void OnBulletHit(BulletScript bullet)
        {
            bullet.OnHit();
            _hitThisFrame = true;

            _currentHP--;
        }
    }
}