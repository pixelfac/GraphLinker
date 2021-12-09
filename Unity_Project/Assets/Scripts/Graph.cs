using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics;

public class Graph : MonoBehaviour
{
	public GameObject nodePrefab;

	public TMP_InputField fromUser;
	public TMP_InputField toUser;
	public Slider AlgSwitch;
	

    //takes in handle, returns linked list of Nodes
    Dictionary<string, Node> nodeList = new Dictionary<string, Node>();
    Dictionary<string, LinkedList<string>> adjList = new Dictionary<string, LinkedList<string>>();

	public void Start()
	{
		
		Stopwatch clock = new Stopwatch();
		clock.Start();
		LoadAllFiles();
		clock.Stop();
		UnityEngine.Debug.Log(clock.Elapsed.TotalMilliseconds);
		
	}

	//calls LoadFromFile each file in GraphData
	void LoadAllFiles()
	{
		int count = 0;
		var currDir = Directory.GetCurrentDirectory();
		foreach (var file in Directory.GetFiles("Assets/GraphData"))
		{
			//ignore Unity-generated meta files
			if (file.EndsWith(".meta")) { continue; }
			UnityEngine.Debug.Log("Reading File: " + count++);
			LoadFromFile(file);
		}
		UnityEngine.Debug.Log(nodeList.Count());
	}

	//creates a root node + assigns following to that node from csv
    void LoadFromFile(string filepath)
	{
        var lines = File.ReadAllLines(filepath);

		Node rootNode = parseLine(lines[0]);
		try
		{
			nodeList.Add(rootNode.handle, rootNode);
			adjList.Add(rootNode.handle, new LinkedList<string>());
		}
		catch (Exception e) { }

        for (int i=1; i<lines.Length; i++)
		{
			Node tempNode = parseLine(lines[i]);
			if (tempNode is null) { continue; }

			//if node already in nodeList, ignore
			try
			{
				nodeList.Add(tempNode.handle, tempNode);
				adjList[rootNode.handle].AddLast(tempNode.handle);
			}
			catch (Exception e) { }
		}
	}

	//parses each line from csv
	//Format: userID,handle,verified,follwerCount,followingCount
	//returns constructed node from line data
	Node parseLine(string line)
	{
		string[] args = line.Split(',');
		if (args.Length < 5) { return null; }

		string userID = args[0];
		string handle = args[1];
		bool verified = Convert.ToBoolean(args[2]);
		int followerCount = Convert.ToInt32(args[3]);
		int followingCount = Convert.ToInt32(args[4]);

		Node newNode = new Node(userID, handle, verified, followerCount, followingCount);

		return newNode;
	}

	public void DijkstraFind()
	{
		//get handles from Canvas
		//check if handles exist in Graph
		//print error if either don't exist
		//start timer
		//Pathfinder.Dijkstra(fromUserHandle, toUserHandle);

		//stop timer
		//record time diff
		//update Canvas with results
		//length (if connected)
		//time to complete operation
		//Visualize node connection

		GameObject startNode = GameObject.Instantiate(nodePrefab, new Vector3(0, 0, 0), Quaternion.identity);
		startNode.transform.position = new Vector3(-3, -3, 0);
	}

	public void BFSFind()
	{
		//get handles from Canvas
		//check if handles exist in Graph
		//print error if either don't exist
		//start timer
		//Pathfinder.Dijkstra(fromUserHandle, toUserHandle);
		//stop timer
		//record time diff
		//update Canvas with results
			//length (if connected)
			//time to complete operation
			//Visualize node connection
	}

	public void OnClick()
	{
		string fromUserHandle = fromUser.text;
		string toUserHandle = toUser.text;
		//Dij = False, BFS = True
		bool alg = ((int)AlgSwitch.value == 1);

		

		DijkstraFind();
	}


}
