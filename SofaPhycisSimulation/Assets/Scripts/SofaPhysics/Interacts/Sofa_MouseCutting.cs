using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sofa_MouseCutting : MonoBehaviour {
    private int index = 0;
    RaycastHit raycastHit;

	void Update () {
        if(Input.GetMouseButtonDown(1))
        {
            remove();
        }
        if(Input.GetKey(KeyCode.Space))
        {
            //index += 1;
            remove1();
        }
	}
    private unsafe void remove1()
    {
        Debug.Log(index);
        SofaPhysicsAPI.RemoveTetrahedra(SofaPhysics.ToChar("TetraModifier"), index);
    }
    private unsafe void remove()
    {
        Debug.Log(index);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out raycastHit))
        {
            switch (raycastHit.transform.gameObject.name)
            {
                case "TriangleTopologyVisual":
                    break;
                case "QuadTopologyVisual":
                    break;
                case "TetrahedraTopologyVisual":
                    Debug.Log("triangle index:" + raycastHit.triangleIndex);
                    int tetrahedraIndex = Mathf.FloorToInt(raycastHit.triangleIndex / 4);
                    Debug.Log("remove:" + tetrahedraIndex);
                    SofaPhysicsAPI.RemoveTetrahedra(SofaPhysics.ToChar("TetraModifier"), tetrahedraIndex);
                    break;
                default:
                    break;
            }
        }
    }
}
