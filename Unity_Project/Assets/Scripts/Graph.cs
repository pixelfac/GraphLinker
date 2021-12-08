using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.IO;
using System.Linq;

public class Graph : MonoBehaviour
{
    //takes in handle, returns linked list of Nodes
    Dictionary<string, Node> nodeList = new Dictionary<string, Node>();

	public void Start()
	{
		LoadAllFiles();
	}

	//calls LoadFromFile each file in GraphData
	void LoadAllFiles()
	{
		var currDir = Directory.GetCurrentDirectory();
		foreach (var file in Directory.GetFiles("Assets/GraphData"))
		{
			Debug.Log(file);
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
		catch (Exception e) { Debug.Log(e.Message); }

        for (int i=1; i<lines.Length; i++)
		{
			Node tempNode = parseLine(lines[i]);
			if (tempNode is null) { continue; }

			//if node already in nodeList, ignore
			try
			{
				nodeList.Add(tempNode.handle, tempNode);
			}
			catch (Exception e) { Debug.Log(e.Message); }

			rootNode.AddFollowing(tempNode.handle);

		}
	}

	//parses each line from csv
	//Format: userID,handle,__,verified,follwerCount,followingCount,__,__
	//returns constructed node from line data
	Node parseLine(string line)
	{
		string[] args = line.Split(',');
		if (args.Length < 8) { return null; }

		string userID = args[0];
		string handle = args[1];
		bool verified = Convert.ToBoolean(args[3]);
		int followerCount = Convert.ToInt32(args[4]);
		int followingCount = Convert.ToInt32(args[5]);

		Node newNode = new Node(userID, handle, verified, followerCount, followingCount);

		return newNode;
	}

}
