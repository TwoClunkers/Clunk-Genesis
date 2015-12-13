
using System;
using System.Collections.Generic;
using UnityEngine;



namespace DataObjects
{
	public class Schematic
	{
		public Dictionary<int, Part> parts = new Dictionary<int, Part> ();
		private int nextIndex = 0;

		public Schematic ()
		{
		}

		public void startSchematic(Part firstPart)
		{
			parts.Clear ();
			PartTag newPart = new PartTag ();
			newPart.index = 0;
			newPart.level = 0;
			newPart.node = -1;
			newPart.parent = -1;

			firstPart.tag = newPart;

			parts.Add (0, firstPart); //this places it in our collection
			nextIndex = 1; //this starts the count in this new collection
		}

		public bool connect (Part newPart, Part targetPart, int nodeIndex)
		{
			Node parentNode = targetPart.getNode(nodeIndex);
			if (parentNode == null)
				return false; //wrong index
			if (!parentNode.validate (newPart))
				return false; //node says we can't connect

			PartTag newTag = new PartTag ();
			PartTag targetTag = targetPart.tag;
			
			newTag.index = nextIndex;
			newTag.level = targetTag.level + 1;
			newTag.parent = targetTag.index;
			newTag.node = nodeIndex;

			newPart.tag = newTag; //this puts our new tag into our Part struct
			parts.Add (newTag.index, newPart); //and into collection

			parentNode.attach (newTag); //so parent can find us in the schematic if needed. 

			nextIndex += 1; //next connection in schematic gets a new index

			return true;
		}

		public bool partExists(int attemptedPart)
		{
			Part temp = null;
			if (parts.TryGetValue (attemptedPart, out temp)) {
				//the part exits in our collection
				return true;
			} else
				return false;
		}

		public void copyBranch(Part targetPart, Schematic targetSchema, int nodeindex)
		{
			//this function should create and return a copy of this schematic
			//starting at the part specified.
			Part newPart = targetPart.getCopy ();
			//the first part is special as it has no parent
			targetSchema.startSchematic (newPart);
			//then we copy through the rest of our parts
			//you registered the first piece, now we need to set the nodes
			//we get the source info from the old tags, but have to connect to newPart in the targetSchema
			for (int i = 0; i<targetPart.nodes.Length; i+=1) {
				Node thisNode = targetPart.getNode (i);
				if (thisNode == null)
					continue; //no more nodes
				PartTag attachedTag = thisNode.getPartTag ();
				if (attachedTag == null)
					continue; //nothing attached
				Part attachedPart = getPart (attachedTag.index);
				if(attachedPart == null) {
					Debug.Log("getPart failed on good index!");
					continue; //failed reference
				}
				//okay, so we have a hot node on the source Part
				copyStem(attachedPart, targetSchema, 1, i);

			}

		}
		public void copyStem(Part targetPart, Schematic targetSchema, int targetIndex, int nodeIndex)
		{
			Part partCopy = targetPart.getCopy (); //copy within this schema
			Part parentPart = targetSchema.getPart(targetIndex); //parent within new schema

			if (parentPart != null) {
				Debug.Log("copyStem failed!");
			}

			Node targetNode = parentPart.getNode (nodeIndex);

			parentPart.clearNode (nodeIndex); //clear off the old referance
			//the new copy gets put into the new schema
			targetSchema.connect (partCopy, parentPart, nodeIndex);

			//now we need to set the nodes
			for (int i = 0; i<targetPart.nodes.Length; i+=1) {
				Node thisNode = targetPart.getNode (i);
				if (thisNode == null)
					continue; //no more nodes
				PartTag attachedTag = thisNode.getPartTag ();
				if (attachedTag == null)
					continue; //nothing attached
				Part attachedPart = getPart (attachedTag.index);
				if(attachedPart == null) {
					Debug.Log("getPart failed on good index!");
					continue; //failed reference
				}
				//okay, so we have a hot node on the source Part
				//we are sending the part we found to the new schema, giving the new index and
				//the node that we are currenty looking at
				copyStem(attachedPart, targetSchema, partCopy.tag.index, i);
				
			}
		}

		public Part getPart(int partIndex)
		{
			Part temp = null;
			if (parts.TryGetValue (partIndex, out temp)) {
				//the part exits in our collection

				//return the referance or else null
				return temp;
			} else
				return null;
		}


	}


}

