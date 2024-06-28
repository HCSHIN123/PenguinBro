using UnityEngine;
using System;

[RequireComponent(typeof(MeshFilter), typeof(MeshCollider))]
public class MeshStratch : MonoBehaviour
{
    [SerializeField] private float modificationRadius = 0.5f;
    [SerializeField] private float modificationAmount = 0.1f;
    [SerializeField] private float meshWidth = 10f;
    [SerializeField] private float meshHeight = 10f;
    // [SerializeField] private float size = 10f; 

    [SerializeField] private int highResolution = 20; 
    [SerializeField] private Vector3 planePos = Vector3.zero;

    private Mesh highResMesh = null;


    void Start()
    {
        // 고해상도 및 저해상도 메쉬 생성
        highResMesh = GeneratePlane(highResolution, meshWidth, meshHeight);
      

        // 메쉬 콜라이더 설정
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = highResMesh;              
       
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = highResMesh;
        
        transform.position = planePos;

      
        
    }

    // void Update()
    // {
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //         RaycastHit hit;

    //         if (Physics.Raycast(ray, out hit))
    //         {
    //             GameObject clickedObject = hit.collider.gameObject;
    //             MeshFilter meshFilter = clickedObject.GetComponent<MeshFilter>();

    //             if (meshFilter != null)
    //             {
    //                 Mesh mesh = meshFilter.mesh;
    //                 Debug.Log(hit.point);
    //                 ModifyVerticesInRadius(mesh, hit.point, Vector3.down, modificationRadius, modificationAmount);

    //                 // 메쉬 콜라이더 업데이트
    //                 MeshCollider meshCollider = null;
    //                 if (clickedObject.TryGetComponent<MeshCollider>(out meshCollider))
    //                 {
    //                     meshCollider.sharedMesh = null;
    //                     meshCollider.sharedMesh = mesh;
    //                 }
    //             }
    //         }
    //     }
    // }

    private Mesh GeneratePlane(int resolution, float _width, float _height)
    {
        Mesh mesh = new Mesh();

        int vertexCount = (resolution + 1) * (resolution + 1);
        Vector3[] vertices = new Vector3[vertexCount];
        Vector2[] uv = new Vector2[vertexCount];
        int[] triangles = new int[resolution * resolution * 6];

        float stepSizeX = _width / resolution;
        float stepSizeY = _height / resolution;

        for (int i = 0, z = 0; z <= resolution; z++)
        {
            for (int x = 0; x <= resolution; x++, i++)
            {
                vertices[i] = new Vector3(x * stepSizeX, 0, z * stepSizeY);
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
        mesh.triangles = triangles;
        mesh.uv = uv;

        mesh.RecalculateNormals();
       // mesh.CombineMeshes();

        return mesh;
    }


    private void ModifyVerticesInRadius(Mesh mesh, Vector3 hitPoint, Vector3 hitNormal, float radius, float amount)
    {
        Vector3[] vertices = mesh.vertices;
        Vector3 localHitPoint = transform.InverseTransformPoint(hitPoint); 

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

        MeshCollider meshCollider = GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
    }

    private void OnCollisionEnter(Collision _collision)
    {
    
        if(_collision.gameObject.CompareTag("Bullet"))
        {   
            Vector3 curpoint = Vector3.one;
            float curDis = float.MaxValue;
            foreach(ContactPoint points in _collision.contacts)
            {
                if(Vector3.Distance(_collision.transform.position, points.point) < curDis)
                {
                    curpoint = points.point;
                    curDis = Vector3.Distance(_collision.transform.position, points.point);
                }
            }
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            ModifyVerticesInRadius(meshFilter.mesh, curpoint, Vector3.down, modificationRadius, modificationAmount);
        }
    }
}
