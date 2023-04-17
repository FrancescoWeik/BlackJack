# BlackJack

This is a repository for the BlackJack game. In this game you'll play as the dealer agains a certain number of players. Deal cards by dragging them onto the players or by throwing them to the players!
 
***Controls***</br>
- Using the mouse you are able to pick up cards, dragging a card to the player will assign it to that certain player. You can also click on a card, move it and then release the mous click to throw the card.
- By clicking on an assigned card on the table you can check its suit and value.
- Clicking c allows you to change the camera view
- If you are in the dealer perspective view then you can look around using the arrow keys or wasd

# Architecture

The main scripts are:
- Manager scripts (LevelManager, GameManager, PlayersManager, CameraManager)
- Card/Deck scripts
- Player scripts
- Dealer script
- Canvas scripts
- Camera scripts

***Manager scripts***</br>
The manager scripts are those that handles the general logic of the game. 
- ***LevelManager***</br> The level manager handles the loading and transition between the different scenes. It is never destroyed between scenes.
- ***GameManager***</br> The game manager handles the logic of the game. It handles the different turns (player turn and dealer turn), it handles the end of a round and how the game should respond in certain situations. Requires reference to the dealer hand, to the deck and to different UI elements. It also has a field for the PlayerNumberData ScriptableObject that is passed from the menu scene, so that it knows how many players to put in the game.
- ***PlayerManager***</br> The player manager handles the logic of the different players. It creates the players and then can perform different operations on them. It requires a list of the possible player names, the prefab of the player and the possible players positions around the table.
- ***CameraManager***</br> The camera manager handles the switch between the different cameras and how they should behave. Requires references to the different type of cameras.

***Card/Deck scripts***</br>
- ***Card.cs***</br> A class holding the value, suit and mesh of the cards.
- ***Deck.cs***</br> The deck script handles the logic of the deck. In particular it holds a list of all the different cards. It contains a FullDeck gameobject, a half deck gameObject and a shuffleDeckMesh gameObject that are useful to give visual feedbacks to the player based on the state of the deck. The deck always creates a void card on the top of itself that is later initialized when the user interacts with it. I decided to use this approach so that the user could always see a deck of cards (placing different cards on top of each other to create a deck resulted in a flat deck since the cards are planes and have no thickness). The deck script handles all the deck logic, the shuffle and reset of the deck.
- ***ThrowableObject.cs***</br> This script handles the logic of the objects that can be thrown inside the scene. It checks for user mouse inputs on a card and responds accordingly.For now only the cards can be thrown, but in the future it can be useful to add the same logic to other game objects. You must add a rigidbody and a collider to the object in order for this script to work
- - ***CardObject.cs***</br> The card object handles the logic of the cards. It's value and suit are intialized when the player first click on it. It is a throwable object. It is assigned to every card created inside the scene. 

***Player scripts***</br>
- ***Player.cs***</br> The player script handles the overall logic of the single player. It handles the collisions, the player hand, the player sounds and more. In its update it updates the current state that the player is in.
- ***PlayerState.cs***</br> It contains the general logic of a player state, handling the switch of animations and updating the logic of the player.
- - ***PlayerDecisionState.cs***</br> At he beginning of each round the player enters this state. This script handles the logic of how the player chooses what to do next, whether it is to draw a card or rejecting cards and more. 
- - ***PlayerIdleState.cs***</br> This script handles the logic of how the player should behave when idling. Currently it only plays the idle animation
- - ***PlayerLostState.cs***</br> This script handles the logic of how the players should behave when they lose. Currently it only plays the lose animation
- - ***PlayerRejectState.cs***</br> This script handles the logic of how the players should behave when they don't want any more cards. It sets a boolean value so that the player always rejects the cards thrown at him.
- - ***PlayerWaitingForCardState.cs***</br> This script handles the logic of how the players should behave when they're waiting for a card. if they are waiting simply play the animation, but if they finally receive a card then check whether to change to the lost state or idle state
- - ***PlayerWinState.cs***</br> This script handles the logic of how the players should behave when they have won. It plays a sound and an animation.
- ***PlayerStateMachine.cs***</br> It handles the logic of initializing the player states and switching from one state to another.

***Dealer***</br>
- ***DealerHand.cs***</br> The dealer hand script handles the logic of the dealer hand. In particular it contains a list of the cards held by the dealer and handles the different situation that can happen when the dealer draws (end the game, update the cards and many others). 

**Camera scripts**</br>
- ***DealerPerspectiveCamera.cs***</br> This script handles how the camera moves based on the user inputs. 

***Canvas scripts***</br>
- The canvas scripts handle the logic of how the ui works and responds to the user clicks.

# Components
The main components in the scene are:
- ***Cameras***:</br> has the CameraManager script attached and contains the cameras used inside the game
- ***GameManager***:</br> A simple GameObject that contains the GameManager script
- ***PlayersManager***:</br> A simple GameObject that contains the PlayersManager script. 
- ***LevelManager***:</br> The GameObject containing the levelManager script, has a canvas child that is shown when the game loads a scene.
- ***ScreenSpaceUI***:</br> A GameObject holding all the UI elements used in the Screen Space UI
- ***WorldButtonCanvas***:</br> A GameObject holding the world space UI (currently only the shuffle button)
- ***The Player***:</br> Prefab containing the players GameObject, along with the associated canvas showing their stats. It contains a GameObject called CardPositions GameObject that is usefull to set the card position so that it goes on the table when assigned. 
- ***The Deck***:</br> Prefab containing the deck GameObject. It contains a GameObject called DeckMesh, one called Black_LittleDeck_00 and one called ShuffleDeckAnim. This GameObject are usefull to handle the visual feedbacks of the deck and giving the user an idea of how many cards are in there. The Deck also always contains a SimpleCard GameObject so that the first card is always already there.
- ***The Cards***:</br> The cards are simple GameObject containing a rigidbody, a collider, an audio source and the CardObject script. They initially have no card image and are initialized when clicked by calling a function from the deck. Cards are always children of the deck.


#Sounds
- "Card Flip" by f4ngy (https://freesound.org/people/f4ngy/sounds/240776/) licensed under CC 4.0
- "Card Schuffling » Shuffling cards 02.WAV" by VKProduktion (https://freesound.org/people/VKProduktion/sounds/217501/) licensed under CC 0
- "Clap" by erkanozan (https://freesound.org/people/erkanozan/sounds/51746/) licensed under CC 0
- "Dealing Card" by f4ngy (https://freesound.org/people/f4ngy/sounds/240777/) licensed under CC 4.0






