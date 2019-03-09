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
                    foreach (var tracker in trackers)
                    {
                        if (!(tracker == null))
                        {
                            Vector3 current = tracker.position;
                            dots.Add(new Tuple<float, float>(current.x, current.z));
                        }
                    }

                    double diagonal = 0;
                    Tuple<float, float> dot1;
                    Tuple<float, float> dot2;
                    for (var i = 0; i < dots.Count(); i++)
                    {
                        for (var j = i+1;i <dots.Count(); j++)
                        {
                            var current = Math.Pow((dots[i].Item1 - dots[j].Item1), 2) + Math.Pow((dots[i].Item2 - dots[j].Item2), 2);
                            if (current > diagonal)
                            {
                                diagonal = current;
                                dot1 = dots[i];
                                dot2 = dots[j];
                            }
                        }
                    }

                    centerPoint[0] = (dot1.Item1 + dot1.Item1) / 2;
                    centerPoint[1] = (dot2.Item2 + dot2.Item2) / 2;
                    width = Math.Abs(dot1.Item1 - dot2.Item1);
                    height = Math.Abs(dot1.Item2 - dot2.Item2);
                    
                    this.transform.position = new Vector3(centerPoint[0], 0, centerPoint[1]);
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
