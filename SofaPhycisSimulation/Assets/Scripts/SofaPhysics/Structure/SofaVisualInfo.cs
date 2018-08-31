using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public unsafe class SofaMeshInfo
{
    public SofaMeshInfo(int i)
    {
        meshIndex = i;
        Update();
    }
    public void Update()
    {
        componentName = SofaPhysics.ToStr(SofaPhysicsAPI.GetMeshName(meshIndex));
        verticesAmount = SofaPhysicsAPI.GetMeshVerticesAmount(meshIndex);
        verticesPosition = SofaPhysicsAPI.GetMeshVerticesPositions(meshIndex);
        triangleAmount = SofaPhysicsAPI.GetMeshTriangleAmount(meshIndex);
        triangleTopology = SofaPhysicsAPI.GetMeshTriangleTopology(meshIndex);
        quadAmount = SofaPhysicsAPI.GetMeshQuadAmount(meshIndex);
        quadTopology = SofaPhysicsAPI.GetMeshQuadTopology(meshIndex);
    }

    public int meshIndex;
    public string componentName;

    public int verticesAmount;
    public float* verticesPosition;
    public int lineAmount;
    public int* lineTopology;
    public int triangleAmount;
    public int* triangleTopology;
    public int quadAmount;
    public int* quadTopology;

    public float* normalInfo;
    public float* uVInfo;
}
public unsafe class SofaMechanicalObjectInfo
{
    public SofaMechanicalObjectInfo(string componentName)
    {
        this.componentName = componentName;
        Update();
    }
    public void Update()
    {
        verticesAmount = SofaPhysicsAPI.GetMechanicalObjectVerticesAmount(SofaPhysics.ToChar(componentName));
        verticesPosition = SofaPhysicsAPI.GetMechanicalObjectVerticesPositions(SofaPhysics.ToChar(componentName));
        triangleAmount = SofaPhysicsAPI.GetMechanicalObjectTriangleAmount(SofaPhysics.ToChar(componentName));
        triangleTopology = SofaPhysicsAPI.GetMechanicalObjectTriangleTopology(SofaPhysics.ToChar(componentName));
        quadAmount = SofaPhysicsAPI.GetMechanicalObjectQuadAmount(SofaPhysics.ToChar(componentName));
        quadTopology = SofaPhysicsAPI.GetMechanicalObjectQuadTopology(SofaPhysics.ToChar(componentName));
        tetrahedraAmount = SofaPhysicsAPI.GetMechanicalObjectTetrahedraAmount(SofaPhysics.ToChar(componentName));
        tetrahedraTopology = SofaPhysicsAPI.GetMechanicalObjectTetrahedraTopology(SofaPhysics.ToChar(componentName));
    }

