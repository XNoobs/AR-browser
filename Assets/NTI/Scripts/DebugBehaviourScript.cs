using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyAR;
using Sample;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace NTI.Scripts
{
    public class DebugBehaviourScript : MonoBehaviour
    {
        private Frame _frame;
        private List<string> trackersNames = new List<string>(new string[] { "tracker0", "tracker1", "tracker2", "tracker3" });
        public List<Transform> trackers = new List<Transform>();
        private ARGlobalSetupBehaviour _setupBehaviour;
        private MeshRenderer meshRenderer;
        private int frameCounter=0;
        public List<Tuple<float, float>> bounds;
        //0 - left top, 1 - right top, 2 - right bottom, 3 - left bottom
        
    
    
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
            frameCounter++;
            if (frameCounter % 10 == 0)
            {
                for (var i = 0; i < 4; i++)
                {
                    try
                    {
                        trackers.Add(GameObject.Find(trackersNames[i]).GetComponent<Transform>());
                    }
                    catch (Exception e)
                    {
                        trackers.Add(null);
                    }
                }

                List<Tuple<float,float>> dots = new List<Tuple<float, float>>();

                if (_setupBehaviour.targets.Count >= 3)
                {
                    foreach (var tracker in trackers)
                    {
                        if (!(tracker == null))
                        {
                            Vector3 current = tracker.position;
                            dots.Add(new Tuple<float, float>(current.x, current.z));
                        }
                    }

                  
                    

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
}
