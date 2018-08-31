using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sofa_MechanicalObject : Sofa_BaseComponent {
    public string srcPath;
    public string srcName;
    public Material triangleMat;
    public Material quadMat;
    public Material tetrahedraMat;
    public bool isRenderingTriangle = false;
    public bool isRenderingQuad = false;
    public bool isRenderingTetrahedra = false;
    public bool isTriangleCastingRay = true;
    public bool isQuadCastingRay = true;
    public bool isTetrahedraCastingRay = true;
}