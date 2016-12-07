using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Scene : MonoBehaviour {

    public GameObject PauseMenu;
    public GameObject Spawner;

	// Use this for initialization
	void Start () {
	
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
                Spawner.SetActive(true);
            }
            else
            {
                PauseMenu.SetActive(true);
                foreach (GameObject p in Spawner.GetComponent<GenCloth>().ClothParticles)
                {
                    p.GetComponent<MonoParticle>().Paused = true;
                }
                Spawner.SetActive(false);
            }
        }
	}

    public void ReloadScene()
    {
        SceneManager.LoadScene("Scene1");
    }
}
