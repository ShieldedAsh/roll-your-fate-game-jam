using UnityEngine;

public class fuckingaround : MonoBehaviour
{
    [SerializeField]
    GameObject spam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Instantiate(spam);
    }
}
