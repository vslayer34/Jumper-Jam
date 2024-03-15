using Godot;
using InputActionNames;
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


    public override void _Process(double delta)
    {
        base._Process(delta);
		HandleDebugControls();
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



	/// <summary>
	/// Handle the quit and reset controls of the game
	/// </summary>
	private void HandleDebugControls()
	{
		if (Input.IsActionJustPressed(ActionName.QUIT))
		{
			GetTree().Quit();
		}

		if (Input.IsActionJustPressed(ActionName.RESET))
		{
			GetTree().ReloadCurrentScene();
		}
	}
}
