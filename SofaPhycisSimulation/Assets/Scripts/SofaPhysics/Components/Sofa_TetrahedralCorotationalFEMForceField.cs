using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sofa_TetrahedralCorotationalFEMForceField : Sofa_BaseComponent {
    public string method="large";
    public double poissonRatio = 0.3;
    public double youngModulus = 3000;
    public bool computeGlobalMatrix = false;
}
