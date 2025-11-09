using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public TextMeshProUGUI endScreenTest;
    private InputManager manager;
    private ScoreMadoodleThingy thingy;

    private void Awake()
    {
        manager = FindFirstObjectByType<InputManager>();
        thingy = FindFirstObjectByType<ScoreMadoodleThingy>();
        endScreenTest.text = $"Covered: {thingy.scorePercent:P}";
    }
    private void Update()
    {
        if(manager.FlickDirection == InputData.FlickAway)
        {
            SceneManager.LoadScene(0);
        }
    }
}