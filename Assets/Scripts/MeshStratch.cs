using UnityEngine;
using System;

[RequireComponent(typeof(MeshFilter), typeof(MeshCollider))]
public class MeshStratch : MonoBehaviour
{
    public Camera mainCamera;
    public float modificationRadius = 0.5f;
    public float modificationAmount = 0.1f;
    public int highResolution = 20; // 높은 해상도
  
    public float size = 10f; // Plane의 크기

    private Mesh highResMesh = null;


    void Start()
    {
        // 고해상도 및 저해상도 메쉬 생성
        highResMesh = GeneratePlane(highResolution);
        mainCamera = Camera.main;
       

        // 메쉬 콜라이더 설정
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = highResMesh;       
        // mainCamera.transform.position = Vector3.one *5f;
       
        if (TryGetComponent<MeshCollider>(out MeshCollider meshCollider))
        {
            meshCollider.sharedMesh = null;
            meshCollider.sharedMesh = highResMesh;
        }
        transform.position = new Vector3(25f, 8f, 50f);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject clickedObject = hit.collider.gameObject;
                MeshFilter meshFilter = clickedObject.GetComponent<MeshFilter>();

                if (meshFilter != null)
                {
                    Mesh mesh = meshFilter.mesh;
                    ModifyVerticesInRadius(mesh, hit.point, hit.normal, modificationRadius, modificationAmount);

                    // 메쉬 콜라이더 업데이트
                    MeshCollider meshCollider = null;
                    if (clickedObject.TryGetComponent<MeshCollider>(out meshCollider))
                    {
                        meshCollider.sharedMesh = null;
                        meshCollider.sharedMesh = mesh;
                    }
                }
            }
        }
    }

    private Mesh GeneratePlane(int resolution)
    {
        Mesh mesh = new Mesh();

        int vertexCount = (resolution + 1) * (resolution + 1);
        Vector3[] vertices = new Vector3[vertexCount];
        Vector2[] uv = new Vector2[vertexCount];
        int[] triangles = new int[resolution * resolution * 6];

        float stepSize = size / resolution;

        for (int i = 0, z = 0; z <= resolution; z++)
        {
            for (int x = 0; x <= resolution; x++, i++)
            {
                vertices[i] = new Vector3(x * stepSize, 0, z * stepSize);
                uv[i] = new Vector2((float)x / resolution, (float)z / resolution);
            }
        }

        for (int ti = 0, vi = 0, y = 0; y < resolution; y++, vi++)
        {
            for (int x = 0; x < resolution; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 1] = vi + resolution + 1;
                triangles[ti + 2] = vi + 1;
                triangles[ti + 3] = vi + 1;
                triangles[ti + 4] = vi + resolution + 1;
                triangles[ti + 5] = vi + resolution + 2;
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
       // mesh.CombineMeshes();

        return mesh;
    }


    void ModifyVerticesInRadius(Mesh mesh, Vector3 hitPoint, Vector3 hitNormal, float radius, float amount)
    {
        Vector3[] vertices = mesh.vertices;
        Vector3 localHitPoint = transform.InverseTransformPoint(hitPoint); // 클릭 지점을 로컬 공간으로 변환

        for (int i = 0; i < vertices.Length; i++)
        {
            float distanceSqr = (vertices[i] - localHitPoint).sqrMagnitude;

            if (distanceSqr < radius * radius)
            {
                float distance = Mathf.Sqrt(distanceSqr);
                float influence = Mathf.SmoothStep(0.0f, 1.0f, 1.0f - (distance / radius));
                vertices[i] += hitNormal * amount * influence;
            }
        }

        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }
}
