/// <Licensing>
/// ©2011-2014 (Copyright) Path-o-logical Games, LLC
/// If purchased from the Unity Asset Store, the following license is superseded 
/// by the Asset Store license.
/// Licensed under the Unity Asset Package Product License (the "License");
/// You may not use this file except in compliance with the License.
/// You may obtain a copy of the License at: http://licensing.path-o-logical.com
/// </Licensing>
using UnityEngine;
using System.Collections.Generic;


namespace PathologicalGames
{
    /// <summary>
    ///	Adding this component to a gameObject will make it detectable to TargetTrackers, recieve 
	/// EventInfo and expose event delegates to run attached custom compponent methods.
    /// </summary>
    [AddComponentMenu("Path-o-logical/TriggerEventPRO/TriggerEventPRO Targetable")]
#if (UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2)
	[RequireComponent(typeof(Rigidbody))]
#endif
    public class Targetable : MonoBehaviour
    {
        #region Public Parameters
		/// <summary>
		/// Indicates whether this <see cref="PathologicalGames.Targetable"/> is targetable. 
		/// If the Targetable is being tracked when this is set to false, it will be removed 
		/// from all Areas. When set to true, it will be added to any Perimieters it is 
		/// inside of, if applicable.
		/// </summary>
		public bool isTargetable
		{
		    get 
			{ 
				// Don't allow targeting if disabled in any way.
				//	If both are false and then the gameobject becomes active, Perimieters will  
				//	detect this rigidbody enterig. Returning false will prevent this odd behavior.
				if (!this.go.activeInHierarchy || !this.enabled)
					return false;
				
				return this._isTargetable; 
			}

		    set
		    {
				// Singleton. Only do something if value changed
				if (this._isTargetable == value)
					return;
						
		        this._isTargetable = value;
				
				// Don't execute logic if this is disabled in any way.
				if (!this.go.activeInHierarchy || !this.enabled)
					return;
				
		        if (!this._isTargetable)
		            this.CleanUp();
				else
					this.BecomeTargetable();
		    }
		}

		public bool _isTargetable = true;  // Public for inspector use

        public DEBUG_LEVELS debugLevel = DEBUG_LEVELS.Off;

        internal List<TargetTracker> trackers = new List<TargetTracker>();

        // Delegate type declarations
		//! [snip_OnDetectedDelegate]
        public delegate void OnDetectedDelegate(TargetTracker source);
		//! [snip_OnDetectedDelegate]
		//! [snip_OnNotDetectedDelegate]
        public delegate void OnNotDetectedDelegate(TargetTracker source);
		//! [snip_OnNotDetectedDelegate]
		//! [snip_OnHit]
        public delegate void OnHitDelegate(EventInfoList eventInfoList, Target target);
		//! [snip_OnHit]
        #endregion Public Parameters


        #region protected Parameters
        public Transform xform;
        public GameObject go;
		public Collider coll;
#if (!UNITY_4_0 && !UNITY_4_0_1 && !UNITY_4_1 && !UNITY_4_2)
		public Collider2D coll2D;
#endif
        // Internal lists for each delegate type
        protected OnDetectedDelegate onDetectedDelegates;
        protected OnNotDetectedDelegate onNotDetectedDelegates;
        protected OnHitDelegate onHitDelegates;	
        #endregion protected Parameters


        #region Events
        protected void Awake()
        {
            // Cache
            this.xform = this.transform;
            this.go = this.gameObject;				
			this.coll = this.collider;
#if (!UNITY_4_0 && !UNITY_4_0_1 && !UNITY_4_1 && !UNITY_4_2)
			this.coll2D = this.collider2D;
			
			if (this.rigidbody == null && this.rigidbody2D == null)
			{
				string msg = "Targetables must have a Rigidbody or Rigidbody2D.";
				throw new MissingComponentException(msg);
			}
#endif
        }

//        protected void OnEnable() 
//		{ 
//			if (this.isTargetable) 
//				this.BecomeTargetable(); 
//		}
        protected void OnDisable() { this.CleanUp(); }
		
