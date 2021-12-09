using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.IO;
using System.Linq;

public class Graph : MonoBehaviour
{
	public GameObject nodePrefab;
	public Canvas menuUI;

    //takes in handle, returns linked list of Nodes
    Dictionary<string, Node> nodeList = new Dictionary<string, Node>();

	public void Start()
	{
		LoadAllFiles();
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
			Debug.Log("Reading File: " + count++);
			LoadFromFile(file);
		}
		Debug.Log(nodeList.Count());
	}

	//creates a root node + assigns following to that node from csv
    void LoadFromFile(string filepath)
	{
        var lines = File.ReadAllLines(filepath);

		Node rootNode = parseLine(lines[0]);
		try
		{
			nodeList.Add(rootNode.handle, rootNode);
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
			}
			catch (Exception e) { }

			rootNode.AddFollowing(tempNode.handle);

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
		//Call Pathfinder.Dijkstra(handle1, handle2)
		//stop timer
		//record time diff
		//update Canvas with results
			//length (if connected)
			//time to complete operation
			//Visualize node connection
		throw new NotImplementedException();
	}

	public void BFSFind()
	{
		//get handles from Canvas
		//check if handles exist in Graph
			//print error if either don't exist
		//start timer
		//Call Pathfinder.Dijkstra(handle1, handle2)
		//stop timer
		//record time diff
		//update Canvas with results
			//length (if connected)
			//time to complete operation
			//Visualize node connection
		throw new NotImplementedException();
	}

}
