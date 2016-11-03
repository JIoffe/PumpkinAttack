using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity;
using System.Collections;

namespace JI.Holographic.PumpkinAttack.UI
{
    /// <summary>
    /// Layer that manages writing text to the user or displaying other graphics
    /// </summary>
    public class UIManager : Singleton<UIManager>
    {
        [System.Serializable]
        public class UISettings
        {
            [Tooltip("The time in seconds that status text will be displayed")]
            public float statusTextLife = 3f;
        }

        [Tooltip("Settings for this UI component")]
        public UISettings settings;

        private Text statusText;
        private float statusTextLife = 0f;

        // Use this for initialization
        void Start()
        {
            statusText = GetComponentInChildren<Text>();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateStatusText();
        }

        public void ShowStatusText(string text)
        {
            statusTextLife = settings.statusTextLife;
            statusText.text = text;
        }
        private void UpdateStatusText()
        {
            if (statusTextLife <= 0f)
                return;

            statusTextLife -= Time.deltaTime;

            if (statusTextLife <= 0f)
                statusText.text = "";
        }
    }
}
