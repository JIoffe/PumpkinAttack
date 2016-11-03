using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VR.WSA.Input;
using HoloToolkit.Unity;
using System.Collections;

namespace JI.Holographic.PumpkinAttack.UI
{
    /// <summary>
    /// Simple layer that responds to air taps, regardless of gaze target
    /// </summary>
    public class AirTapListener : Singleton<AirTapListener>
    {
        [Tooltip("Events to invoke on tap")]
        public UnityEvent[] events;

        [Tooltip("The keycode to use to simulate taps within the Unity editor")]
        public KeyCode editorKeycode = KeyCode.Space;

        private GestureRecognizer gestureRecognizer;

        void Start()
        {
            gestureRecognizer = new GestureRecognizer();
            gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap);

            gestureRecognizer.TappedEvent += OnTappedEvent;

            gestureRecognizer.StartCapturingGestures();
        }

        void OnDestroy()
        {
            gestureRecognizer.StopCapturingGestures();
            gestureRecognizer.TappedEvent -= OnTappedEvent;
        }

        //In the Unity editor we need to have a workaround to simulate taps
#if UNITY_EDITOR
        void LateUpdate()
        {
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(editorKeycode))
            {
                OnTap();
            }
        }
#endif

        private void OnTappedEvent(InteractionSourceKind source, int tapCount, Ray headRay)
        {
            OnTap();
        }

        private void OnTap()
        {
            if (events == null || events.Length == 0)
                return;

            foreach (UnityEvent unityEvent in events)
                unityEvent.Invoke();
        }
    }
}