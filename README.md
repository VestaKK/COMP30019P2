

**The University of Melbourne**
# COMP30019 – Graphics and Interaction

## Teamwork plan/summary

<!-- [[StartTeamworkPlan]] PLEASE LEAVE THIS LINE UNTOUCHED -->

<!-- Fill this section by Milestone 1 (see specification for details) -->

For the most part, the team will make an effort to come together both online
and in person to work on and discuss the development of the game. A discord
channel is created where each team member can communicate with eachother and 
either ask for help or provide feedback to other team members. 

Whilst everyone will have a part in many different aspects of developing the game
we do have people who have expressed interest in specific areas of the project:
- Event Handling/Procedural Generation: Bryn
- Shaders and Graphics Processing: Matthew Pham
- Gameplay and UI: Takumi 
- Particle System and All-Rounder: Matthew Lin

The general plan of progression for the game is as follows:
1. Solidfy Git Workflow and Project File Structure
2. Character Controls and Test Environment
3. Get basic graphics working (main shader style)
4. Event Handling
5. Weapons

For now this is the base plan before we progress on to things like:
1. Enemies and procedural enemy generation
2. Procedural Dungeon generation
3. Player Progression

### Milestone 2:
#### Teamwork in different areas up to this point:
- Event Handling:
  - Bryn
  - Matthew Pham
- Procedural Gen:
  - Takumi
  - Bryn
- Shaders and Graphics Processing:
  - Matthew Pham
- Gameplay:
  - Matthew Pham
  - Takumi
  - Bryn
- Weapons:
  - Matthew Lin
- UI:
  - Matthew Pham
- Particle System:
  - Matthew Lin
- Menu System:
  - Matthew Pham
  - Bryn
- All-Round QA and feature implementation:
  - Takumi
  - Matthew Pham
  - Bryn

### Milestone 3:
#### Teamwork in different areas up to this point:
- Event Handling:
  - Bryn
  - Matthew Pham
- Procedural Gen:
  - Takumi
  - Bryn
- Shaders and Graphics Processing:
  - Matthew Pham
- Gameplay:
  - Matthew Pham
  - Takumi
  - Bryn
  - Matthew Lin
- Weapons:
  - Matthew Pham
  - Matthew Lin
- UI:
  - Matthew Pham
- Particle System:
  - Matthew Lin
- Menu System:
  - Matthew Pham
  - Bryn
- Audio:
  - Takumi
- Game Balancing/Tweaking:
  - Takumi
  - Matthew Pham
- Game Testing/Interviewing:
  - Takumi
  - Matthew Pham
- Report Writing
  - Takumi
  - Matthew Pham
  - Bryn
  - Matthew Lin
- All-Round QA and feature implementation:
  - Takumi
  - Matthew Pham
  - Bryn
  - Matthew Lin

<!-- [[EndTeamworkPlan]] PLEASE LEAVE THIS LINE UNTOUCHED -->

## Final report

