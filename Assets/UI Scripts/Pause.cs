using UnityEngine;

public class Pause : MonoBehaviour
{
    public bool paused = false;
    public GameObject pausemenu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pausemenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void pauseGame()
    {
        if (paused)
        {
            paused = false;
            pausemenu.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
            
            paused = true;
            pausemenu.SetActive(true);
        }
    }
}
