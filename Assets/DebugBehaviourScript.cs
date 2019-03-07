using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyAR;
using Sample;

public class DebugBehaviourScript : MonoBehaviour
{
    private Frame _frame;
    private string[] trackersNames = {"tracker0","tracker1", "tracker2", "tracker3"};
    public GameObject tracker0;
    public GameObject tracker1;
    public GameObject tracker2;
    public GameObject tracker3;
    public GameObject[] trackers;
    private ARGlobalSetupBehaviour _setupBehaviour;
    List<GameObject> gameObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        _setupBehaviour = GameObject.Find("EasyAR_Startup").GetComponent<ARGlobalSetupBehaviour>();
        trackers[0] = tracker0;
        trackers[1] = tracker1;
        trackers[2] = tracker2;
        trackers[3] = tracker3;
    }

    // Update is called once per frame
    void Update()
    {
        float posX;
        float posZ;

        float sumX=0;
        float sumZ=0;

        if (_setupBehaviour.targets.Count >= 3)
        {
            foreach (var target in _setupBehaviour.targets)
            {
                var current = trackersNames.IndexOf(target.Name);
                gameObjects.Add(current);
                sumX += current.transform.position.x;
                sumZ += current.transform.position.z;
            }
            posX = sumX / _setupBehaviour.targets.Count;
            posZ = sumZ / _setupBehaviour.targets.Count;

            Debug.Log(posX);
            Debug.Log(posZ);

            this.transform.position = new Vector3(posX, 0, posZ);
        }
    }
}
