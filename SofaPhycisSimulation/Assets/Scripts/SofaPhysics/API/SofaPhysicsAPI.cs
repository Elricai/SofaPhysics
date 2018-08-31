using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;

public unsafe class SofaPhysicsAPI : MonoBehaviour
{
    //模拟流程相关
    [DllImport("runSofa", EntryPoint = "HelloSofa")]
    public static extern bool HelloSofa();
    [DllImport("runSofa", EntryPoint = "Instance")]
    public static extern bool Instance();
    [DllImport("runSofa", EntryPoint = "InitSimulation")]
    public static extern bool InitSimulation();
    [DllImport("runSofa", EntryPoint = "CreateRoot")]
    public static extern bool CreateRoot(char* rootName);
    [DllImport("runSofa", EntryPoint = "StartSimulation")]
    public static extern bool StartSimulation();
    [DllImport("runSofa", EntryPoint = "IsAnimated")]
    public static extern bool IsAnimated();
    [DllImport("runSofa", EntryPoint = "SetAnimate")]
    public static extern void SetAnimate(bool boo);
    [DllImport("runSofa", EntryPoint = "GetTimeStep")]
    public static extern double GetTimeStep();
    [DllImport("runSofa", EntryPoint = "SetTimeStep")]
    public static extern void SetTimeStep(double dt);
    [DllImport("runSofa", EntryPoint = "ResetScene")]
    public static extern void ResetScene();
    [DllImport("runSofa", EntryPoint = "CleanUp")]
    public static extern void CleanUp();
    //Mesh信息相关
    [DllImport("runSofa", EntryPoint = "StepVisual")]
    public static extern void StepVisual();
    [DllImport("runSofa", EntryPoint = "GetMeshAmount")]
    public static extern int GetMeshAmount();
    [DllImport("runSofa", EntryPoint = "GetMeshName")]
    public static extern IntPtr GetMeshName(int meshID);
    [DllImport("runSofa", EntryPoint = "GetMeshTriangleAmount")]
    public static extern int GetMeshTriangleAmount(int meshID);
    [DllImport("runSofa", EntryPoint = "GetMeshTriangleTopology")]
    public static extern unsafe int* GetMeshTriangleTopology(int meshID);
    [DllImport("runSofa", EntryPoint = "GetMeshQuadAmount")]
    public static extern int GetMeshQuadAmount(int meshID);
    [DllImport("runSofa", EntryPoint = "GetMeshQuadTopology")]
    public static extern unsafe int* GetMeshQuadTopology(int meshID);
    [DllImport("runSofa", EntryPoint = "GetMeshVerticesPositions")]
    public static extern unsafe float* GetMeshVerticesPositions(int meshID);
    [DllImport("runSofa", EntryPoint = "GetMeshVerticesAmount")]
    public static extern int GetMeshVerticesAmount(int meshID);
    [DllImport("runSofa", EntryPoint = "GetMeshVerticesNormals")]
    public static extern unsafe float* GetMeshVerticesNormals(int meshID);
    //MechanicalObject信息相关
    [DllImport("runSofa", EntryPoint = "GetMechanicalObjectVerticesAmount")]
    public static extern int GetMechanicalObjectVerticesAmount(char* mechanicalObjectName);
    [DllImport("runSofa", EntryPoint = "GetMechanicalObjectVerticesPositions")]
    public static extern unsafe float* GetMechanicalObjectVerticesPositions(char* mechanicalObjectName);
    [DllImport("runSofa", EntryPoint = "GetMechanicalObjectTriangleAmount")]
    public static extern int GetMechanicalObjectTriangleAmount(char* mechanicalObjectName);
    [DllImport("runSofa", EntryPoint = "GetMechanicalObjectTriangleTopology")]
    public static extern unsafe int* GetMechanicalObjectTriangleTopology(char* mechanicalObjectName);
    [DllImport("runSofa", EntryPoint = "GetMechanicalObjectQuadAmount")]
    public static extern int GetMechanicalObjectQuadAmount(char* mechanicalObjectName);
    [DllImport("runSofa", EntryPoint = "GetMechanicalObjectQuadTopology")]
    public static extern unsafe int* GetMechanicalObjectQuadTopology(char* mechanicalObjectName);
    [DllImport("runSofa", EntryPoint = "GetMechanicalObjectTetrahedraAmount")]
    public static extern int GetMechanicalObjectTetrahedraAmount(char* mechanicalObjectName);
    [DllImport("runSofa", EntryPoint = "GetMechanicalObjectTetrahedraTopology")]
    public static extern unsafe int* GetMechanicalObjectTetrahedraTopology(char* mechanicalObjectName);
    [DllImport("runSofa", EntryPoint = "ChangeMechanicalObjectPosition")]
    public static extern void ChangeMechanicalObjectPosition(char* mechanicalObjectName,int positionID,double x,double y ,double z);
    //获取StiffSpringForceField信息相关
    [DllImport("runSofa", EntryPoint = "SpringAddToStiffSpringForceField")]
    public static extern void SpringAddToStiffSpringForceField(char* stiffSpringForceFieldName, double m1, double m2, double ks, double kd, double lengthz);
    //节点、组件操作
    //节点、组件添加
    [DllImport("runSofa", EntryPoint = "AddChildNode")]
    public static extern void AddChildNode(char* parentName, char* nodeName);
    [DllImport("runSofa", EntryPoint = "AddDefaultPipeline")]
    public static extern void AddDefaultPipeline(char* nodeName, char* componentName);
    [DllImport("runSofa", EntryPoint = "AddBruteForceDetection")]
    public static extern void AddBruteForceDetection(char* nodeName, char* componentName);
    [DllImport("runSofa", EntryPoint = "AddDefaultContactManager")]
    public static extern void AddDefaultContactManager(char* nodeName, char* componentName);
    [DllImport("runSofa", EntryPoint = "AddDiscreteIntersection")]
    public static extern void AddDiscreteIntersection(char* nodeName, char* componentName);
    [DllImport("runSofa", EntryPoint = "AddEulerImplicSolver")]
    public static extern void AddEulerImplicSolver(char* nodeName, char* componentName, double rayleighStiffness, double rayleighMass);
    [DllImport("runSofa", EntryPoint = "AddCGLinearSolver")]
    public static extern void AddCGLinearSolver(char* nodeName, char* componentName, int iterations, double tolerance, double threshold);
    [DllImport("runSofa", EntryPoint = "AddMeshGmshLoader")]
    public static extern void AddMeshGmshLoader(char* nodeName, char* componentName, char* filePath);
    [DllImport("runSofa", EntryPoint = "AddTetrahedronSetTopologyContainer")]
    public static extern void AddTetrahedronSetTopologyContainer(char* nodeName, char* componentName, char* srcPath, char* srcName);
    [DllImport("runSofa", EntryPoint = "AddMechanicalObjectArray")]
    public static extern void AddMechanicalObjectArray(char* nodeName, char* componentName,double[] positions,int size);
    [DllImport("runSofa", EntryPoint = "AddMechanicalObjectSrcPath")]
    public static extern void AddMechanicalObjectSrcPath(char* nodeName, char* componentName, char* srcPath, char* srcName);
    [DllImport("runSofa", EntryPoint = "AddTetrahedronSetGeometryAlgorithms")]
    public static extern void AddTetrahedronSetGeometryAlgorithms(char* nodeName, char* componentName);
    [DllImport("runSofa", EntryPoint = "AddDiagonalMass")]
    public static extern void AddDiagonalMass(char* nodeName, char* componentName, double massDensity);
    [DllImport("runSofa", EntryPoint = "AddTetrahedralCorotationalFEMForceField")]
    public static extern void AddTetrahedralCorotationalFEMForceField(char* nodeName, char* componentName, char* method, double poissonRatio, double youngModulus, bool computeGlobalMatrix);
    [DllImport("runSofa", EntryPoint = "AddFixedConstraint")]
    public static extern void AddFixedConstraint(char* nodeName, char* componentName, int[] indices, int indiceAmount);
    [DllImport("runSofa", EntryPoint = "AddOglModel")]
    public static extern void AddOglModel(char* nodeName, char* componentName, char* fileMesh);
    [DllImport("runSofa", EntryPoint = "AddBarycentricMapping")]
    public static extern void AddBarycentricMapping(char* nodeName, char* componentName, char* input, char* output);
    [DllImport("runSofa", EntryPoint = "AddStiffSpringForceField")]
    public static extern void AddStiffSpringForceField(char* nodeName, char* componentName, char* obj1, char* obj2, double ks, double kd, double rayleighStiffness);
    [DllImport("runSofa", EntryPoint = "AddTetrahedronSetTopologyModifier")]
    public static extern void AddTetrahedronSetTopologyModifier(char* nodeName, char* componentName);
    [DllImport("runSofa", EntryPoint = "AddTetrahedronSetTopologyAlgorithms")]
    public static extern void AddTetrahedronSetTopologyAlgorithms(char* nodeName, char* componentName);
    //节点、组件移除
    [DllImport("runSofa", EntryPoint = "RemoveMechanicalObject")]
    public static extern void RemoveMechanicalObject(char* nodeName, char* componentName);
    [DllImport("runSofa", EntryPoint = "RemoveStiffSpringForceField")]
    public static extern void RemoveStiffSpringForceField(char* nodeName, char* componentName);
    //鼠标拖拽功能
    [DllImport("runSofa", EntryPoint = "MouseDragInitial")]
    public static extern void MouseDragInitial();
    [DllImport("runSofa", EntryPoint = "MouseDragButtonDown")]
    public static extern void MouseDragButtonDown(int verticesID);
    [DllImport("runSofa", EntryPoint = "MouseDragMoveUpdate")]
    public static extern void MouseDragMoveUpdate(int verticesID, double dx, double dy, double dz);
    [DllImport("runSofa", EntryPoint = "MouseDragButtonUp")]
    public static extern void MouseDragButtonUp();
        //节点、组件移除
    [DllImport("runSofa", EntryPoint = "MouseDragCleanUp")]
    public static extern void MouseDragCleanUp();
    //Debug相关的delegate和方法注册
    private delegate void DebugCallback(string message);
    [DllImport("runSofa", EntryPoint = "RegisterDebugCallback")]
    private static extern bool RegisterDebugCallback(DebugCallback callback);

    public static void RegisterDebugCallBack()
    {
        RegisterDebugCallback(new DebugCallback(DebugMethod));
    }
    private static void DebugMethod(string message)
    {
        Debug.Log("Sofa:" + message);
    }

    //测试切割功能
    [DllImport("runSofa", EntryPoint = "RemoveTetrahedra")]
    public static extern bool RemoveTetrahedra(char* tetrahedronModifierName, int index);
}
