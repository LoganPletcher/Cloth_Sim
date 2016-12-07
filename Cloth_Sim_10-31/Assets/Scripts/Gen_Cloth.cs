using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Convert
{
    public Vector3 Vec3toVector3(Vec3 v)
    {
        Vector3 output = new Vector3(v.x, v.y, v.z);
        return output;
    }
    public Vec3 Vector3toVec3(Vector3 v)
    {
        Vec3 output = new Vec3(v.x, v.y, v.z);
        return output;
    }
}

public class Gen_Cloth : MonoBehaviour
{
    Convert c = new Convert();
    public GameObject sphere;
    public GameObject springDamp;
    public GameObject triangle;
    public GameObject floor;
    public GameObject wall1;
    public GameObject wall2;
    public int row = 10, col = 10, sdCount = 0;
    public List<GameObject> clothParticles = new List<GameObject>();
    public List<GameObject> springDampers = new List<GameObject>();
    public List<GameObject> triangles = new List<GameObject>();
    public Vector3 Wind = new Vector3(0, 0, 0);
    public float Ks, Kd, L0, tensileStr, grav;
    public Slider windForce;
    public Text wText;
    public Slider tearing;
    public Text tText;
    public Slider springStrength;
    public Text sText;
    public Slider gravityStrength;
    public Text gText;
    public MonoParticle lastgrabbed;
    Camera camera;
    float parseTest;
    // Use this for initialization
    void Start()
    {
        camera = FindObjectOfType<Camera>();
        for (int i = 0; i < col; i++)
        {
            //Debug.Log(i);
            if (i == 9)
            {
                Debug.Log("Broke Row");
            }
            for (int j = 0; j < row; j++)
            {
                GameObject particle = Instantiate(sphere);
                particle.gameObject.name = "Sphere" + ((j + (row * i)) + 1).ToString();
                particle.transform.position = new Vector3(0 + (j * 5), 20 + (i * 5), 40);
                if ((i == col - 1))
                {
                    //Debug.Log("pinned");
                    particle.GetComponent<MonoParticle>().p.anchor = true;
                }
                clothParticles.Add(particle);
                if ((j + (row * i)) != 0)
                {
                    if (j != 0)
                    {
                        GameObject SD = Instantiate(springDamp);
                        sdCount++;
                        SD.name += sdCount.ToString();
                        SD.GetComponent<SpringDamper>().P1 = clothParticles[(j + (row * i)) - 1].GetComponent<MonoParticle>();
                        SD.GetComponent<SpringDamper>().P2 = clothParticles[(j + (row * i))].GetComponent<MonoParticle>();
                        springDampers.Add(SD);
                    }
                    if (i != 0)
                    {
                        GameObject SD = Instantiate(springDamp);
                        sdCount++;
                        SD.name += sdCount.ToString();
                        SD.GetComponent<SpringDamper>().P1 = clothParticles[(j + (row * (i - 1)))].GetComponent<MonoParticle>();
                        SD.GetComponent<SpringDamper>().P2 = clothParticles[(j + (row * i))].GetComponent<MonoParticle>();
                        springDampers.Add(SD);
                    }
                    if (i != 0 && j != row - 1)
                    {
                        GameObject SD = Instantiate(springDamp);
                        sdCount++;
                        SD.name += sdCount.ToString();
                        SD.GetComponent<SpringDamper>().P1 = clothParticles[((j + 1) + (row * (i - 1)))].GetComponent<MonoParticle>();
                        SD.GetComponent<SpringDamper>().P2 = clothParticles[(j + (row * i))].GetComponent<MonoParticle>();
                        springDampers.Add(SD);
                    }
                    if (i != 0 && j != 0)
                    {
                        GameObject SD = Instantiate(springDamp);
                        sdCount++;
                        SD.name += sdCount.ToString();
                        SD.GetComponent<SpringDamper>().P1 = clothParticles[((j - 1) + (row * (i - 1)))].GetComponent<MonoParticle>();
                        SD.GetComponent<SpringDamper>().P2 = clothParticles[(j + (row * i))].GetComponent<MonoParticle>();
                        springDampers.Add(SD);
                    }
                }
            }
        }
        foreach (GameObject p in clothParticles)
        {
            foreach (GameObject sd in springDampers)
            {
                if (sd.GetComponent<SpringDamper>().P1 == p.GetComponent<MonoParticle>())
                    p.GetComponent<MonoParticle>().sj.Add(sd.GetComponent<SpringDamper>());
                else if (sd.GetComponent<SpringDamper>().P2 == p.GetComponent<MonoParticle>())
                    p.GetComponent<MonoParticle>().sj.Add(sd.GetComponent<SpringDamper>());
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
                        triangles[T].GetComponent<ClothTriangle>().P1 = clothParticles[(j - 1) + (i * row)].GetComponent<MonoParticle>();
                        triangles[T].GetComponent<ClothTriangle>().P2 = clothParticles[j + (i * row)].GetComponent<MonoParticle>();
                        triangles[T].GetComponent<ClothTriangle>().P3 = clothParticles[(j - 1) + ((i + 1) * row)].GetComponent<MonoParticle>();
                        T++;
                        triangles[T].GetComponent<ClothTriangle>().P1 = clothParticles[j + ((i + 1) * row)].GetComponent<MonoParticle>();
                        triangles[T].GetComponent<ClothTriangle>().P2 = clothParticles[(j - 1) + ((i + 1) * row)].GetComponent<MonoParticle>();
                        triangles[T].GetComponent<ClothTriangle>().P3 = clothParticles[j + (i * row)].GetComponent<MonoParticle>();
                        T++;
                        triangles[T].GetComponent<ClothTriangle>().P1 = clothParticles[j + (i * row)].GetComponent<MonoParticle>();
                        triangles[T].GetComponent<ClothTriangle>().P2 = clothParticles[(j - 1) + (i * row)].GetComponent<MonoParticle>();
                        triangles[T].GetComponent<ClothTriangle>().P3 = clothParticles[j + ((i + 1) * row)].GetComponent<MonoParticle>();
                        T++;
                        triangles[T].GetComponent<ClothTriangle>().P1 = clothParticles[(j - 1) + ((i + 1) * row)].GetComponent<MonoParticle>();
                        triangles[T].GetComponent<ClothTriangle>().P2 = clothParticles[j + ((i + 1) * row)].GetComponent<MonoParticle>();
                        triangles[T].GetComponent<ClothTriangle>().P3 = clothParticles[(j - 1) + (i * row)].GetComponent<MonoParticle>();
                        T++;
                    }
                }
            }
        }
        //populateLists();
    }

    void populateLists()
    {
        foreach (MonoParticle p in FindObjectsOfType<MonoParticle>())
            clothParticles.Add(p.gameObject);
        foreach (SpringDamper sd in FindObjectsOfType<SpringDamper>())
            springDampers.Add(sd.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
        foreach (GameObject p in clothParticles)
        {
            Vector3 pPos = Camera.main.WorldToScreenPoint(p.transform.position);
            p.GetComponent<MonoParticle>().p.ApplyGravity(grav);
            if (p.GetComponent<MonoParticle>().p.r.x <= wall1.transform.position.x + .5f)
            {
                p.GetComponent<MonoParticle>().p.v += -p.GetComponent<MonoParticle>().p.v * 2;
            }
            else if (p.GetComponent<MonoParticle>().p.r.x >= wall2.transform.position.x - .5f)
            {
                p.GetComponent<MonoParticle>().p.v += -p.GetComponent<MonoParticle>().p.v * 2;
            }
            if (p.GetComponent<MonoParticle>().p.r.y <= floor.transform.position.y + .5f)
            {
                if (p.GetComponent<MonoParticle>().p.broken)
                    p.GetComponent<MonoParticle>().p.v = new Vec3(0, 0, p.GetComponent<MonoParticle>().p.v.z);
                else
                    p.GetComponent<MonoParticle>().p.v += -p.GetComponent<MonoParticle>().p.v * 2;
                p.GetComponent<MonoParticle>().p.ApplyGravity(-grav * 2);
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
                if (sd.GetComponent<SpringDamper>().l > tensileStr)
                {
                    sd.GetComponent<SpringDamper>().broken = true;

                    foreach (GameObject p in clothParticles)
                    {
                        int iterator = 0;
                        foreach (SpringDamper spring in p.GetComponent<MonoParticle>().sj)
                        {
                            if (spring.broken)
                                iterator++;
                        }
                        if (iterator == p.GetComponent<MonoParticle>().sj.Count)
                        {
                            p.GetComponent<MonoParticle>().p.broken = true;
                            p.GetComponent<MonoParticle>().p.v = new Vec3(0, 0, 0);
                            p.GetComponent<MonoParticle>().p.ApplyGravity(grav);
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
                if (ct.GetComponent<ClothTriangle>().P1.p.broken)
                {
                    ct.GetComponent<ClothTriangle>().broken = true;
                }
                else if (ct.GetComponent<ClothTriangle>().P2.p.broken)
                {
                    ct.GetComponent<ClothTriangle>().broken = true;
                }
                else if (ct.GetComponent<ClothTriangle>().P3.p.broken)
                {
                    ct.GetComponent<ClothTriangle>().broken = true;
                }
            }
        }
        
    }

    public void ChangeWind()
    {
        Wind = new Vector3(0, 0, windForce.value);
        wText.text = "Wind Force: " + Wind.z.ToString();
    }

    public void ChangeTearing()
    {
        tensileStr = tearing.value;
        tText.text = "Tearing Point: " + tensileStr.ToString();
    }

    public void ChangeSpringConstant()
    {
        Ks = springStrength.value;
        sText.text = Ks.ToString() + " :Spring Strength";
    }
    public void ChangeGravity()
    {
        grav = gravityStrength.value;
        gText.text = grav.ToString() + " :Gravity Strength";
    }
}
