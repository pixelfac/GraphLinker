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

    public Node(string userID, string handle, bool verified, int followerCount, int followingCount)
	{
        this.userID = userID;
        this.handle = handle;
        this.verified = verified;
        this.followerCount = followerCount;
        this.followingCount = followingCount;
	}

    //returns edge weight for any edge going to this node
    public float GetEdgeWeight()
	{
        return (1 / (float)followerCount) * 10000000;
	}




}
