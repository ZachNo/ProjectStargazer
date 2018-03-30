using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GenerateMesh : MonoBehaviour {

    public Mesh galaxyMesh;

    int segments = 16;
    int vertsPerSegment = 4;
    float overall_spread = 0.1f;

    int galaxyType = -1;
    float size = 0;

    class MeshData
    {
        public List<Vector3> vertices;
        public List<int> triangles;
    }

    void Start()
    {
        if (transform.parent.GetComponent<Galaxy>() != null)
        {
            galaxyType = transform.parent.GetComponent<Galaxy>().galaxyType;
            size = transform.parent.GetComponent<Galaxy>().size * 50;
        }

        if(galaxyType == -1)
        {
            return;
        }

        galaxyMesh = new Mesh();

        if(GetComponent<ParticleSystem>() != null)
        {
            ParticleSystem.ShapeModule s = GetComponent<ParticleSystem>().shape;
            s.mesh = galaxyMesh;
        }
        else if(GetComponent<MeshFilter>() != null)
        {
            GetComponent<MeshFilter>().mesh = galaxyMesh;
        }

        MeshData m = generate(galaxyType);

        galaxyMesh.Clear();
        galaxyMesh.SetVertices(m.vertices);
        //galaxyMesh.SetNormals(m.vertices);
        galaxyMesh.SetTriangles(m.triangles, 0);

        galaxyMesh.RecalculateBounds();
    }

    //Generates spiral galaxy mesh
    MeshData generate(int type)
    {
        MeshData m = new MeshData();
        m.vertices = new List<Vector3>();
        m.triangles = new List<int>();

        switch (type)
        {
            case 0:
                IcoSphere(ref m, size / 2);
                IcoSphere(ref m, size);

                break;
            case 1:
                List<int> lastVeritices = new List<int>();

                for (int i = 0; i < segments; ++i)
                    AddSegment(i, ref m, ref lastVeritices, size * 2);

                MirrorAndAdd(ref m);
                break;
        }

        return m;
    }

    void AddSegment(int i, ref MeshData m, ref List<int> lastVeritices, float scale)
    {
        float t = i / (segments - 1f) * 5f;
        float x = Mathf.Sqrt(t) * Mathf.Cos(t);
        float y = Mathf.Sqrt(t) * Mathf.Sin(t);
        float spread = (5.5f - t) * overall_spread;

        Vector3 pos = new Vector3(x, 0, y);

        t = (i + 0.1f) / (segments - 1f) * 5f;

        Vector3 nextForDir = pos - new Vector3(Mathf.Sqrt(t) * Mathf.Cos(t), 0, Mathf.Sqrt(t) * Mathf.Sin(t));

        Quaternion direction = Quaternion.LookRotation(nextForDir.normalized);

        if (i == segments - 1)
        {
            //Last vert only needs to add self and connect to last verts
            int vertIndex = m.vertices.Count;
            m.vertices.Add(pos * scale);
            for (int j = 0; j < lastVeritices.Count; ++j)
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
            for (int j = 0; j < vertsPerSegment; ++j)
            {
                newVerts.Add(m.vertices.Count);
                Vector3 offset = vertIndexToVec(j, direction.eulerAngles.y) * spread;
                offset.y *= 0.2f; // Squish up/down

                m.vertices.Add((pos + offset) * scale);
            }

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

            lastVeritices.Clear();
            lastVeritices.AddRange(newVerts);
        }
    }

    void MirrorAndAdd(ref MeshData m)
    {
        int vertCount = m.vertices.Count;
        int triCount = m.triangles.Count;

        for(int i = 0; i < vertCount; ++i)
            m.vertices.Add(-m.vertices[i]);

        for (int i = 0; i < triCount; ++i)
            m.triangles.Add(m.triangles[i] + vertCount);
    }

    Vector3 vertIndexToVec(int i, float angle)
    {
        return Quaternion.Euler(0, angle, i * 360f / vertsPerSegment) * Vector3.up;
    }

    void IcoSphere(ref MeshData m, float scale)
    {
        int offset = m.vertices.Count;

        float t = 0.5f + Mathf.Sqrt(5) / 2;

        m.vertices.Add(new Vector3(-1, +t, 0) * scale);
        m.vertices.Add(new Vector3(+1, +t, 0) * scale);
        m.vertices.Add(new Vector3(-1, -t, 0) * scale);
        m.vertices.Add(new Vector3(+1, -t, 0) * scale);

        m.vertices.Add(new Vector3(0, -1, +t) * scale);
        m.vertices.Add(new Vector3(0, +1, +t) * scale);
        m.vertices.Add(new Vector3(0, -1, -t) * scale);
        m.vertices.Add(new Vector3(0, +1, -t) * scale);

        m.vertices.Add(new Vector3(+t, 0, -1) * scale);
        m.vertices.Add(new Vector3(+t, 0, +1) * scale);
        m.vertices.Add(new Vector3(-t, 0, -1) * scale);
        m.vertices.Add(new Vector3(-t, 0, +1) * scale);

        m.triangles.Add(0 + offset);
        m.triangles.Add(11 + offset);
        m.triangles.Add(5 + offset);

        m.triangles.Add(0 + offset);
        m.triangles.Add(5 + offset);
        m.triangles.Add(1 + offset);

        m.triangles.Add(0 + offset);
        m.triangles.Add(1 + offset);
        m.triangles.Add(7 + offset);

        m.triangles.Add(0 + offset);
        m.triangles.Add(7 + offset);
        m.triangles.Add(10 + offset);

        m.triangles.Add(0 + offset);
        m.triangles.Add(10 + offset);
        m.triangles.Add(11 + offset);

        m.triangles.Add(1 + offset);
        m.triangles.Add(5 + offset);
        m.triangles.Add(9 + offset);

        m.triangles.Add(5 + offset);
        m.triangles.Add(11 + offset);
        m.triangles.Add(4 + offset);

        m.triangles.Add(11 + offset);
        m.triangles.Add(10 + offset);
        m.triangles.Add(2 + offset);

        m.triangles.Add(10 + offset);
        m.triangles.Add(7 + offset);
        m.triangles.Add(6 + offset);

        m.triangles.Add(7 + offset);
        m.triangles.Add(1 + offset);
        m.triangles.Add(8 + offset);

        m.triangles.Add(3 + offset);
        m.triangles.Add(9 + offset);
        m.triangles.Add(4 + offset);

        m.triangles.Add(3 + offset);
        m.triangles.Add(4 + offset);
        m.triangles.Add(2 + offset);

        m.triangles.Add(3 + offset);
        m.triangles.Add(2 + offset);
        m.triangles.Add(6 + offset);

        m.triangles.Add(3 + offset);
        m.triangles.Add(6 + offset);
        m.triangles.Add(8 + offset);

        m.triangles.Add(3 + offset);
        m.triangles.Add(8 + offset);
        m.triangles.Add(9 + offset);

        m.triangles.Add(4 + offset);
        m.triangles.Add(9 + offset);
        m.triangles.Add(5 + offset);

        m.triangles.Add(2 + offset);
        m.triangles.Add(4 + offset);
        m.triangles.Add(11 + offset);

        m.triangles.Add(6 + offset);
        m.triangles.Add(2 + offset);
        m.triangles.Add(10 + offset);

        m.triangles.Add(8 + offset);
        m.triangles.Add(6 + offset);
        m.triangles.Add(7 + offset);

        m.triangles.Add(9 + offset);
        m.triangles.Add(8 + offset);
        m.triangles.Add(1 + offset);
    }
}
