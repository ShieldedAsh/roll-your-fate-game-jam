using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;

/// <summary>
/// Manages sounds, not music.
/// </summary>
public class SoundManager : MonoBehaviour
{
    //Fields
    [SerializeField]
    [Tooltip ("A list of all collision sounds.")]
    private List<AudioResource> _collisions;

    [SerializeField]
    [Tooltip("Reference to the shredding sound.")]
    private AudioResource _shredding;

    [SerializeField]
    [Tooltip("A list of all spawning sounds.")]
    private List<AudioResource> _spawns;

    [SerializeField]
    [Tooltip ("Reference to the audio source")]
    private AudioSource _source;

    //Methods
    /// <summary>
    /// Plays a random collision sound.
    /// </summary>
    public void CollisionSound()
    {
        int index = Random.Range(0, _collisions.Count);
        _source.resource = _collisions[index];
        _source.Play();
    }

    /// <summary>
    /// Plays a shredding sound.
    /// </summary>
    public void ShreddingSound()
    {
        _source.resource = _shredding;
        _source.Play();
    }

    /// <summary>
    /// Plays a random spawn sound.
    /// </summary>
    public void SpawnSound()
    {
        int index = Random.Range(0, _spawns.Count);
        _source.resource = _spawns[index];
        _source.Play();
    }
}
