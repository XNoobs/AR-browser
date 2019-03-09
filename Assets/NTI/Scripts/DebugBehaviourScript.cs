using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyAR;
using Sample;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Security.Policy;
using UnityEditor.IMGUI.Controls;

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
        public float height;
        public float width;
        public float[] centerPoint = new float[2];
        
    
    
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
                    List<float> XValues = new List<float>();
                    List<float> ZValues = new List<float>();
                    
                    foreach (var tracker in trackers)
                    {
                        if (!(tracker == null))
                        {
                            Vector3 current = tracker.position;
                            XValues.Add(current.x);
                            ZValues.Add(current.z);
                            
                        }
                    }

                    width = XValues.Max() - XValues.Min();
                    height = ZValues.Max() - ZValues.Min();

                    var currentSize = meshRenderer.bounds.size;
                    var scaleFactor = height / currentSize.x;
                    this.transform.localScale= new Vector3(scaleFactor, scaleFactor, scaleFactor );
                    transform.position = new Vector3(-width/2, 0, -height/2);
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