using Godot;
using System;

public partial class SpawnManager : Node
{
	[Export]
	private Node2D _spawnParent;

	[Export]
	private PackedScene _PlatformScene;

	private Platform _spawnedPlatform;


    public override void _Ready()
    {
        base._Ready();
		CreatePlatform(new Vector2(100.0f, 300.0f));
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
		_spawnParent.AddChild(_spawnedPlatform);
		return _spawnedPlatform;
	}
}
