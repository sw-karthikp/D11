using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Google.MiniJSON;

public class Dummy : MonoBehaviour
{
    // Firebase 
    public DatabaseReference database;

    public List<ListOfData> datas= new List<ListOfData>();

    private void Start()
    {
        database = FirebaseDatabase.DefaultInstance.RootReference;
        GetDataFromDatabase();
    }

    private void GetDataFromDatabase()
    {
        FirebaseDatabase.DefaultInstance.GetReference("Ram/TeamPlayers").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot dataSnapshot = task.Result;

                string json = dataSnapshot.GetRawJsonValue();
                Debug.Log(json);

                datas= JsonConvert.DeserializeObject<List<ListOfData>>(json);
            }
        });
    }

    

    [Serializable]
    public class ListOfData
    {
        public string TeamName;
        public JsonObjectAttribute Players;
    }



    [Serializable]
    public class Player
    {
        public string FPoint;
        public string ID;
        public string Name;
        public string Type;
        public string URL;
    }
}
