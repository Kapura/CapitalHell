using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using BulletHell.UI;

namespace BulletHell
{
    [CreateAssetMenu(fileName = "PlayerData.asset", menuName = "New Player Data")]
    public class PlayerData : ScriptableObject
    {
        public Character selectedCharacter;

        private long _startTime;

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void RecordStartTime()
        {
            _startTime = DateTime.Now.Ticks;
        }

        public void GetTotalTime()
        {
            DateTime startTime = new DateTime(_startTime);
            DateTime endTime = DateTime.Now;

            TimeSpan duration = endTime - startTime;
            Analytics.CustomEvent("timeToWin", new Dictionary<string, object> { { "duration", duration.ToString() } });
        }
    }
}