### Table of contents
* [Game Summary](#game-summary)
* [How to Play](#how-to-play)
* [Gameplay Design Decisions](#gameplay-design-decisions)
* [Graphics Pipeline](#graphics-pipeline)
* [Procedural Generation](#procedural-generation)
* [Particle System](#particle-system)
* [Querying and Observational Methods](#querying-and-observational-methods)
* [Changes Made due to Feedback](#changes-made-due-to-feedback)
* [References](#references)

### Game Summary
_Cyber Souls_ is a 2.5D roguelike dungeon-crawler set in space. Control a lone space dude as you fight through enemies and navigate to the exit of the dungeon to advance to the next level. Defeating enemies have a chance to drop item relics that aid the player in different ways to help with the ever increasing difficulty as you clear more levels. Try to get as far as possible and clear as many dungeons and enemies as possible to attain the highest score!

### How to Play
_**Player Controls:**_
| Buttons | Actions |
| --- | --- |
| WASD | Move up/left/down/right |
| Left Click | Swing melee / shoot gun (if aiming gun) |
| Right Click | Aim gun | 
| R | Auto-lock onto the nearest enemy |
| Space | Roll |
| Tab | Open/close inventory |
| E | Interact with teleporter (when near it) to advance to next level |
| Esc | Pause game |

<ins>_**Instructions:**_</ins> <br/>
Players spawn in a dungeon with a sword and a gun. The score increases per enemy killed and levels advanced. The objective of the game is to fight through enemies (or optionally avoid them), keep advancing to new levels and obtain as high of a score as possible until the player dies.

Killed enemies have a chance to drop item relics that, when picked up, aid the player in some way. Below are the items in this game:
- Bullet ammo, which regenerates a portion of the ammo capacity of the player, up to a cap.
- Damage Boost, which increases the damage dealt by a percentage.
- Health, which regenerates a portion of the player’s health.
- Max Health Boost, which increases the maximum health of the player (without affecting the percentage of the player’s current health).

Due to the nature of progression in this game, to gain the full experience of the game, one run (starting a game until death) will take between 10-20 minutes, depending on skill level.

Below are some additional tips to the game:
- Rolling is slightly faster than simply walking.
- During the roll animation, the player receives half the damage.
- Upon advancing to the next level, a portion of the player’s health is restored.
- Timing the melee swing allows for a secondary swing that deals more damage.

### Gameplay Design Decisions

<ins>_**Player Controls:**_</ins> <br/>
A significant part of development was focused on making the player’s controls feel clean, intuitive and responsive, yet require a fair amount of skill to properly use them in the context of the game. The main design challenge was to implement the controls so the player had multiple different ways to respond to the enemies in the dungeon, and that there were always clear, noticeable consequences to the actions they took.

<ins>Movement:</ins> <br/>
The player has two main ways to traverse around a level. The player can move around using WASD, and also has the ability to roll around using SPACE. The main trade off between the two is that normal player movement responds to direction changes more easily, whereas rolling can move the player away from danger faster, but prevents the player from doing anything until the roll is finished (the player can still turn, albeit at a reduced turning speed). Importantly, the game never takes control away from the player’s movement - every player input will move the player in some way, and the player’s challenge is to weigh the two options when approaching and dodging enemy attacks. For example, whilst it is quite easy to roll away instantly when an enemy attacks to dodge an attack, it is quicker to simply move away from the enemy as they attack, letting the player respond quicker, albeit being harder to time.

<ins>Combat:</ins> <br/>
The player’s primary method of combat is in their sword attack, which itself is a two-click combo attack. The player can decide to aim this attack themselves using the mouse, or they can choose to lock onto an enemy, automatically aiming the melee towards them. Players can opt for the former for more control over their attacks, and the latter to be certain of where their attacks are aimed. 

The first attack deals a low amount of damage and can be spammed repeatedly, whereas the second attack deals increased damage, whilst being less spammable. While attacking with their sword, the player cannot move. As such, if a player is about to be attacked whilst attacking, they are forced to tank the attack. Enemies attack and respond to the player predictably, so the main challenge for the player is learning how to approach and attack enemies both individually and in groups e.g. trying to balance dealing high damage per second whilst also dodging attacks.

Players also can use a gun with limited ammo. Unlike the sword, the player must always manually aim this attack, but the gun responds immediately to being fired and has no attack cooldown. It also has the advantage of being able to engage with enemies with much less risk, due to how shooting does not lock the player into an animation, and how being at a distance from an enemy makes it easier to avoid their attacks. The main challenge for the player is gambling when to use these bullets - they are quite powerful but limited and can only be gained through killing enemies.

<ins>Animation and Visuals:</ins> <br/>
The animations of the player are designed to help players intuitively understand how their inputs affect the different attacking options that are available to them.

When players aren’t locked onto enemies and are moving, they will always be animated moving forward relative to their moving direction. However, when locked on, the player will always face the enemy regardless of the direction they move, hopefully displaying the player’s melee aiming options intuitively.
Holding right click switches the player’s weapon to a gun and allows them to aim the player model in any direction they want regardless of the player’s movement.

The visual look of the player’s HUD is designed to highlight important actions that the player should be aware of.
	
Certain actions, such as the player getting damage or healing, make the screen flash red and green so that the player doesn’t need to look closely at their health to be aware of their approximate health pool.
Item Pickups and interaction events display text events to the screen

There were other small adjustments made so that there were no jarring animations and camera movements that made the player less responsive and harder to view.

The turning of the player when they move is a fast smooth angle interpolation rather than an instant transform rotation to the desired angle, making the turning of the player model more visually appealing. However, actions such as rolling, attacking and aiming rotate the player instantly in their desired direction. As such, the smoother visuals do not get in the way of responsive controls.
The camera smoothly lerps between its previous location and its next location, smoothing out sudden displacements in the player’s transform. This made it easier to import animations and adjust animation transitions, such as rolling and attacks, since we didn’t have to worry about how the camera would respond.

<ins>_**Enemy Design:**_</ins> <br/>
In the game, the player faces two different enemies: One that attacks the player at close range and one that does so by shooting a projectile at the player from a further range. Both of these enemies have a predictable pattern to their actions, which is intended given the scaling number of enemies already adding to the complexity as players advance, as covered in the dungeon progression section later. The two are contrastingly different to add to the complexity of the combat. The rather low number of types of enemies is so that in face of many enemies, it does not overcomplicate the combat.

The enemies work on a nav mesh system to path to the player, and will only pursue/attack them if in the same room as the player, or when the player is within a certain detection distance.

The long range enemy (referred to as Chet from here onwards) is designed to be a pesky enemy to face due to how it can engage with the player from a further distance. As such, his health is intentionally lower. There is also an animation and audio cue whenever he shoots to give the player a chance to react.

The close range enemy (referred to as Leela from here onwards) is designed to be a slow (both the movement speed and the attack speed) but bulky enemy that poses less of a threat but takes more time to defeat. Much like Chet, her melee animation and audio cue predictably telegraph when she will attack, so players can learn to respond to her accordingly

<ins>_**Dungeon Progression:**_</ins> <br/>
At the beginning of the game, the dungeon is generated by a dungeon spawner gameobject, which spawns all the rooms and enemies, with a chance of giving them an item drop. Every level, a new dungeon spawns, with the amount of enemies spawned per room increasing by one. Enemies also see an increase to their maximum health, however their damage is not affected. The goal of the player is to traverse through the dungeon, and gain as much score as possible before they die.

At a high level, the dungeon is designed to facilitate two different gameplay styles that blend into each other over time. Initially, the player is pushed towards a slower, more precise style of approaching and attacking enemies. As they continue to progress, the player becomes stronger and harder to kill, and enemies are easier to defeat. However, as the number of enemies themselves increases, the player is forced to adopt a faster paced dodge and weave gameplay style.

<ins>_**Items and Player Stat Scaling:**_</ins> <br/>
The player’s max health and sword damage increase linearly depending on the amount of buff items picked up. Since the health scaling of enemies is based on sqrt(Current Level), the player will be able to kill enemies more easily as they progress. Additionally, bullet damage scales exponentially for each damage item picked up, and this is intended. As the player gets stronger and progresses through levels, bullets become more valuable as they will usually be able to instantly kill enemies late into the game without risking getting damaged, which is useful when the enemy counts per room reach higher numbers.

<ins>_**Healing:**_</ins> <br/>
There are only two ways for the player to heal in a run. For every level clear, the player heals a flat 60 health, which is designed so that early on a player doesn’t necessarily need to play perfectly to continue a run. However, because this value doesn’t scale, it provides the player with a diminishing level of healing for each level cleared as their maximum health increases. The other option is a rare healing item that heals 15% of the player’s health. Whilst early on this is quite minimal healing, it’s designed to become the player’s main health source in the later levels.

<ins>_**Player Progression Options:**_</ins> <br/>
The aim of a player **should** be to kill enemies in a level, get stronger and gain as much score as possible. However, the player is always able to progress to the next level, regardless of the number of enemies they’ve cleared, provided that they have found the exit room. Since progressing to the next level also gives a score, it is intended that players decide between risking death for stat increases against gaining free score from completing a level. For example, a player might gamble by killing enemies so that they have the chance of getting extra bullets late in a game, or decide to go to the next level but lose out on getting extra items.

### Graphics Pipeline

**Cel Shader (With Outlines):**
- ShaderLab File Location:
  - [Assets/GFX/Shaders/CelOutline.shader](Assets/GFX/Shaders/CelOutline.shader)
  - [Assets/GFX/Shaders/Cel.shader](Assets/GFX/Shaders/Cel.shader)
- HLSL Code File Location:
  - [Assets/GFX/Shaders/Cel.cginc](Assets/GFX/Shaders/Cel.cginc)

In the case that the initial pass for the outline shader in [Assets/GFX/Shaders/CelOutline.shader](Assets/GFX/Shaders/CelOutline.shader) is considered as a separate shader to the cel shader passes in the same shaderlab file, please look to the main cel shader ([Assets/GFX/Shaders/Cel.shader](Assets/GFX/Shaders/Cel.shader) or [Assets/GFX/Shaders/Cel.cginc](Assets/GFX/Shaders/Cel.cginc)) as the shader to mark. It is essentially CelOutline.shader without the initial outline pass. However, the outline shader is only paired with CelOutline.shader in game, and is never a stand-alone shader program, so the outline shader will be described regardless, due to it being fairly simple compared to the cel shader.

The CelOutline.shader shaderlab file consists of two main sections - the outline and the cel shader. The rationale for the cel shader was to evoke an arcade aesthetic in the game’s visuals, despite the team’s collective and absolute lack of artistic talent. Since it was decided early on that the game would be an arcade style roguelike in some form or another, this seemed like a valid visual direction to pursue. As for the outline shader, its main purpose is as a visual indicator of the player’s lock-on mechanic, where the enemy that the player is locked onto is outlined in green.

<ins>**Cel Shader:**</ins> <br/>
The Cel Shader is the main shader used for all entities, static props and environment geometry in the game. The shader is heavily derivative of Blinn-Phong shading, but instead clamps the colour of geometry into clearly defined color segments based on surface lighting, rather than a more realistic smooth gradient between light and shadow. These segments are based on where ambient, diffuse and specular lighting would usually be simulated by Blinn-Phong shading, along with some extra lighting around the rim of objects for stylistic effect. The size and color of these segments, along with the glossiness of surfaces, can be adjusted in the Unity editor for each material asset. The shader optionally allows for the use of albedo and normal texture maps to simulate the effect of complex 3D geometry and reduce mesh complexity, (see [Assets/MiscellaneousGFX/PlaneMat.mat](Assets/MiscellaneousGFX/PlaneMat.mat) for an example).

For shadows and real-time lighting support, the Cel Shader is based on the forward rendering pipeline available in Unity’s Built-in Render Pipeline, and makes use of the in-built “AutoLight.cginc” and “Lighting.cginc files”. There is a base pass for lighting based on the main directional lighting in a scene, and then additional passes where other lighting, such as point lights and spot lights, is added onto the surface of geometry using the “Blend One One” option. Shadow support is integrated using Unity’s “Legacy Shaders/VertexLit/SHADOWCASTER” as an additional render pass, although this can only support shadows cast from the main directional light.

<ins>**Outline Shader:**</ins> <br/>
The Outline Shader is used as a pre-lighting pass that creates a simple pseudo-outline ( not 100% accurate ) around objects. It expands the vertices of a mesh outwards two ways: either using the vertex normal or the vector from (0,0,0) to the vertex, which is decided based on the angle between the two vectors. The look of the outline is very much based on where a mesh’s vertices are with respect to their local-space origin, so this angle can be adjusted by the developer per 3D mesh asset. ZWrite is turned off as it is intended for other render passes to draw over this initial outline pass, which allows for the use of back-face culling rather than front face culling, making the outline of the object cleaner and more solid when next to other objects. The use of a post-processing effect for the outlines was considered, however since our in-game objects weren’t outlined most of the time, this option was scrapped for the current approach.

<ins>**Additional Note:**</ins> <br/>
Enemies that are outlined need to be rendered in Unity’s transparency queue, whilst normal geometry is rendered in Unity’s geometry queue. To change the mesh’s render queue at runtime, a script ([Assets/Code/Utils/SwitchShaderOnMaterial.cs](Assets/Code/Utils/SwitchShaderOnMaterial.cs)) on the enemy game object switches the mesh material between the two render queues whenever the player locks onto an enemy. This script also switches the shader from the standard “Cel.shader” to “CelOutline.shader”, giving an enemy an outline, and vice versa for removing an enemy’s outline.

**Post Processing (Lens Distortion, Chromatic Abberation, Pixelation):**
- **ShaderLab && HLSL Code File Location:**
  - [Assets/GFX/Shaders/PostProcessing.shader](Assets/GFX/Shaders/PostProcessing.shader)

The reasoning behind having a post-processing shader (lens distortion and chromatic aberration) is mainly to give players a better visual indication of their health, so that they don’t have to guess at what the state of their health is while also fighting hordes of enemies. This also helps heighten the feeling of panic in the player at a low health. On the other hand, the pixelation of the screen is simply a stylistic choice given the game’s arcade aesthetic. It also makes the aliasing of 3D geometry less jarring and lets the player focus on enemies rather than the details on in-game meshes.

<ins>**Lens Dsitortion:**</ins> <br/>
As the player approaches lower health, the shader distorts the camera render texture’s UVs to imitate a lens. To do this, the UVs are remapped according to the shape of a circular lens (not considering the screen’s aspect ratio). Any UVs that are remapped outside of the (0,1) range of the camera’s render textures are instead sampled normally from the regular UVs, but the colours are darkened to focus attention towards the center of the screen.

<ins>**Pixelation:**</ins> <br/>
Every UV is then remapped to a rounded UV value based on optional \_SampleX and \_SampleY values representing the dimensions of a virtual screen that has been stretched to fit the player’s screen.

<ins>**Chromatic Aberration:**</ins> <br/>
Imitating chromatic aberration involves displacing the red, green and blue color channels of the camera’s render texture. In our case, the intensity of the effect also “breathes” based on a sine wave, which can be adjusted in the editor. The intensity also scales with the magnitude of distortion of the lens above, which makes it seem like the player is looking through an old, powerful lens as they approach a lower health.

### Procedural Generation

In this project, the dungeon generation in each level is procedurally generated, majorly employing binary partitioning to do so. The code for it is located in (Assets/Code/Dungeon gen). 

The dungeon was chosen to be procedurally generated to give a sense of being in a different environment each time the player clears a level, adding to the sense of progression. Binary partitioning was the method of choice because keeping the dungeon and rooms controllably random was simple. Rooms are always rectangular and corridors are always orthogonal, for example, as such placing the spawn and exit room at predictable locations of the dungeon is easy. This is important because the intention is that while the dungeons should look different each time to add to the visual variability and the sense of progression, they should not look unfamiliar to the point that the player feels disorientated and is not sure where to go. Also, with the predictable rooms, generating props was trivial too, which was important to add visual elements to the otherwise bland room, without them being an obstruction.

The method for generating is as follows:
1. Given a dimension of the entire dungeon, use binary partitioning to recursively partition it into two at random.
    1. Represent the dungeon as a binary tree, the first node representing the entire dungeon.
    2. Create two new nodes under this node, each representing a section of the dungeon when split by a randomly generated horizontal or vertical line.
    3. Repeat the above two steps of choosing a node (without any children nodes) and splitting them into two until the defined maximum number of iterations has been reached or no partitions can be made without creating a section smaller than the defined dimension.
2. Randomly create a room within each section. Define the coordinates of floors and walls of a room accordingly.
3. Define the spawn and exit room (where the player spawns and where the player shall go to advance to the next level). 
    1. The spawn room is defined as the room closest to the center of the entire dungeon and the exit is defined as one of the four corner rooms. 
    2. Adjust the size of these rooms accordingly. This simply helps in standardizing how these two rooms look across levels.
4. Connect the rooms with corridors.
    1. Navigate the tree using Depth-First Search.
    2. On a node with children, attempt to connect the children with a corridor
        1. Check the relative position of one child node over the other (up, down, left or right).
        2. Select a leaf node from one of the child nodes at random that borders with the other child node (e.g. if child node 1 is above child node 2, we select any leaf node under child node 1 that is at the bottom of child node 1).
        3. Sift through the other child node for a leaf node that can connect to the previously connected leaf node and out of them, select a leaf node closest to the border.
        4. If the above two don’t connect, continuously sift through the leaf nodes of the child nodes at step 4bii and select a new leaf node until they both connect.
        5. Create a new corridor connecting the two leaf nodes. Define the coordinates of floors and walls accordingly, while also destroying walls of the rooms that it’s connected to.
    3. Repeat the above until the top of the tree is reached.
5. Populate the rooms with props
    1. Except for the spawn and exit rooms that were defined earlier, assign each room a “type” of room (library, storage room, lab room, etc.) depending on certain variables of the room (dimension, size, the number of connected corridors, etc.).
    2. Populate the rooms according to the type of room it is, with elements of randomness where relevant.
    3. *** There is a very rare bug where a prop may unintentionally spawn in front of the entrance to a corridor, which if unlucky enough could entirely block the entrance.
6. Instantiate the floor, walls, props, ceiling and lights per room and corridor into the scene.
7. Once the dungeon is constructed, bake a new navmesh onto the floor of the dungeon, for enemies to use as a way of pathfinding. Include props in the navmesh filter
8. Spawn a variable amount of enemies in rooms in safe locations (locations where they won’t spawn on top of props, or where they won’t clip into each other)

### Particle System

Particle System Location: [Assets/ParticleSystem/BasicFireInverted_PS.prefab](Assets/ParticleSystem/BasicFireInverted_PS.prefab) <br/>
The particle system to be marked is labeled BasicFireInverted_PS.prefab. 

This low poly fire particle system is generated in the corners of the end room that lets the player get to the next dungeon level. This helps in distinguishing this room to the others and hence marks where the player should go to advance to the next level. Also, it fits in with the cyberpunk aesthetic and requires a small number of polygons, helping with performance.

The Sprites-Default Material which is part of Resources/unity_builtin_extra was chosen as the material to be rendered for this particle system. Within the emission section the rate over time is set at 25 to also aid in performance.

The shape attribute needed to be a cone shape with an angle of 5 so that the particles would drift towards the center mimicking the effect of a fire in real life where the center rises the highest and is most dense with flames. Color over lifetime was also used to mimic fire and make the particle generation effect look as natural and seamless as possible. The fade in and fade out effect is created with the start and end transparency keyframes which set the alpha to 0 and two middle keyframes which are set to maximum alpha. While the color keyframes start from a bright yellow to orange to red and then grey to mimic smoke. 

Start Lifetime was set between 1 and 4, while Start Speed was set between -3 and -1.5, utilizing randomness to generate values between two constants. This meant that the individual fire ember moved and died at different rates which makes sense due to air resistance creating difference of speed, and  the natural variability in “lifespan” of an ember, thus making the fire look more dynamic.

Size over lifetime was set to a downward sloping curve so that fire embers would get smaller as they rise and disappear and the noise strength was set to 0.1 so that there is the illusion of a small amount of air in motion.

### Querying and Observational Methods

7 participants of varying genders and ages ranging between 17 and 25 have helped in this process. Many have had at least a few years of experience with video games, but the genres of commonly played video games varied rather largely between participants. Some had backgrounds of mainly First Person Shooter games while others played commonly puzzle games and Role-Playing games.

For observational methods, we opted to use a cooperative evaluation method, and for querying techniques, we hosted interviews with participants. The entire process was handled over a Discord call. After explaining the process of how we were going to conduct the testing and evaluation, we handed our participants the game and had them do as they pleased in the game as they streamed the game throughout. We let them do what they wanted to do with little input on our part on what to do, unless they asked themselves or they were getting clearly stuck. As they played, we had them ask any questions they may have had and tell us their opinions, criticisms, feedbacks and suggestions) on certain aspects of the game. After some time, we ask some open-ended questions in an interview-like format that covers their thoughts on the game in general, like how fun they found it, what was the best and worst part of the game, etc..

The general feedback for the game was relatively positive, many of them excited to see the direction and development of the game. The common feedbacks given are as follows:
- Two aspects of the game that were most enjoyed by the participants were the item relics and the sense of progression and becoming stronger, and the challenging but engaging combat loop, in particular rolling. 
  - However, some have mentioned that attacking felt a bit clunky and slow at times, and did not enjoy being locked into a relatively long animation for it.
- Unanimously, the participants, while enjoying the chromatic aberration effect while on low health per se, did not enjoy having that as the only indicator of being on low health, as they found themselves surprised when they “suddenly” died not knowing they were that close to death. 
  - Some were off-put by the chromatic aberration effect after some time however, as the effect got jarringly strong as their health dropped.
- Many were also confused about the controls and/or objective of the game initially, largely pointing to the fact that the UI often did not indicate enough information on how to play the game. 
  - Many took a while to figure out that the player can in fact shoot with his gun. They complained that there was little indication that the player had a gun to shoot with, and the description for the controls did not help.
- Some felt there were little ways of healing themselves, making committing a mistake and taking damage very crucial in the long run. 
  - As a consequence, a few even took approaches of avoiding enemies entirely despite the item relics they could obtain to become more powerful after defeating an enemy, saying that it sometimes isn’t worth it given the cost of taking damage. 
- Some felt that the game felt a little repetitive and boring after a while of seeing the same enemies and items continuously.
- Item relics felt clunky to many of the participants, due to how slow it chases the player. There were cases where the player had no idea the item didn’t get picked up.

### Changes Made due to Feedback

Changes were made to the game addressing the given feedback. The negative feedbacks were taken to fix parts of the game that were less enjoyable while the positive ones were taken to reinforce the good aspects of the game. These changes, alongside the feedbacks they are based on, are as follows:
- The attack speed of the player was increased to deal with the clunkiness of attacking some players felt and help make the combat loop feel smoother and hence more fun.
- A health bar for the player was added, as well as an additional lens distortion effect as health decreased. This makes it clearer to the players when they are low health, addressing the unanimous feedback of how they felt that they died out of the blue.
- The chromatic aberration effect was weakened at the center of the screen and strengthened towards the edge of it. This should address the jarringness that some people faced with the effect, since the players should be mainly fixated at the center of the screen where the character always is.
- A continuous audio cue that gets louder as the player approaches the exit room was added. This should help address the issue where players were not sure of where the objective of advancing to the next level was.
- A text prompt to the player to pause the game to show the instructions has been added, helping with the lack of clarification in controls and objectives.
- A gun asset was added to the player to give a visual cue that the player can in fact shoot.
- The bullet display was moved to the main player HUD to further indicate the presence of a gun, as well as make information more easily available to the player. Hopefully now the player can realise that they, indeed, have a gun and, furthermore, can shoot.
- A new item was added to the drop pool of enemies that will heal a portion of the player’s health. This addresses the criticism of the lack of healing and the lack of diversity in items.
- A change was added where clearing a level heals the player to some extent, addressing the issue of the lack of healing in the game.
- The items across the board were buffed to provide a stronger boost to the player to encourage players to engage with enemies more and make the progression from obtaining items even more fun.
- A damage reduction effect was added to the player while rolling, hopefully making engaging enemies and risking taking damage less punishable while adding to the combat loop that participants largely enjoyed.
- The item chase speed was increased and delay between being instantiated and chasing the player was reduced in hopes of addressing the clunkiness of picking up items.

### References

Player Controls:
- THIRD PERSON MOVEMENT in Unity [https://www.youtube.com/watch?v=4HpC--2iowE](https://www.youtube.com/watch?v=4HpC--2iowE)
  - Attribution: Learn how unity processes input and move transforms
- How To Make a Combo System in Unity in Less than 7 Minutes [https://www.youtube.com/watch?v=gHaJUNiItmQ](https://www.youtube.com/watch?v=gHaJUNiItmQ)
  - Attribution: Understanding how to code the animations for the attacks

Cel Shader (with Outlines):
- Shader Basics, Blending & Textures • Shaders for Game Devs [Part 1] [https://youtu.be/kfM-yu0iQBk](https://youtu.be/kfM-yu0iQBk)
  - Attribution: Basic HLSL syntax and Unity ShaderLab structure
- Healthbars, SDFs & Lighting • Shaders for Game Devs [Part 2] [https://youtu.be/mL8U8tIiRRg](https://youtu.be/mL8U8tIiRRg)
  - Attribution: Refresher for basic rendering concepts
- Normal Maps, Tangent Space & IBL • Shaders for Game Devs [Part 3] [https://youtu.be/E4PHFnvMzFc](https://youtu.be/E4PHFnvMzFc)
	- Attribution: Understanding normal maps and Unity forward rendering
- Toon Shader by Roystan [https://roystan.net/articles/toon-shader/](https://roystan.net/articles/toon-shader/)
  - Attribution: Refresher on Blinn-Phong and learning how to manipulate the look of surface colours
- Outline Shader [https://github.com/blightue/Perfect-Outline-Shader-In-Unity/blob/master/outline.shader](https://github.com/blightue/Perfect-Outline-Shader-In-Unity/blob/master/outline.shader)
  - Attribution: Learn how to outline objects properly by manipulation vertices

Post Processing Shader:
- Chromatic Aberration [https://www.shadertoy.com/view/Mds3zn](https://www.shadertoy.com/view/Mds3zn) [https://blog.yarsalabs.com/lens-distortion-and-chromatic-aberration-unity-part2/](https://blog.yarsalabs.com/lens-distortion-and-chromatic-aberration-unity-part2/)
  - Attribution: Understanding the concept of chromatic aberration and how it should scale.
- Lens Distortion [https://blog.yarsalabs.com/lens-distortion-and-chromatic-aberration-unity-part1/](https://blog.yarsalabs.com/lens-distortion-and-chromatic-aberration-unity-part1/)
  - Attribution: Manipulating UV coordinates to warp the camera’s render texture.

Animation:
- How to Animate Characters in Unity 3D | Animator Explained [https://youtu.be/vApG8aYD5aI](https://youtu.be/vApG8aYD5aI)
  - Attribution: Unity’s animation system is incredibly unintuitive.
- How to Animate Characters in Unity 3D | Animation Transitions With Booleans [https://youtu.be/FF6kezDQZ7s](https://youtu.be/FF6kezDQZ7s)
  - Attribution: Understanding how to change the state of animators from scripts.
- How To Animate Characters In Unity 3D | Animation Layers Explained [https://youtu.be/W0eRZGS6dhQ](https://youtu.be/W0eRZGS6dhQ)
  - Attribution: Unity’s animation system is incredibly unintuitive.

UIManager:
- How to make a UI management system in Unity [https://www.youtube.com/watch?v=rdXC2om16lo](https://www.youtube.com/watch?v=rdXC2om16lo)
  - Attribution: Implementing a UI system

Procedural Dungeon:
- Unity 3d procedural dungeon generator, Sunny Valley Studio [https://www.youtube.com/playlist?list=PLcRSafycjWFfEPbSSjGMNY-goOZTuBPMW](https://www.youtube.com/playlist?list=PLcRSafycjWFfEPbSSjGMNY-goOZTuBPMW)
  - Attribution: Understanding how binary partitioning works 

Audio:
- Introduction to AUDIO in Unity, Brackeys [https://www.youtube.com/watch?v=6OT43pvUyfY&ab_channel=Brackeys](https://www.youtube.com/watch?v=6OT43pvUyfY&ab_channel=Brackeys)
  - Attribution: Understanding what an audio manager is and does

Game Assets:
- Ultimate Modular Sci-Fi Pack [https://quaternius.com/packs/ultimatemodularscifi.html](https://quaternius.com/packs/ultimatemodularscifi.html)
- Cyberpunk Game Kit [https://quaternius.com/packs/cyberpunkgamekit.html](https://quaternius.com/packs/cyberpunkgamekit.html)
- Ultimate Modular Men Pack [https://quaternius.com/packs/ultimatemodularcharacters.html](https://quaternius.com/packs/ultimatemodularcharacters.html)
- Mixamo [https://www.mixamo.com/#/](https://www.mixamo.com/#/)

Audio Assets:
- Minimal UI Sounds, cabled_mess [https://assetstore.unity.com/packages/audio/sound-fx/minimal-ui-sounds-78266](https://assetstore.unity.com/packages/audio/sound-fx/minimal-ui-sounds-78266)
- Sci-fi Guns SFX Pack, Mikael Vanninen [https://assetstore.unity.com/packages/audio/sound-fx/sci-fi-guns-sfx-pack-181144](https://assetstore.unity.com/packages/audio/sound-fx/sci-fi-guns-sfx-pack-181144)
- Dark Future Music, Daniel Gooding [https://assetstore.unity.com/packages/audio/music/electronic/dark-future-music-3777](https://assetstore.unity.com/packages/audio/music/electronic/dark-future-music-3777)
- SoundBits | Free Sound FX Collection, SoundBits [https://assetstore.unity.com/packages/audio/sound-fx/soundbits-free-sound-fx-collection-31837](https://assetstore.unity.com/packages/audio/sound-fx/soundbits-free-sound-fx-collection-31837)
- RPG Audio, Kenney [https://www.kenney.nl/assets/rpg-audio](https://www.kenney.nl/assets/rpg-audio)
- Sci-Fi Sounds, Kenney [https://www.kenney.nl/assets/sci-fi-sounds](https://www.kenney.nl/assets/sci-fi-sounds)
- SWING01 [https://www.soundboard.com/sb/sound/931006](https://www.soundboard.com/sb/sound/931006)
- SWING02 [https://www.soundboard.com/sb/sound/931007](https://www.soundboard.com/sb/sound/931007)
- Pistol Draw Unholster, nioczkus [https://pixabay.com/sound-effects/pistol-draw-unholster-80799/](https://pixabay.com/sound-effects/pistol-draw-unholster-80799/)

Particles:
- Low Poly Fire VFX Unity Particle System || How to make a Low Poly Fire in unity Particle System VFX [https://www.youtube.com/watch?v=RMPhKtakU2I](https://www.youtube.com/watch?v=RMPhKtakU2I)
  - Attribution: Learning to implement fire particles
- Unity Shader Graph - Stylized Trails Tutorial [https://www.youtube.com/watch?v=wvK6MNlmCCE](https://www.youtube.com/watch?v=wvK6MNlmCCE)
  - Attribution: Learning to implement bullet trails

Navmesh:
- Unity NavMesh Tutorial - Basics [https://www.youtube.com/watch?v=CHV1ymlw-P8](https://www.youtube.com/watch?v=CHV1ymlw-P8)
  - Attribution: Implementing Navmeshes
- Navmesh Docs [https://docs.unity3d.com/ScriptReference/AI.NavMesh.html](https://docs.unity3d.com/ScriptReference/AI.NavMesh.html)
