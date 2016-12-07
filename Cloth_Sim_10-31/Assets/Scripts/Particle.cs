using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
public class Vec3
{
    public Vec3()
    {
        X = 0;
        Y = 0;
        Z = 0;
    }
    public Vec3(float a, float b, float c)
    {
        X = a;
        Y = b;
        Z = c;
    }
    public Vec3(Vec3 other)
    {
        this.X = other.X;
        this.Y = other.Y;
        this.Z = other.Z;
    }
    float X = 0, Y = 0, Z = 0;
    public float x { get { return X; } }
    public float y { get { return Y; } }
    public float z { get { return Z; } }

    public static Vec3 operator *(Vec3 v, float f)
    {
        var result = new Vec3
        {
            X = v.X*f,
            Y = v.Y*f,
            Z = v.Z*f
        };
        return result;
    }
    public static Vec3 operator *(float f, Vec3 v)
    {
        var result = new Vec3
        {
            X = v.X*f,
            Y = v.Y*f,
            Z = v.Z*f
        };
        return result;
    }
    public static Vec3 operator +(Vec3 v1, Vec3 v2)
    {
        var result = new Vec3
        {
            X = v1.X + v2.X,
            Y = v1.Y + v2.Y,
            Z = v1.Z + v2.Z
        };
        return result;
    }
    public static Vec3 operator -(Vec3 v1, Vec3 v2)
    {
        var result = new Vec3
        {
            X = v1.X - v2.X,
            Y = v1.Y - v2.Y,
            Z = v1.Z - v2.Z
        };
        return result;
    }
    public static Vec3 operator -(Vec3 v)
    {
        var result = new Vec3(-v.x, -v.y, -v.z);
        return result;
    }
}
public class Particle
{
    public Particle()
    {
        M = 1;
        Anchor = false;
        Broken = false;
        R = new Vec3(0, 0, 0);
        V = new Vec3(0, 0, 0);
        A = new Vec3(0, 0, 0);
        Force = new Vec3(0, 0, 0);
        G = new Vec3(0, -9.8f, 0);
        Fgravity = new Vec3(0, 0, 0);
    }
    public Particle(Vec3 position)
    {
        R = new Vec3(position);
        M = 1;
        Anchor = false;
        Broken = false;
        V = new Vec3(0, 0, 0);
        A = new Vec3(0, 0, 0);
        Force = new Vec3(0, 0, 0);
        G = new Vec3(0, -9.8f, 0);
        Fgravity = new Vec3(0, 0, 0);
    }
    public Particle(float mass)
    {
        M = mass;
        Anchor = false;
        Broken = false;
        R = new Vec3(0, 0, 0);
        V = new Vec3(0, 0, 0);
        A = new Vec3(0, 0, 0);
        Force = new Vec3(0, 0, 0);
        G = new Vec3(0, -9.8f, 0);
        Fgravity = new Vec3(0, 0, 0);
    }
    public Particle(float mass, Vec3 position)
    {
        M = mass;
        R = new Vec3(position);
        Anchor = false;
        Broken = false;
        V = new Vec3(0, 0, 0);
        A = new Vec3(0, 0, 0);
        Force = new Vec3(0, 0, 0);
        G = new Vec3(0, -9.8f, 0);
        Fgravity = new Vec3(0, 0, 0);
    }


    public float M = 10;
    public bool Anchor;
    public bool Broken = false;
    public Vec3 R;
    public Vec3 V;
    public Vec3 A;
    public Vec3 Force;
    public Vec3 Gravity = new Vec3(0, -9.8f, 0);
    public Vec3 G;
    public Vec3 Fgravity;

    public void Update(float deltaTime)
    {
        if (!Anchor)
        {
            //Calculate acceleration
            A = (1 / M) * Force;

            //Calculate velocity
            V += (A * deltaTime);

            //Calculate position
            R += (V * deltaTime);

            //Reset Force
            Force = new Vec3(0, 0, 0);
        }
    }
    public void ApplyGravity(float i)
    {
        G = Gravity * (M);
        Fgravity = G * M * i;
        AddForce(Fgravity);
    }

    public void AddForce(Vec3 force)
    {
        if (!Anchor)
            Force += force;
    }
}