

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

<h3>Milestone 2:</h3>
<p><h4>Teamwork in different areas up to this point:</h4>
  <ul>
    <li>
      <h5>Event Handling:</h5>
      <ul>
        <li>Bryn</li>
        <li>Matthew Pham</li>
      </ul>
    </li>
    <li>
      <h5>Procedural Gen:</h5>
      <ul>
        <li>Takumi</li>
        <li>Bryn</li>
      </ul>
    </li>
    <li>
      <h5>Shaders and Graphics Processing:</h5>
      <ul>
        <li>Matthew Pham</li>
      </ul>
    </li>
    <li>
      <h5>Gameplay:</h5>
      <ul>
        <li>Matthew Pham</li>
        <li>Takumi</li>
        <li>Bryn</li>
      </ul>
    </li>
    <li>
      <h5>Weapons:</h5>
      <ul>
        <li>Matthew Lin</li>
      </ul>
    </li>
    <li>
      <h5>UI:</h5>
      <ul>
        <li>Matthew Pham</li>
      </ul>
    </li>
    <li>
      <h5>Particle System:</h5>
      <ul>
        <li>Matthew Lin</li>
      </ul>
    </li>
    <li>
      <h5>Menu System:</h5>
      <ul>
        <li>Matthew Pham</li>
        <li>Bryn</li>
      </ul>
    </li>
    <li>
      <h5>All-Round QA and feature implementation:</h5>
      <ul>
        <li>Takumi</li>
        <li>Matthew Pham</li>
        <li>Bryn</li>
      </ul>
    </li>
  </ul>
</p>

<h3>Milestone 3:</h3>
<p><h4>Teamwork in different areas up to this point:</h4>
  <ul>
    <li>
      <h5>Event Handling:</h5>
      <ul>
        <li>Bryn</li>
        <li>Matthew Pham</li>
      </ul>
    </li>
    <li>
      <h5>Procedural Gen:</h5>
      <ul>
        <li>Takumi</li>
        <li>Bryn</li>
      </ul>
    </li>
    <li>
      <h5>Shaders and Graphics Processing:</h5>
      <ul>
        <li>Matthew Pham</li>
      </ul>
    </li>
    <li>
      <h5>Gameplay:</h5>
      <ul>
        <li>Matthew Pham</li>
        <li>Takumi</li>
        <li>Bryn</li>
        <li>Matthew Lin</li>
      </ul>
    </li>
    <li>
      <h5>Weapons:</h5>
      <ul>
        <li>Matthew Pham</li>
        <li>Matthew Lin</li>
      </ul>
    </li>
    <li>
      <h5>UI:</h5>
      <ul>
        <li>Matthew Pham</li>
      </ul>
    </li>
    <li>
      <h5>Particle System:</h5>
      <ul>
        <li>Matthew Lin</li>
      </ul>
    </li>
    <li>
      <h5>Menu System:</h5>
      <ul>
        <li>Matthew Pham</li>
        <li>Bryn</li>
      </ul>
    </li>
    <li>
      <h5>Audio: </h5>
      <ul>
        <li>Takumi</li>
      </ul>
    </li>
    <li>
      <h5>Game Balancing/Tweaking</h5>
      <ul>
        <li>Takumi</li>
        <li>Matthew Pham</li>
      </ul>
    </li>
    <li>
      <h5>Game Testing/Interviewing</h5>
      <ul>
        <li>Takumi</li>
        <li>Matthew Pham</li>
      </ul>
    </li>
    <li>
      <h5>Report Writing</h5>
      <ul>
        <li>Takumi</li>
        <li>Matthew Pham</li>
        <li>Bryn</li>
        <li>Matthew Lin</li>
      </ul>
    </li>
    <li>
      <h5>All-Round QA and feature implementation:</h5>
      <ul>
        <li>Takumi</li>
        <li>Matthew Pham</li>
        <li>Bryn</li>
      </ul>
    </li>
  </ul>
</p>
<!-- [[EndTeamworkPlan]] PLEASE LEAVE THIS LINE UNTOUCHED -->

## Final report

Read the specification for details on what needs to be covered in this report... 

Remember that _"this document"_ should be `well written` and formatted **appropriately**. 
Below are examples of markdown features available on GitHub that might be useful in your report. 
For more details you can find a guide [here](https://docs.github.com/en/github/writing-on-github).

### Table of contents
* [Game Summary](#game-summary)
* [Technologies](#technologies)
* [Using Images](#using-images)
* [Code Snipets](#code-snippets)

### Game Summary
_Cyber-Souls_ is a roguelike dungeon-crawler set in space. Control a space dude as you try to defeat all
the enemies guarding a derelict ship to get to the awesome space treasure

### Technologies
Project is created with:
* Unity 2022.1.9f1 
* Ipsum version: 2.33
* Ament library version: 999

### Using Images

You can include images/gifs by adding them to a folder in your repo, currently `Gifs/*`:

<p align="center">
  <img src="Gifs/sample.gif" width="300">
</p>

To create a gif from a video you can follow this [link](https://ezgif.com/video-to-gif/ezgif-6-55f4b3b086d4.mov).

### Code Snippets 

You may wish to include code snippets, but be sure to explain them properly, and don't go overboard copying
every line of code in your project!

```c#
public class CameraController : MonoBehaviour
{
    void Start ()
    {
        // Do something...
    }
}
```
