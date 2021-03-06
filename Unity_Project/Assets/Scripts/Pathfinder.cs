using System.Collections.Generic;
using System;
using System.Text;
using System.Linq;
using UnityEngine;

public static class Pathfinder
{
    public static List<string> Dijkstra(string node1, string  node2, Dictionary<string, List<KeyValuePair<string, float>>> adjList, Dictionary<string, Node> nodeList)
	{
		HashSet<string> computed = new HashSet<string>();
		HashSet<string> process = new HashSet<string>();

		//O(n)
		foreach (var node in adjList)
			process.Add(node.Key);

		Dictionary<string, float> distance = new Dictionary<string, float>(nodeList.Count);
		Dictionary<string, string> previous = new Dictionary<string, string>(nodeList.Count);

		//O(m)
		foreach (var entry in nodeList)
		{
			distance[entry.Key] = Int32.MaxValue;
			previous[entry.Key] = "";
		}
			
		var workingNode = node1;
		distance[workingNode] = 0;

		//O(v(v+e))
		while (process.Count > 0)
		{
			computed.Add(workingNode);

			//O(p)
			foreach (var child in adjList[workingNode])
			{
				//O(1)
				if (distance[child.Key] > distance[workingNode] + child.Value)
				{
					distance[child.Key] = distance[workingNode] + child.Value;
					previous[child.Key] = workingNode;
				}
			}

			var iter = process.GetEnumerator();
			iter.MoveNext();
			var nextNode = iter.Current;
			//O(n)
			for (int i = 0; i < process.Count; i++)
			{
				if (distance[iter.Current] < distance[nextNode])
					nextNode = iter.Current;
				
				iter.MoveNext();
			}

			workingNode = nextNode;
			process.Remove(workingNode);
		}

		List<string> result = new List<string>();

		string curr = node2;

		//O(n)
		while (curr != node1)
		{
			if (!previous.ContainsKey(curr)) { return null; }
			string next = previous[curr];
			result.Add(curr);
			curr = next;
		}

		result.Add(curr);

		result.Reverse();
		return result;
	}
	public static List<string> BFS(string node1, string node2, Dictionary<string, List<KeyValuePair<string, float>>> adjList)
	{
		Queue<KeyValuePair<string, List<string>>> queue = new Queue<KeyValuePair<string, List<string>>>();
		Dictionary<string, bool> visited = new Dictionary<string, bool>();

		//O(n)
		// Set all values in visted to false
		foreach (var entry in adjList)
		{
			visited.Add(entry.Key, false);
		}

		queue.Enqueue(new KeyValuePair<string, List<string>>(node1, new List<string>()));

		//O(e*v)
		while (queue.Count > 0)
		{
			// Skip nodes we have already visited
			if (visited[queue.Peek().Key])
			{
				queue.Dequeue();
				continue;
			}

			foreach (var next in adjList[queue.Peek().Key])
			{
				if (next.Key == node2)
				{
					var parents = queue.Peek().Value;

					parents.Add(queue.Peek().Key);
					parents.Add(node2);

					return parents;
				}

				if (!adjList.ContainsKey(next.Key))
					continue;
				
				List<string> newParents = new List<string>();
				foreach (var newPerson in queue.Peek().Value)
					newParents.Add(newPerson);
				newParents.Add(queue.Peek().Key);
				queue.Enqueue(new KeyValuePair<string, List<string>>(next.Key, newParents));
			}

			visited[queue.Peek().Key] = true;
			queue.Dequeue();
		}

		return null;
	}
}
