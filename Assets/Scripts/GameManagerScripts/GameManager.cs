//using System.Collections;
//using System.Collections.Generic;
//using Photon.Pun;
//using UnityEngine;

//public class GameManager : MonoBehaviourPunCallbacks
//{
//    // [SerializeField] private UIManager uiManager;
//    [SerializeField] private PlayerVR playerVR;
//    [SerializeField] private MeshStratch EventObj = null;
//    public Transform[] testSpawnPoints = null;
//    public GameObject[] testPlayer = null;
//    public int gameTurn = 0;
//    public bool isGameStart = false;



//    private void Awake()
//    {   
//        InitializeGame();
//        // testPlayer = Resources.Load("TestCannon") as GameObject;
//    }
//    private void InitializeGame()
//    {
//        gameTurn = 0;

//        for(int i = 0; i < testPlayer.GetLength(0) ; ++i)
//        {
//            testPlayer[i].transform.position = testSpawnPoints[i].position;
//            testPlayer[i].transform.rotation = testSpawnPoints[i].rotation;
//        }
//        testPlayer[1].GetComponent<Cannon>().isMyTurn = true;

//    }

//    /// <summary>
//    /// Todo List
//    /// 1. Initialize WorldMap
//    /// 2. Instantiate Player Character 
//    /// 3. Player Position Initialize
//    /// </summary>
//    /// 

//    [PunRPC]
//    private void StartGame()
//    {   
//        ++gameTurn;
//        foreach(GameObject g in testPlayer)
//        {
//            g.GetComponent<Cannon>().isMyTurn = !g.GetComponent<Cannon>().isMyTurn;

//            if(g.GetComponent<Cannon>().isMyTurn) g.GetComponent<Cannon>().startTurn();
//            else g.GetComponent<Cannon>().EndTurn();
//        }

//        Debug.LogError(gameTurn);
//    }

//    private void Update()
//    {
//        if(Input.GetKeyDown(KeyCode.A))
//        {
//            Debug.Log("Turn Start");
//            // StartGame();
//            photonView.RPC("StartGame", RpcTarget.All);
//        }
//    }





//}
