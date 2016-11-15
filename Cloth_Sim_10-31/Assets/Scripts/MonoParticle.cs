using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



public class MonoParticle : MonoBehaviour
{
    Convert c = new Convert();
    public Particle p = new Particle();
    //public float m = 10;
    //public float s = 1;
    //public bool anchor;
    //public bool broken = false;
    //public Vector3 r;
    //public Vector3 v;
    //public Vector3 a;
    //public Vector3 Force;
    //public Vector3 g = new Vector3(0, -9.8f, 0);
    //public Vector3 Fgravity;
    public Vector3 screenPoint;
    public Vector3 offset;
    public List<SpringDamper> sj;
    Camera camera;
    public bool Paused = false;

    void Start()
    {
        camera = FindObjectOfType<Camera>();
        p.Force = new Vec3(0, 0, 0);
        p.r = c.Vector3toVec3(transform.position);
        if (!p.anchor)
        {
            //Calculate acceleration
            p.a = (1 / p.m) * p.Force;

            //Calculate velocity
            p.v += (p.a * Time.deltaTime);

            //Calculate position
            p.r += (p.v * Time.deltaTime);
        }
        transform.position = c.Vec3toVector3(p.r);
    }

    void Update()
    {
        if (!Paused)
        {
            Vector3 screenPos = camera.WorldToScreenPoint(transform.position);
            if ((Math.Abs(screenPos.x - Input.mousePosition.x) < 5f) && (Math.Abs(screenPos.y - Input.mousePosition.y) < 5f))
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    //Debug.Log(transform.position);
                    Debug.Log("gotcha");
                }
                if (Input.GetButtonDown("Fire2"))
                {
                    if (!p.anchor)
                        p.anchor = true;
                    else
                        p.anchor = false;
                }
            }
            p.r = c.Vector3toVec3(transform.position);

            p.Update(Time.deltaTime);

            transform.position = c.Vec3toVector3(p.r);
        }
    }

    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        FindObjectOfType<Gen_Cloth>().lastgrabbed = this;
    }

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;
        p.r = c.Vector3toVec3(transform.position);
    }
}
