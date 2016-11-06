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
    //public Slider windForce;
    //public Text wText;
    Camera camera;
    float parseTest;
    // Use this for initialization
    void Start()
    {
        camera = FindObjectOfType<Camera>();
        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < row; j++)
            {
                GameObject particle = Instantiate(sphere);
                particle.gameObject.name = "Sphere" + ((j + (row * i)) + 1).ToString();
                particle.transform.position = new Vector3(0 + (j * 5), 20 + (i * 5), 0);
                if ((i == col - 1))
                {
                    //Debug.Log("pinned");
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
        
        //if(Input.GetButtonDown("Fire1"))
        //{
        //    Vector3 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
        //    Vector3 pPos = camera.WorldToScreenPoint(clothParticles[0].transform.position);
        //    Debug.Log("Particle: " + pPos);
        //    Debug.Log("Mouse: " + Input.mousePosition);
        //}
        //Wind = new Vector3(0, 0, windForce.value);
        foreach (GameObject p in clothParticles)
        {
            p.GetComponent<Particle>().ApplyGravity();
            if(p.GetComponent<Particle>().selected)
            {
                if(Input.GetButtonUp("Fire1"))
                {
                    p.GetComponent<Particle>().selected = false;
                }
            }
        }
        foreach (GameObject sd in springDampers)
        {
            sd.GetComponent<SpringDamper>().Ks = Ks;
            sd.GetComponent<SpringDamper>().Kd = Kd;
            sd.GetComponent<SpringDamper>().l0 = L0;
            sd.GetComponent<SpringDamper>().ComputeForces();
            //if (sd.GetComponent<SpringDamper>().l > 20)
            //{
            //    springDampers.Remove(sd);
            //    Destroy(sd);
            //}
        }
        foreach (GameObject ct in triangles)
        {
            ct.GetComponent<ClothTriangle>().Vair = Wind;
            ct.GetComponent<ClothTriangle>().CalcAeroForce();
        }
        //wText.text = "Wind Force: " + Wind.z.ToString();
    }
}
