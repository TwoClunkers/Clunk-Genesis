using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public static GameObject dragItem;



	#region IBeginDragHandler implementation

	public void OnBeginDrag (PointerEventData eventData)
	{
		throw new System.NotImplementedException ();
	}

	#endregion

	#region IDragHandler implementation

	public void OnDrag (PointerEventData eventData)
	{
		throw new System.NotImplementedException ();
	}

	#endregion

	#region IEndDragHandler implementation

	public void OnEndDrag (PointerEventData eventData)
	{
		throw new System.NotImplementedException ();
	}

	#endregion



}
