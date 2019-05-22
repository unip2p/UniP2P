using UnityEngine;

namespace UniP2P.HLAPI
{
    public interface IInstantiatedListener
    {
        void OnInstantiated(GameObject obj,string path);
    }
}