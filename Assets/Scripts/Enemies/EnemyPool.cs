using UnityEngine;

namespace BulletHell.Enemies
{
    public class EnemyPool : SpawnPool<Enemy>
    {
        // TODO: enemy weights?
        public EnemyDefinition[] possibleEnemies;

        public override Enemy SpawnNewItem(Vector3 position)
        {
            Enemy newEnemy = base.SpawnNewItem(position);

            EnemyDefinition def = possibleEnemies[Random.Range(0, possibleEnemies.Length)];
            newEnemy.SetEnemyDefinition(def);
            return newEnemy;
        }
    }
}