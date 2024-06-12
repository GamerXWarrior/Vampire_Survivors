# Vampire_Survivors
Vampire Survivors game replica made in unity.

Vampire Survival Game

-	ON Starting the game, Game is started directly with the level 1.

-	on Completing the Level, level continues following with the required power up Choices and the enemies spawning interval changes.

Classes Structure - 

Mono Singleton Generic: This is the only single generic singleton class for the all other singletons in the game to inherit it. So that there is only one major singleton class and other singletons are the child class of this one.

Service Classes: These are the classes which are managing the external services such as , Object Pooling, Object Pooling Managers.

Manager Classes: These classes are managing the core elements of the Game such as the Player, enemies, Collectibles Object, Projectile Objects etc. .
Game Elements Classes: these classes are implemented with their functionalities, such as player movement, enemy, xp collectible, wand projectile.

Additional: 
-	Player can move fast with the Left Shift button pressed while moving
-	Player can choose to increase want projectile speed power every time till level 10 which is not possible beyond an extent, so the minimum projectile speed interval is set to 0.2 seconds.

