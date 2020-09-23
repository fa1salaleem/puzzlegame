using UnityEngine;
using System.Collections;

public class GestureStateTracker : MonoBehaviour 
{
    public UnityEngine.XR.WSA.Input.GestureRecognizer gesture;

	void Awake() 
    {
        if( !gesture )
            gesture = GetComponent<UnityEngine.XR.WSA.Input.GestureRecognizer>();
	}

    void OnEnable()
    {
        if( gesture )
            gesture.OnStateChanged += gesture_OnStateChanged;
    }

    void OnDisable()
    {
        if( gesture )
            gesture.OnStateChanged -= gesture_OnStateChanged;
    }

    void gesture_OnStateChanged( UnityEngine.XR.WSA.Input.GestureRecognizer source )
    {
        Debug.Log( "Gesture " + source + " changed from " + source.PreviousState + " to " + source.State );
    }
}
