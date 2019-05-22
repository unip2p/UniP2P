using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniP2P.HLAPI
{
    public class NetworkErrorCanvas : MonoBehaviour
    {
        public static bool isError;
        public GameObject Panel;

        private void Start()
        {
            if (isError)
            {
                Panel.SetActive(true);
            }
        }
    }
}