# BlackJack

This is a repository for the BlackJack game. In this game you'll play as the dealer agains a certain number of players. Deal cards by dragging them onto the players or by throwing them to the players!
 
***Controls***</br>
- Using the mouse you are able to pick up cards, dragging a card to the player will assign it to that certain player. You can also click on a card, move it and then release the mous click to throw the card.
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
- ***LevelManager***</br> The level manager handles the loading and transition between the different scenes
- ***GameManager***</br> The game manager handles the logic of the game. It handles the different turns (player turn and dealer turn), it handles the end of a round and how the game should respond in certain situations.
- ***PlayerManager***</br> The player manager handles the logic of the different players. It creates the players and then can perform different operations on them.
- ***CameraManager***</br> The camera manager handles the switch between the different cameras and how they should behave.

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

***Canvas scripts***,<br>
- The canvas scripts handle the logic of how the ui works and responds to the user clicks.





