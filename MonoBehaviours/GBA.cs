using UnityEngine;

namespace BlissScrap.MonoBehaviours
{
    public class GBA : GrabbableObject
    {
        private bool screenOn = false;
        int emissiveIntensity = 1;
        Color emissiveColor = Color.white;

        public AudioSource noiseAudio;

        public AudioClip startupSFX;
        public float loudness = 1f;

        [Space(3f)]
        public float noiseRange;

        private Material screenMaterial;
        private float screenOnTimer = 0f;

        public override void ItemActivate(bool used, bool buttonDown = true)
        {
            base.ItemActivate(used, buttonDown);
            if (!(GameNetworkManager.Instance.localPlayerController == null))
            {
                if (screenMaterial == null) {
                    screenMaterial = mainObjectRenderer.material;
                }
                if (!screenOn)
                {
                    noiseAudio.PlayOneShot(startupSFX, loudness);
                    WalkieTalkie.TransmitOneShotAudio(noiseAudio, startupSFX, loudness);
                    RoundManager.Instance.PlayAudibleNoise(base.transform.position, noiseRange, loudness, 0, isInElevator && StartOfRound.Instance.hangarDoorsClosed);
                    screenOn = true;
                    isBeingUsed = true;
                }
                else if(screenOn && !noiseAudio.isPlaying)
                {
                    screenMaterial.SetColor("_EmissiveColor", emissiveColor * 0);
                    screenOn = false;
                    isBeingUsed = false;
                    screenOnTimer = 0f;
                }
            }
        }

        public override void Update() {
            base.Update();
            if (screenOn && screenOnTimer < 1f) {
                screenOnTimer += Time.deltaTime;
                screenMaterial.SetColor("_EmissiveColor", emissiveColor * Mathf.Lerp(0, emissiveIntensity, screenOnTimer));
            }
        }
    }
}
