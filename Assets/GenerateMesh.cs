using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GenerateMesh : MonoBehaviour {

    public Mesh galaxyMesh;

    int segments = 21;

    class MeshData
    {
        public List<Vector3> vertices;
        public List<int> triangles;
    }

    void Start()
    {
        galaxyMesh = new Mesh();

        GetComponent<MeshFilter>().mesh = galaxyMesh;

        MeshData m = generate();

        galaxyMesh.Clear();
        galaxyMesh.SetVertices(m.vertices);
        //galaxyMesh.SetNormals(m.vertices);
        galaxyMesh.SetTriangles(m.triangles, 0);

        galaxyMesh.RecalculateBounds();

        /*foreach(Vector3 v in m.vertices)
        {
            Debug.Log(v);
        }*/
    }

    //Generates spiral galaxy mesh
    MeshData generate()
    {
        MeshData m = new MeshData();
        m.vertices = new List<Vector3>();
        m.triangles = new List<int>();

        float halfSegments = segments / 2f;

        List<int> lastVeritices = new List<int>();

        for(int i = 0; i < segments; ++i)
        {
            float negative = i > halfSegments ? 1 : -1;
            float t = Mathf.Abs(i - halfSegments) / halfSegments * 5f;
            float x = negative * Mathf.Sqrt(t) * Mathf.Cos(t);
            float y = negative * Mathf.Sqrt(t) * Mathf.Sin(t);
            float spread = (5.5f - t) * 0.1f;

            Vector3 pos = new Vector3(x, 0, y);

            Vector3 nextForDir = new Vector3(negative * Mathf.Sqrt(t + 0.1f) * Mathf.Cos(t + 0.1f), 0, negative * Mathf.Sqrt(t + 0.1f) * Mathf.Sin(t + 0.1f));

            Quaternion direction = Quaternion.LookRotation((pos - nextForDir).normalized);

            if(i == 0)
            {
                //Add first vert
                m.vertices.Add(pos);
                lastVeritices.Add(0);
            }
            else if(i == segments - 1)
            {
                //Last vert only needs to add self and connect to last verts
                int vertIndex = m.vertices.Count;
                m.vertices.Add(pos);
                for(int j = 0; j < lastVeritices.Count; ++j)
                {
                    m.triangles.Add(vertIndex);
                    m.triangles.Add(lastVeritices[j]);
                    m.triangles.Add(lastVeritices[(j + 1) % lastVeritices.Count]);
                }
            }
            else
            {
                List<int> newVerts = new List<int>();

                //Only supports 4 sided right now
                //Generate vertices for new segment
                for (int j = 0; j < 4; ++j)
                {
                    newVerts.Add(m.vertices.Count);

                    Vector3 offset = new Vector3();

                    //Needs different code to support arbitrary sided mesh
                    switch (j)
                    {
                        case 0:
                            offset = new Vector3(spread, 0, 0); break;
                        case 1:
                            offset = new Vector3(0, spread, 0); break;
                        case 2:
                            offset = new Vector3(-spread, 0, 0); break;
                        case 3:
                            offset = new Vector3(0, -spread, 0); break;
                    }
                    pos = pos + direction * offset;

                    m.vertices.Add(pos);
                }

                if (i == 1)
                {
                    for (int j = 0; j < newVerts.Count; ++j)
                    {
                        m.triangles.Add(lastVeritices[0]);
                        m.triangles.Add(newVerts[j]);
                        m.triangles.Add(newVerts[(j + 1) % newVerts.Count]);
                    }
                }
                else
                {
                    //Add polygons attaching new segment to old segment
                    for (int j = 0; j < lastVeritices.Count; ++j)
                    {
                        int anchor1 = j;
                        int anchor2 = (j + 1) % lastVeritices.Count;

                        m.triangles.Add(newVerts[anchor1]);
                        m.triangles.Add(lastVeritices[anchor2]);
                        m.triangles.Add(lastVeritices[anchor1]);

                        m.triangles.Add(newVerts[anchor1]);
                        m.triangles.Add(newVerts[anchor2]);
                        m.triangles.Add(lastVeritices[anchor2]);
                        
                    }
                }

                lastVeritices.Clear();
                lastVeritices.AddRange(newVerts);

                Assert.IsTrue(lastVeritices.Count == 4);
            }
        }

        return m;
    }
}
