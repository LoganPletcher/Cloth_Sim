using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class ClothTriangle : MonoBehaviour
{
    public Particle P1, P2, P3;
    public float p, Cd, a;
    public Vector3 Faero, v, n, Vair;

    void CalcAverageVelocity()
    {
        
    }

    void CalcNormal()
    {
        n = Vector3.Scale((P2.r - P1.r), (P3.r - P1.r)) / (Vector3.Scale((P2.r - P1.r), (P3.r - P1.r))).magnitude;
    }

    void CalcArea()
    {
        float A = (1 / 2) * Vector3.Scale((P2.r - P1.r), (P3.r - P1.r)).magnitude;
        a = A * (Vector3.Dot(v, n) / v.magnitude);
    }

    void CalcAeroForce()
    {
        //Calculate Average Velocity
        Vector3 Vsurface = (P1.v + P2.v + P3.v) / 3;
        v = Vsurface + Vair;


        CalcNormal();
        CalcArea();
        Faero = -(1 / 2) * (p * (v.magnitude * v.magnitude) * Cd * a * n);
    }
}