using System.Collections;
using System.Collections.Generic;
using UniP2P;
using UnityEngine;

public class PacketCapture
{
    public static ulong counter;
    public static void Write(string dist, int length ,string info)
    {
        var src = UniP2PManager.GetEnableIPEndPoint().ToString();
        //Debug.Log(string.Format("Send:[{0}] Src:{2} Dist:{3} Length:{4} Info:{5} Time:{1}", counter,Time.realtimeSinceStartup, src, dist, length, info));
        counter++;
    }
}
