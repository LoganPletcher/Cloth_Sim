using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Scene : MonoBehaviour {

    public GameObject PauseMenu;
    public GameObject Spawner;
    public GameObject PersistentPause;
	// Use this for initialization
	void Start () {
        PersistentPause = GameObject.Find("PreviousPause");
        PauseMenu.SetActive(PersistentPause.GetComponent<PersistingPauseBool>().Paused);
        foreach (GameObject p in Spawner.GetComponent<GenCloth>().ClothParticles)
        {
            p.GetComponent<MonoParticle>().Paused = PersistentPause.GetComponent<PersistingPauseBool>().Paused;
        }
        Spawner.SetActive(!PersistentPause.GetComponent<PersistingPauseBool>().Paused);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("p"))
        {
            if (PauseMenu.activeSelf)
            {
                PauseMenu.SetActive(false);
                foreach(GameObject p in Spawner.GetComponent<GenCloth>().ClothParticles)
                {
                    p.GetComponent<MonoParticle>().Paused = false;
                }
                PersistentPause.GetComponent<PersistingPauseBool>().Paused = false;
                Spawner.SetActive(true);
            }
            else
            {
                PauseMenu.SetActive(true);
                foreach (GameObject p in Spawner.GetComponent<GenCloth>().ClothParticles)
                {
                    p.GetComponent<MonoParticle>().Paused = true;
                }
                PersistentPause.GetComponent<PersistingPauseBool>().Paused = true;
                Spawner.SetActive(false);
            }
        }
        if (Input.GetKeyDown("escape"))
            End();
	}

    public void ReloadScene()
    {
        DontDestroyOnLoad(PersistentPause);
        SceneManager.LoadScene("Scene1");
    }

    void End()
    {
        Application.Quit();
    }
}
