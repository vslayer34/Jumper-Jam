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
}

public partial class PlatformParent : Node2D
{
	private int _sectionSize;

	[Export]
	private PackedScene _PlatformScene;


	[Export]
	private GamePlay _gamePlayManager;


	private Platform _spawnedPlatform;
	private Vector2 _viewportSize;


	private int _generatedPlatformsCount;

	private PlatformMinMaxValues _platformPositionValues;


	// reference to the last created platform and new section spawn point threashold
	private Platform _lastCreatedPlatform;
	private float _newSpawnThreashold;


	// prevent the Generate new level from calling each update
	// only set it true in the one frame that creates new section
	// then set it back to false when the section is created
	private bool _isNewSectionNeeded;



    public override void _Ready()
    {
        base._Ready();
		// Get the section size value from the game manager
		_sectionSize = _gamePlayManager.SectionSize;
		_viewportSize = GetViewportRect().Size;
		CreateGround();
		GenerateLevel();
    }


    public override void _Process(double delta)
    {
        base._Process(delta);


		// Track the player Y position and spawn a new section when he/she reach a certain threashold
		if (_gamePlayManager.PlayerYPosition < _newSpawnThreashold && !_isNewSectionNeeded)
		{
			_isNewSectionNeeded = true;
			GenerateLevel();
		}
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
			// assign the last created platform to use its Y position in calculating other spawned platforms
			_lastCreatedPlatform = CreatePlatform(new Vector2(i * platformWidth, _viewportSize.Y - platformHeight));
		}

		SetPositionGenerationValues(platformWidth);
	}



	/// <summary>
	/// Take the info from the platform values struct and generate a new start position<br/>
	/// and other position based on the minimum and maximum values from the struct and level size
	/// </summary>
	private void GenerateLevel()
	{
		// set it to false to prevent calling it more than one time when the player reaches the threashold point
		_isNewSectionNeeded = false;
		float verticalDistanceBetweenPlatforms; 
		Vector2 platormSpawnLocation;

		for (int i = 0; i < _sectionSize; i++)
		{
			// get new randomized x and y offset and calculate new spawn vector from them
			verticalDistanceBetweenPlatforms = (float)GD.RandRange(_platformPositionValues.minYDistance, _platformPositionValues.maxYDistance);

			platormSpawnLocation.X = (float)GD.RandRange(_platformPositionValues.minXDistance, _platformPositionValues.maxXDistance);
			platormSpawnLocation.Y = _lastCreatedPlatform.GlobalPosition.Y - verticalDistanceBetweenPlatforms;

			_lastCreatedPlatform = CreatePlatform(platormSpawnLocation);
			_generatedPlatformsCount++;
		}


		GD.Print(_lastCreatedPlatform.GlobalPosition.Y);
		// set the spawn threashold to be the final third of the previuse section
		// to spawn a ne section when the player reaches this point
		_newSpawnThreashold = _lastCreatedPlatform.GlobalPosition.Y / 3 * 2;
		GD.Print(_newSpawnThreashold);
	}
}
