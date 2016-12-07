using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Convert
{
    public Vector3 Vec3ToVector3(Vec3 v)
    {
        var output = new Vector3(v.x, v.y, v.z);
        return output;
    }
    public Vec3 Vector3ToVec3(Vector3 v)
    {
        var output = new Vec3(v.x, v.y, v.z);
        return output;
    }
}

public class GenCloth : MonoBehaviour
{
    private Convert _c = new Convert();
    public GameObject Sphere;
    public GameObject SpringDamp;
    public GameObject Triangle;
    public GameObject Floor;
    public GameObject Wall1;
    public GameObject Wall2;
    public int Row = 10, Col = 10, SdCount = 0;
    public List<GameObject> ClothParticles = new List<GameObject>();
    public List<GameObject> SpringDampers = new List<GameObject>();
    public List<GameObject> Triangles = new List<GameObject>();
    public Vector3 Wind = new Vector3(0, 0, 0);
    public float Ks, Kd, L0, TensileStr, Grav;
    public Slider WindForce;
    public Text Wtext;
    public Slider Tearing;
    public Text Ttext;
    public Slider SpringStrength;
    public Text Stext;
    public Slider GravityStrength;
    public Text Gtext;
    public MonoParticle Lastgrabbed;
    private Camera _camera;
    private float _parseTest;
    // Use this for initialization
    void Start()
    {
        _camera = FindObjectOfType<Camera>();
        for (var i = 0; i < Col; i++)
        {
            //Debug.Log(i);
            if (i == 9)
            {
                Debug.Log("Broke Row");
            }
            for (var j = 0; j < Row; j++)
            {
                var particle = Instantiate(Sphere);
                particle.gameObject.name = "Sphere" + ((j + (Row * i)) + 1);
                particle.transform.position = new Vector3(0 + (j * 5), 20 + (i * 5), 40);
                if ((i == Col - 1))
                {
                    //Debug.Log("pinned");
                    particle.GetComponent<MonoParticle>().P.Anchor = true;
                }
                ClothParticles.Add(particle);
                if ((j + (Row * i)) != 0)
                {
                    if (j != 0)
                    {
                        var sd = Instantiate(SpringDamp);
                        SdCount++;
                        sd.name += SdCount.ToString();
                        sd.GetComponent<SpringDamper>().P1 = ClothParticles[(j + (Row * i)) - 1].GetComponent<MonoParticle>();
                        sd.GetComponent<SpringDamper>().P2 = ClothParticles[(j + (Row * i))].GetComponent<MonoParticle>();
                        SpringDampers.Add(sd);
                    }
                    if (i != 0)
                    {
                        var sd = Instantiate(SpringDamp);
                        SdCount++;
                        sd.name += SdCount.ToString();
                        sd.GetComponent<SpringDamper>().P1 = ClothParticles[(j + (Row * (i - 1)))].GetComponent<MonoParticle>();
                        sd.GetComponent<SpringDamper>().P2 = ClothParticles[(j + (Row * i))].GetComponent<MonoParticle>();
                        SpringDampers.Add(sd);
                    }
                    if (i != 0 && j != Row - 1)
                    {
                        var sd = Instantiate(SpringDamp);
                        SdCount++;
                        sd.name += SdCount.ToString();
                        sd.GetComponent<SpringDamper>().P1 = ClothParticles[((j + 1) + (Row * (i - 1)))].GetComponent<MonoParticle>();
                        sd.GetComponent<SpringDamper>().P2 = ClothParticles[(j + (Row * i))].GetComponent<MonoParticle>();
                        SpringDampers.Add(sd);
                    }
                    if (i != 0 && j != 0)
                    {
                        var sd = Instantiate(SpringDamp);
                        SdCount++;
                        sd.name += SdCount.ToString();
                        sd.GetComponent<SpringDamper>().P1 = ClothParticles[((j - 1) + (Row * (i - 1)))].GetComponent<MonoParticle>();
                        sd.GetComponent<SpringDamper>().P2 = ClothParticles[(j + (Row * i))].GetComponent<MonoParticle>();
                        SpringDampers.Add(sd);
                    }
                }
            }
        }
        foreach (GameObject p in ClothParticles)
        {
            foreach (GameObject sd in SpringDampers)
            {
                if (sd.GetComponent<SpringDamper>().P1 == p.GetComponent<MonoParticle>())
                    p.GetComponent<MonoParticle>().Sj.Add(sd.GetComponent<SpringDamper>());
                else if (sd.GetComponent<SpringDamper>().P2 == p.GetComponent<MonoParticle>())
                    p.GetComponent<MonoParticle>().Sj.Add(sd.GetComponent<SpringDamper>());
            }
        }
        for (var i = 0; i < (((Col - 1) * (Row - 1)) * 4); i++)
        {
            var t = Instantiate(Triangle);
            t.name += (i + 1).ToString();
            Triangles.Add(t);
        }
        var T = 0;
        for (var i = 0; i < Col; i++)
        {
            for (var j = 0; j < Row; j++)
            {
                if (i != (Col - 1))
                {
                    if (j != 0)
                    {
                        Triangles[T].GetComponent<ClothTriangle>().P1 = ClothParticles[(j - 1) + (i * Row)].GetComponent<MonoParticle>();
                        Triangles[T].GetComponent<ClothTriangle>().P2 = ClothParticles[j + (i * Row)].GetComponent<MonoParticle>();
                        Triangles[T].GetComponent<ClothTriangle>().P3 = ClothParticles[(j - 1) + ((i + 1) * Row)].GetComponent<MonoParticle>();
                        T++;
                        Triangles[T].GetComponent<ClothTriangle>().P1 = ClothParticles[j + ((i + 1) * Row)].GetComponent<MonoParticle>();
                        Triangles[T].GetComponent<ClothTriangle>().P2 = ClothParticles[(j - 1) + ((i + 1) * Row)].GetComponent<MonoParticle>();
                        Triangles[T].GetComponent<ClothTriangle>().P3 = ClothParticles[j + (i * Row)].GetComponent<MonoParticle>();
                        T++;
                        Triangles[T].GetComponent<ClothTriangle>().P1 = ClothParticles[j + (i * Row)].GetComponent<MonoParticle>();
                        Triangles[T].GetComponent<ClothTriangle>().P2 = ClothParticles[(j - 1) + (i * Row)].GetComponent<MonoParticle>();
                        Triangles[T].GetComponent<ClothTriangle>().P3 = ClothParticles[j + ((i + 1) * Row)].GetComponent<MonoParticle>();
                        T++;
                        Triangles[T].GetComponent<ClothTriangle>().P1 = ClothParticles[(j - 1) + ((i + 1) * Row)].GetComponent<MonoParticle>();
                        Triangles[T].GetComponent<ClothTriangle>().P2 = ClothParticles[j + ((i + 1) * Row)].GetComponent<MonoParticle>();
                        Triangles[T].GetComponent<ClothTriangle>().P3 = ClothParticles[(j - 1) + (i * Row)].GetComponent<MonoParticle>();
                        T++;
                    }
                }
            }
        }
        //populateLists();
    }

