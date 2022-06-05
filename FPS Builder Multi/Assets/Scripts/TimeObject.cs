using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeObject : MonoBehaviour
{
    bool isRewinding = false;

    public float recordTime = 5f;

    public List<PointInTime> pointsInTime;

    Rigidbody rb;

    void Start()
    {
    	pointsInTime = new List<PointInTime>();
    	rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
    	if(Input.GetKeyDown(KeyCode.A))
    		StartRewind();
    	if(Input.GetKeyUp(KeyCode.A))
    		StopRewind();
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
