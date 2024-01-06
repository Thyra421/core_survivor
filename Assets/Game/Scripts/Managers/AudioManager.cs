    using UnityEngine;

    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField]
        private GameObject audioSourcePrefab;

        public void SpawnAudioSource(AudioClip clip, Vector3 position)
        {
            GameObject g = Instantiate(audioSourcePrefab, position, Quaternion.identity);
            g.GetComponent<DestroyableSound>().Initialize(clip);
        }
    }
