using UnityEngine;
using UnityEngine.UI;
using UniP2P.HLAPI;
using UniP2P.LLAPI;
using UniRx;
using UniRx.Async;

public class TankShooting : MonoBehaviour , ISyncReceiverByteArray
{
    public Rigidbody m_Shell;                 // Prefab of the shell.
    public Transform m_FireTransform;         // A child of the tank where the shells are spawned.
    public Slider m_AimSlider;                // A child of the tank that displays the current launch force.
    public AudioSource m_ShootingAudio;       // Reference to the audio source used to play the shooting audio. NB: different to the movement audio source.
    public AudioClip m_ChargingClip;          // Audio that plays when each shot is charging up.
    public AudioClip m_FireClip;              // Audio that plays when each shot is fired.
    public float m_Force = 15f;      // The force given to the shell if the fire button is not held.

    private string m_FireButton = "Fire1";            // The input axis that is used for launching shells.
    private Rigidbody m_Rigidbody;          // Reference to the rigidbody component.
    
    public SyncGameObject SyncGameObject;

    [HideInInspector]
    public Button ShotButton;
    
    private void Awake()
    {
        // Set up the references.
        m_Rigidbody = GetComponent<Rigidbody>();
        SyncGameObject = GetComponent<SyncGameObject>();
    }


    private void Start()
    {
        if (ShotButton != null)
        {
            ShotButton.OnClickAsObservable().Subscribe(_ => FireButton());
        }
    }

    private void Update()
    {
        if (!SyncGameObject.IsMine)       
            return;

        if (Input.GetButtonDown(m_FireButton))
        {
            FireMax();
        }
    }

    public void FireButton()
    {
        if (Application.isMobilePlatform)
        {
            FireMax();
        }
    }
    
    public async void FireMax()
    {
        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();

        CmdFire();

        await SyncGameObject.SendAsync(new Shotinfo(), typeof(TankShooting));
    }

    private void CmdFire()
    {
        // Create an instance of the shell and store a reference to it's rigidbody.
        Rigidbody shellInstance =
             Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation);

        // Create a velocity that is the tank's velocity and the launch force in the fire position's forward direction.
        Vector3 velocity = m_Rigidbody.velocity + m_Force * m_FireTransform.forward;

        // Set the shell's velocity to this velocity.
        shellInstance.velocity = velocity;
    }


    public void OnReceiveByteArray(byte[] value, Peer peer)
    {
        CmdFire();
    }
}

public class Shotinfo
{

}