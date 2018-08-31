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
        verticesNormalInfo = SofaPhysicsAPI.GetMeshVerticesNormals(meshIndex);
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

    public float* verticesNormalInfo;
    public float* verticesUVInfo;
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
        verticesPosition = new Vector3[0];
        verticesNormalInfo = new Vector3[0];
        if (componentName==sofaMeshInfo.componentName)
        {
            if (sofaMeshInfo.triangleAmount != 0)
            {
                verticesPosition = new Vector3[sofaMeshInfo.triangleAmount * 3];
                verticesNormalInfo = new Vector3[sofaMeshInfo.triangleAmount * 3];
                triangleTopology = new int[sofaMeshInfo.triangleAmount * 3];
                for (int i = 0; i < sofaMeshInfo.triangleAmount; i++)
                {
                    triangleTopology[i * 3 + 0] = i * 3 + 0;
                    verticesPosition[i * 3 + 0].x = sofaMeshInfo.verticesPosition[sofaMeshInfo.triangleTopology[i * 3 + 0] * 3 + 0];
                    verticesPosition[i * 3 + 0].y = sofaMeshInfo.verticesPosition[sofaMeshInfo.triangleTopology[i * 3 + 0] * 3 + 1];
                    verticesPosition[i * 3 + 0].z = sofaMeshInfo.verticesPosition[sofaMeshInfo.triangleTopology[i * 3 + 0] * 3 + 2];
                    verticesNormalInfo[i * 3 + 0].x = sofaMeshInfo.verticesNormalInfo[sofaMeshInfo.triangleTopology[i * 3 + 0] * 3 + 0];
                    verticesNormalInfo[i * 3 + 0].y = sofaMeshInfo.verticesNormalInfo[sofaMeshInfo.triangleTopology[i * 3 + 0] * 3 + 1];
                    verticesNormalInfo[i * 3 + 0].z = sofaMeshInfo.verticesNormalInfo[sofaMeshInfo.triangleTopology[i * 3 + 0] * 3 + 2];

                    triangleTopology[i * 3 + 1] = i * 3 + 1;
                    verticesPosition[i * 3 + 1].x = sofaMeshInfo.verticesPosition[sofaMeshInfo.triangleTopology[i * 3 + 1] * 3 + 0];
                    verticesPosition[i * 3 + 1].y = sofaMeshInfo.verticesPosition[sofaMeshInfo.triangleTopology[i * 3 + 1] * 3 + 1];
                    verticesPosition[i * 3 + 1].z = sofaMeshInfo.verticesPosition[sofaMeshInfo.triangleTopology[i * 3 + 1] * 3 + 2];
                    verticesNormalInfo[i * 3 + 1].x = sofaMeshInfo.verticesNormalInfo[sofaMeshInfo.triangleTopology[i * 3 + 1] * 3 + 0];
                    verticesNormalInfo[i * 3 + 1].y = sofaMeshInfo.verticesNormalInfo[sofaMeshInfo.triangleTopology[i * 3 + 1] * 3 + 1];
                    verticesNormalInfo[i * 3 + 1].z = sofaMeshInfo.verticesNormalInfo[sofaMeshInfo.triangleTopology[i * 3 + 1] * 3 + 2];

                    triangleTopology[i * 3 + 2] = i * 3 + 2;
                    verticesPosition[i * 3 + 2].x = sofaMeshInfo.verticesPosition[sofaMeshInfo.triangleTopology[i * 3 + 2] * 3 + 0];
                    verticesPosition[i * 3 + 2].y = sofaMeshInfo.verticesPosition[sofaMeshInfo.triangleTopology[i * 3 + 2] * 3 + 1];
                    verticesPosition[i * 3 + 2].z = sofaMeshInfo.verticesPosition[sofaMeshInfo.triangleTopology[i * 3 + 2] * 3 + 2];
                    verticesNormalInfo[i * 3 + 2].x = sofaMeshInfo.verticesNormalInfo[sofaMeshInfo.triangleTopology[i * 3 + 2] * 3 + 0];
                    verticesNormalInfo[i * 3 + 2].y = sofaMeshInfo.verticesNormalInfo[sofaMeshInfo.triangleTopology[i * 3 + 2] * 3 + 1];
                    verticesNormalInfo[i * 3 + 2].z = sofaMeshInfo.verticesNormalInfo[sofaMeshInfo.triangleTopology[i * 3 + 2] * 3 + 2];
                }
            }
            else
            {
                verticesPosition = new Vector3[sofaMeshInfo.quadAmount * 6];
                verticesNormalInfo = new Vector3[sofaMeshInfo.quadAmount * 6];
                triangleTopology = new int[sofaMeshInfo.quadAmount * 6];
                for (int i = 0; i < sofaMeshInfo.quadAmount; i++)
                {
                    triangleTopology[i * 6 + 0] = i * 6 + 0;
                    verticesPosition[i * 6 + 0].x = sofaMeshInfo.verticesPosition[sofaMeshInfo.quadTopology[i * 4 + 0] * 3 + 0];
                    verticesPosition[i * 6 + 0].y = sofaMeshInfo.verticesPosition[sofaMeshInfo.quadTopology[i * 4 + 0] * 3 + 1];
                    verticesPosition[i * 6 + 0].z = sofaMeshInfo.verticesPosition[sofaMeshInfo.quadTopology[i * 4 + 0] * 3 + 2];
                    verticesNormalInfo[i * 6 + 0].x = sofaMeshInfo.verticesPosition[sofaMeshInfo.quadTopology[i * 4 + 0] * 3 + 0];
                    verticesNormalInfo[i * 6 + 0].y = sofaMeshInfo.verticesPosition[sofaMeshInfo.quadTopology[i * 4 + 0] * 3 + 1];
                    verticesNormalInfo[i * 6 + 0].z = sofaMeshInfo.verticesPosition[sofaMeshInfo.quadTopology[i * 4 + 0] * 3 + 2];

                    triangleTopology[i * 6 + 1] = i * 6 + 1;
                    verticesPosition[i * 6 + 1].x = sofaMeshInfo.verticesPosition[sofaMeshInfo.quadTopology[i * 4 + 1] * 3 + 0];
                    verticesPosition[i * 6 + 1].y = sofaMeshInfo.verticesPosition[sofaMeshInfo.quadTopology[i * 4 + 1] * 3 + 1];
                    verticesPosition[i * 6 + 1].z = sofaMeshInfo.verticesPosition[sofaMeshInfo.quadTopology[i * 4 + 1] * 3 + 2];
                    verticesNormalInfo[i * 6 + 1].x = sofaMeshInfo.verticesPosition[sofaMeshInfo.quadTopology[i * 4 + 1] * 3 + 0];
                    verticesNormalInfo[i * 6 + 1].y = sofaMeshInfo.verticesPosition[sofaMeshInfo.quadTopology[i * 4 + 1] * 3 + 1];
                    verticesNormalInfo[i * 6 + 1].z = sofaMeshInfo.verticesPosition[sofaMeshInfo.quadTopology[i * 4 + 1] * 3 + 2];

                    triangleTopology[i * 6 + 2] = i * 6 + 2;
                    verticesPosition[i * 6 + 2].x = sofaMeshInfo.verticesPosition[sofaMeshInfo.quadTopology[i * 4 + 2] * 3 + 0];
                    verticesPosition[i * 6 + 2].y = sofaMeshInfo.verticesPosition[sofaMeshInfo.quadTopology[i * 4 + 2] * 3 + 1];
                    verticesPosition[i * 6 + 2].z = sofaMeshInfo.verticesPosition[sofaMeshInfo.quadTopology[i * 4 + 2] * 3 + 2];
                    verticesNormalInfo[i * 6 + 2].x = sofaMeshInfo.verticesPosition[sofaMeshInfo.quadTopology[i * 4 + 2] * 3 + 0];
                    verticesNormalInfo[i * 6 + 2].y = sofaMeshInfo.verticesPosition[sofaMeshInfo.quadTopology[i * 4 + 2] * 3 + 1];
                    verticesNormalInfo[i * 6 + 2].z = sofaMeshInfo.verticesPosition[sofaMeshInfo.quadTopology[i * 4 + 2] * 3 + 2];

                    triangleTopology[i * 6 + 3] = i * 6 + 3;
                    verticesPosition[i * 6 + 3].x = sofaMeshInfo.verticesPosition[sofaMeshInfo.quadTopology[i * 4 + 2] * 3 + 0];
                    verticesPosition[i * 6 + 3].y = sofaMeshInfo.verticesPosition[sofaMeshInfo.quadTopology[i * 4 + 2] * 3 + 1];
                    verticesPosition[i * 6 + 3].z = sofaMeshInfo.verticesPosition[sofaMeshInfo.quadTopology[i * 4 + 2] * 3 + 2];
                    verticesNormalInfo[i * 6 + 3].x = sofaMeshInfo.verticesPosition[sofaMeshInfo.quadTopology[i * 4 + 2] * 3 + 0];
                    verticesNormalInfo[i * 6 + 3].y = sofaMeshInfo.verticesPosition[sofaMeshInfo.quadTopology[i * 4 + 2] * 3 + 1];
                    verticesNormalInfo[i * 6 + 3].z = sofaMeshInfo.verticesPosition[sofaMeshInfo.quadTopology[i * 4 + 2] * 3 + 2];

                    triangleTopology[i * 6 + 4] = i * 6 + 4;
                    verticesPosition[i * 6 + 4].x = sofaMeshInfo.verticesPosition[sofaMeshInfo.quadTopology[i * 4 + 3] * 3 + 0];
                    verticesPosition[i * 6 + 4].y = sofaMeshInfo.verticesPosition[sofaMeshInfo.quadTopology[i * 4 + 3] * 3 + 1];
                    verticesPosition[i * 6 + 4].z = sofaMeshInfo.verticesPosition[sofaMeshInfo.quadTopology[i * 4 + 3] * 3 + 2];
                    verticesNormalInfo[i * 6 + 4].x = sofaMeshInfo.verticesPosition[sofaMeshInfo.quadTopology[i * 4 + 3] * 3 + 0];
                    verticesNormalInfo[i * 6 + 4].y = sofaMeshInfo.verticesPosition[sofaMeshInfo.quadTopology[i * 4 + 3] * 3 + 1];
                    verticesNormalInfo[i * 6 + 4].z = sofaMeshInfo.verticesPosition[sofaMeshInfo.quadTopology[i * 4 + 3] * 3 + 2];

                    triangleTopology[i * 6 + 5] = i * 6 + 5;
                    verticesPosition[i * 6 + 5].x = sofaMeshInfo.verticesPosition[sofaMeshInfo.quadTopology[i * 4 + 0] * 3 + 0];
                    verticesPosition[i * 6 + 5].y = sofaMeshInfo.verticesPosition[sofaMeshInfo.quadTopology[i * 4 + 0] * 3 + 1];
                    verticesPosition[i * 6 + 5].z = sofaMeshInfo.verticesPosition[sofaMeshInfo.quadTopology[i * 4 + 0] * 3 + 2];
                    verticesNormalInfo[i * 6 + 5].x = sofaMeshInfo.verticesPosition[sofaMeshInfo.quadTopology[i * 4 + 0] * 3 + 0];
                    verticesNormalInfo[i * 6 + 5].y = sofaMeshInfo.verticesPosition[sofaMeshInfo.quadTopology[i * 4 + 0] * 3 + 1];
                    verticesNormalInfo[i * 6 + 5].z = sofaMeshInfo.verticesPosition[sofaMeshInfo.quadTopology[i * 4 + 0] * 3 + 2];
                }
            }
            dynamicMesh.Clear();
            dynamicMesh.vertices = verticesPosition;
            dynamicMesh.normals = verticesNormalInfo;
            dynamicMesh.triangles = triangleTopology;
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
    public Vector3[] verticesNormalInfo;
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
            verticesPosition = new Vector3[sofaMechanicalObjectInfo.verticesAmount * 3];
            for(int i =0;i<sofaMechanicalObjectInfo.verticesAmount;i++)
            {
                verticesPosition[i].x = sofaMechanicalObjectInfo.verticesPosition[i * 3 + 0];
                verticesPosition[i].y = sofaMechanicalObjectInfo.verticesPosition[i * 3 + 1];
                verticesPosition[i].z = sofaMechanicalObjectInfo.verticesPosition[i * 3 + 2];
            }

            triangleVerticesPosition = new Vector3[sofaMechanicalObjectInfo.triangleAmount * 3];
            triangleTopology = new int[sofaMechanicalObjectInfo.triangleAmount * 3];
            for (int i = 0; i < sofaMechanicalObjectInfo.triangleAmount; i++)
            {
                triangleTopology[i * 3 + 0] = i * 3 + 0;
                triangleVerticesPosition[i * 3 + 0].x = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.triangleTopology[i * 3 + 0] * 3 + 0];
                triangleVerticesPosition[i * 3 + 0].y = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.triangleTopology[i * 3 + 0] * 3 + 1];
                triangleVerticesPosition[i * 3 + 0].z = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.triangleTopology[i * 3 + 0] * 3 + 2];

                triangleTopology[i * 3 + 1] = i * 3 + 1;
                triangleVerticesPosition[i * 3 + 1].x = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.triangleTopology[i * 3 + 1] * 3 + 0];
                triangleVerticesPosition[i * 3 + 1].y = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.triangleTopology[i * 3 + 1] * 3 + 1];
                triangleVerticesPosition[i * 3 + 1].z = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.triangleTopology[i * 3 + 1] * 3 + 2];

                triangleTopology[i * 3 + 2] = i * 3 + 2;
                triangleVerticesPosition[i * 3 + 2].x = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.triangleTopology[i * 3 + 2] * 3 + 0];
                triangleVerticesPosition[i * 3 + 2].y = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.triangleTopology[i * 3 + 2] * 3 + 1];
                triangleVerticesPosition[i * 3 + 2].z = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.triangleTopology[i * 3 + 2] * 3 + 2];
            }
            
            quadVerticesPosition = new Vector3[sofaMechanicalObjectInfo.quadAmount * 6];
            quadTopology = new int[sofaMechanicalObjectInfo.quadAmount * 6];
            for (int i = 0; i < sofaMechanicalObjectInfo.quadAmount; i++)
            {
                quadTopology[i * 6 + 0] = i * 6 + 0;
                quadVerticesPosition[i * 6 + 0].x = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.quadTopology[i * 4 + 0] * 3 + 0];
                quadVerticesPosition[i * 6 + 0].y = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.quadTopology[i * 4 + 0] * 3 + 1];
                quadVerticesPosition[i * 6 + 0].z = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.quadTopology[i * 4 + 0] * 3 + 2];

                quadTopology[i * 6 + 1] = i * 6 + 1;
                quadVerticesPosition[i * 6 + 1].x = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.quadTopology[i * 4 + 1] * 3 + 0];
                quadVerticesPosition[i * 6 + 1].y = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.quadTopology[i * 4 + 1] * 3 + 1];
                quadVerticesPosition[i * 6 + 1].z = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.quadTopology[i * 4 + 1] * 3 + 2];

                quadTopology[i * 6 + 2] = i * 6 + 2;
                quadVerticesPosition[i * 6 + 2].x = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.quadTopology[i * 4 + 2] * 3 + 0];
                quadVerticesPosition[i * 6 + 2].y = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.quadTopology[i * 4 + 2] * 3 + 1];
                quadVerticesPosition[i * 6 + 2].z = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.quadTopology[i * 4 + 2] * 3 + 2];

                quadTopology[i * 6 + 3] = i * 6 + 3;
                quadVerticesPosition[i * 6 + 3].x = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.quadTopology[i * 4 + 2] * 3 + 0];
                quadVerticesPosition[i * 6 + 3].y = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.quadTopology[i * 4 + 2] * 3 + 1];
                quadVerticesPosition[i * 6 + 3].z = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.quadTopology[i * 4 + 2] * 3 + 2];

                quadTopology[i * 6 + 4] = i * 6 + 4;
                quadVerticesPosition[i * 6 + 4].x = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.quadTopology[i * 4 + 3] * 3 + 0];
                quadVerticesPosition[i * 6 + 4].y = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.quadTopology[i * 4 + 3] * 3 + 1];
                quadVerticesPosition[i * 6 + 4].z = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.quadTopology[i * 4 + 3] * 3 + 2];

                quadTopology[i * 6 + 5] = i * 6 + 5;
                quadVerticesPosition[i * 6 + 5].x = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.quadTopology[i * 4 + 0] * 3 + 0];
                quadVerticesPosition[i * 6 + 5].y = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.quadTopology[i * 4 + 0] * 3 + 1];
                quadVerticesPosition[i * 6 + 5].z = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.quadTopology[i * 4 + 0] * 3 + 2];
            }

            tetrahedraVerticesPosition = new Vector3[sofaMechanicalObjectInfo.tetrahedraAmount * 12];
            tetrahedraTopology = new int[sofaMechanicalObjectInfo.tetrahedraAmount * 12];
            for (int i = 0; i < sofaMechanicalObjectInfo.tetrahedraAmount; i++)
            {
                tetrahedraTopology[i * 12 + 0] = i * 12 + 0;
                tetrahedraVerticesPosition[i * 12 + 0].x = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 0] * 3 + 0];
                tetrahedraVerticesPosition[i * 12 + 0].y = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 0] * 3 + 1];
                tetrahedraVerticesPosition[i * 12 + 0].z = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 0] * 3 + 2];

                tetrahedraTopology[i * 12 + 1] = i * 12 + 1;
                tetrahedraVerticesPosition[i * 12 + 1].x = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 2] * 3 + 0];
                tetrahedraVerticesPosition[i * 12 + 1].y = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 2] * 3 + 1];
                tetrahedraVerticesPosition[i * 12 + 1].z = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 2] * 3 + 2];

                tetrahedraTopology[i * 12 + 2] = i * 12 + 2;
                tetrahedraVerticesPosition[i * 12 + 2].x = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 1] * 3 + 0];
                tetrahedraVerticesPosition[i * 12 + 2].y = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 1] * 3 + 1];
                tetrahedraVerticesPosition[i * 12 + 2].z = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 1] * 3 + 2];

                tetrahedraTopology[i * 12 + 3] = i * 12 + 3;
                tetrahedraVerticesPosition[i * 12 + 3].x = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 0] * 3 + 0];
                tetrahedraVerticesPosition[i * 12 + 3].y = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 0] * 3 + 1];
                tetrahedraVerticesPosition[i * 12 + 3].z = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 0] * 3 + 2];

                tetrahedraTopology[i * 12 + 4] = i * 12 + 4;
                tetrahedraVerticesPosition[i * 12 + 4].x = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 3] * 3 + 0];
                tetrahedraVerticesPosition[i * 12 + 4].y = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 3] * 3 + 1];
                tetrahedraVerticesPosition[i * 12 + 4].z = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 3] * 3 + 2];

                tetrahedraTopology[i * 12 + 5] = i * 12 + 5;
                tetrahedraVerticesPosition[i * 12 + 5].x = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 2] * 3 + 0];
                tetrahedraVerticesPosition[i * 12 + 5].y = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 2] * 3 + 1];
                tetrahedraVerticesPosition[i * 12 + 5].z = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 2] * 3 + 2];

                tetrahedraTopology[i * 12 + 6] = i * 12 + 6;
                tetrahedraVerticesPosition[i * 12 + 6].x = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 0] * 3 + 0];
                tetrahedraVerticesPosition[i * 12 + 6].y = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 0] * 3 + 1];
                tetrahedraVerticesPosition[i * 12 + 6].z = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 0] * 3 + 2];

                tetrahedraTopology[i * 12 + 7] = i * 12 + 7;
                tetrahedraVerticesPosition[i * 12 + 7].x = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 1] * 3 + 0];
                tetrahedraVerticesPosition[i * 12 + 7].y = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 1] * 3 + 1];
                tetrahedraVerticesPosition[i * 12 + 7].z = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 1] * 3 + 2];

                tetrahedraTopology[i * 12 + 8] = i * 12 + 8;
                tetrahedraVerticesPosition[i * 12 + 8].x = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 3] * 3 + 0];
                tetrahedraVerticesPosition[i * 12 + 8].y = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 3] * 3 + 1];
                tetrahedraVerticesPosition[i * 12 + 8].z = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 3] * 3 + 2];

                tetrahedraTopology[i * 12 + 9] = i * 12 + 9;
                tetrahedraVerticesPosition[i * 12 + 9].x = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 1] * 3 + 0];
                tetrahedraVerticesPosition[i * 12 + 9].y = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 1] * 3 + 1];
                tetrahedraVerticesPosition[i * 12 + 9].z = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 1] * 3 + 2];

                tetrahedraTopology[i * 12 + 10] = i * 12 + 10;
                tetrahedraVerticesPosition[i * 12 + 10].x = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 2] * 3 + 0];
                tetrahedraVerticesPosition[i * 12 + 10].y = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 2] * 3 + 1];
                tetrahedraVerticesPosition[i * 12 + 10].z = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 2] * 3 + 2];

                tetrahedraTopology[i * 12 + 11] = i * 12 + 11;
                tetrahedraVerticesPosition[i * 12 + 11].x = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 3] * 3 + 0];
                tetrahedraVerticesPosition[i * 12 + 11].y = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 3] * 3 + 1];
                tetrahedraVerticesPosition[i * 12 + 11].z = sofaMechanicalObjectInfo.verticesPosition[sofaMechanicalObjectInfo.tetrahedraTopology[i * 4 + 3] * 3 + 2];
            }
           if(triangleDynamicMesh!=null)
           {
               //triangleDynamicMesh.Clear();
               triangleDynamicMesh.triangles = new int[0];
               triangleDynamicMesh.vertices = new Vector3[0];

               triangleDynamicMesh.vertices = triangleVerticesPosition;
               triangleDynamicMesh.triangles = triangleTopology;
               triangleDynamicMesh.RecalculateNormals();
               triangleMeshCollider.sharedMesh = triangleDynamicMesh;

               triangleMeshRenderer.enabled = node.GetComponent<Sofa_MechanicalObject>().isRenderingTriangle;
               triangleMeshCollider.enabled = node.GetComponent<Sofa_MechanicalObject>().isTriangleCastingRay;
           }

           if (quadDynamicMesh != null)
           {
               //quadDynamicMesh.Clear();
               quadDynamicMesh.triangles = new int[0];
               quadDynamicMesh.vertices = new Vector3[0];

               quadDynamicMesh.vertices = quadVerticesPosition;
               quadDynamicMesh.triangles = quadTopology;
               quadDynamicMesh.RecalculateNormals();
               quadMeshCollider.sharedMesh = quadDynamicMesh;

               quadMeshRenderer.enabled = node.GetComponent<Sofa_MechanicalObject>().isRenderingQuad;
               quadMeshCollider.enabled = node.GetComponent<Sofa_MechanicalObject>().isQuadCastingRay;
           }
           if(tetrahedraDynamicMesh!=null)
           {
               //tetrahedraDynamicMesh.Clear();
               tetrahedraDynamicMesh.triangles = new int[0];
               tetrahedraDynamicMesh.vertices = new Vector3[0];

               tetrahedraDynamicMesh.vertices = tetrahedraVerticesPosition;
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
    public Vector3[] triangleVerticesPosition;
    public Vector3[] quadVerticesPosition;
    public Vector3[] tetrahedraVerticesPosition;
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