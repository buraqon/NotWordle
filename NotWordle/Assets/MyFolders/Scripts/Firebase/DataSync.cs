// using System;
// using UnityEngine;
// using System.Collections.Generic;
// using Firebase.Firestore;


// public class DataSync : MonoBehaviour
// {
//     public static DataSync Instance;  

//     ListenerRegistration _listenerRegistration;
//     List<UserData> userList;
    
//     FirebaseFirestore db;
//     Message message = new Message();

//     private void Awake()
//     {   
//         if (Instance == null)
//         {
//             Instance = this;
//         }
//         else
//         {
//             Debug.LogError("Multiple instances of a singleton in the scene");
//             Destroy(this);
//         }
//         DontDestroyOnLoad(this.gameObject);
//         db = FirebaseFirestore.DefaultInstance;
//     }

//     public void PostMessage(Message message, Action callback, Action<AggregateException> fallback)
//     {
//         db.Collection("Message").Document().SetAsync(message).ContinueWith(task =>
//         {
//             if(task.IsCanceled || task.IsFaulted)
//             {
//                 fallback(task.Exception);
//             }
//             else
//             {
//                 callback();
//             }
//         });
//     }

//     public void PostData()
//     {
//         var gameData = new GameData
//         {
//             data = GameManager.Instance.grid.SetDataTable(),
//             levelData = GameManager.Instance.level
//         };
//         db.Document("GameData/Data").SetAsync(gameData);
//     }

//     public void GetData()
//     {
//         db.Document("GameData/Data").Listen(snapshot =>
//         {
//             DocumentSnapshot documentSnapshot = snapshot;
//             GameData gameData = documentSnapshot.ConvertTo<GameData>();
//             if(!GameManager.Instance.isPlayer)
//                 GameManager.Instance.grid.GetDataTable(gameData.data);
//         });
//     }

//     public void AddUser(string path, string userName, int coins)
//     {
//         var userData = new UserData
//         {
//             Name = userName,
//             Coins = coins
//         };

//         PlayersManager.Instance.myUser = userData;
//         var firestore = FirebaseFirestore.DefaultInstance;
//         firestore.Document(path).SetAsync(userData);
//         GetOwner();
//         GetUsers();
//     }

    

//     public void GetUsers()
//     {
//         FirebaseFirestore firestore = FirebaseFirestore.DefaultInstance;
//         firestore.Collection("user_sheets").Listen(snapshot =>
//         {
//             QuerySnapshot eventsQuerySnapshot = snapshot;
//             userList = new List<UserData>();
//             foreach (DocumentSnapshot documentSnapshot in eventsQuerySnapshot.Documents)
//             {
//                 UserData userData = documentSnapshot.ConvertTo<UserData>();
//                 userList.Add(userData);
//             }
//             PlayersManager.Instance.SetViewers(userList);
//             PlayersManager.Instance.CheckOwner();
//         });
//     }

//     public void GetOwner()
//     {
//         FirebaseFirestore firestore = FirebaseFirestore.DefaultInstance;
//         firestore.Document("owner/owner").Listen(snapshot =>
//         {
//             DocumentSnapshot documentSnapshot = snapshot;
//             UserData owner = documentSnapshot.ConvertTo<UserData>();
//             PlayersManager.Instance.SetOwner(owner);
//         });
//     }
// }
