using UnityEngine;
using System.Collections.Generic;

public class Gen_Cloth : MonoBehaviour
{
    public GameObject sphere;
    public GameObject springDamp;
    public int row = 10, col = 10, sdCount = 0;
    public List<GameObject> clothParticles = new List<GameObject>();
    List<Vector3> particlePos = new List<Vector3>();
    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < row; j++)
            {
                GameObject particle = Instantiate(sphere);
                particle.gameObject.name = "Sphere" + ((j + (row * i)) + 1).ToString();
                particle.transform.position = new Vector3(-22.5f + (j * 5), (i * 5), 0);
                clothParticles.Add(particle);
                if ((j + (row * i)) != 0)
                {
                    if (j != 0)
                    {
                        GameObject SD = Instantiate(springDamp);
                        sdCount++;
                        SD.name += sdCount.ToString();
                        SD.GetComponent<SpringDamper>().P1 = clothParticles[(j + (row * i)) - 1].GetComponent<Particle>();
                        SD.GetComponent<SpringDamper>().P2 = clothParticles[(j + (row * i))].GetComponent<Particle>();
                    }
                    if(i != 0)
                    {
                        GameObject SD = Instantiate(springDamp);
                        sdCount++;
                        SD.name += sdCount.ToString();
                        SD.GetComponent<SpringDamper>().P1 = clothParticles[(j + (row * (i - 1)))].GetComponent<Particle>();
                        SD.GetComponent<SpringDamper>().P2 = clothParticles[(j + (row * i))].GetComponent<Particle>();
                    }
                    if(i != 0 && j!= row - 1)
                    {
                        GameObject SD = Instantiate(springDamp);
                        sdCount++;
                        SD.name += sdCount.ToString();
                        SD.GetComponent<SpringDamper>().P1 = clothParticles[((j + 1) + (row * (i - 1)))].GetComponent<Particle>();
                        SD.GetComponent<SpringDamper>().P2 = clothParticles[(j + (row * i))].GetComponent<Particle>();
                    }
                    if (i != 0 && j != 0)
                    {
                        GameObject SD = Instantiate(springDamp);
                        sdCount++;
                        SD.name += sdCount.ToString();
                        SD.GetComponent<SpringDamper>().P1 = clothParticles[((j - 1) + (row * (i - 1)))].GetComponent<Particle>();
                        SD.GetComponent<SpringDamper>().P2 = clothParticles[(j + (row * i))].GetComponent<Particle>();
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
