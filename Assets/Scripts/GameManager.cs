using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using System;

[Serializable]
public class SavedData
{
    [Serializable]
    public class PosRot
    {
        public float[] pos = new float[3];
        public float[] rot = new float[4];
    }

    public int      cubeSize        = 0;
    public PosRot[] posRot          = null;
    public PosRot   rubiksPosRot    = null;

    public SavedData(int size, int cubeCount)
    {
        cubeSize = size;
        posRot = new PosRot[cubeCount];

        for (uint i = 0u; i < posRot.Length; ++i)
        {
            posRot[i] = new PosRot();
        }

        rubiksPosRot = new PosRot();
    }
}

public class GameManager : MonoBehaviour
{
    // Automatic save
    void Save()
    {
        RubiksCube rubiksCube = FindObjectOfType(typeof(RubiksCube)) as RubiksCube;
        SavedData save = rubiksCube.GetSavedData();
            
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/save.dat");
 
        bf.Serialize(file, save);
        file.Close();
    }

    public static SavedData GetSave()
    {
        if (File.Exists(Application.persistentDataPath + "/save.dat"))
        {
            BinaryFormatter bf      = new BinaryFormatter();
            FileStream      file    = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
            SavedData       data    = bf.Deserialize(file) as SavedData;
            file.Close();
 
            return data;
        }

        return null;
    }

    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Save();
            Application.Quit();
        }
    }
}
