//=============================================================================================================================
//
// Copyright (c) 2015-2018 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
// EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
// and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//=============================================================================================================================

using System;
using UnityEngine;
using EasyAR;
using System.Collections.Generic;
using System.Linq;


namespace Sample
{
    public class ARGlobalSetupBehaviour : MonoBehaviour
    {
        private const string title = "Please enter KEY first!";
        private const string boxtitle = "===PLEASE ENTER YOUR KEY HERE===";
        private const string keyMessage = ""
            + "Steps to create the key for this sample:\n"
            + "  1. login www.easyar.com\n"
            + "  2. create app with\n"
            + "      Name: HelloAR (Unity)\n"
            + "      Bundle ID: cn.easyar.samples.unity.helloar\n"
            + "  3. find the created item in the list and show key\n"
            + "  4. replace all text in TextArea with your key";

        public Dictionary<int, Target> Targets = new Dictionary<int, Target>();        
        public Dictionary<int, Transform> TargetsTransforms = new Dictionary<int, Transform>();
        
        private void Awake()
        {
            var EasyARBehaviour = FindObjectOfType<EasyARBehaviour>();
            if (EasyARBehaviour.Key.Contains(boxtitle))
            {
#if UNITY_EDITOR
                UnityEditor.EditorUtility.DisplayDialog(title, keyMessage, "OK");
#endif
                Debug.LogError(title + " " + keyMessage);
            }
            EasyARBehaviour.Initialize();
            foreach (var behaviour in ARBuilder.Instance.ARCameraBehaviours)
            {
                behaviour.TargetFound += OnTargetFound;
                behaviour.TargetLost += OnTargetLost;
                behaviour.TextMessage += OnTextMessage;
            }
            foreach (var behaviour in ARBuilder.Instance.ImageTrackerBehaviours)
            {
                behaviour.TargetLoad += OnTargetLoad;
                behaviour.TargetUnload += OnTargetUnload;
            }
        }

        void OnTargetFound(ARCameraBaseBehaviour arcameraBehaviour, TargetAbstractBehaviour targetBehaviour, Target target)
        {
            Debug.Log("<Global Handler> Found: " + target.Id);
            
            var index = Convert.ToInt32(target.Name[target.Name.Length - 1]);
            var targetTransform = GameObject.Find(target.Name).GetComponent<Transform>();
            
            Targets.Add(index, target);
            TargetsTransforms.Add(index, targetTransform);

        }

        void OnTargetLost(ARCameraBaseBehaviour arcameraBehaviour, TargetAbstractBehaviour targetBehaviour, Target target)
        {
            Debug.Log("<Global Handler> Lost: " + target.Id);

            var index = Convert.ToInt32(target.Name[target.Name.Length - 1]); 
            
            Targets.Remove(index);
            TargetsTransforms.Remove(index);
        }

        void OnTargetLoad(ImageTrackerBaseBehaviour trackerBehaviour, ImageTargetBaseBehaviour targetBehaviour, Target target, bool status)
        {
            Debug.Log("<Global Handler> Load target (" + status + "): " + target.Id + " (" + target.Name + ") " + " -> " + trackerBehaviour);
        }

        void OnTargetUnload(ImageTrackerBaseBehaviour trackerBehaviour, ImageTargetBaseBehaviour targetBehaviour, Target target, bool status)
        {
            Debug.Log("<Global Handler> Unload target (" + status + "): " + target.Id + " (" + target.Name + ") " + " -> " + trackerBehaviour);
        }

        private void OnTextMessage(ARCameraBaseBehaviour arcameraBehaviour, string text)
        {
            Debug.Log("got text: " + text);
        }
    }
}
