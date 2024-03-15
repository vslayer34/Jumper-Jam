using Godot;
using System;

public partial class GameCamera : Camera2D
{
	private Player _player;

	private Vector2 _viewPortSize;

    public override void _Ready()
    {
        base._Ready();
		_viewPortSize = GetViewportRect().Size;

		SetCameraStartPosition();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
		ChangeCameraButtomLimit();
    }


    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
		FollowPlayer();
    }


	/// <summary>
	/// Follow the Y axis direction of the player
	/// </summary>
	private void FollowPlayer()
	{
		if (_player != null)
		{
			GlobalPosition = new Vector2(GlobalPosition.X, _player.GlobalPosition.Y);
		}
	}


	/// <summary>
	/// Set the center of the X point of camera position to the center of the view port<br/>
	/// Set the Ground Limit of the Camera
	/// </summary>
	private void SetCameraStartPosition()
	{
		// Set the camera X Center
		GlobalPosition = new Vector2(_viewPortSize.X / 2.0f, GlobalPosition.Y);

		// Set the buttom limit to match the view port at the start of the game
		LimitBottom = (int)_viewPortSize.Y;
		LimitLeft = 0;
		LimitRight = (int)_viewPortSize.X;
	}


	/// <summary>
	/// Dynamically decrease Comara buttom limit relative to the player y position
	/// </summary>
	private void ChangeCameraButtomLimit()
	{
		float _margin = _viewPortSize.Y / 2.0f;

		if (LimitBottom > _player.GlobalPosition.Y + _margin)
		{
			LimitBottom = (int)(_player.GlobalPosition.Y + _margin);
		}
	}



    public Player Player { set => _player = value; }
}
