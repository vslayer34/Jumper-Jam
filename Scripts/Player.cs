using Godot;
using InputActionNames;
using System;

public partial class Player : CharacterBody2D
{
	// player input handler
	[ExportCategory("player movment properties")]
	[Export]
	private float _speed;

	[Export]
	private float _gravity = 15.0f;

	private Vector2 _movmentDirection;

	// screen teleport properties
	[Export]
	private Sprite2D _playerSprite;

	private float _spriteWidthHalfed;
	private Vector2 _viewportSize;



    public override void _Ready()
    {
        base._Ready();
		_viewportSize = GetViewportRect().Size;
		_spriteWidthHalfed = _playerSprite.Texture.GetSize().X * _playerSprite.Scale.X / 4.0f;
    }


    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
		MovePlayer();
		TeleportBetweenScreenEdges();
		ApplyGravity();
    }


	/// <summary>
	/// Take the input and set the direction of the movment
	/// and steadly slow down the player to a halt when thers's no input
	/// </summary>
	private void MovePlayer()
	{
		float easeToHalt = 10.0f;
		float direction = Input.GetAxis(ActionName.MOVE_LEFT, ActionName.MOVE_RIGHT);

		if (direction != 0)
		{
			_movmentDirection = new Vector2(direction * _speed, Velocity.Y);
			Velocity = _movmentDirection;
		}
		else
		{
			_movmentDirection = new Vector2(0.0f, Velocity.Y);
			Velocity = Velocity.MoveToward(_movmentDirection, _speed / easeToHalt);
		}

		MoveAndSlide();
	}


	/// <summary>
	/// When the player reach either side of the screen edges
	/// it teleports back in on the other side
	/// </summary>
	private void TeleportBetweenScreenEdges()
	{
		if (GlobalPosition.X > _viewportSize.X + _spriteWidthHalfed)
		{
			GlobalPosition = new Vector2(-1 * _spriteWidthHalfed, GlobalPosition.Y);
		}
		else if (GlobalPosition.X < 0 - _spriteWidthHalfed)
		{
			GlobalPosition = new Vector2(_viewportSize.X + _spriteWidthHalfed, GlobalPosition.Y);
		}
	}


	/// <summary>
	/// Apply gravity to the player
	/// </summary>
	private void ApplyGravity()
	{
		float maxGravity = 1000.0f;
		Velocity = new Vector2(Velocity.X, Mathf.Clamp(Velocity.Y + _gravity, Velocity.Y, maxGravity));
	}
}
