using UnityEngine;

namespace BulletHell.Player
{
    public class PlayerMover : MonoBehaviour
    {
        public Transform moveTransform;
        public float minX, maxX, minY, maxY;

        public float speed = 3f;

        private void Update()
        {
            Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * speed * Time.deltaTime;

            Vector3 position = moveTransform.position;
            position.x = Mathf.Clamp(position.x + movement.x, minX, maxX);
            position.y = Mathf.Clamp(position.y + movement.y, minY, maxY);
            moveTransform.position = position;
        }
    }
}