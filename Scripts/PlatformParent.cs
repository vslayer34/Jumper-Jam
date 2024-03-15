using Godot;
using System;


[Serializable]
struct PlatformMinMaxValues
{
	/// <summary>
	/// minimum vertical distance between platfoms
	/// </summary>
	public float minYDistance;

	/// <summary>
	/// maximum vertical distance between platfoms
	/// </summary>
	public float maxYDistance;

	/// <summary>
	/// minimum horizontal distance between platfoms
	/// </summary>
	public float minXDistance;

	/// <summary>
	/// maximum horizontal distance between platfoms
	/// </summary>
	public float maxXDistance;

	/// <summary>
	/// The first platform that spawn form the ground
	/// </summary>
	public float startYPosition;
}

public partial class PlatformParent : Node2D
{
	[Export]
	private float _levelSize;

	[Export]
	private PackedScene _PlatformScene;

	private Platform _spawnedPlatform;
	private Vector2 _viewportSize;

	private PlatformMinMaxValues _platformPositionValues;


    public override void _Ready()
    {
        base._Ready();
		_viewportSize = GetViewportRect().Size;
		CreateGround();
		GenerateLevel();
    }


	/// <summary>
	/// Fill the values struct with the random horicontal position and vertical distance between platforms based on viewport<br/>
	/// Set the start Y position too 
	/// </summary>
	/// <param name="platformWidth">the platfrom width: usefull for spawning distance at the right side of the screen</param>
	private void SetPositionGenerationValues(float platformWidth)
	{
		_platformPositionValues.minYDistance = 100.0f;
		_platformPositionValues.maxYDistance = 200.0f;

		_platformPositionValues.minXDistance = 0.0f;
		_platformPositionValues.maxXDistance = _viewportSize.X - platformWidth;

		_platformPositionValues.startYPosition =  _viewportSize.Y - (float)GD.RandRange(_platformPositionValues.minYDistance, _platformPositionValues.maxYDistance);
	}


    /// <summary>
    /// Create a new platform at the specific <c>location</c> as a child of spawn parent
    /// </summary>
    /// <param name="location">The spawn location of the newly created platform</param>
    /// <returns>The newly created platform</returns>
    public Platform CreatePlatform(Vector2 location)
	{
		_spawnedPlatform = _PlatformScene.Instantiate() as Platform;
		_spawnedPlatform.GlobalPosition = location;
		AddChild(_spawnedPlatform);
		return _spawnedPlatform;
	}


	/// <summary>
	/// Create the ground taking the width and the height of the viewport into consideration
	/// to look all the same in multible devices
	/// </summary>
	private void CreateGround()
	{
		// safe margin so platforms won't stick together
		int safeMargin = 3;

		_spawnedPlatform = _PlatformScene.Instantiate() as Platform;

		var colliderShape = _spawnedPlatform.GetNode<CollisionShape2D>("CollisionShape2D").Shape as RectangleShape2D;
		float platformWidth = colliderShape.Size.X + safeMargin;
		float platformHeight = colliderShape.Size.Y + safeMargin;

		// get the number ceiling to be on the safe side and to prevent any small gaps after (floor to int conversion) 4.5 => 5 not 4
		int numberOfPlatforms = (int)Mathf.Ceil(_viewportSize.X / platformWidth);

		for (int i = 0; i < numberOfPlatforms; i++)
		{
			CreatePlatform(new Vector2(i * platformWidth, _viewportSize.Y - platformHeight));
		}

		SetPositionGenerationValues(platformWidth);
	}



	/// <summary>
	/// Take the info from the platform values struct and generate a new start position<br/>
	/// and other position based on the minimum and maximum values from the struct and level size
	/// </summary>
	private void GenerateLevel()
	{
		float startPlatformYPosition = _platformPositionValues.startYPosition;
		float verticalDistanceBetweenPlatforms; 
		Vector2 platormSpawnLocation;

		for (int i = 0; i < _levelSize; i++)
		{
			// get new randomized x and y offset and calculate new spawn vector from them
			verticalDistanceBetweenPlatforms = (float)GD.RandRange(_platformPositionValues.minYDistance, _platformPositionValues.maxYDistance);

			platormSpawnLocation.X = (float)GD.RandRange(_platformPositionValues.minXDistance, _platformPositionValues.maxXDistance);
			platormSpawnLocation.Y = startPlatformYPosition - (i * 200.0f);

			CreatePlatform(platormSpawnLocation);
		}
	}
}
