using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Node
{
    public string userID { get; }
    public string handle { get; }
    public bool verified { get; }
    public int followingCount { get; }
    public int followerCount { get; }
    HashSet<string> followingList = new HashSet<string>();

    public Node(string userID, string handle, bool verified, int followerCount, int followingCount)
	{
        this.userID = userID;
        this.handle = handle;
        this.verified = verified;
        this.followerCount = followerCount;
        this.followingCount = followingCount;
	}

    //adds handle to followingList
    public void AddFollowing(string handle)
	{
        followingList.Add(handle);
	}

    //returns true if handle is in followingList, false otherwise
    public bool IsFollowing(string handle)
	{
        return followingList.Contains(handle);
	}

    //returns edge weight for any edge going to this node
    public int GetEdgeWeight()
	{
        return followerCount;
	}




}
