using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeonEffect : MonoBehaviour
{

    [SerializeField] private float lineSize;
    [SerializeField] private Color lineColor;
    [SerializeField] private Color fillColor;
    private MeshFilter meshFilter;
	// Use this for initialization
	void Start ()
	{
	    setNeonLine();
	}


    public void setNeonLine()
    {
        meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            throw new Exception("No meshFilter on object");
        }
        GetComponent<MeshRenderer>().material = Resources.Load("materials/BlockMaterial") as Material;
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        meshFilter.sharedMesh = Instantiate(cube.GetComponent<MeshFilter>().sharedMesh);
        DestroyImmediate(cube);

        

        Mesh mesh = meshFilter.sharedMesh;


        List<Vector3> vertex = new List<Vector3>(mesh.vertices);
        int vertexSize = vertex.Count;
        vertex.AddRange(new Vector3[mesh.vertices.Length]);
        bool[] boolVertex = new bool[vertex.Count];

        List<int> triangles = new List<int>(mesh.triangles);
        List<Vector3> normals = new List<Vector3>(mesh.normals);
        normals.AddRange(new Vector3[mesh.vertices.Length]);
        List<Color> colors = new List<Color>(new Color[vertex.Count]);

        int trianglesSize = triangles.Count;
        List<int> newTriangles = new List<int>();

        for (var i = 0; i < trianglesSize; i += 3)
        {
            int index = -1;

            if (Math.Abs(Vector3.Dot(vertex[triangles[i + 1]] - vertex[triangles[i]], vertex[triangles[i + 2]] - vertex[triangles[i]])) <= float.Epsilon)
            {
                index = i;
            }
            else if (Math.Abs(Vector3.Dot(vertex[triangles[i + 1]] - vertex[triangles[i + 2]], vertex[triangles[i]] - vertex[triangles[i + 2]])) <= float.Epsilon)
            {
                index = i + 2;
            }
            else if (Math.Abs(Vector3.Dot(vertex[triangles[i + 2]] - vertex[triangles[i + 1]], vertex[triangles[i]] - vertex[triangles[i + 1]])) <= float.Epsilon)
            {
                index = i + 1;
            }

            if (index == -1)
            {
                return;
            }
            Vector3 temp = ((vertex[triangles[(index + 1) % 3 + i]] - vertex[triangles[index]]) +
                            (vertex[triangles[(index + 2) % 3 + i]] - vertex[triangles[index]]));
            temp.Normalize();
            temp *= lineSize;
            temp.x /= transform.lossyScale.x;
            temp.y /= transform.lossyScale.y;
            temp.z /= transform.lossyScale.z;

            vertex[triangles[index] + vertexSize] = temp + vertex[triangles[(index)]];
            normals[triangles[index] + vertexSize] = normals[triangles[index]];

            newTriangles.Add(triangles[index]);
            newTriangles.Add(triangles[(index + 1) % 3 + i]);
            newTriangles.Add(triangles[index] + vertexSize);

            newTriangles.Add(triangles[index]);
            newTriangles.Add(triangles[index] + vertexSize);
            newTriangles.Add(triangles[(index + 2) % 3 + i]);

            for (int j = 1; j < 3; ++j)
            {
                if (!boolVertex[triangles[(index + j) % 3 + i] + vertexSize])
                {
                    Vector3 t = ((vertex[triangles[index]] - vertex[triangles[(index + j) % 3 + i]]) +
                                 (vertex[triangles[(index + j + (j == 1 ? 1 : 2)) % 3 + i]] - vertex[triangles[index]]));
                    t.Normalize();
                    t *= lineSize;
                    t.x /= transform.lossyScale.x;
                    t.y /= transform.lossyScale.y;
                    t.z /= transform.lossyScale.z;

                    vertex[triangles[(index + j) % 3 + i] + vertexSize] = t + vertex[triangles[(index + j) % 3 + i]];
                    normals[triangles[(index + j) % 3 + i] + vertexSize] = normals[triangles[(index + j) % 3 + i]];
                    boolVertex[triangles[(index + j) % 3 + i] + vertexSize] = true;
                }

                if (j != 2)
                {
                    newTriangles.Add(triangles[(index + j) % 3 + i]);
                    newTriangles.Add(triangles[(index + j) % 3 + i] + vertexSize);
                    newTriangles.Add(triangles[index] + vertexSize);
                }
                else
                {
                    newTriangles.Add(triangles[(index + j) % 3 + i]);
                    newTriangles.Add(triangles[index] + vertexSize);
                    newTriangles.Add(triangles[(index + j) % 3 + i] + vertexSize);

                }
            }
        }

        for (var i = 0; i < trianglesSize; i += 3)
        {

            colors[triangles[i]] = colors[triangles[i + 1]] = colors[triangles[i + 2]] =
                colors[triangles[i] + vertexSize] =
                    colors[triangles[i + 1] + vertexSize] = colors[triangles[i + 2] + vertexSize] = lineColor;

            for (int j = 0; j < 3; ++j)
            {
                vertex.Add(vertex[triangles[i + j] + vertexSize]);
                newTriangles.Add(vertex.Count - 1);
                normals.Add(normals[triangles[i + j] + vertexSize]);
                colors.Add(fillColor);
            }

        }

        meshFilter.sharedMesh.vertices = vertex.ToArray();
        meshFilter.sharedMesh.normals = normals.ToArray();
        meshFilter.sharedMesh.triangles = newTriangles.ToArray();
        meshFilter.sharedMesh.colors = colors.ToArray();
    }
}
