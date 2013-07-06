#pragma strict

//open-close

public var isOpen = 0; //door state
public var smooth = 8;
public var positionOpen : Vector3 = new Vector3(0, .5, 0);
public var positionClosed : Vector3 = new Vector3(0, -.1, 0);
private var newPosition : Vector3;

function Awake ()
{
    newPosition = transform.position;
    positionOpen = positionOpen + transform.position;
    positionClosed =  positionClosed + transform.position;
}

function Update ()
{
    PositionChanging();
}

function PositionChanging ()
{
    if(isOpen) 
    {
        newPosition = positionOpen;
    }
    else
    {
        newPosition = positionClosed;
    }
    
    transform.position = Vector3.Lerp(transform.position, newPosition, smooth * Time.deltaTime);
}

