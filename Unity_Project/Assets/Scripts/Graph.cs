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
	private GameObject[] pathNodes = null;
	public LineRenderer line;

	[Header("UI")]
	public TMP_InputField fromUser;
	public TMP_InputField toUser;
	public Slider AlgSwitch;
	public GameObject ErrorMessageField;
	public GameObject SuccessMessageField;

	[Header("Visualizer")]
	public int depth;
	public int margin;

	[Header("Stats Page")]
	public TextMeshProUGUI lineCount;
	public TextMeshProUGUI numFiles;
	public TextMeshProUGUI timeToLoad;
	public TextMeshProUGUI sizeOfDict;



	//takes in handle, returns linked list of Nodes
	Dictionary<string, Node> nodeList = new Dictionary<string, Node>();
    Dictionary<string, List<KeyValuePair<string, float>>> adjList = new Dictionary<string, List<KeyValuePair<string, float>>>();

	public void Start()
	{
		//hide line
		line.SetPosition(0, new Vector3(-2, -7, 0));
		line.SetPosition(1, new Vector3(-2, 7, 0));

		//hide message fields
		ErrorMessageField.SetActive(false);
		SuccessMessageField.SetActive(false);

		//time loading all files
		Stopwatch clock = new Stopwatch();
		clock.Start();
		LoadAllFiles();
		clock.Stop();
		UnityEngine.Debug.Log(clock.Elapsed.TotalMilliseconds);

		//set Stats in Stats Page
		var lines = 0;
		foreach (var file in Directory.GetFiles("Assets/GraphData"))
		{
			if (file.EndsWith(".meta")) { continue; }
			lines += File.ReadAllLines(file).Count();
		}
		lineCount.text = "Lines Parsed: " + lines;
		timeToLoad.text = "Time to Load Data: " + clock.Elapsed.TotalMilliseconds + "ms";
		sizeOfDict.text = "Number of Vertices: " + nodeList.Count();


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
		numFiles.text = "Number of Files Parsed: " + count;
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
		string fromHandle = fromUser.text;
		string toHandle = toUser.text;
		//check if handles exist in Graph
		if (!adjList.ContainsKey(fromHandle) || !nodeList.ContainsKey(toHandle))
		{
			//print error if either don't exist
			ErrorMessageField.SetActive(true);
			SuccessMessageField.SetActive(false);
			ErrorMessageField.GetComponent<TextMeshProUGUI>().text = "ERROR: A key doesn't exist!";
			return;
		}
		//start timer
		Stopwatch clock = new Stopwatch();
		clock.Start();
		List<string> pathHandles = Pathfinder.Dijkstra(fromHandle, toHandle, adjList, nodeList);
		//stop timer
		clock.Stop();
		//record time diff
		double timeMilli = clock.Elapsed.TotalMilliseconds;

		if (pathHandles == null)
		{
			//print error if no path
			ErrorMessageField.SetActive(true);
			SuccessMessageField.SetActive(false);
			ErrorMessageField.GetComponent<TextMeshProUGUI>().text = "ERROR: A link doesn't exist!";
			return;
		}

		//update Canvas with results
		//length (if connected)
		ErrorMessageField.SetActive(false);
		SuccessMessageField.SetActive(true);
		//time to complete operation
		SuccessMessageField.GetComponent<TextMeshProUGUI>().text = "Length: " + pathHandles.Count() + "\tTime: " + timeMilli + "ms";
		UnityEngine.Debug.Log(pathHandles.Count());
		for (int i = 0; i < pathHandles.Count(); i++)
		{
			UnityEngine.Debug.Log(pathHandles[i]);

		}

		//Visualize node connection
		DrawNodes(pathHandles, true);
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
			SuccessMessageField.SetActive(false);
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

		if (pathHandles == null)
		{
			//print error if no path
			ErrorMessageField.SetActive(true);
			SuccessMessageField.SetActive(false);
			ErrorMessageField.GetComponent<TextMeshProUGUI>().text = "ERROR: A link doesn't exist!";
			return;
		}
		//update Canvas with results
		//length (if connected)
		ErrorMessageField.SetActive(false);
		SuccessMessageField.SetActive(true);
		//time to complete operation
		SuccessMessageField.GetComponent<TextMeshProUGUI>().text = "Length: " + pathHandles.Count() + "\tTime: " + timeMilli + "ms";
		UnityEngine.Debug.Log(pathHandles.Count());
		for (int i=0; i<pathHandles.Count(); i++)
		{
			UnityEngine.Debug.Log(pathHandles[i]);

		}

		//Visualize node connection
		DrawNodes(pathHandles, false);
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

	void DrawNodes(List<string> pathHandles, bool isDijk)
	{
		float nodeDist = (18 - (2 * margin)) / (float)(pathHandles.Count() - 1);

		//clear old nodes  if they exist
		if (pathNodes != null)
		{
			for (int i = 0; i < pathNodes.Length; i++)
			{
				Destroy(pathNodes[i]);
			}
		}
		line.gameObject.SetActive(false);

		line.SetPosition(0, new Vector3(-7,-2,0));
		line.SetPosition(1, new Vector3(7, -2, 0));
		line.gameObject.SetActive(true);


		//make and draw new nodes
		pathNodes = new GameObject[pathHandles.Count()];
		for (int i = 0; i < pathHandles.Count(); i++)
		{
			float nodeX = -9 + margin + nodeDist * (i);
			pathNodes[i] = GameObject.Instantiate(nodePrefab, new Vector3(nodeX, depth, 0), Quaternion.identity);
			pathNodes[i].GetComponentsInChildren<TextMesh>()[0].text = pathHandles[i];
			if (isDijk)
			{
				pathNodes[i].GetComponentsInChildren<TextMesh>()[1].text = "Weight: " + nodeList[pathHandles[i]].GetEdgeWeight();
			} 
			else
			{
				pathNodes[i].GetComponentsInChildren<TextMesh>()[1].text = "";
			}
		}
	}


}
