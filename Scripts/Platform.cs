using Godot;
using System;

public partial class Platform : Area2D
{
	public override void _Ready()
	{
		BodyEntered += ReactToPlayer;
	}

    private void ReactToPlayer(Node2D body)
    {
        GD.Print("Player entered the platform");
    }

}
