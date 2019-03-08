using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyAR;
using Sample;
using System;

namespace NTI.Scripts
{
    public class DebugBehaviourScript : MonoBehaviour
    {
        private Frame _frame;
        private List<string> trackersNames = new List<string>(new string[] { "tracker0", "tracker1", "tracker2", "tracker3" });
        public List<Transform> trackers = new List<Transform>();
        private ARGlobalSetupBehaviour _setupBehaviour;
        private MeshRenderer meshRenderer;
        
    
    
        // Start is called before the first frame update
        void Start()
        {
            _setupBehaviour = GameObject.Find("EasyAR_Startup").GetComponent<ARGlobalSetupBehaviour>();
            meshRenderer = gameObject.GetComponent<MeshRenderer>();
            meshRenderer.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
             for (var i = 0; i < 4; i++)
             {
                 try {
                     trackers.Add(GameObject.Find(trackersNames[i]).GetComponent<Transform>());
                 }
                 catch (Exception e)
                 {
                    trackers.Add(null);
                 }
             }
             /*
            Debug.Log("start");     
            try
            {
                Transform[] hinges = GameObject.FindObjectsOfType(typeof(Transform)) as Transform[];
                foreach (var hinge in hinges)
                {
                    Debug.Log(hinge.name);
                }
                Debug.Log("end");
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }*/
    
            float posX;
            float posZ;
    
            float sumX=0;
            float sumZ=0;

            if (_setupBehaviour.targets.Count >= 2)
            {
                foreach (var tracker in trackers)
                {
                    if (!(tracker == null))
                    {
                        sumX += tracker.transform.position.x;
                        sumZ += tracker.transform.position.z;
                    }
                }

                posX = sumX / trackers.Count;
                posZ = sumZ / trackers.Count;
    
                Debug.Log(posX);
                Debug.Log(posZ);
    
                this.transform.position = new Vector3(posX, 0, posZ);
                meshRenderer.enabled = true;
            }
            else
            {
                meshRenderer.enabled = false;
            }
            trackers = new List<Transform>();
        }
    }
}
