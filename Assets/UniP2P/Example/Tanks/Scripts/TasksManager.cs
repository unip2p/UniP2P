using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniP2P.HLAPI;
using UniP2P.LLAPI;
using UniRx.Async;
using UnityEngine.UI;

namespace UniP2P.Example.Tanks
{
    public class TasksManager : MonoBehaviour , IInstantiatedListener
    {
        public Camera MainCamera;
        public Transform[] Respowns;
        public List<TankController> Tanks = new List<TankController>();

        public GameObject Canvas;
        
        private IInstantiatedListener _instantiatedListenerImplementation;

        private const string TankPath = "Tank";

        public Button ShotButton;
        
        private void Awake()
        {
            if (Application.isMobilePlatform)
            {
                Canvas.SetActive(true);
            }
            else
            {
                Canvas.SetActive(false);
            }
            DataEventManager.ListenInstantiated(this);
            Spawn();
        }
        
        void Spawn()
        {
            var obj = UniP2PManager.Instantiate(TankPath, Respowns[UniP2PManager.GetMyPeerOrder()].position, Respowns[UniP2PManager.GetMyPeerOrder()].rotation) as GameObject;
            obj.GetComponent<TankShooting>().ShotButton = ShotButton;
            Tanks.Add(obj.GetComponent<TankController>());
        }

        private async void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                await UniP2PManager.DisConnectAllPeerAsync();
                Application.Quit();
            }
        }

        public void OnInstantiated(GameObject obj,string path)
        {
            if (path == TankPath)
            {
                Tanks.Add(obj.GetComponent<TankController>());
            }
        }
    }
}