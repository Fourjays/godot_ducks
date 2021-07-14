# Godot Ducks
An attempt to test Godot's performance with three different methods of instancing, using both GDScript and C#. This test was aimed at testing Godot's capabilities with large numbers of moving entities for my own needs, and shouldn't be used as a reliable benchmark of performance for all situations.

Each example spawns a number of Duck sprites at the start on a 2D tilemap with collision. Each entity will move once per second, checking for collisions before it moves.

The three methods of instancing entities used here are:
- **Instanced**: Each `Duck` scene is responsible for its own movement and collision checks.
- **Managed**: The `DuckManager` is responsible for each `Duck` scene's movement and collision checks.
- **Server**: The `DuckManager` draws each `Duck` via Godot's VisualServer after collision checks (no `Duck` scene).

The number of ducks spawned can be controlled through the `DuckManager` node's properties in the inspector panel.

## Results
| Ducks Spawned       | 2500 | 5000 | 7500 | 10000 | 20000 | 30000 | 40000 | 50000 | 60000 |
|---------------------|------|------|------|-------|-------|-------|-------|-------|-------|
| GD Instanced FPS    | 450  | 250  | 120  | 70    | 8     | 0     | 0     | 0     | 0     |
| C# Instanced FPS    | 500  | 200  | 90   | 35    | 4     | 0     | 0     | 0     | 0     |
| GD Managed FPS      | 650  | 340  | 210  | 155   | 80    | 50    | 0     | 0     | 0     |
| C# Managed FPS      | 690  | 320  | 205  | 145   | 65    | 35    | 0     | 0     | 0     |
| GD VisualServer FPS | 2300 | 1300 | 830  | 650   | 260   | 145   | 100   | 70    | 60    |
| C# VisualServer FPS | 2000 | 1300 | 890  | 615   | 250   | 130   | 115   | 85    | 65    |

*Test System: AMD Ryzen 2600x, 16GB RAM, GTX1070*

This project mostly involves calls into Godot's API, which carry an invocation hit for C# (Mono). The `ducks_instanced_mono` example demonstrates this well, where even an empty `_Physics_Process` function results in the same performance. C# would be faster than GDScript with more intensive calculations being performed on each entity. This is already more apparent in the `ducks_server_mono` project, which has an increased amount of calculations occuring compared to the `ducks_instanced` and `ducks_managed` projects.

## Credits
- Sprites and fonts by [Kenney.nl](https://kenney.nl/), licensed under [Creative Commons Zero (CC0)](http://creativecommons.org/publicdomain/zero/1.0/).
