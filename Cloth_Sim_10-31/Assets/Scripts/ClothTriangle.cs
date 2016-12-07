using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ClothTriangle : MonoBehaviour
{
    private Convert _c = new Convert();
    public MonoParticle P1, P2, P3;
    public float P, Cd;
    public Vector3 Vair, Vsurface;
    public Vector3 P1V, P2V, P3V, V;
    public bool Broken = false;

    void Start()
    {
    }

    public void CalcAeroForce()
    {
        //Calculate Average Velocity
        Vsurface = (_c.Vec3ToVector3(P1.P.V + P2.P.V + P3.P.V)) / 3;
        V = Vsurface - Vair;
        var n = Vector3.Cross(_c.Vec3ToVector3(P2.P.R - P1.P.R), _c.Vec3ToVector3(P3.P.R - P1.P.R)) /
            (Vector3.Cross(_c.Vec3ToVector3(P2.P.R - P1.P.R), _c.Vec3ToVector3(P3.P.R - P1.P.R))).magnitude;
        var A = .5f * Vector3.Cross(_c.Vec3ToVector3(P2.P.R - P1.P.R), _c.Vec3ToVector3(P3.P.R - P1.P.R)).magnitude;
        if (V.magnitude != 0)
        {
            var a = A * (Vector3.Dot(V, n) / V.magnitude);
            var faero = (-.5f * (P * (V.magnitude * V.magnitude) * Cd * a * n)) / 3;
            P1.P.AddForce(_c.Vector3ToVec3(faero));
            P2.P.AddForce(_c.Vector3ToVec3(faero));
            P3.P.AddForce(_c.Vector3ToVec3(faero));
        }
    }
}