        protected void OnDestroy() { this.CleanUp(); }
        protected void CleanUp()
        {
            if (!Application.isPlaying) return; // Game was stopped.
			
			var copy = new List<TargetTracker>(this.trackers);
            foreach (TargetTracker tt in copy)
            {
                // Protect against async destruction
                if (tt == null || tt.area == null || tt.area.Count == 0)
                    continue;

                tt.area.Remove(this);
#if UNITY_EDITOR
                if (this.debugLevel > DEBUG_LEVELS.Off)
                    Debug.Log(string.Format
                    (
                        "Targetable ({0}): On Disabled, Destroyed or !isTargetable- " +
                        	"Removed from {1}.",
                        this.name,
                        tt.name
                    ));
#endif
            }
			
			this.trackers.Clear();

        }
				
		protected void BecomeTargetable()
		{
			// Toggle the collider, only if it was enabled to begin with, to let the physics 
			//   systems refresh.
			if (this.coll != null && this.coll.enabled)
			{
				this.coll.enabled = false;
				this.coll.enabled = true;
			}
#if (!UNITY_4_0 && !UNITY_4_0_1 && !UNITY_4_1 && !UNITY_4_2)
			else if (this.coll2D != null && this.coll2D.enabled)
			{
				this.coll2D.enabled = false;
				this.coll2D.enabled = true;
			}
#endif
			#region Old way...
//			//
//			// OLD WAY. Working code. However, this will be slightly less accurate. This was good 
//			//          because it relied on code-based detection instead of toggling tricks.
//			//          With the new iterations of this system, the toggling seems better
//			//
//			// Make the OverlapSphere test more accurate by trying to increase the 
//			//	test radius based on this Targetable's collider. This has to 
//			//	use the largest dimension for non-uniform sized colliders to avoid 
//			//  situations where the object is inside but not added.
//			//
//			// NOTE: Area.IsInRange uses this pattern as well.
//			//
//			float testSize = 0.01f;
//			Area currArea;
//			var areas = new List<Area>();
//			if (this.coll != null)
//			{
//				Vector3 size = this.coll.bounds.size;
//				testSize = Mathf.Max(Mathf.Max(size.x, size.y), size.z);
//				Collider[] colls = Physics.OverlapSphere(this.xform.position, testSize);
//				for (int i = 0; i < colls.Length; i++)
//				{
//					// Ignore this Targetable's own collider
//					if (colls[i] == this.coll)
//						continue;
//					
//					// Ignore colliders that are not attached to a Area
//					currArea = colls[i].GetComponent<Area>();
//					if (currArea == null)
//						continue;
//
//					// Found one. Keep it
//					areas.Add(currArea);
//				}
//			}
//#if (!UNITY_4_0 && !UNITY_4_0_1 && !UNITY_4_1 && !UNITY_4_2)
//			else if (this.coll2D != null)
//			{
//				var coll2Ds = new List<Collider2D>();
//				var box2d = this.coll2D as BoxCollider2D;
//				if (box2d != null)
//				{
//					var pos2D = new Vector2(this.xform.position.x, this.xform.position.y);
//					Vector2 worldPos2D = box2d.center + pos2D;
//					Vector2 extents = box2d.size * 0.5f;
//
//					var pntA = worldPos2D + extents;
//					var pntB = worldPos2D - extents;
//
//					coll2Ds.AddRange(Physics2D.OverlapAreaAll(pntA, pntB));
//				}
//				else
//				{
//					var circ2D = this.coll2D as CircleCollider2D;
//					if (circ2D != null)
//					{
//						coll2Ds.AddRange
//						(
//							Physics2D.OverlapCircleAll(this.xform.position, circ2D.radius * 2)
//						);
//					}
//				}
//				for (int i = 0; i < coll2Ds.Count; i++)
//				{
//					// Ignore this Targetable's own collider
//					if (coll2Ds[i] == this.coll)
//						continue;
//					
//					// Ignore colliders that are not attached to a Area
//					currArea = coll2Ds[i].GetComponent<Area>();
//					if (currArea == null)
//						continue;
//					
//					// Found one. Keep it
//					areas.Add(currArea);
//				}
//			}
//#endif
//			else
//			{
//				throw new System.Exception("Unexpected Error: No colliders set!");
//			}
//
//			for (int i = 0; i < areas.Count; i++)
//			{
//				currArea = areas[i];
//				
//#if UNITY_EDITOR
//                if (this.debugLevel > DEBUG_LEVELS.Off)
//                {
//					Debug.Log(string.Format
//                    (
//                        "Targetable ({0}): Becoming Targetable - " +
//                        	"Adding to perimieter {1}.",
//                        this.name,
//						currArea.name
//					));
//                }
//#endif
//				currArea.Add(this);
//				this.trackers.Add(currArea.targetTracker);
//			}
			#endregion
		}
		


