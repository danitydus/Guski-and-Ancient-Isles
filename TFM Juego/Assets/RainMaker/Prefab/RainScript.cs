using UnityEngine;
using System.Collections;

namespace DigitalRuby.RainMaker
{
    public class RainScript : BaseRainScript
    {
        [Tooltip("The height above the camera that the rain will start falling from")]
        public float RainHeight = 25.0f;

        [Tooltip("How far the rain particle system is ahead of the player")]
        public float RainForwardOffset = -7.0f;

        [Tooltip("The top y value of the mist particles")]
        public float RainMistHeight = 3.0f;

        private bool playerInTrigger = false;
        public float rainIntensity = 0.0f;
        public float rainChangeSpeed = 0.05f; // Velocidad de cambio de la intensidad de la lluvia

        private void UpdateRain()
        {
            // keep rain and mist above the player
            if (RainFallParticleSystem != null)
            {
                if (FollowCamera)
                {
                    var s = RainFallParticleSystem.shape;
                    s.shapeType = ParticleSystemShapeType.ConeVolume;
                    RainFallParticleSystem.transform.position = Camera.transform.position;
                    RainFallParticleSystem.transform.Translate(0.0f, RainHeight, RainForwardOffset);
                    RainFallParticleSystem.transform.rotation = Quaternion.Euler(0.0f, Camera.transform.rotation.eulerAngles.y, 0.0f);
                    if (RainMistParticleSystem != null)
                    {
                        var s2 = RainMistParticleSystem.shape;
                        s2.shapeType = ParticleSystemShapeType.Hemisphere;
                        Vector3 pos = Camera.transform.position;
                        pos.y += RainMistHeight;
                        RainMistParticleSystem.transform.position = pos;
                    }
                }
                else
                {
                    var s = RainFallParticleSystem.shape;
                    s.shapeType = ParticleSystemShapeType.Box;
                    if (RainMistParticleSystem != null)
                    {
                        var s2 = RainMistParticleSystem.shape;
                        s2.shapeType = ParticleSystemShapeType.Box;
                        Vector3 pos = RainFallParticleSystem.transform.position;
                        pos.y += RainMistHeight;
                        pos.y -= RainHeight;
                        RainMistParticleSystem.transform.position = pos;
                    }
                }
            }
        }

        private void UpdateRainIntensity()
        {
            if (playerInTrigger)
            {
                // Aumentar la intensidad de la lluvia hasta 1 cuando el jugador está en el trigger
                rainIntensity = Mathf.Min(1.0f, rainIntensity + rainChangeSpeed * Time.deltaTime);
            }
            else
            {
                // Disminuir la intensidad de la lluvia hasta 0 cuando el jugador sale del trigger
                rainIntensity = Mathf.Max(0.0f, rainIntensity - rainChangeSpeed * Time.deltaTime);
            }

            // Ajustar la intensidad de la lluvia
            if (RainFallParticleSystem != null)
            {
                var main = RainFallParticleSystem.main;
                main.startLifetime = Mathf.Lerp(0.5f, 1.5f, rainIntensity); // Aumentar la duración de las gotas con la intensidad
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInTrigger = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInTrigger = false;
            }
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
            UpdateRainIntensity();
            UpdateRain();
        }
    }
}
