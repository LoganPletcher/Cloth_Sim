using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class SpringDamper : MonoBehaviour
{
    public float Fspring, Fdamp, Ks = 0, Kd = 0, l0 = 0, l;
    Vector3  Fsd1, Fsd2, x;
    public Vector3 e;
    public Particle P1, P2;
    Vector3 []particlePos = new Vector3[2];

    void Start()
    {
        //if(P1 != null)
        //    Debug.Log(P1.gameObject.name);
        //if (P2 != null)
        //{
        //    Debug.Log(P2.gameObject.name);
        //    //particlePos[0] = P1.r;
        //    //particlePos[1] = P2.r;
        //    //gameObject.GetComponent<LineRenderer>().SetPositions(particlePos);
        //}
        particlePos[0] = P1.r;
        particlePos[1] = P2.r;
        gameObject.GetComponent<LineRenderer>().SetPositions(particlePos);
    }

    void Update()
    {
        Vector3 prevE = e;
        CalcUnitLength();
        //if (prevE != e)
        //{
        particlePos[0] = P1.r;
        particlePos[1] = P2.r;
        gameObject.GetComponent<LineRenderer>().SetPositions(particlePos);
        //}

    }

    void CalcUnitLength()
    {
        Vector3 eNorm = (P2.r - P1.r).normalized;
        l = eNorm.magnitude;
        e = eNorm / l;
    }

    void Calc1DVelocities()
    {
        P1.v1D = Vector3.Dot(e, P1.v);
    }

    void CalcSpringForce()
    {
        Fspring = -Ks * (l0 - l);
    }

    void CalcDampingForce()
    {
        Fdamp = -Kd * (P1.v1D - P2.v1D);
    }

    void CalcSpringDamper()
    {
        Fsd1 = (Fspring + Fdamp) * e;
        Fsd2 = -Fsd1;
    }

    void ComputeForces()
    {
        CalcSpringForce();
        CalcDampingForce();
        CalcSpringDamper();
    }

}