        /// <summary>
        /// Triggered when a target is hit
        /// </summary>
		/// <param name="source">The EventInfoList to send</param>
        /// <param name="source">
        /// The target struct used to cache this target when sent
        /// </param>
        public void OnHit(EventInfoList eventInfoList, Target target)
        {
#if UNITY_EDITOR
            // Normal level debug and above
            if (this.debugLevel > DEBUG_LEVELS.Off)
            {
                Debug.Log
                (
                    string.Format
                    (
						"Targetable ({0}): EventInfoList[{1}]",
                        this.name,
                        eventInfoList.ToString()
                    )
                );
            }
#endif

            // Set the hitTime for all eventInfos in the list.
            eventInfoList = eventInfoList.CopyWithHitTime();

            if (this.onHitDelegates != null)
                this.onHitDelegates(eventInfoList, target);
        }

        /// <summary>
        /// Triggered when a target is first found by an Area
        /// </summary>
        /// <param name="source">The TargetTracker which triggered this event</param>
        internal void OnDetected(TargetTracker source)
        {
#if UNITY_EDITOR
            // Higest level debug
            if (this.debugLevel > DEBUG_LEVELS.Normal)
            {
                string msg = "Detected by " + source.name;
                Debug.Log(string.Format("Targetable ({0}): {1}", this.name, msg));
            }
#endif
			this.trackers.Add(source);

            if (this.onDetectedDelegates != null) this.onDetectedDelegates(source);
        }

        /// <summary>
		/// Triggered when a target is first found by an Area
        /// </summary>
        /// <param name="source">The TargetTracker which triggered this event</param>
        internal void OnNotDetected(TargetTracker source)
        {
#if UNITY_EDITOR
            // Higest level debug
            if (this.debugLevel > DEBUG_LEVELS.Normal)
            {
                string msg = "No longer detected by " + source.name;
                Debug.Log(string.Format("Targetable ({0}): {1}", this.name, msg));
            }
#endif
			this.trackers.Remove(source);

			if (this.onNotDetectedDelegates != null) this.onNotDetectedDelegates(source);
        }
        #endregion Events



        #region Target Tracker Members
        public float strength { get; set; }

        /// <summary>
        /// Waypoints is just a list of positions used to determine the distance to
        /// the final destination. See distToDest.
        /// </summary>
        [HideInInspector]
        public List<Vector3> waypoints = new List<Vector3>();

        /// <summary>
        /// Get the distance from this GameObject to the nearest waypoint and then
        /// through all remaining waypoints.
        /// Set wayPoints (List of Vector3) to use this feature.
        /// The distance is kept as a sqrMagnitude for faster performance and
        /// comparison.
        /// </summary>
        /// <returns>The distance as sqrMagnitude</returns>
        public float distToDest
        {
            get
            {
                if (this.waypoints.Count == 0) return 0;  // if no points, return

                // First get the distance to the first point from the current position
                float dist = this.GetSqrDistToPos(waypoints[0]);

                // Add the distance to each point from the one before.
                for (int i = 0; i < this.waypoints.Count - 2; i++)  // -2 keeps this in bounds
                    dist += (waypoints[i] - waypoints[i + 1]).sqrMagnitude;

                return dist;
            }
        }


        /// <summary>
        /// Get the distance from this Transform to another position in space.
        /// The distance is returned as a sqrMagnitude for faster performance and
        /// comparison
        /// </summary>
        /// <param name="other">The position to find the distance to</param>
        /// <returns>The distance as sqrMagnitude</returns>
        public float GetSqrDistToPos(Vector3 other)
        {
            return (this.xform.position - other).sqrMagnitude;
        }

