using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// This class holds the data for each object that will be batched
/// </summary>
public class ObjData
{
    //location data
    public Vector3 pos;
    public Vector3 scale;
    public Quaternion rot;

    //matrix conversion
    public Matrix4x4 matrix
    {
        get
        {
            return Matrix4x4.TRS(pos, rot, scale);

        }
    }

    //constructor
    public ObjData(Vector3 pos, Vector3 scale, Quaternion rot)
    {
        this.pos = pos;
        this.scale = scale;
        this.rot = rot;
    }
}

/// <summary>
/// This class handles the batching of the models
/// </summary>
public class GPUSpawner : MonoBehaviour
{
    //These arrays hold the location data in vector3 and quaternion form for all of the models that will be batched
    public Vector3[] positions;
    public Vector3[] scales;
    public Quaternion[] rotations;
    //public GameObject[] gameObjects;
    public Mesh objMesh; //the mesh to instance at the location of the object batching
    public Material objMat; //the material to apply to the instanced mesh
    public int batchIndexMax = 942; //the number of instances in each batch, max is 1023

    private List<List<ObjData>> batches = new List<List<ObjData>>(); //the list to keep track of all the batches
    void Start()
    {

        int batchIndexNum = 0;
        List<ObjData> currBatch = new List<ObjData>(); //the current batch
        for (int i = 0; i < positions.Length; i++) //for every position coordinate in the array
        {
            //add an object at that position to the batch
            AddObj(currBatch, i);
            batchIndexNum++;

            if (batchIndexNum >= batchIndexMax) //once enough objects have been added to the batch, build the next batch
            {
                batches.Add(currBatch);
                currBatch = BuildNewBatch();
                batchIndexNum = 0;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        RenderBatches();
    }

    /// <summary>
    /// This method adds a new object to a match given the index of the location data to use
    /// </summary>
    /// <param name="currBatch"></param> The current batch to add to
    /// <param name="i"></param> The index of the position, rotation, and scale in the storage arrays
    private void AddObj(List<ObjData> currBatch, int i)
    {
        currBatch.Add(new ObjData(positions[i], scales[i], rotations[i]));
        // Debug.Log("Added Mesh");
    }
    private List<ObjData> BuildNewBatch()
    {
        return new List<ObjData>();
    }
    /// <summary>
    /// This method renders all of the batches in the list using the mesh instancing method provided in the graphics library
    /// </summary>
    private void RenderBatches()
    {
        foreach (var batch in batches)
        {
            Graphics.DrawMeshInstanced(objMesh, 0, objMat, batch.Select((a) => a.matrix).ToList());
        }
    }
}


