John Di Girolamo 6202918

The game has a Button active at all times. This button toggles the AI Behaviour between "Kinematic On" and "Steering On".

The teams are controlled by their own respective controllers.

Each member of the team is given a goal based on a priority system granted that this member is not currently frozen or holding the flag.
	- 1st Priority: Pursue an enemy if that enemy enters a character's zone and is either seeking the flag or already has the flag.
	- 1st Priority: If the character has the enemy's flag, Arrive to the player's base to score a point.
	- 2nd Priority: If an ally is frozen, Seek to unfreeze them.
	- 3rd Priority: If an enemy enters the character's zone, but is not seeking the flag, Pursue and tag them.
	- 4th Priority: If above conditions are satisfied, Arrive to take the enemy's Flag.
	- Last Priority: The player Wanders. 
			 If the player is in the enemy's zone and is being pursued by an enemy:
				- If Kinematic Behaviour is active: the character flees this pursuer.
				- If Steering Behaviour is active: the character evades the pursuer.

A team wins if all members of the opposite team are frozen (therefore allowing the team with active players remaining to score an endless amount of points).

If both teams somehow have all their members frozen at the same time, there is a draw.