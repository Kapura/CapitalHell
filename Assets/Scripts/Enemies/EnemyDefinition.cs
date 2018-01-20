using System;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Enemies
{
    [CreateAssetMenu(fileName = "NewEnemy.asset", menuName = "Create New Enemy")]
    public class EnemyDefinition : ScriptableObject
    {
        public Sprite imageSprite;
        public int HP;
    }
}