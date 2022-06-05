using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerManagerSolo : MonoBehaviour
{
	GameObject controller;

	void Awake()
	{

	}
    // Start is called before the first frame update
    void Start()
    {
        
        CreateController();
        
    }

    void CreateController()
    {
        Transform spawnpoint = SpawnManager.Instance.GetSpawnpoint();
    	//controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), spawnpoint.position, spawnpoint.rotation, 0, new object[] {PV.ViewID});
    	Debug.Log("instantiated player controller");
    }

    public void Die()
    {
        Destroy(controller);
        CreateController();
    }
}
