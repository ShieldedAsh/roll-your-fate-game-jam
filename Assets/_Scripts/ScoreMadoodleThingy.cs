using UnityEngine;

public class ScoreMadoodleThingy : MonoBehaviour
{
    public float scorePercent;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (FindFirstObjectByType<ScoreMadoodleThingy>() != this)
            Destroy(this.gameObject);
    }
}
