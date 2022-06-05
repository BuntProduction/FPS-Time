using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class TimeBody : MonoBehaviourPunCallbacks
{
    bool isRewinding = false;

    public float recordTime = 5f;

    public List<PointInTime> pointsInTime;

    Rigidbody rb;

    PhotonView PV;

    [SerializeField] GameObject rewindModeUI;


    void Start()
    {
    	if(PV.IsMine)
    	{
    	pointsInTime = new List<PointInTime>();
    	rb = GetComponent<Rigidbody>();
    	}

    	rewindModeUI.SetActive(false);
    }

    void Update()
    {
    	if(Input.GetKeyDown(KeyCode.E))
    	{
    		StartRewind();
    		rewindModeUI.SetActive(true);
    	}
    	if(Input.GetKeyUp(KeyCode.E))
    	{
    		StopRewind();
    		rewindModeUI.SetActive(false);
    	}
    }
    void FixedUpdate()
    {
    	if (isRewinding)
    		Rewind();
    	else
    		Record();
    }

    void Rewind()
    {
    	if(pointsInTime.Count > 0)
    	{
    		PointInTime pointInTime = pointsInTime[0];
    		transform.position = pointInTime.position;
    		transform.rotation = pointInTime.rotation;
    		pointsInTime.RemoveAt(0);
    	}
    	else
    	{
    		StopRewind();
    	}
    }

    void Record()
    {
    	if(pointsInTime.Count > Mathf.Round(recordTime / Time.fixedDeltaTime))
    	{
    		pointsInTime.RemoveAt(pointsInTime.Count - 1);
    	}

    	pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation));
    }

    public void StartRewind()
    {
    	isRewinding = true;
    	rb.isKinematic = true;
    }
    public void StopRewind()
    {
    	isRewinding = false;
    	rb.isKinematic = false;

    }
}
