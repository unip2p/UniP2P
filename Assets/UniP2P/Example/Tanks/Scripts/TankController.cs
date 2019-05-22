using System.Collections;
using System.Collections.Generic;
using UniP2P.HLAPI;
using UnityEngine;
using UnityEngine.AI;

namespace UniP2P.Example.Tanks
{
    public class TankController : MonoBehaviour
    {
        public float m_Speed = 12f; // How fast the tank moves forward and back.
        public float m_TurnSpeed = 180f; // How fast the tank turns in degrees per second.
        public float m_PitchRange = 0.2f; // The amount by which the pitch of the engine noises can vary.

        public AudioSource
            m_MovementAudio; // Reference to the audio source used to play engine sounds. NB: different to the shooting audio source.

        public AudioClip m_EngineIdling; // Audio to play when the tank isn't moving.
        public AudioClip m_EngineDriving; // Audio to play when the tank is moving.
        public ParticleSystem m_LeftDustTrail; // The particle system of dust that is kicked up from the left track.
        public ParticleSystem m_RightDustTrail; // The particle system of dust that is kicked up from the rightt track.
        public Rigidbody m_Rigidbody; // Reference used to move the tank.

        private string m_MovementAxis; // The name of the input axis for moving forward and back.
        private string m_TurnAxis; // The name of the input axis for turning.
        private float m_MovementInput; // The current value of the movement input.
        private float m_TurnInput; // The current value of the turn input.
        private float m_OriginalPitch; // The pitch of the audio source at the start of the scene.

        private SyncGameObject SyncGameObject;

        private void Awake()
        {
            SyncGameObject = GetComponent<SyncGameObject>();
            m_Rigidbody = GetComponent<Rigidbody>();
        }


        private void Start()
        {
            // The axes are based on player number.
            m_MovementAxis = "Vertical";
            m_TurnAxis = "Horizontal";

            // Store the original pitch of the audio source.
            m_OriginalPitch = m_MovementAudio.pitch;
        }

        private void Update()
        {
            if (SyncGameObject.IsMine)
            {
                m_MovementInput = Input.GetAxis(m_MovementAxis);
                m_TurnInput = Input.GetAxis(m_TurnAxis);      

                EngineAudio();
            }
        }


        private void EngineAudio()
        {
            // If there is no input (the tank is stationary)...
            if (Mathf.Abs(m_MovementInput) < 0.1f && Mathf.Abs(m_TurnInput) < 0.1f)
            {
                // ... and if the audio source is currently playing the driving clip...
                if (m_MovementAudio.clip == m_EngineDriving)
                {
                    // ... change the clip to idling and play it.
                    m_MovementAudio.clip = m_EngineIdling;
                    m_MovementAudio.pitch =
                        Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                    m_MovementAudio.Play();
                }
            }
            else
            {
                // Otherwise if the tank is moving and the idling clip is currently playing...
                if (m_MovementAudio.clip == m_EngineIdling)
                {
                    // ... change the clip to driving and playing.
                    m_MovementAudio.clip = m_EngineDriving;
                    m_MovementAudio.pitch =
                        Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                    m_MovementAudio.Play();
                }
            }
        }


        private void FixedUpdate()
        {
            if (!SyncGameObject.IsMine)
                return;

            // Adjust the rigidbodies position and orientation in FixedUpdate.
            Move();
            Turn();
        }


        private void Move()
        {
            // Create a movement vector based on the input, speed and the time between frames, in the direction the tank is facing.
            Vector3 movement = transform.forward * m_MovementInput * m_Speed * Time.deltaTime;

            // Apply this movement to the rigidbody's position.
            m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
        }


        private void Turn()
        {
            // Determine the number of degrees to be turned based on the input, speed and time between frames.
            float turn = m_TurnInput * m_TurnSpeed * Time.deltaTime;

            // Make this into a rotation in the y axis.
            Quaternion inputRotation = Quaternion.Euler(0f, turn, 0f);

            // Apply this rotation to the rigidbody's rotation.
            m_Rigidbody.MoveRotation(m_Rigidbody.rotation * inputRotation);
        }


        // This function is called at the start of each round to make sure each tank is set up correctly.
        public void SetDefaults()
        {
            m_Rigidbody.velocity = Vector3.zero;
            m_Rigidbody.angularVelocity = Vector3.zero;

            m_MovementInput = 0f;
            m_TurnInput = 0f;

            m_LeftDustTrail.Clear();
            m_LeftDustTrail.Stop();

            m_RightDustTrail.Clear();
            m_RightDustTrail.Stop();
        }

        public void ReEnableParticles()
        {
            m_LeftDustTrail.Play();
            m_RightDustTrail.Play();
        }

        //We freeze the rigibody when the control is disabled to avoid the tank drifting!
        protected RigidbodyConstraints m_OriginalConstrains;

        void OnDisable()
        {
            m_OriginalConstrains = m_Rigidbody.constraints;
            m_Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }

        void OnEnable()
        {
            m_Rigidbody.constraints = m_OriginalConstrains;
        }

    }

}