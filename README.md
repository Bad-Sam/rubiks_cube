# Rubik's Cube

![preview.gif](Docs/preview.gif)

Rubik's Cube implementation in Unity 3D, by Sami AMARA and Thomas DALLARD.
The main logic was implemented over the span of 2 days.

## Running the project
You can either:
- [download the build](https://github.com/Bad-Sam/rubiks_cube/releases/download/GOLD/RubiksCube.zip), unzip it, and run RubiksCube.exe
- open the projet in Unity, through [Assets/Scenes/main.unity](Assets/Scenes/main.unity), and hit the "Play" button

## What is and is not done
All the features implemented are listed in the [subject](Docs/Subject.pdf) provided in this repository.

## Controls
|         Input         |           Action          |
| --------------------- | ------------------------- |
| Left click + Drag     | Rotate the whole cube     |
| Right click + Drag    | Rotate the selected face  |
| Mouse wheel           | Zoom in/out               |
| Mouse wheel           | Zoom in/out               |

You can see the description of each button in-game, by pressing the "Show instructions" button.

## Architecture
Most of the logic of the game is in the scripts, in [Assets/Scripts/](Assets/Scripts/).

- The scripts in `Controls/` are related to how the cube is handled: moving the cube, moving its faces, zooming in/out, etc.
    - The rotation of the cube is coded line 27 of [RotateCube.cs](Assets/Scripts/Controls/RotateCube.cs)
    - The rotation of the faces is coded in the method `RotateFace()` of [RubiksCube.cs](Assets/Scripts/Generation/RubiksCube.cs)
    - The camera zoom is line 16 of [Zoom.cs](Assets/Scripts/Controls/Zoom.cs)
- The scripts in `UI/` are responsible for all interactions with the UI
- The scripts in `Generation/` are in charge of the generation of the Rubik's Cube
    - [Cube.cs](Assets/Scripts/Generation/Cube.cs) handles the generation of the smaller cubes
    - Whether the cube is solved or not is coded line 39 of [RubiksCube.cs](Assets/Scripts/Generation/RubiksCube.cs)
    - The generation of the Rubik's Cube is line 191 of [RubiksCube.cs](Assets/Scripts/Generation/RubiksCube.cs)
    - The shuffling of the cube is line 245 of [RubiksCube.cs](Assets/Scripts/Generation/RubiksCube.cs)
- `GameManager.cs` ensures the state of the game is saved and loaded
