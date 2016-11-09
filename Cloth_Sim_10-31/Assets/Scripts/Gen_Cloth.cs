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
    public Particle lastgrabbed;
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
        foreach(GameObject p in clothParticles)
        {
            foreach(GameObject sd in springDampers)
            {
                if (sd.GetComponent<SpringDamper>().P1 == p.GetComponent<Particle>())
                    p.GetComponent<Particle>().sj.Add(sd.GetComponent<SpringDamper>());
                else if (sd.GetComponent<SpringDamper>().P2 == p.GetComponent<Particle>())
                    p.GetComponent<Particle>().sj.Add(sd.GetComponent<SpringDamper>());
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
            Vector3 pPos = Camera.main.WorldToScreenPoint(p.transform.position);
            p.GetComponent<Particle>().ApplyGravity(1);
            if(pPos.x <= 5.5)
            {
                p.GetComponent<Particle>().v += -p.GetComponent<Particle>().v * 2;
            }
            else if (pPos.x >= Screen.width - 5.5)
            {
                p.GetComponent<Particle>().v += -p.GetComponent<Particle>().v * 2;
            }
            if (pPos.y <= 5.5)
            {
                if (p.GetComponent<Particle>().broken)
                    p.GetComponent<Particle>().v = Vector3.zero;
                else
                    p.GetComponent<Particle>().v += -p.GetComponent<Particle>().v * 2;
                p.GetComponent<Particle>().ApplyGravity(-5f);
                //p.GetComponent<Particle>().Force -= p.GetComponent<Particle>().Force * 2;
            }
        }
        foreach (GameObject sd in springDampers)
        {
            if (!sd.GetComponent<SpringDamper>().broken)
            {
                sd.GetComponent<SpringDamper>().Ks = Ks;
                sd.GetComponent<SpringDamper>().Kd = Kd;
                sd.GetComponent<SpringDamper>().l0 = L0;
                sd.GetComponent<SpringDamper>().ComputeForces();
                if (sd.GetComponent<SpringDamper>().l > 20)
                {
                    sd.GetComponent<SpringDamper>().broken = true;

                    foreach(GameObject p in clothParticles)
                    {
                        int iterator = 0;
                        foreach (SpringDamper spring in p.GetComponent<Particle>().sj)
                        {
                            if (spring.broken)
                                iterator++;
                        }
                        if(iterator == p.GetComponent<Particle>().sj.Count)
                        {
                            p.GetComponent<Particle>().broken = true;
                            p.GetComponent<Particle>().v = Vector3.zero;
                            p.GetComponent<Particle>().ApplyGravity(1);
                        }
                    }
                }
            }
        }



        foreach (GameObject ct in triangles)
        {
            if (!ct.GetComponent<ClothTriangle>().broken)
            {
                ct.GetComponent<ClothTriangle>().Vair = Wind;
                ct.GetComponent<ClothTriangle>().CalcAeroForce();
                if (ct.GetComponent<ClothTriangle>().P1.broken)
                {
                    ct.GetComponent<ClothTriangle>().broken = true;
                }
                else if (ct.GetComponent<ClothTriangle>().P2.broken)
                {
                    ct.GetComponent<ClothTriangle>().broken = true;
                }
                else if (ct.GetComponent<ClothTriangle>().P3.broken)
                {
                    ct.GetComponent<ClothTriangle>().broken = true;
                }
            }
        }
        //wText.text = "Wind Force: " + Wind.z.ToString();
    }
}
