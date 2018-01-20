using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using BulletHell.Enemies;
using BulletHell.UI;

namespace BulletHell
{
    public class GameManager : MonoBehaviour
    {
        public EnemyPool ePool;
        public MoneyLayer money;

        [Header("Enemies")]
        public int roundLength = 40;
        public float roundSpawnDeduction = 0.1f;
        public float roundSpeedupFactor = 1.3f;

        [Space]
        public float spawnInterval = 1f;
        public float downwardVelocity = 2f;

        public float minX, maxX, initialY;

        [Header("Midboss")]
        public TutorialDialogue midBossDialogue;
        public float midbossAppearTime = 45f;

        [Header("Corporate Donors")]
        public float donorInterval = 0.5f;

        [Header("GameOver")]
        public float sceneLoadDelay = 5f;
        public string nextSceneName;
        public UnityEvent onGameOver;

        private float _nextSpawnTime = float.PositiveInfinity;
        private float _midbossStartTime = float.PositiveInfinity;
        private float _nextCorporateDonation = float.PositiveInfinity;

        public int NumKills { get; private set; }

        private bool gameOver = false;

        public void StartEnemies()
        {
            _nextSpawnTime = Time.time;
        }

        public void ResetMidbossTimer()
        {
            _midbossStartTime = Time.time + midbossAppearTime;
        }

        public void StopEnemies()
        {
            _nextSpawnTime = float.PositiveInfinity;
        }

        public void StartDonations()
        {
            _nextCorporateDonation = Time.time;
        }

        private void Update()
        {
            if (gameOver) return;

            if (Time.time >= _midbossStartTime)
            {
                _nextSpawnTime = float.PositiveInfinity;  // Stop enemies during midboss
                _midbossStartTime = float.PositiveInfinity;
                midBossDialogue.DisplayDialogue();
            }

            if (Time.time >= _nextSpawnTime)
            {
                // Spawn new enemy
                Vector3 spawnPosition = new Vector3(Random.Range(minX, maxX), initialY, 0f);

                Enemy enemy = ePool.SpawnNewItem(spawnPosition);
                enemy.SetGameManager(this);
                float velocity = downwardVelocity;
                int attackRoll = Random.Range(0, 20) + 1;
                if (attackRoll == 1)
                    velocity *= 0.5f;
                if (attackRoll > 18)
                    velocity *= 3f;
                enemy.SetDownwardVelocity(velocity);
                
                _nextSpawnTime += spawnInterval;
            }

            if (Time.time >= _nextCorporateDonation)
            {
                _nextCorporateDonation += donorInterval;
                OnEnemyDeath();
            }
        }

        public void OnEnemyDeath()
        {
            gameOver = money.GetMoney();
            if (gameOver)
            {
                StartCoroutine(LoadAfterDelay());
                onGameOver.Invoke();
                return;
            }

            NumKills++;

            if (NumKills >= roundLength)
            {
                spawnInterval -= roundSpawnDeduction;
                downwardVelocity *= roundSpeedupFactor;
                NumKills = 0;
            }
        }

        private IEnumerator LoadAfterDelay()
        {
            yield return new WaitForSeconds(sceneLoadDelay);
            SceneManager.LoadScene(nextSceneName);
        }
    }
}