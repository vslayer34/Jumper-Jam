using Godot;
using InputActionNames;
using System;

public partial class GamePlay : Node2D
{
	[Export]
	private PackedScene _cameraScene;

	[Export]
	private Player _player;


	[Export]
	/// <summary>
	/// Section size when the player near it's end spawn another section
	/// </summary>
	private int _sectionSize;


    public override void _Ready()
    {
        base._Ready();
		InstantiateTheCamera();
    }


    public override void _Process(double delta)
    {
        base._Process(delta);
		HandleDebugControls();

		// if (_player.GlobalPosition.Y >)
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


	// getters and setters
	public int SectionSize { get => _sectionSize; }

	/// <summary>
	/// Get the player Global Y position
	/// </summary>
	public float PlayerYPosition { get => _player.GlobalPosition.Y; }
}
