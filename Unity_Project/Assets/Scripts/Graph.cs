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

	[Header("UI")]
	public TMP_InputField fromUser;
	public TMP_InputField toUser;
	public Slider AlgSwitch;
	public GameObject ErrorMessageField;
	public GameObject SuccessMessageField;

	[Header("Visualizer")]
	public int depth;
	public int margin;
	

    //takes in handle, returns linked list of Nodes
    Dictionary<string, Node> nodeList = new Dictionary<string, Node>();
    Dictionary<string, List<KeyValuePair<string, float>>> adjList = new Dictionary<string, List<KeyValuePair<string, float>>>();

	public void Start()
	{
		//hide message fields
		ErrorMessageField.SetActive(false);
		SuccessMessageField.SetActive(false);

		//time loading all files
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
			adjList.Add(rootNode.handle, new List<KeyValuePair<string, float>>());
			nodeList.Add(rootNode.handle, rootNode);
		}
		catch (Exception e) { UnityEngine.Debug.Log(e.Message); }

		for (int i=1; i<lines.Length; i++)
		{
			Node tempNode = parseLine(lines[i]);
			if (tempNode is null) { continue; }

			//if node already in nodeList, ignore
			try
			{
				adjList[rootNode.handle].Add(new KeyValuePair<string,float>(tempNode.handle, tempNode.GetEdgeWeight()));
				nodeList.Add(tempNode.handle, tempNode);
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

		string[] pathHandles = { "1", "2", "3", "4", "5" };
		float nodeDist = (18 - (2 * margin)) / (float)(pathHandles.Length-1);


		GameObject[] pathNodes = new GameObject[pathHandles.Length];
		for (int i = 0; i < pathHandles.Length; i++)
		{
			float nodeX = -9 + margin + nodeDist*(i);
			pathNodes[i] = GameObject.Instantiate(nodePrefab, new Vector3(nodeX, depth, 0), Quaternion.identity);
			pathNodes[i].GetComponentInChildren<TextMesh>().text = pathHandles[i];
		}
	}

	public void BFSFind()
	{
		//get handles from Canvas
		string fromHandle = fromUser.text;
		string toHandle = toUser.text;
		//check if handles exist in Graph
		if (!adjList.ContainsKey(fromHandle) || !nodeList.ContainsKey(toHandle))
		{
			//print error if either don't exist
			ErrorMessageField.SetActive(true);
			ErrorMessageField.GetComponent<TextMeshProUGUI>().text = "ERROR: A key doesn't exist!";
			return;
		}

		//start timer
		Stopwatch clock = new Stopwatch();
		clock.Start();
		List<string> pathHandles = Pathfinder.BFS(fromHandle, toHandle, adjList);
		//stop timer
		clock.Stop();
		//record time diff
		double timeMilli = clock.Elapsed.TotalMilliseconds;
		//update Canvas with results
		//length (if connected)
		SuccessMessageField.SetActive(true);
		SuccessMessageField.GetComponent<TextMeshProUGUI>().text = "Length: " + pathHandles.Count() + "\tTime: " + timeMilli + "ms";
		//time to complete operation
		//Visualize node connection
		UnityEngine.Debug.Log(pathHandles.Count());
		for (int i=0; i<pathHandles.Count(); i++)
		{
			UnityEngine.Debug.Log(pathHandles[i]);

		}
	}

	public void OnClick()
	{
		//Dij = False, BFS = True
		bool alg = ((int)AlgSwitch.value == 1);

		if (alg)
		{
			BFSFind();
		}
		else
		{
			DijkstraFind();
		}

	}


}
