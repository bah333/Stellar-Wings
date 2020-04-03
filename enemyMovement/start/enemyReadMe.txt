/*****************
	Important
*****************/

I included the whole Unity project in case references need to be made. 	

When integrating everything together, I'm sure adjustments will need to be made for enemy tracking distances and such since everything will be scaled differently.

Also I'm sure I've missed some things to include...

- ship folder: Needed for the enemy and player models used.
	-> The actual prefabs are in the folders "badGuyStuff and playerShipStuff",
	nothing should have to be done with the "ship" folder. Just needs to be there!
	
- badGuyStuff folder: Contains enemy ship and enemy spawner prefabs as well as scripts used.

	-> badGuy7 prefab uses the EnemyMovement and EnemyStats scripts.
		-> Uses the nav mesh agent component to assist in dodging asteroids. I don't really understand
		the attributes, but I'll include screenshots for what I used. 
	-> EnemySpawner prefab uses the EnemySpawner script with the badGuy7 prefab attached in the inspector.
		-> EnemySpawner should be able to be placed anywhere in the scene
	-> Screenshots for inspector menu reference: badGuyA, badGuyB, badGuyC.
	
- playerShipStuff folder: The prefab I used for enemy tracking. Only used for testing purposes, **but we need to copy over a couple components on our actual player ship we're using. 
Also includes really bad WASD and arrow movement script that also isn't necessary.

	-> playerShip prefab: Consists of the playerShip has the parent, ship, camera, and GameObject for children.
		-> playerShip: Used to position the actual model, which is the ship child. It also uses the WASD movement script, and has the tag "player". ** Our actual player ship used will need a "player" tag
		->ship: The ship was modelled in a different coordinate system. When imported to Unity, its coordinates are different than what Unity uses. playerShip rotates everything so that it looks correct.
		**->GameObject: This is just an empty object that is set in front of the player. This is actually what enemies navigate toward when the player is a certain distance away from the enemy. This is so the enemy "intercepts" the player. 
		When the playerShip is actually close enough, the enemy will shift its attention to the player instead of the empty GameObject. Also, this empty GameObject uses the tag "targetInFront" and will be needed.
	-> Screenshots for inspector menu reference: playerShipA, playerShipB, playerShipC, playerShipD
	
- Game manager and globals folder: The Globals prefab will need the tag "GlobalVariables". This is used with the EnemyStats script
	
/*****************
	Side Notes
*****************/

- The EnemySpawner spawns enemies randomly around the player in a certain radius. Change the playerRadius variable in the script to change the spawning distance. 
Also contains the number of enemies to spawn. Enemies have a 1 second of invincibility to asteroid collisions when spawning, 
though I've not tested this so I don't know how they will interact with asteroids when first spawning :).

- EnemyMovement script contains the speed of the enemy ships. The speeds are also randomly generated within certain thresholds so they can feel more "unique". 
The switchDistance variable is the distance when the enemy switches its focus from the targetInFront, to the player and vice versa. 
The slowDownDistance variable is the distance when the enemy ship starts to slow down.

- Theres a scene the ProceduralAsteroid folder that should work for demonstartion purposes.