using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TimeManager : MonoBehaviourPunCallbacks
{
    public float slowdownFactor = 0.05f;
    public float slowdownLength = 2f; //duration of the slow motion

    void Update()
    {
    	Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
    	Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
    }

    public void DoSlowmotion ()
    {
    	Time.timeScale = slowdownFactor;
    	//Time.fixedDeltaTime = Time.timeScale * .02f;
    }
}
