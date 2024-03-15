using Godot;
using System;

public partial class GamePlay : Node2D
{
	[Export]
	private PackedScene _cameraScene;

	[Export]
	private Player _player;


    public override void _Ready()
    {
        base._Ready();
		InstantiateTheCamera();
    }



	/// <summary>
	/// Create the camera at the start of the game and make it follow the player
	/// </summary>
	private void InstantiateTheCamera()
	{
		GameCamera _cameraScript = _cameraScene.Instantiate() as GameCamera;
		_cameraScript.Player = _player;
		AddChild(_cameraScript);
	}
}
