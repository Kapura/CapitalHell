using UnityEngine;
using UnityEngine.UI;

namespace BulletHell.UI
{
    public class SelectablePanel : MonoBehaviour
    {
        public Image characterImage;
        public Image selectionImage;
        public Text characterName;

        [Space]
        public Character associatedCharacter;

        private void Awake()
        {
            selectionImage.enabled = false;
            Populate();
        }

        [ContextMenu("Populate")]
        private void Populate()
        {
            selectionImage.color = associatedCharacter.shipColor;
            characterImage.sprite = associatedCharacter.characterImage;
            characterName.text = associatedCharacter.characterName;
        }

        public void SelectPanel()
        {
            selectionImage.enabled = true;
        }

        public void DeselectPanel()
        {
            selectionImage.enabled = false;
        }
    }
}