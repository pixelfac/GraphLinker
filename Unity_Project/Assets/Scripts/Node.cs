using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public string userID { get; }
    public string handle { get; }
    bool verified { get; }
    public int followingCount { get; }
    public int followerCount { get; }
    HashSet<string> followingList;

    public void AddFollowing(string handle)
	{
        followingList.Add(handle);
	}

}