        /// <summary>
        /// Get the distance from this Transform to another position in space.
        /// The distance is returned as a float for simple min/max testing, etc. 
        /// For distance comparisons, use GetSqrDistToPos(...)
        /// </summary>
        /// <param name="other">The position to find the distance to</param>
        /// <returns>The distance as a simple float</returns>
        public float GetDistToPos(Vector3 other)
        {
            return (this.xform.position - other).magnitude;
        }


        #region Delegate Add/Set/Remove Functions
        #region OnDetectedDelegates
        /// <summary>
		/// Add a new delegate to be triggered when a target is first found by an Area.
        /// The delegate signature is:  delegate(TargetTracker source)
        /// See TargetTracker documentation for usage of the provided 'source'
        /// **This will only allow a delegate to be added once.**
        /// </summary>
        /// <param name="del"></param>
        public void AddOnDetectedDelegate(OnDetectedDelegate del)
        {
            this.onDetectedDelegates -= del;  // Cheap way to ensure unique (add only once)
            this.onDetectedDelegates += del;
        }

        /// <summary>
        /// This replaces all older delegates rather than adding a new one to the list.
        /// See docs for AddOnDetectedDelegate()
        /// </summary>
        /// <param name="del"></param>
        public void SetOnDetectedDelegate(OnDetectedDelegate del)
        {
            this.onDetectedDelegates = del;
        }

        /// <summary>
        /// Removes a OnDetectedDelegate 
        /// See docs for AddOnDetectedDelegate()
        /// </summary>
        /// <param name="del"></param>
        public void RemoveOnDetectedDelegate(OnDetectedDelegate del)
        {
            this.onDetectedDelegates -= del;
        }
        #endregion OnDetectedDelegates


        #region OnNotDetectedDelegate
        /// <summary>
        /// Add a new delegate to be triggered when a target is dropped by a perimieter for
        /// any reason; leaves or is removed.
        /// The delegate signature is:  delegate(TargetTracker source)
        /// See TargetTracker documentation for usage of the provided 'source'
        /// **This will only allow a delegate to be added once.**
        /// </summary>
        /// <param name="del"></param>
        public void AddOnNotDetectedDelegate(OnNotDetectedDelegate del)
        {
            this.onNotDetectedDelegates -= del;  // Cheap way to ensure unique (add only once)
            this.onNotDetectedDelegates += del;
        }

        /// <summary>
        /// This replaces all older delegates rather than adding a new one to the list.
        /// See docs for AddOnNotDetectedDelegate()
        /// </summary>
        /// <param name="del"></param>
        public void SetOnNotDetectedDelegate(OnNotDetectedDelegate del)
        {
            this.onNotDetectedDelegates = del;
        }

        /// <summary>
        /// Removes a OnNotDetectedDelegate 
        /// See docs for AddOnNotDetectedDelegate()
        /// </summary>
        /// <param name="del"></param>
        public void RemoveOnNotDetectedDelegate(OnNotDetectedDelegate del)
        {
            this.onNotDetectedDelegates -= del;
        }
        #endregion OnNotDetectedDelegate


        #region OnHitDelegate
        /// <summary>
        /// Add a new delegate to be triggered when the target is hit.
		/// The delegate signature is:  delegate(EventInfoList eventInfoList, Target target)
		/// See EventInfoList documentation for usage.
        /// **This will only allow a delegate to be added once.**
        /// </summary>
        /// <param name="del"></param>
        public void AddOnHitDelegate(OnHitDelegate del)
        {
            this.onHitDelegates -= del;  // Cheap way to ensure unique (add only once)
            this.onHitDelegates += del;
        }

        /// <summary>
        /// This replaces all older delegates rather than adding a new one to the list.
        /// See docs for AddOnHitDelegate()
        /// </summary>
        /// <param name="del"></param>
        public void SetOnHitDelegate(OnHitDelegate del)
        {
            this.onHitDelegates = del;
        }

        /// <summary>
        /// Removes a OnHitDelegate 
        /// See docs for AddOnHitDelegate()
        /// </summary>
        /// <param name="del"></param>
        public void RemoveOnHitDelegate(OnHitDelegate del)
        {
            this.onHitDelegates -= del;
        }
        #endregion OnHitDelegate

        #endregion Delegate Add/Set/Remove Functions

        #endregion Target Tracker Members

    }
}