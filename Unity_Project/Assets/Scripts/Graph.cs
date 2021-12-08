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
    static Dictionary<string, Node> nodeList;

	public void Start()
	{
		LoadAllFiles();
	}

	static void LoadAllFiles()
	{
		//TODO
		//call LoadFromFile for each .csv in data folder
		var currDir = Directory.GetCurrentDirectory();
		foreach (var file in Directory.GetFiles("Assets/GraphData"))
		{
			LoadFromFile(file);
		}
		Debug.Log(nodeList.Count());
	}

    static void LoadFromFile(string filepath)
	{
        var lines = File.ReadAllLines(filepath);

		Node rootNode = parseLine(lines[0]);
		nodeList.Add(rootNode.handle, rootNode);

        for (int i=1; i<lines.Length; i++)
		{
			Node tempNode = parseLine(lines[i]);

			//if node already in nodeList, ignore
			try
			{
				nodeList.Add(tempNode.handle, tempNode);
			}
			catch (Exception e) { }

			rootNode.AddFollowing(tempNode.handle);

		}
	}

	static Node parseLine(string line)
	{
		Node newNode = new Node();

		//TODO
		//parse csv line into Node object

		return newNode;
	}

}
