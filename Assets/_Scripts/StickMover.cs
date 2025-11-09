using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class StickMover : MonoBehaviour
{
    //Fields
    [SerializeField] private float moveSpeed = 1f;

    [SerializeField]
    [Tooltip ("Reference to the sound manager")]
    private SoundManager _soundManager;

    [Tooltip ("A list of all sticks")]
    private List<GameObject> _sticks;

    private InputManager manager;
    private void Awake()
    {
        manager = FindFirstObjectByType<InputManager>();
        _soundManager = FindFirstObjectByType<SoundManager>();
        _sticks = FindFirstObjectByType<GameManager>().sticks;
    }

    private void Update()
    {
        transform.position += new Vector3(Mathf.Clamp(manager.Rotation - 180, -1, 1) * moveSpeed * Time.deltaTime, 0, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(_sticks.Contains(collision.gameObject))
        {
            _soundManager.CollisionSound();
            Destroy(this);
        }
    }
}