    public string componentName;
    public int verticesAmount;
    public float* verticesPosition;
    public int lineAmount;
    public int* lineTopology;
    public int triangleAmount=0;
    public int* triangleTopology;
    public int quadAmount;
    public int* quadTopology;
    public int tetrahedraAmount;
    public int* tetrahedraTopology;
}
public unsafe class UnityMeshInfo
{
    public UnityMeshInfo(Transform node, string componentName, Material mat)
    {
        this.node = node;
        this.componentName = componentName;
        this.mat = mat;
        dynamicMesh = new Mesh();
        meshFilter = node.gameObject.AddComponent<MeshFilter>();
        meshRenderer = node.gameObject.AddComponent<MeshRenderer>();
        meshCollider = node.gameObject.AddComponent<MeshCollider>();
        meshFilter.mesh = dynamicMesh;
        meshRenderer.material = this.mat;
        meshCollider.sharedMesh = dynamicMesh;
    }
    public void Update(SofaMeshInfo sofaMeshInfo)
    {
        if(componentName==sofaMeshInfo.componentName)
        {
            verticesPosition = new Vector3[sofaMeshInfo.verticesAmount];
            for (int i = 0; i < sofaMeshInfo.verticesAmount; i++)
            {
                verticesPosition[i].x = sofaMeshInfo.verticesPosition[i * 3 + 0];
                verticesPosition[i].y = sofaMeshInfo.verticesPosition[i * 3 + 1];
                verticesPosition[i].z = sofaMeshInfo.verticesPosition[i * 3 + 2];
            }
            if (sofaMeshInfo.triangleAmount != 0)
            {
                triangleTopology = new int[sofaMeshInfo.triangleAmount * 3];

                for (int i = 0; i < sofaMeshInfo.triangleAmount; i++)
                {
                    //1triangletopology = 3 vertices
                    //1triangletopology = 1 triangles
                    //由topology生成vertice！
                    triangleTopology[i * 3 + 0] = sofaMeshInfo.triangleTopology[i * 3 + 0];
                    triangleTopology[i * 3 + 1] = sofaMeshInfo.triangleTopology[i * 3 + 1];
                    triangleTopology[i * 3 + 2] = sofaMeshInfo.triangleTopology[i * 3 + 2];
                }
            }
            else
            {
                triangleTopology = new int[sofaMeshInfo.quadAmount * 6];
                for (int i = 0; i < sofaMeshInfo.quadAmount; i++)
                {
                    //1quadtopology = 6 vertices
                    //1quadtopology = 2 triangles
                    //由topology生成vertice！
                    triangleTopology[i * 6 + 0] = sofaMeshInfo.quadTopology[i * 4 + 0];
                    triangleTopology[i * 6 + 1] = sofaMeshInfo.quadTopology[i * 4 + 1];
                    triangleTopology[i * 6 + 2] = sofaMeshInfo.quadTopology[i * 4 + 2];
                    triangleTopology[i * 6 + 3] = sofaMeshInfo.quadTopology[i * 4 + 2];
                    triangleTopology[i * 6 + 4] = sofaMeshInfo.quadTopology[i * 4 + 3];
                    triangleTopology[i * 6 + 5] = sofaMeshInfo.quadTopology[i * 4 + 0];
                }
            }

            dynamicMesh.vertices = verticesPosition;
            dynamicMesh.triangles = triangleTopology;
            dynamicMesh.RecalculateNormals();
            meshCollider.sharedMesh = dynamicMesh;

            meshRenderer.enabled = node.GetComponent<Sofa_OglModel>().isRenderingObject;
            meshCollider.enabled = node.GetComponent<Sofa_OglModel>().isCastingRay;
        }
        else
        {
            Debug.Log("Unity Mesh Info Update Failed : Soaf Mesh Info Name Not Match.");
        }
    }

    public Transform node;
    public string componentName;
    public Vector3[] verticesPosition;
    public int[] triangleTopology;
    public Vector3[] normalInfo;
    public Vector2[] uVInfo;