    void PopulateLists()
    {
        foreach (MonoParticle p in FindObjectsOfType<MonoParticle>())
            ClothParticles.Add(p.gameObject);
        foreach (SpringDamper sd in FindObjectsOfType<SpringDamper>())
            SpringDampers.Add(sd.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
        foreach (GameObject p in ClothParticles)
        {
            var pPos = Camera.main.WorldToScreenPoint(p.transform.position);
            p.GetComponent<MonoParticle>().P.ApplyGravity(Grav);
            if (p.GetComponent<MonoParticle>().P.R.x <= Wall1.transform.position.x + .5f)
            {
                p.GetComponent<MonoParticle>().P.V += -p.GetComponent<MonoParticle>().P.V * 2;
            }
            else if (p.GetComponent<MonoParticle>().P.R.x >= Wall2.transform.position.x - .5f)
            {
                p.GetComponent<MonoParticle>().P.V += -p.GetComponent<MonoParticle>().P.V * 2;
            }
            if (p.GetComponent<MonoParticle>().P.R.y <= Floor.transform.position.y + .5f)
            {
                if (p.GetComponent<MonoParticle>().P.Broken)
                    p.GetComponent<MonoParticle>().P.V = new Vec3(0, 0, p.GetComponent<MonoParticle>().P.V.z);
                else
                    p.GetComponent<MonoParticle>().P.V += -p.GetComponent<MonoParticle>().P.V * 2;
                p.GetComponent<MonoParticle>().P.ApplyGravity(-Grav * 2);
                //p.GetComponent<Particle>().Force -= p.GetComponent<Particle>().Force * 2;
            }
        }
        foreach (GameObject sd in SpringDampers)
        {
            if (!sd.GetComponent<SpringDamper>().Broken)
            {
                sd.GetComponent<SpringDamper>().Ks = Ks;
                sd.GetComponent<SpringDamper>().Kd = Kd;
                sd.GetComponent<SpringDamper>().L0 = L0;
                sd.GetComponent<SpringDamper>().ComputeForces();
                if (sd.GetComponent<SpringDamper>().L > TensileStr)
                {
                    sd.GetComponent<SpringDamper>().Broken = true;

                    foreach (GameObject p in ClothParticles)
                    {
                        var iterator = 0;
                        foreach (SpringDamper spring in p.GetComponent<MonoParticle>().Sj)
                        {
                            if (spring.Broken)
                                iterator++;
                        }
                        if (iterator == p.GetComponent<MonoParticle>().Sj.Count)
                        {
                            p.GetComponent<MonoParticle>().P.Broken = true;
                            p.GetComponent<MonoParticle>().P.V = new Vec3(0, 0, 0);
                            p.GetComponent<MonoParticle>().P.ApplyGravity(Grav);
                        }
                    }
                }
            }
        }
        
        foreach (GameObject ct in Triangles)
        {
            if (!ct.GetComponent<ClothTriangle>().Broken)
            {
                ct.GetComponent<ClothTriangle>().Vair = Wind;
                ct.GetComponent<ClothTriangle>().CalcAeroForce();
                if (ct.GetComponent<ClothTriangle>().P1.P.Broken)
                {
                    ct.GetComponent<ClothTriangle>().Broken = true;
                }
                else if (ct.GetComponent<ClothTriangle>().P2.P.Broken)
                {
                    ct.GetComponent<ClothTriangle>().Broken = true;
                }
                else if (ct.GetComponent<ClothTriangle>().P3.P.Broken)
                {
                    ct.GetComponent<ClothTriangle>().Broken = true;
                }
            }
        }
        
    }

    public void ChangeWind()
    {
        Wind = new Vector3(0, 0, WindForce.value);
        Wtext.text = "Wind Force: " + Wind.z;
    }

    public void ChangeTearing()
    {
        TensileStr = Tearing.value;
        Ttext.text = "Tearing Point: " + TensileStr;
    }

    public void ChangeSpringConstant()
    {
        Ks = SpringStrength.value;
        Stext.text = Ks + " :Spring Strength";
    }
    public void ChangeGravity()
    {
        Grav = GravityStrength.value;
        Gtext.text = Grav + " :Gravity Strength";
    }
}
