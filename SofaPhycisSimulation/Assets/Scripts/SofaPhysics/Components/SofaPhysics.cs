using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
using System.Threading;

public unsafe class SofaPhysics : MonoBehaviour {
    public static bool isSofaPhysicsInitialed = false;
    //mesh、mechanicalobject可视化结构体字典
    public static Dictionary<string, SofaMeshInfo> sofaMeshInfoDic;
    public static Dictionary<string, SofaMechanicalObjectInfo> sofaMechanicalObjectInfoDic;
    public static Dictionary<string, SofaStiffSpringForceFieldInfo> sofaStiffSpringForceFieldInfoDic;
    public static Dictionary<string, UnityMeshInfo> unityMeshInfoDic;
    public static Dictionary<string, UnityMechanicalObjectInfo> unityMechanicalObjectInfoDic;
    //其他组件字典
    public static Dictionary<string, UnityStiffSpringForceFieldInfo> unityStiffSpringForceFieldInfoDic;
    //节点遍历相关参数
    public static Transform[] nodes;
    //Sofa模拟进程相关参数
    private bool isSimulationInited = false;
    private bool isSimulationStarted = false;
    private bool isSofaInfoInited = false;

    private void Awake()
    {
        //注册DebugInUnity回调函数
        SofaPhysicsAPI.RegisterDebugCallBack();
        //注册Unity信息类、Sofa信息类字典
        VisualInfoDictionaryInit();
        //Sofa模拟初始化
        SofaSimulationInit();
        //遍历root下的所有节点和组件，并初始化Unity信息类
        SofaErgodicNodes();
    }

