using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class ClothTriangle : MonoBehaviour
{
    public Particle P1, P2, P3;
    public float p, Cd;
    public Vector3 Vair, Vsurface;
    public Vector3 P1v, P2v, P3v;

    void Start()
    {
    }

    public void CalcAeroForce()
    {
        //Calculate Average Velocity
        Vsurface = (P1.v + P2.v + P3.v) / 3;
        Vector3 v = Vsurface - Vair;
        Vector3 n = Vector3.Cross((P2.r - P1.r), (P3.r - P1.r)) / (Vector3.Cross((P2.r - P1.r), (P3.r - P1.r))).magnitude;
        float A = .5f * Vector3.Cross((P2.r - P1.r), (P3.r - P1.r)).magnitude;
        if (v.magnitude != 0)
        {
            float a = A * (Vector3.Dot(v, n) / v.magnitude);
            Vector3 Faero = (-.5f * (p * (v.magnitude * v.magnitude) * Cd * a * n)) / 3;
            P1.AddForce(Faero);
            P2.AddForce(Faero);
            P3.AddForce(Faero);
        }
    }
}