
// This class contains metadata for your submission. It plugs into some of our
// grading tools to extract your game/team details. Ensure all Gradescope tests
// pass when submitting, as these do some basic checks of this file.
public static class SubmissionInfo
{
    // TASK: Fill out all team + team member details below by replacing the
    // content of the strings. Also ensure you read the specification carefully
    // for extra details related to use of this file.

    // URL to your group's project 2 repository on GitHub.
    public static readonly string RepoURL = "https://github.com/COMP30019/project-2-from-malware";
    
    // Come up with a team name below (plain text, no more than 50 chars).
    public static readonly string TeamName = "From Malware";
    
    // List every team member below. Ensure student names/emails match official
    // UniMelb records exactly (e.g. avoid nicknames or aliases).
    public static readonly TeamMember[] Team = new[]
    {
        new TeamMember("Bryn Meachem", "bmeachem@student.unimelb.edu.au"),
        new TeamMember("Matthew Lin", "matlin@student.unimelb.edu.au"),
        new TeamMember("Takumi Takeshige", "ttakeshige@student.unimelb.edu.au"),
        // Remove the following line if you have a group of 3
        new TeamMember("Matthew Thao Dang Pham", "matthewtp@student.unimelb.edu.au"), 
    };

    // This may be a "working title" to begin with, but ensure it is final by
    // the video milestone deadline (plain text, no more than 50 chars).
    public static readonly string GameName = "Punk Souls";

    // Write a brief blurb of your game, no more than 200 words. Again, ensure
    // this is final by the video milestone deadline.
    public static readonly string GameBlurb = 
@"In this 2.5D top-down roguelike set in a futuristic cyber-fantasy dystopian Alabama, the player is the victim of twisted experiments, leaving them with the ability to become enraged when on the brink of death. 
Motivated by the need for revenge, the player must fight through hordes of enemies in the Alabama underground, collecting items and power ups along the way. All enemies and rooms will be procedurally generated, with enemies being generated from a set of models and abilities. The monsters of the underground have been twisted by both corrupt organisations and the twisted environment.
Since being experimented on, the player is slowly losing their concept of humanity, and so to remind them of their humanity there is a familiar cat at each checkpoint that the player can interact with to heal at discrete intervals throughout the game.";
    
    // By the gameplay video milestone deadline this should be a direct link
    // to a YouTube video upload containing your video. Ensure "Made for kids"
    // is turned off in the video settings. 
    public static readonly string GameplayVideo = "https://youtu.be/YtWY_4ws5ts";
    
    // No more info to fill out!
    // Please don't modify anything below here.
    public readonly struct TeamMember
    {
        public TeamMember(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public string Name { get; }
        public string Email { get; }
    }
}