    void Update()
    {
        //初始化完成后开始模拟
        if(isSimulationInited == true && isSimulationStarted == false)
        {
            SofaSimulationStart();
            isSimulationStarted = true;
        }
        //初始化Sofa信息类
        if(isSimulationStarted == true && isSofaInfoInited == false)
        {
            SofaSimulationStepVisual();
            SofaMechanicalObjectInfoInit();
            SofaMeshInfoInit();
            isSofaPhysicsInitialed = true;
            isSofaInfoInited = true;
        }
        //更新Sofa信息类并同步到Unity信息类中
        if (isSofaInfoInited == true)
        {
            SofaSimulationStepVisual();
            MechanicalObjectInfoUpdate();
            MeshInfoUpdate();
        }
        //重置场景
        if(Input.GetKeyDown(KeyCode.R))
        {
            SofaSimulationResetScene();
        }
    }
    /// <summary>
    /// 注册Unity信息类、Sofa信息类字典
    /// </summary>
    private void VisualInfoDictionaryInit()
    {
        sofaMeshInfoDic = new Dictionary<string, SofaMeshInfo>();
        sofaMechanicalObjectInfoDic = new Dictionary<string, SofaMechanicalObjectInfo>();
        unityMeshInfoDic = new Dictionary<string, UnityMeshInfo>();
        unityMechanicalObjectInfoDic = new Dictionary<string, UnityMechanicalObjectInfo>();
    }
    /// <summary>
    /// Sofa模拟初始化，包括开启模拟、创建根节点、添加节点、组件、控制模拟进行等
    /// </summary>
    private unsafe void SofaSimulationInit()
    {
        //初始化Sofa、开启模拟、创建根节点
        SofaPhysicsAPI.HelloSofa();
        SofaPhysicsAPI.Instance();
        SofaPhysicsAPI.InitSimulation();
        SofaPhysicsAPI.CreateRoot(ToChar("root"));

        //初始化鼠标拖拽功能(需要在Sofa模拟开启后进行)，第二次初始化时unity崩溃，需要修改CleanUp方法，释放所有动态生成的节点和组件
        //SofaPhysicsAPI.MouseDragInitial();
        //isMouseDragInitialed = true;
        
    }
    /// <summary>
    /// 遍历root下的所有节点
    /// </summary>
    private void SofaErgodicNodes()
    {
        nodes = GetComponentsInChildren<Transform>();
        foreach(Transform node in nodes)
        {
            SofaErgodicComponents(node);
        }
        isSimulationInited = true;
    }
    /// <summary>
    /// 根据每个节点的组件信息调用SofaPhysicsAPI，对于mechanicalObject和olgModel组件，动态创建unity相关渲染组件
    /// </summary>
    /// <param name="node"></param>
    private void SofaErgodicComponents(Transform node)
    {
        if (node.name != "root")
        {
            SofaPhysicsAPI.AddChildNode(ToChar(node.parent.name), ToChar(node.name));
        }
        Component[] allComponents;
        allComponents = node.gameObject.GetComponents<Component>();
        foreach(Component com in allComponents)
        {
            switch (com.GetType().Name)
            {
                case "Sofa_DefaultPipeline":
                    Sofa_DefaultPipeline defaultPipeline = node.gameObject.GetComponent<Sofa_DefaultPipeline>();
                    SofaPhysicsAPI.AddDefaultPipeline(ToChar(node.name),ToChar(defaultPipeline.componentName));
                    break;
                case "Sofa_BruteForceDetection":
                    Sofa_BruteForceDetection bruteForceDetection = node.gameObject.GetComponent<Sofa_BruteForceDetection>();
                    SofaPhysicsAPI.AddBruteForceDetection(ToChar(node.name), ToChar(bruteForceDetection.componentName));
                    break;
                case "Sofa_DefaultContactManager":
                    Sofa_DefaultContactManager defaultContactManager = node.gameObject.GetComponent<Sofa_DefaultContactManager>();
                    SofaPhysicsAPI.AddDefaultContactManager(ToChar(node.name), ToChar(defaultContactManager.componentName));
                    break;
                case "Sofa_DiscreteIntersection":
                    Sofa_DiscreteIntersection discreteIntersection = node.gameObject.GetComponent<Sofa_DiscreteIntersection>();
                    SofaPhysicsAPI.AddDiscreteIntersection(ToChar(node.name), ToChar(discreteIntersection.componentName));
                    break;
                case "Sofa_EulerImplicSolver":
                    Sofa_EulerImplicSolver eulerImplicSolver = node.gameObject.GetComponent<Sofa_EulerImplicSolver>();
                    SofaPhysicsAPI.AddEulerImplicSolver(ToChar(node.name), ToChar(eulerImplicSolver.componentName),eulerImplicSolver.rayleighStiffness,eulerImplicSolver.rayleighMass);
                    break;
                case "Sofa_CGLinearSolver":
                    Sofa_CGLinearSolver cGLinearSolver = node.gameObject.GetComponent<Sofa_CGLinearSolver>();
                    SofaPhysicsAPI.AddCGLinearSolver(ToChar(node.name), ToChar(cGLinearSolver.componentName),cGLinearSolver.iterations,cGLinearSolver.tolerance,cGLinearSolver.threshold);
                    break;
                case "Sofa_MeshGmshLoader":
                    Sofa_MeshGmshLoader meshGmshLoader = node.gameObject.GetComponent<Sofa_MeshGmshLoader>(); 
                    SofaPhysicsAPI.AddMeshGmshLoader(ToChar(node.name), ToChar(meshGmshLoader.componentName),ToChar(meshGmshLoader.filePath));
                    break;
                case "Sofa_TetrahedronSetTopologyContainer":
                    Sofa_TetrahedronSetTopologyContainer tetrahedronSetTopologyContainer = node.gameObject.GetComponent<Sofa_TetrahedronSetTopologyContainer>();
                    SofaPhysicsAPI.AddTetrahedronSetTopologyContainer(ToChar(node.name), ToChar(tetrahedronSetTopologyContainer.componentName),ToChar(tetrahedronSetTopologyContainer.srcPath),ToChar(tetrahedronSetTopologyContainer.srcName));
                    break;
                case "Sofa_MechanicalObject":
                    Sofa_MechanicalObject mechanicalObject = node.gameObject.GetComponent<Sofa_MechanicalObject>();
                    SofaPhysicsAPI.AddMechanicalObjectSrcPath(ToChar(node.name), ToChar(mechanicalObject.componentName),ToChar(mechanicalObject.srcPath),ToChar(mechanicalObject.srcName));
                    UnityMechanicalObjectInfoInit(node, mechanicalObject.componentName,mechanicalObject.triangleMat,mechanicalObject.quadMat,mechanicalObject.tetrahedraMat);
                    break;
                case "Sofa_TetrahedronSetGeometryAlgorithms":
                    Sofa_TetrahedronSetGeometryAlgorithms tetrahedronSetGeometryAlgorithms = node.gameObject.GetComponent<Sofa_TetrahedronSetGeometryAlgorithms>();
                    SofaPhysicsAPI.AddTetrahedronSetGeometryAlgorithms(ToChar(node.name), ToChar(tetrahedronSetGeometryAlgorithms.componentName));
                    break;
                case "Sofa_TetrahedronSetTopologyModifier":
                    Sofa_TetrahedronSetTopologyModifier tetrahedronSetTopologyModifier = node.gameObject.GetComponent<Sofa_TetrahedronSetTopologyModifier>();
                    SofaPhysicsAPI.AddTetrahedronSetTopologyModifier(ToChar(node.name), ToChar(tetrahedronSetTopologyModifier.componentName));
                    break;
                case "Sofa_TetrahedronSetTopologyAlgorithms":
                    Sofa_TetrahedronSetTopologyAlgorithms tetrahedronSetTopologyAlgorithms = node.gameObject.GetComponent<Sofa_TetrahedronSetTopologyAlgorithms>();
                    SofaPhysicsAPI.AddTetrahedronSetTopologyAlgorithms(ToChar(node.name), ToChar(tetrahedronSetTopologyAlgorithms.componentName));
                    break;
                case "Sofa_DiagonalMass":
                    Sofa_DiagonalMass diagonalMass = node.gameObject.GetComponent<Sofa_DiagonalMass>();
                    SofaPhysicsAPI.AddDiagonalMass(ToChar(node.name), ToChar(diagonalMass.componentName),diagonalMass.massDensity);
                    break;
                case "Sofa_TetrahedralCorotationalFEMForceField":
                    Sofa_TetrahedralCorotationalFEMForceField tetrahedralCorotationalFEMForceField = node.gameObject.GetComponent<Sofa_TetrahedralCorotationalFEMForceField>();
                    SofaPhysicsAPI.AddTetrahedralCorotationalFEMForceField(ToChar(node.name), ToChar(tetrahedralCorotationalFEMForceField.componentName),ToChar(tetrahedralCorotationalFEMForceField.method), tetrahedralCorotationalFEMForceField.poissonRatio, tetrahedralCorotationalFEMForceField.youngModulus, tetrahedralCorotationalFEMForceField.computeGlobalMatrix);
                    break;
                case "Sofa_FixedConstraint":
                    Sofa_FixedConstraint fixedConstraint = node.gameObject.GetComponent<Sofa_FixedConstraint>();
                    SofaPhysicsAPI.AddFixedConstraint(ToChar(node.name), ToChar(fixedConstraint.componentName),fixedConstraint.indices,fixedConstraint.indices.Length);
                    break;
                case "Sofa_OglModel":
                    Sofa_OglModel oglModel = node.gameObject.GetComponent<Sofa_OglModel>();
                    SofaPhysicsAPI.AddOglModel(ToChar(node.name), ToChar(oglModel.componentName),ToChar(oglModel.fileMesh));
                    UnityMeshInfoInit(node,oglModel.componentName,oglModel.mat);
                    break;
                case "Sofa_BarycentricMapping":
                    Sofa_BarycentricMapping baryCentricMapping = node.gameObject.GetComponent<Sofa_BarycentricMapping>();
                    SofaPhysicsAPI.AddBarycentricMapping(ToChar(node.name), ToChar(baryCentricMapping.componentName),ToChar(baryCentricMapping.input),ToChar(baryCentricMapping.output));
                    break;
                default:
                    break;
            }
        }
    }
    /// <summary>
    /// 初始化UnityMechanicalObject渲染相关组件
    /// </summary>
    /// <param name="node"></param>
    /// <param name="componentName"></param>
    /// <param name="mat"></param>
    private void UnityMechanicalObjectInfoInit(Transform node,string componentName,Material triangleMat,Material quadMat,Material tetrahedraMat)
    {
        
        UnityMechanicalObjectInfo unityMechanicalObjectInfo = new UnityMechanicalObjectInfo(node, componentName, triangleMat, quadMat,tetrahedraMat);
        unityMechanicalObjectInfoDic.Add(unityMechanicalObjectInfo.componentName, unityMechanicalObjectInfo);
    }
    /// <summary>
    /// 初始化UnityMesh渲染相关组件
    /// </summary>
    /// <param name="node"></param>
    /// <param name="componentName"></param>
    /// <param name="mat"></param>
    private void UnityMeshInfoInit(Transform node,string componentName,Material mat)
    {
        UnityMeshInfo unityMeshInfo = new UnityMeshInfo(node,componentName,mat);
        unityMeshInfoDic.Add(unityMeshInfo.componentName, unityMeshInfo);
    }
    /// <summary>
    /// 初始化SofaMechanicalObject信息
    /// </summary>
    private void SofaMechanicalObjectInfoInit()
    {
        foreach(string componentName in unityMechanicalObjectInfoDic.Keys)
        {
            SofaMechanicalObjectInfo sofaMechanicalObjectInfo = new SofaMechanicalObjectInfo(componentName);
            sofaMechanicalObjectInfoDic.Add(sofaMechanicalObjectInfo.componentName, sofaMechanicalObjectInfo);
        }
    }
    /// <summary>
    /// 初始化SofaMesh信息
    /// </summary>
    private void SofaMeshInfoInit()
    {
        for (int i =0; i<SofaPhysicsAPI.GetMeshAmount();i++)
        {
            SofaMeshInfo sofaMeshInfo = new SofaMeshInfo(i);
            sofaMeshInfoDic.Add(sofaMeshInfo.componentName,sofaMeshInfo);
        }
    }
    /// <summary>
    /// 控制模拟开启
    /// </summary>
    private void SofaSimulationStart()
    {
        
        SofaPhysicsAPI.StartSimulation();
        SofaPhysicsAPI.SetAnimate(false);
        SofaPhysicsAPI.ResetScene();
        SofaPhysicsAPI.StepVisual();
        SofaPhysicsAPI.SetAnimate(true);
    }
    /// <summary>
    /// 在API端更新Mesh信息
    /// </summary>
    private void SofaSimulationStepVisual()
    {
        SofaPhysicsAPI.StepVisual();
    }
    /// <summary>
    /// 重新加载场景
    /// </summary>
    private void SofaSimulationResetScene()
    {
        SofaPhysicsAPI.ResetScene();
    }
    /// <summary>
    /// 更新、获取SofaMechanicalObject信息，同步到UnityMechanicalObject信息中
    /// </summary>
    private void MechanicalObjectInfoUpdate()
    {
        foreach(SofaMechanicalObjectInfo sofaMechanicalObjectInfo in sofaMechanicalObjectInfoDic.Values)
        {
            sofaMechanicalObjectInfo.Update();

            foreach(UnityMechanicalObjectInfo unityMechanicalObjectInfo in unityMechanicalObjectInfoDic.Values)
            {
                if (unityMechanicalObjectInfo.componentName == sofaMechanicalObjectInfo.componentName)
                {
                    unityMechanicalObjectInfo.Update(sofaMechanicalObjectInfo);
                }
            }
        }
    }
    /// <summary>
    /// 更新、获取SofaMesh信息，同步到UnityMesh信息中
    /// </summary>
    private void MeshInfoUpdate()
    {
        foreach (SofaMeshInfo sofaMeshInfo in sofaMeshInfoDic.Values)
        {
            sofaMeshInfo.Update();
            foreach(UnityMeshInfo unityMeshInfo in unityMeshInfoDic.Values)
            {
                if(unityMeshInfo.componentName == sofaMeshInfo.componentName)
                {
                    unityMeshInfo.Update(sofaMeshInfo);
                }
            }
        }
    }
    /// <summary>
    /// 将string类型变量转换为char*类型变量
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static unsafe char* ToChar(string str)
    {
        char* p = (char*)System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(str).ToPointer();
        return p;
    }
    /// <summary>
    /// 将IntPtr(char*)类型变量转换为string类型变量
    /// </summary>
    /// <param name="ch"></param>
    /// <returns></returns>
    public static string ToStr(IntPtr ch)
    {
        return Marshal.PtrToStringAnsi(ch);
    }
    /// <summary>
    /// 将Double(16字节)类型变量转换为IntPtr类型变量
    /// </summary>
    /// <param name="double16"></param>
    /// <returns></returns>
    public static IntPtr DoubleArrayToIntPtr(double[] myDoubleArray)
    {
        IntPtr dstDoubleArrayPtr = Marshal.AllocCoTaskMem(myDoubleArray.Length * Marshal.SizeOf(typeof(double)));
        Marshal.Copy(myDoubleArray, 0, dstDoubleArrayPtr, myDoubleArray.Length);
        return dstDoubleArrayPtr;
    }
    /// <summary>
    /// 停止时对sofa模拟进行cleanup
    /// </summary>
    private void OnDestroy()
    {
        //isSofaWorking = false;
        SofaPhysicsAPI.CleanUp();
    }
    /// <summary>
    /// 停止时对sofa模拟进行cleanup
    /// </summary>
    private void OnDisable()
    {
        //isSofaWorking = false;
        SofaPhysicsAPI.CleanUp();
    }
    /// <summary>
    /// 停止时对sofa模拟进行cleanup
    /// </summary>
    private void OnApplicationQuit()
    {
       SofaPhysicsAPI.CleanUp();
    }
}