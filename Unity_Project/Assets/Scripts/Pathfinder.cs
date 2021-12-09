using System.Collections.Generic;
using System;
using System.Text;
using System.Linq;

public static class Pathfinder
{
    public static List<string> Dijkstra(string node1, string  node2)
	{
		throw new NotImplementedException("Dikstra's Not Yet Implemented");
	}
	public static List<string> BFS(string node1, string node2, Dictionary<string, List<string>> adjList)
	{
		Queue<KeyValuePair<string, List<string>>> queue;
		Dictionary<string, bool> visited = new Dictionary<string, bool>();

		// Set all values in visted to false
		foreach (var entry in adjList)
			visited.Add(entry.Key, false);		

		queue.Enqueue(new KeyValuePair<string, List<string>>(node1, new List<string>()));

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
				if (next == node2)
				{
					var parents = queue.Peek().Value;

					parents.Add(queue.Peek().Key);
					parents.Add(node2);

					return parents;
				}
				
				var parent = queue.Peek().Value;
				parent.Add(queue.Peek().Key);
				queue.Enqueue(new KeyValuePair<string, List<string>>(next, parent));
			}

			visited[queue.Peek().Key] = true;
			queue.Dequeue();
		}

		return null;
	}
}
