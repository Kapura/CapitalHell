using UnityEngine;
using BulletHell.Shooting;

namespace BulletHell.Player
{
    public class PlayerShooter : MonoBehaviour
    {
        public BulletPool pool;
        public PlayerData pData;
        public SpriteRenderer characterImage;
        public SpriteRenderer shipImage;

        public Transform[] bulletSpawnPoints;

        public float bulletSpawnInterval;
        public AudioSource src;
        public AudioClip fireClip;

        private float _nextSpawnTime = 0f;
        private bool _firing = false;

        private bool _shootingAllowed = false;

        private void Start()
        {
            characterImage.sprite = pData.selectedCharacter.characterImage;
            shipImage.color = pData.selectedCharacter.shipColor;
        }

        public void AllowShooting()
        {
            _shootingAllowed = true;
        }

        private void Update()
        {
            if (!_shootingAllowed)
                return;

            if (Input.GetButtonDown("Fire"))
            {
                _firing = !_firing;
                _nextSpawnTime = Time.time;
            }

            if (_firing && Time.time >= _nextSpawnTime)
            {
                for (int i = 0; i < bulletSpawnPoints.Length; i++)
                {
                    pool.SpawnNewItem(bulletSpawnPoints[i].position);
                }

                _nextSpawnTime += bulletSpawnInterval;
                if (_nextSpawnTime < Time.time)
                    _nextSpawnTime = Time.time + bulletSpawnInterval;
                src.PlayOneShot(fireClip);
            }
        }
    }
}