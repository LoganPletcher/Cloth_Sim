using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Particle : MonoBehaviour
{
    public float m;
    public float s;
    public float v1D;
    public Vector3 r;
    public Vector3 v;
    public Vector3 a;
    public Vector3 Force;
    public Vector3 g = new Vector3(0, -9.8f, 0);
    public Vector3 Fgravity;

    void Start()
    {
        r = transform.position;
    }

    void Update()
    {
        r = transform.position;
    }

    void CalcVelocity()
    {
        v += (a * Time.deltaTime);
    }

    void CalcPosition()
    {
        r += (v * Time.deltaTime);
    }

    void CalcAcceleration()
    {
        a = (1 / m) * Force;
    }

    void CalcGravity()
    {
        g = new Vector3(0, -9.8f, 0) * (m / (s * s));
    }

    void CalcGravForce()
    {
        Fgravity = g * m;
    }
}
