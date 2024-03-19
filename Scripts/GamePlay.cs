using Background;
using Godot;
using InputActionNames;
using System;

public partial class GamePlay : Node2D
{
	[Export]
	private PackedScene _cameraScene;

	[Export]
	private PackedScene _playerScene;

	[Export]
	private ParallaxBGHandler _parallaxHandller;

	private Player _player;

	private Sprite2D _groundSprite;

	private Vector2 _viewportSize;


	[Export]
	/// <summary>
	/// Section size when the player near it's end spawn another section
	/// </summary>
	private int _sectionSize;


    public override void _Ready()
    {
        base._Ready();
		_groundSprite = GetNode<Sprite2D>("GroundSprite");
		_viewportSize = GetViewportRect().Size;

		_parallaxHandller.SetupParallaxBG(ViewportSize);

		SpawnPlayer();
		SetGroundSpritePosition();
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



	/// <summary>
	/// Spawn the player to the scene and sets its position in relation to the view port
	/// </summary>
	private void SpawnPlayer()
	{
		float groundOffset = 150.0f;

		_player = _playerScene.Instantiate() as Player;
		_player.GlobalPosition = new Vector2(_viewportSize.X / 2.0f, _viewportSize.Y - groundOffset);
		AddChild(_player);
	}


	/// <summary>
	/// Set the position of the ground sprite according to screen rect
	/// </summary>
	private void SetGroundSpritePosition()
	{
		_groundSprite.GlobalPosition = new Vector2()
		{
			X = _viewportSize.X / 2.0f,
			Y = _viewportSize.Y
		};
	}


	// getters and setters
	public int SectionSize { get => _sectionSize; }

	/// <summary>
	/// Get the player Global Y position
	/// </summary>
	public float PlayerYPosition { get => _player.GlobalPosition.Y; }

	public Vector2 ViewportSize { get => _viewportSize; }
}
