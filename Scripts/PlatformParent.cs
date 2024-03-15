using Godot;
using System;

public partial class PlatformParent : Node2D
{
	[Export]
	private PackedScene _PlatformScene;

	private Platform _spawnedPlatform;
	private Vector2 _viewportSize;


    public override void _Ready()
    {
        base._Ready();
		_viewportSize = GetViewportRect().Size;
		CreatePlatform(new Vector2(100.0f, 300.0f));
		CreateGround();
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

		var colliderShape = _spawnedPlatform.GetNode<CollisionShape2D>("CollisionShape2D").Shape as RectangleShape2D;
		float platformWidth = colliderShape.Size.X + safeMargin;
		float platformHeight = colliderShape.Size.Y + safeMargin;

		// get the number ceiling to be on the safe side and to prevent any small gaps after (floor to int conversion) 4.5 => 5 not 4
		int numberOfPlatforms = (int)Mathf.Ceil(_viewportSize.X / platformWidth);

		for (int i = 0; i < numberOfPlatforms; i++)
		{
			CreatePlatform(new Vector2(i * platformWidth, _viewportSize.Y - platformHeight));
		}
	}
}
