using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour {

    struct Complex
    {
        public List<Vector3> cells;
        public List<Vector3> positions;
        public Dictionary<Vector3, int> cache;
    }

    public int subDivisions = 0;

    int lastSubdiv;
    Mesh sphereMesh;

    void Start()
    {
        lastSubdiv = -1;
        sphereMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = sphereMesh;
    }

    // Update is called once per frame
    void Update ()
    {
        if (lastSubdiv != subDivisions)
        {
            if(subDivisions > 6)
            {
                subDivisions = 6;
            }
            Complex sphere = Icosphere(subDivisions);
            sphereMesh.Clear();
            sphereMesh.SetVertices(sphere.positions);
            sphereMesh.SetNormals(sphere.positions);

            List<int> tris = new List<int>();
            foreach (Vector3 cell in sphere.cells)
            {
                tris.Add((int)cell[0]);
                tris.Add((int)cell[1]);
                tris.Add((int)cell[2]);
            }
            sphereMesh.SetTriangles(tris, 0);
            lastSubdiv = subDivisions;

            sphereMesh.RecalculateBounds();
        }
	}

    Complex Icosphere(int subdivisions)
    {
        subdivisions = +subdivisions | 0;

        Complex complex = new Complex();
        complex.positions = new List<Vector3>();
        complex.cells = new List<Vector3>();
        complex.cache = new Dictionary<Vector3, int>();
        float t = 0.5f + Mathf.Sqrt(5) / 2;

        complex.positions.Add(new Vector3(-1, +t, 0));
        complex.positions.Add(new Vector3(+1, +t, 0));
        complex.positions.Add(new Vector3(-1, -t, 0));
        complex.positions.Add(new Vector3(+1, -t, 0));

        complex.positions.Add(new Vector3(0, -1, +t));
        complex.positions.Add(new Vector3(0, +1, +t));
        complex.positions.Add(new Vector3(0, -1, -t));
        complex.positions.Add(new Vector3(0, +1, -t));

        complex.positions.Add(new Vector3(+t, 0, -1));
        complex.positions.Add(new Vector3(+t, 0, +1));
        complex.positions.Add(new Vector3(-t, 0, -1));
        complex.positions.Add(new Vector3(-t, 0, +1));

        for(int i = 0; i < complex.positions.Count; i++)
        {
            complex.cache[complex.positions[i]] = i;
        }

        complex.cells.Add(new Vector3(0, 11, 5));
        complex.cells.Add(new Vector3(0, 5, 1));
        complex.cells.Add(new Vector3(0, 1, 7));
        complex.cells.Add(new Vector3(0, 7, 10));
        complex.cells.Add(new Vector3(0, 10, 11));

        complex.cells.Add(new Vector3(1, 5, 9));
        complex.cells.Add(new Vector3(5, 11, 4));
        complex.cells.Add(new Vector3(11, 10, 2));
        complex.cells.Add(new Vector3(10, 7, 6));
        complex.cells.Add(new Vector3(7, 1, 8));

        complex.cells.Add(new Vector3(3, 9, 4));
        complex.cells.Add(new Vector3(3, 4, 2));
        complex.cells.Add(new Vector3(3, 2, 6));
        complex.cells.Add(new Vector3(3, 6, 8));
        complex.cells.Add(new Vector3(3, 8, 9));

        complex.cells.Add(new Vector3(4, 9, 5));
        complex.cells.Add(new Vector3(2, 4, 11));
        complex.cells.Add(new Vector3(6, 2, 10));
        complex.cells.Add(new Vector3(8, 6, 7));
        complex.cells.Add(new Vector3(9, 8, 1));

        while (subdivisions-- > 0)
        {
            Subdivide(ref complex);
        }

        List<Vector3> normPos = new List<Vector3>();
        foreach(Vector3 pos in complex.positions)
        {
            normPos.Add(pos.normalized);
        }
        complex.positions = normPos;

        return complex;
    }

    void Subdivide(ref Complex complex)
    {
        List<Vector3> newCells = new List<Vector3>();
        int l = complex.positions.Count;

        foreach(Vector3 cell in complex.cells)
        {
            Vector3 v0 = complex.positions[(int)cell[0]];
            Vector3 v1 = complex.positions[(int)cell[1]];
            Vector3 v2 = complex.positions[(int)cell[2]];

            Vector3 a = Midpoint(v0, v1);
            Vector3 b = Midpoint(v1, v2);
            Vector3 c = Midpoint(v2, v0);

            int ai = AddPoint(a, ref l, ref complex);
            int bi = AddPoint(b, ref l, ref complex);
            int ci = AddPoint(c, ref l, ref complex);
            int v0i = AddPoint(v0, ref l, ref complex);
            int v1i = AddPoint(v1, ref l, ref complex);
            int v2i = AddPoint(v2, ref l, ref complex);

            newCells.Add(new Vector3(v0i, ai, ci));
            newCells.Add(new Vector3(v1i, bi, ai));
            newCells.Add(new Vector3(v2i, ci, bi));
            newCells.Add(new Vector3(ai, bi, ci));
        }

        complex.cells = newCells;
    }

    Vector3 Midpoint(Vector3 a, Vector3 b)
    {
        return new Vector3(
            (a.x + b.x) / 2f,
            (a.y + b.y) / 2f,
            (a.z + b.z) / 2f
        );
    }

    int AddPoint(Vector3 p, ref int index, ref Complex complex)
    {
        if (complex.cache.ContainsKey(p))
        {
            return complex.cache[p];
        }
        else
        {
            complex.positions.Add(p);
            complex.cache[p] = index;
            return index++;
        }
    }
}
