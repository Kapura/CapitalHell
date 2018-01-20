using UnityEngine;

namespace BulletHell.UI
{
    [CreateAssetMenu(fileName = "NewCharacter.asset", menuName = "Create New Character")]
    public class Character : ScriptableObject
    {
        public string characterName;
        public Sprite characterImage;
        public Color shipColor;

        public string FirstName { get { return characterName.Split(' ')[0]; } }
    }
}