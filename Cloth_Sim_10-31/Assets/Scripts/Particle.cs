using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Particle : MonoBehaviour
{
    public float m = 1;
    public float s = 1;
    public bool anchor;
    public bool broken = false;
    public Vector3 Pos0;
    public Vector3 r;
    public Vector3 v;
    public Vector3 a;
    public Vector3 Force;
    public Vector3 g = new Vector3(0, -9.8f, 0);
    public Vector3 Fgravity;
    public Vector3 screenPoint;
    public Vector3 offset;
    public List<SpringDamper> sj;
    Camera camera;
    void Start()
    {
        camera = FindObjectOfType<Camera>();
        Force = Vector3.zero;
        Pos0 = transform.position;
        r = transform.position;
        //Calculate acceleration
        a = (1 / m) * Force;

        //Calculate velocity
        v += (a * Time.deltaTime);

        //Calculate position
        r += (v * Time.deltaTime);

        transform.position = r;
    }

    void Update()
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
                if (!anchor)
                    anchor = true;
                else
                    anchor = false;
            }
        }
        r = transform.position;

        if (!anchor)
        {
            //if(broken)
            //{
            //    Force = Vector3.zero;
            //    //ApplyGravity(1);
            //}

            //Calculate acceleration
            a = (1 / m) * Force;

            //Calculate velocity
            v += (a * Time.deltaTime);

            //Calculate position
            r += (v * Time.deltaTime);

            //Reset Force
            Force = Vector3.zero;
        }

        transform.position = r;
    }

    public void UpdateParticle()
    {

    }

    public void ApplyGravity(float i)
    {
        g = new Vector3(0, -9.8f, 0) * (m);
        Fgravity = g * m * i;
        AddForce(Fgravity);
    }

    public void AddForce(Vector3 force)
    {
        if(!anchor)
            Force += force;
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
        r = transform.position;
    }
}
