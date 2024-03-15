using Godot;
using System;

public partial class GameCamera : Camera2D
{
	private Player _player;


    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
		SetCameraXPosition();
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
	/// Set the center of the X point of camera position to the center of the view port
	/// </summary>
	private void SetCameraXPosition() => GlobalPosition = new Vector2(GetViewportRect().Size.X / 2.0f, GlobalPosition.Y);


    public Player Player { set => _player = value; }
}
