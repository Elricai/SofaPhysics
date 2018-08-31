using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sofa_MouseDragging : MonoBehaviour {
    public string mechanicalObjectName = "MouseDragMechanicalObject";
    public string stiffSpringForceFieldName = "MouseDragStiffSpringForceField";
    public double dragSpringKs = 1000;
    public double dragSpringKd = 0.1;
    public Camera mainCamera;
    public bool showGizmos = true;
    //当前拖拽mechanicalObject相关信息
    private string draggingMechanicalObjectName;
    private int draggingMechanicalObjectPointIndex = -1;
    private float miniumDis = -1;
    private Vector3 draggingMechanicalObjectPointPosition = Vector3.zero;
    //鼠标位置相对拖拽点的映射位置
    private Vector3 draggingMousePosition = Vector3.zero;
    //开启拖拽gizmos的flag
    private bool drawGizmos=false;

	void Update () {
        if(SofaPhysics.isSofaPhysicsInitialed == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GetNearestDraggingPoint();
                MouseDragUpdate();
                MouseDragAdd();
            }
            if(Input.GetMouseButton(0))
            {
                MouseDragUpdate();
                drawGizmos = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                drawGizmos = false;
                MouseDragRemove();
            }
        }  
    }
    /// <summary>
    /// 动态添加鼠标的MehcanicalObject组件至root节点，生成其信息。添加StiffSpringForceField组件，连接目标物体和鼠标MechanicalObject
    /// </summary>
    private unsafe void MouseDragAdd()
    {
        if(!SofaPhysics.unityMechanicalObjectInfoDic.ContainsKey(mechanicalObjectName) &&!SofaPhysics.sofaMechanicalObjectInfoDic.ContainsKey(mechanicalObjectName))
        {
            //加入mechanicalObject组件及相关info
            double[] dou = new double[3] { draggingMousePosition.x, draggingMousePosition.y, draggingMousePosition.z };
            SofaPhysicsAPI.AddMechanicalObjectArray(SofaPhysics.ToChar("root"), SofaPhysics.ToChar(mechanicalObjectName), dou, 1);
            for (int i = 0; i < SofaPhysics.nodes.Length; i++)
            {
                if (SofaPhysics.nodes[i].gameObject.name == "root")
                {
                    UnityMechanicalObjectInfo unityMechanicalObjectInfo = new UnityMechanicalObjectInfo(SofaPhysics.nodes[i], mechanicalObjectName);
                    SofaPhysics.unityMechanicalObjectInfoDic.Add(unityMechanicalObjectInfo.componentName, unityMechanicalObjectInfo);
                }
            }
            foreach (string componentName in SofaPhysics.unityMechanicalObjectInfoDic.Keys)
            {
                if (mechanicalObjectName == componentName)
                {
                    SofaMechanicalObjectInfo sofaMechanicalObjectInfo = new SofaMechanicalObjectInfo(componentName);
                    SofaPhysics.sofaMechanicalObjectInfoDic.Add(sofaMechanicalObjectInfo.componentName, sofaMechanicalObjectInfo);
                }
            }
            //加入stiffSpringForceField组件及其Spring
            SofaPhysicsAPI.AddStiffSpringForceField(SofaPhysics.ToChar("root"), SofaPhysics.ToChar(stiffSpringForceFieldName), SofaPhysics.ToChar("@./"), SofaPhysics.ToChar(GetSrcPathToRoot()), dragSpringKs, 0.1, 1);
            SofaPhysicsAPI.SpringAddToStiffSpringForceField(SofaPhysics.ToChar(stiffSpringForceFieldName), 0, draggingMechanicalObjectPointIndex, dragSpringKs, dragSpringKd, 0);
        }
    }
    /// <summary>
    /// 更新鼠标位置、并根据鼠标位置更新其MechaniclObject位置
    /// </summary>
    private unsafe void MouseDragUpdate()
    {
        MousePositionUpdate();
        MouseMechanicalObjectInfoChange();
        MouseMechanicalObjectInfoUpdate();
    }
    /// <summary>
    /// 清除鼠标MechanicalObject、StiffSpringForceField组件及其字典中储存的信息，将临时变量归零
    /// </summary>
    private unsafe void MouseDragRemove()
    {
        //移除StiffSpringForceFiled组件
        SofaPhysicsAPI.RemoveStiffSpringForceField(SofaPhysics.ToChar("root"), SofaPhysics.ToChar(stiffSpringForceFieldName));
        //移除MechanicalObject组件及字典中储存的信息
        SofaPhysicsAPI.RemoveMechanicalObject(SofaPhysics.ToChar("root"), SofaPhysics.ToChar(mechanicalObjectName));
        SofaPhysics.unityMechanicalObjectInfoDic.Remove(mechanicalObjectName);
        SofaPhysics.sofaMechanicalObjectInfoDic.Remove(mechanicalObjectName);
    }
    /// <summary>
    /// 获取拖拽点，即所有SofaMechanicalObject信息中，离鼠标点击屏幕位置最近的点
    /// </summary>
    private unsafe void GetNearestDraggingPoint()
    {
        draggingMechanicalObjectPointIndex = -1;
        miniumDis = -1;
        draggingMechanicalObjectName = "";
        foreach (SofaMechanicalObjectInfo sofaMechanicalObjectInfo in SofaPhysics.sofaMechanicalObjectInfoDic.Values)
        {
            if (sofaMechanicalObjectInfo.componentName != mechanicalObjectName)
            {
                for (int i = 0; i < sofaMechanicalObjectInfo.verticesAmount; i++)
                {
                    Vector3 mechanicalObjectPosition = new Vector3();
                    mechanicalObjectPosition.x = sofaMechanicalObjectInfo.verticesPosition[i * 3 + 0];
                    mechanicalObjectPosition.y = sofaMechanicalObjectInfo.verticesPosition[i * 3 + 1];
                    mechanicalObjectPosition.z = sofaMechanicalObjectInfo.verticesPosition[i * 3 + 2];
                    float dis = Vector3.Distance(Input.mousePosition, mainCamera.WorldToScreenPoint(mechanicalObjectPosition));
                    if (miniumDis == -1 || dis < miniumDis)
                    {
                        miniumDis = dis;
                        draggingMechanicalObjectPointIndex = i;
                        draggingMechanicalObjectName = sofaMechanicalObjectInfo.componentName;
                        draggingMechanicalObjectPointPosition = mechanicalObjectPosition;
                    }
                }
            }
        }
    }
    /// <summary>
    /// 更新鼠标位置
    /// </summary>
    private void MousePositionUpdate()
    {
        foreach (UnityMechanicalObjectInfo unityMechanicalObjectInfo in SofaPhysics.unityMechanicalObjectInfoDic.Values)
        {
            if (draggingMechanicalObjectName == unityMechanicalObjectInfo.componentName)
            {
                draggingMechanicalObjectPointPosition = unityMechanicalObjectInfo.verticesPosition[draggingMechanicalObjectPointIndex];
            }
        }
        //鼠标的Z轴深度与拖拽点到屏幕的深度相同
        Plane camPlane = new Plane(mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0)), new Vector3(100, 0, 0), new Vector3(0, 100, 0));
        float distanceZ = camPlane.GetDistanceToPoint(draggingMechanicalObjectPointPosition);
        draggingMousePosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceZ));
    }
    /// <summary>
    /// 更改鼠标MehcanicalObject信息
    /// </summary>
    private unsafe void MouseMechanicalObjectInfoChange()
    {
        SofaPhysicsAPI.ChangeMechanicalObjectPosition(SofaPhysics.ToChar(mechanicalObjectName), 0, draggingMousePosition.x, draggingMousePosition.y, draggingMousePosition.z);
    }
    /// <summary>
    /// 更新鼠标MechanicalObject信息
    /// </summary>
    private void MouseMechanicalObjectInfoUpdate()
    {
        foreach(SofaMechanicalObjectInfo sofaMechanicalObjectInfo in SofaPhysics.sofaMechanicalObjectInfoDic.Values)
        {
            if(mechanicalObjectName == sofaMechanicalObjectInfo.componentName)
            {
                sofaMechanicalObjectInfo.Update();
            }
        }
    }
    /// <summary>
    /// 获得拖拽的物体节点到Root的路径
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    private string GetSrcPathToRoot()
    {
        Transform node = null;
        foreach(UnityMechanicalObjectInfo mechanicalObject in SofaPhysics.unityMechanicalObjectInfoDic.Values)
        {
            if(mechanicalObject.componentName == draggingMechanicalObjectName)
            {
                node = mechanicalObject.node;
            }
        }
        if(node != null)
        {
            for (int i = 0; i < SofaPhysics.nodes.Length; i++)
            {
                if (SofaPhysics.nodes[i].name == draggingMechanicalObjectName)
                {
                    node = SofaPhysics.nodes[i];
                }
            }
            string path = "";
            if (node.parent == null)
            {
                path = "./";
            }
            else
            {
                while (node.parent != null)
                {
                    if (node.parent.name == "root")
                    {
                        path = node.name + path;
                        node = node.parent;
                    }
                    else
                    {
                        path = "/" + node.name + path;
                        node = node.parent;
                    }
                }
            }
            path = "@" + path;
            return path;
        }
        else
        {
            return "";
        }
    }
    private void OnDrawGizmos()
    {
        if(drawGizmos ==true&&showGizmos==true)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(draggingMechanicalObjectPointPosition, 0.1f);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(draggingMousePosition, 0.1f);
        }
    }
}
