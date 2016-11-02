using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Gen_Cloth : MonoBehaviour
{
    public GameObject sphere;
    public GameObject springDamp;
    public GameObject triangle;
    public int row = 10, col = 10, sdCount = 0;
    public List<GameObject> clothParticles = new List<GameObject>();
    public List<GameObject> springDampers = new List<GameObject>();
    public List<GameObject> triangles = new List<GameObject>();
    public Vector3 Wind = new Vector3(0,0,0);
    public float Ks, Kd, L0;
    public InputField windForce;
    float parseTest;
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
                if ((i == col - 1 && j == row - 1) || (i == col - 1 && j == 0))
                {
                    Debug.Log("pinned");
                    particle.GetComponent<Particle>().anchor = true;
                }
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
                        springDampers.Add(SD);
                    }
                    if (i != 0)
                    {
                        GameObject SD = Instantiate(springDamp);
                        sdCount++;
                        SD.name += sdCount.ToString();
                        SD.GetComponent<SpringDamper>().P1 = clothParticles[(j + (row * (i - 1)))].GetComponent<Particle>();
                        SD.GetComponent<SpringDamper>().P2 = clothParticles[(j + (row * i))].GetComponent<Particle>();
                        springDampers.Add(SD);
                    }
                    if (i != 0 && j != row - 1)
                    {
                        GameObject SD = Instantiate(springDamp);
                        sdCount++;
                        SD.name += sdCount.ToString();
                        SD.GetComponent<SpringDamper>().P1 = clothParticles[((j + 1) + (row * (i - 1)))].GetComponent<Particle>();
                        SD.GetComponent<SpringDamper>().P2 = clothParticles[(j + (row * i))].GetComponent<Particle>();
                        springDampers.Add(SD);
                    }
                    if (i != 0 && j != 0)
                    {
                        GameObject SD = Instantiate(springDamp);
                        sdCount++;
                        SD.name += sdCount.ToString();
                        SD.GetComponent<SpringDamper>().P1 = clothParticles[((j - 1) + (row * (i - 1)))].GetComponent<Particle>();
                        SD.GetComponent<SpringDamper>().P2 = clothParticles[(j + (row * i))].GetComponent<Particle>();
                        springDampers.Add(SD);
                    }
                }
            }
        }
        for (int i = 0; i < (((col - 1) * (row - 1)) * 4); i++)
        {
            GameObject t = Instantiate(triangle);
            t.name += (i + 1).ToString();
            triangles.Add(t);
        }
        int T = 0;
        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < row; j++)
            {
                if (i != (col - 1))
                {
                    if (j != 0)
                    {
                        Debug.Log(clothParticles[(j) + (i * row)].name);
                        triangles[T].GetComponent<ClothTriangle>().P1 = clothParticles[(j - 1) + (i * row)].GetComponent<Particle>();
                        triangles[T].GetComponent<ClothTriangle>().P2 = clothParticles[j + (i * row)].GetComponent<Particle>();
                        triangles[T].GetComponent<ClothTriangle>().P3 = clothParticles[(j - 1) + ((i + 1) * row)].GetComponent<Particle>();
                        T++;
                        triangles[T].GetComponent<ClothTriangle>().P1 = clothParticles[j + ((i + 1) * row)].GetComponent<Particle>();
                        triangles[T].GetComponent<ClothTriangle>().P2 = clothParticles[(j - 1) + ((i + 1) * row)].GetComponent<Particle>();
                        triangles[T].GetComponent<ClothTriangle>().P3 = clothParticles[j + (i * row)].GetComponent<Particle>();
                        T++;
                        triangles[T].GetComponent<ClothTriangle>().P1 = clothParticles[j + (i * row)].GetComponent<Particle>();
                        triangles[T].GetComponent<ClothTriangle>().P2 = clothParticles[(j - 1) + (i * row)].GetComponent<Particle>();
                        triangles[T].GetComponent<ClothTriangle>().P3 = clothParticles[j + ((i + 1) * row)].GetComponent<Particle>();
                        T++;
                        triangles[T].GetComponent<ClothTriangle>().P1 = clothParticles[(j - 1) + ((i + 1) * row)].GetComponent<Particle>();
                        triangles[T].GetComponent<ClothTriangle>().P2 = clothParticles[j + ((i + 1) * row)].GetComponent<Particle>();
                        triangles[T].GetComponent<ClothTriangle>().P3 = clothParticles[(j - 1) + (i * row)].GetComponent<Particle>();
                        T++;
                    }
                }
            }
        }
        //populateLists();
    }

    void populateLists()
    {
        foreach (Particle p in FindObjectsOfType<Particle>())
            clothParticles.Add(p.gameObject);
        foreach (SpringDamper sd in FindObjectsOfType<SpringDamper>())
            springDampers.Add(sd.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (float.TryParse(windForce.text,out parseTest))
        {
            Wind = new Vector3(0, 0, float.Parse(windForce.text));
        }
        foreach (GameObject p in clothParticles)
            p.GetComponent<Particle>().ApplyGravity();

        foreach (GameObject sd in springDampers)
        {
            sd.GetComponent<SpringDamper>().Ks = Ks;
            sd.GetComponent<SpringDamper>().Kd = Kd;
            sd.GetComponent<SpringDamper>().l0 = L0;
            sd.GetComponent<SpringDamper>().ComputeForces();
        }
        foreach (GameObject ct in triangles)
        {
            ct.GetComponent<ClothTriangle>().Vair = Wind;
            ct.GetComponent<ClothTriangle>().CalcAeroForce();
        }
        windForce.text = Wind.z.ToString();
    }
}
