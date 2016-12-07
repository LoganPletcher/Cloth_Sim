using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class MonoParticle : MonoBehaviour
{
    private Convert _c = new Convert();
    public Particle P = new Particle();
    //public float m = 10;
    //public float s = 1;
    //public bool anchor;
    //public bool Broken = false;
    //public Vector3 r;
    //public Vector3 v;
    //public Vector3 a;
    //public Vector3 Force;
    //public Vector3 g = new Vector3(0, -9.8f, 0);
    //public Vector3 Fgravity;
    public Vector3 ScreenPoint;
    public Vector3 Offset;
    public List<SpringDamper> Sj;
    private Camera _camera;
    public bool Paused = false;

    void Start()
    {
        _camera = FindObjectOfType<Camera>();
        P.Force = new Vec3(0, 0, 0);
        P.R = _c.Vector3ToVec3(transform.position);
        if (!P.Anchor)
        {
            //Calculate acceleration
            P.A = (1 / P.M) * P.Force;

            //Calculate velocity
            P.V += (P.A * Time.deltaTime);

            //Calculate position
            P.R += (P.V * Time.deltaTime);
        }
        transform.position = _c.Vec3ToVector3(P.R);
    }

    void Update()
    {
        if (!Paused)
        {
            var screenPos = _camera.WorldToScreenPoint(transform.position);
            if ((Math.Abs(screenPos.x - Input.mousePosition.x) < 5f) && (Math.Abs(screenPos.y - Input.mousePosition.y) < 5f))
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    //Debug.Log(transform.position);
                    Debug.Log("gotcha");
                }
                if (Input.GetButtonDown("Fire2"))
                {
                    if (!P.Anchor)
                        P.Anchor = true;
                    else
                        P.Anchor = false;
                }
            }
            P.R = _c.Vector3ToVec3(transform.position);

            P.Update(Time.deltaTime);

            transform.position = _c.Vec3ToVector3(P.R);
        }
    }

    void OnMouseDown()
    {
        ScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
        Offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, ScreenPoint.z));
        FindObjectOfType<GenCloth>().Lastgrabbed = this;
    }

    void OnMouseDrag()
    {
        var curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, ScreenPoint.z);
        var curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + Offset;
        transform.position = curPosition;
        P.R = _c.Vector3ToVec3(transform.position);
    }
}
