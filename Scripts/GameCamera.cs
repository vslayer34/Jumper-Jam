using Godot;
using Godot.Collections;
using System;

public partial class GameCamera : Camera2D
{
	private Player _player;

	private Vector2 _viewPortSize;

	private Area2D _platformDestroyer;
	private CollisionShape2D _destroyerCollider;

    public override void _Ready()
    {
        base._Ready();
		_viewPortSize = GetViewportRect().Size;

		_platformDestroyer = GetNode<Area2D>("PlatformDestroyer");
		_destroyerCollider = GetNode<CollisionShape2D>("PlatformDestroyer/CollisionShape2D");

		SetCameraStartPosition();
		SetPlatformDestroyerCollider();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
		ChangeCameraButtomLimit();
		DestroyPlatforms();
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


	/// <summary>
	/// Set the collider shape of the destroyer and it position according to the view port
	/// </summary>
	private void SetPlatformDestroyerCollider()
	{
		// to set at the end of the screem
		_platformDestroyer.Position = new Vector2(_platformDestroyer.Position.X, _viewPortSize.Y);

		RectangleShape2D rectangle = new RectangleShape2D()
		{
			Size = new Vector2(_viewPortSize.X, 100.0f)
		};

		_destroyerCollider.Shape = rectangle;
	}



	/// <summary>
	/// Go through the over lapping arease
	/// and check if it's a Platform and release if that is the case
	/// </summary>
	private void DestroyPlatforms()
	{
		Array<Area2D> overlappingAreas = _platformDestroyer.GetOverlappingAreas();

		if (overlappingAreas.Count == 0)
		{
			return;
		}

		foreach (var area in overlappingAreas)
		{
			if (area is Platform)
			{
				area.QueueFree();
			}
		}
	}



	// Getters and Setters
    public Player Player { set => _player = value; }
}
