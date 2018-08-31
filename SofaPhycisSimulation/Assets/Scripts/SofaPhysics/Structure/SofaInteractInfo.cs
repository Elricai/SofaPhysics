using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public unsafe class SofaStiffSpringForceFieldInfo
{
    public SofaStiffSpringForceFieldInfo()
    {

    }
    public void Update()
    {

    }

    public string componentName;
    public string obj1;
    public string obj2;
    public double ks;
    public double kd;
    public double rayleighStiffness;
    public spring[] springs;

    public struct spring
    {
        public int index1;
        public int index2;
        public double ks;
        public double kd;
        public double L;
    }
}
public unsafe class UnityStiffSpringForceFieldInfo
{
    public UnityStiffSpringForceFieldInfo()
    {

    }
    public void Update()
    {

    }
}