    public Material mat;
    public Mesh dynamicMesh;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    public MeshCollider meshCollider;
}
public unsafe class UnityMechanicalObjectInfo
{
    public UnityMechanicalObjectInfo(Transform node,string componentName)
    {
        this.node = node;
        this.componentName = componentName;
    }
    public UnityMechanicalObjectInfo(Transform node, string componentName, Material triangleMat, Material quadMat, Material tetrahedraMat)
    {
        this.node = node;
        this.componentName = componentName;

        GameObject triangleTopologyVisual = new GameObject("TriangleTopologyVisual");
        triangleTopologyVisual.transform.SetParent(node);
        this.triangleMat = triangleMat;
        triangleDynamicMesh = new Mesh();
        triangleMeshFilter = triangleTopologyVisual.AddComponent<MeshFilter>();
        triangleMeshRenderer = triangleTopologyVisual.AddComponent<MeshRenderer>();
        triangleMeshCollider = triangleTopologyVisual.AddComponent<MeshCollider>();
        triangleMeshFilter.mesh = triangleDynamicMesh;
        triangleMeshRenderer.material =triangleMat;
        triangleMeshCollider.sharedMesh = triangleDynamicMesh;

        GameObject quadTopologyVisual = new GameObject("QuadTopologyVisual");
        quadTopologyVisual.transform.SetParent(node);
        this.quadMat = quadMat;
        quadDynamicMesh = new Mesh();
        quadMeshFilter = quadTopologyVisual.AddComponent<MeshFilter>();
        quadMeshRenderer = quadTopologyVisual.AddComponent<MeshRenderer>();
        quadMeshCollider = quadTopologyVisual.AddComponent<MeshCollider>();
        quadMeshFilter.mesh = quadDynamicMesh;
        quadMeshRenderer.material = quadMat;
        quadMeshCollider.sharedMesh = quadDynamicMesh;

        GameObject tetrahedraTopologyVisual = new GameObject("TetrahedraTopologyVisual");
        tetrahedraTopologyVisual.transform.SetParent(node);
        this.tetrahedraMat = tetrahedraMat;
        this.tetrahedraDynamicMesh = new Mesh();
        tetrahedraMeshFilter = tetrahedraTopologyVisual.AddComponent<MeshFilter>();
        tetrahedraMeshRenderer = tetrahedraTopologyVisual.AddComponent<MeshRenderer>();
        tetrahedraMeshCollider = tetrahedraTopologyVisual.AddComponent<MeshCollider>();
        tetrahedraMeshFilter.mesh = tetrahedraDynamicMesh;
        tetrahedraMeshRenderer.material = tetrahedraMat;
        tetrahedraMeshCollider.sharedMesh = tetrahedraDynamicMesh;
    }
    public void Update(SofaMechanicalObjectInfo sofaMechanicalObjectInfo)
    {
        if(componentName == sofaMechanicalObjectInfo.componentName)
        {
            verticesPosition = new Vector3[sofaMechanicalObjectInfo.verticesAmount];
            for (int i = 0; i < sofaMechanicalObjectInfo.verticesAmount; i++)
            {
                verticesPosition[i].x = sofaMechanicalObjectInfo.verticesPosition[i * 3 + 0];
                verticesPosition[i].y = sofaMechanicalObjectInfo.verticesPosition[i * 3 + 1];
                verticesPosition[i].z = sofaMechanicalObjectInfo.verticesPosition[i * 3 + 2];
            }
            triangleTopology = new int[sofaMechanicalObjectInfo.triangleAmount * 3];
            for (int i = 0; i < sofaMechanicalObjectInfo.triangleAmount; i++)
            {
                //1triangletopology = 3 vertices
                //1triangletopology = 1 triangles
                //由topology生成vertice！
                triangleTopology[i * 3 + 0] = sofaMechanicalObjectInfo.triangleTopology[i * 3 + 0];
                triangleTopology[i * 3 + 1] = sofaMechanicalObjectInfo.triangleTopology[i * 3 + 1];
                triangleTopology[i * 3 + 2] = sofaMechanicalObjectInfo.triangleTopology[i * 3 + 2];
            }
            quadTopology = new int[sofaMechanicalObjectInfo.quadAmount * 6];
            for (int i = 0; i < sofaMechanicalObjectInfo.quadAmount; i++)
            {
                //1quadtopology = 6 vertices
                //1quadtopology = 2 triangles
                //由topology生成vertice！
                quadTopology[i * 6 + 0] = sofaMechanicalObjectInfo.quadTopology[i * 4 + 0];
                quadTopology[i * 6 + 1] = sofaMechanicalObjectInfo.quadTopology[i * 4 + 1];
                quadTopology[i * 6 + 2] = sofaMechanicalObjectInfo.quadTopology[i * 4 + 2];
                quadTopology[i * 6 + 3] = sofaMechanicalObjectInfo.quadTopology[i * 4 + 2];
                quadTopology[i * 6 + 4] = sofaMechanicalObjectInfo.quadTopology[i * 4 + 3];
                quadTopology[i * 6 + 5] = sofaMechanicalObjectInfo.quadTopology[i * 4 + 0];
            }
            tetrahedraTopology = new int[sofaMechanicalObjectInfo.tetrahedraAmount * 12];
            for (int i = 0; i < sofaMechanicalObjectInfo.tetrahedraAmount; i++)
            {
                //1tetrahedratopology = 12 vertices
                //1tetrahedratopology = 4 triangles
                //由topology生成vertice！
                tetrahedraTopology[i * 12 + 0] = sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 0];
                tetrahedraTopology[i * 12 + 1] = sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 1];
                tetrahedraTopology[i * 12 + 2] = sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 2];
                tetrahedraTopology[i * 12 + 3] = sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 0];
                tetrahedraTopology[i * 12 + 4] = sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 2];
                tetrahedraTopology[i * 12 + 5] = sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 3];
                tetrahedraTopology[i * 12 + 6] = sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 0];
                tetrahedraTopology[i * 12 + 7] = sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 3];
                tetrahedraTopology[i * 12 + 8] = sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 1];
                tetrahedraTopology[i * 12 + 9] = sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 1];
                tetrahedraTopology[i * 12 + 10] = sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 3];
                tetrahedraTopology[i * 12 + 11] = sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 2];
            }
            if(triangleDynamicMesh!=null)
            {
                triangleDynamicMesh.vertices = verticesPosition;
                triangleDynamicMesh.triangles = triangleTopology;
                triangleDynamicMesh.RecalculateNormals();
                triangleMeshCollider.sharedMesh = triangleDynamicMesh;

                triangleMeshRenderer.enabled = node.GetComponent<Sofa_MechanicalObject>().isRenderingTriangle;
                triangleMeshCollider.enabled = node.GetComponent<Sofa_MechanicalObject>().isTriangleCastingRay;
            }
            if (quadDynamicMesh != null)
            {
                quadDynamicMesh.vertices = verticesPosition;
                quadDynamicMesh.triangles = quadTopology;
                quadDynamicMesh.RecalculateNormals();
                quadMeshCollider.sharedMesh = quadDynamicMesh;

                quadMeshRenderer.enabled = node.GetComponent<Sofa_MechanicalObject>().isRenderingQuad;
                quadMeshCollider.enabled = node.GetComponent<Sofa_MechanicalObject>().isQuadCastingRay;
            }
            if(tetrahedraDynamicMesh!=null)
            {
                tetrahedraDynamicMesh.vertices = verticesPosition;
                tetrahedraDynamicMesh.triangles = tetrahedraTopology;
                tetrahedraDynamicMesh.RecalculateNormals();
                tetrahedraMeshCollider.sharedMesh = tetrahedraDynamicMesh;

                tetrahedraMeshRenderer.enabled = node.GetComponent<Sofa_MechanicalObject>().isRenderingTetrahedra;
                tetrahedraMeshCollider.enabled = node.GetComponent<Sofa_MechanicalObject>().isTetrahedraCastingRay;
            }
        }
        else
        {
            Debug.Log("Unity MechanicalObject Info Update Failed : Soaf MechanicalObject Info Name Not Match.");
        }
    }

    public Transform node;
    public string componentName;
    public Vector3[] verticesPosition;
    public int[] triangleTopology;
    public int[] quadTopology;
    public int[] tetrahedraTopology;

    public Material triangleMat;
    public Mesh triangleDynamicMesh;
    public MeshFilter triangleMeshFilter;
    public MeshRenderer triangleMeshRenderer;
    public MeshCollider triangleMeshCollider;

    public Material quadMat;
    public Mesh quadDynamicMesh;
    public MeshFilter quadMeshFilter;
    public MeshRenderer quadMeshRenderer;
    public MeshCollider quadMeshCollider;

    public Material tetrahedraMat;
    public Mesh tetrahedraDynamicMesh;
    public MeshFilter tetrahedraMeshFilter;
    public MeshRenderer tetrahedraMeshRenderer;
    public MeshCollider tetrahedraMeshCollider;
}