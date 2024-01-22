# Wacky Races

## Notes

VContainer was used for Dependency Injection management in the project, avoiding the use of singletons and other complexities that come with DI. However, the initial idea was to implement ECS (Entity Component System) using leoecs. Due to time constraints, I didn't want to take any risks. The project also adhered to the K.I.S.S (Keep It Simple, Stupid) principle, keeping it as simple and readable as possible.

All the requirements were developed, including the use of requested assets, UI, responsiveness, object pooling, the requested gameplay logic (Fibonacci, rounds, dynamic races), and more.

For the sake of simplicity, only essential assets were used, without additional features like background textures, among others.

A shader was developed to change the coloring of sprites, aiming to provide the best user experience when encountering the running monsters.

### Requirements:
- Unity (2022.3.17), which is the latest LTS version to date.

## Test

### Overview
The primary objective is to implement a dynamic Monster run game with specific behaviors and UI elements.


### Assets
- The assets to be used will be sent along side the instructions.

### Requirements

#### Rounds
- The game should be in rounds.

#### Round count
- With each round, the Fibonacci sequence should advance.
- There is no limit in how many rounds is possible to have. 

#### Round End
- After all Monsters have left the screen, the round ends.
- There should be a brief interval after one round ends before another begins.

#### Monster Instantiation per round
- Implement the instantiation of Monsters in a Fibonacci sequence based on the round number. Utilize object pooling techniques and DO NOT use loops (such as `foreach`, `for`, `while`, and `do/while`).

#### Monster Dynamics
- Ensure that each Monster's velocity is randomized, but all moving in the same direction.

#### Monster Destruction
- Program Monsters to automatically destroy themselves one second after moving out of the screen boundaries and be pooled.

#### User Interface
  - Develop a simple UI to display the total number of Monsters created.
  - Include a timer to show the elapsed time since the round started.
  - Use TextMeshPro

#### Responsiveness
  - Ensure that both the UI elements and the Monster destruction mechanics are responsive to the size of the any screen.

### Important Notes
- Keep performance optimization in mind, particularly with regards to the Monster creation and destruction processes.
- Testing on various screen sizes is recommended to ensure responsiveness and functionality.

### Additional (Not Needed)
- Add Unit Tests using Unity Test Framework (UTF)
- Feel free to add more information on the UI and other features or functionalities that you judge that could help the user.