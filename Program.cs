Console.OutputEncoding = System.Text.Encoding.Default;

const int INTRO_SCENE = 0;
const int SHOW_SHADOW_FIGURE = 1;
const int SHOW_SKELETONS = 2;
const int HAUNTED_ROOM = 3;
const int CAMERA_SCENE = 4;
const int STRANGE_CREATURE = 5;
const int EXIT = 6;
const int DEAD_ROOM = 7;

const string DEAD_END = "\nYou find that this door opens into a wall.";

{
    char direction = 'n';
    bool directionIsNotValid = false, weaponFound = false, ghoulIsAlive = true, quit = false;
    int room = INTRO_SCENE, count = 0;
    string name = Introduction();

    while (!quit)
    {
        switch (room)
        {
            case INTRO_SCENE:
                IntroScene(ref directionIsNotValid, ref direction, ref room);
                break;
            case SHOW_SHADOW_FIGURE:
                ShowShadowFigure(ref directionIsNotValid, ref direction, ref room);
                break;
            case CAMERA_SCENE:
                CameraScene(ref directionIsNotValid, ref direction, ref room);
                break;
            case HAUNTED_ROOM:
                HauntedRoom(ref directionIsNotValid, ref direction, ref room);
                break;
            case SHOW_SKELETONS:
                ShowSkeletons(ref directionIsNotValid, ref direction, ref room, ref weaponFound);
                break;
            case STRANGE_CREATURE:
                if (ghoulIsAlive)
                {
                    StrangeCreature(ref directionIsNotValid, ref direction, ref room, ref ghoulIsAlive, ref weaponFound);
                }
                else
                {
                    StrangeCreatureIsDead(ref directionIsNotValid, ref direction, ref room, ref count);
                }
                break;
            case EXIT:
                Console.WriteLine($"\nYou made it {name}! You've found an exit");
                quit = true;
                break;
            case DEAD_ROOM:
                Console.WriteLine($"\nMultiple Ghoul-like creatures start emerging as you enter the room. Sorry {name}, you are killed.");
                quit = true;
                break;
            default:
                if (room != 99) { Console.WriteLine($"Hm, you should not be here {name}."); }
                quit = true;
                break;
        }
    }
}

string Introduction()
{
    Console.Clear();
    Console.WriteLine("Welcome to the Adventure Game!\n==============================\nAs an avid traveler, you have decided to visit the Catacombs of Paris.\nHowever, during your exploration, you find yourself lost.\nYou can choose to walk in multiple directions to find a way out.");
    Console.WriteLine("Let's start with your name: ");
    string name = Console.ReadLine()!;
    Console.WriteLine($"Good luck, {name}!");
    return name;
}

char GetDirectionOfPlayer(ref bool directionIsNotValid, ref char direction, string first, string second, string third, string fourth)
{
    do
    {
        if (directionIsNotValid) { Console.WriteLine("Invalid input. 😭 Please try again "); }
        directionIsNotValid = false;
        Console.Write($"Where would you like to go?\nYour options are {first}{second}{third}{fourth}: ");
        direction = Console.ReadKey().KeyChar;
        Console.WriteLine();

        if (direction is not 'n' and not 'e' and not 's' and not 'w')
        {
            directionIsNotValid = true;
        }
    } while (directionIsNotValid);
    return direction;
}

void IntroScene(ref bool directionIsNotValid, ref char direction, ref int room)
{
    Console.WriteLine("\nYou are at a crossroads, and you can choose to go down any of the four hallways.");

    direction = GetDirectionOfPlayer(ref directionIsNotValid, ref direction, "north/", "east/", "west/", "south");
    switch (direction)
    {
        case 'n':
            Console.WriteLine(DEAD_END);
            break;
        case 'e':
            room = SHOW_SKELETONS;
            break;
        case 's':
            room = HAUNTED_ROOM;
            break;
        case 'w':
            room = SHOW_SHADOW_FIGURE;
            break;
    }
}

void ShowShadowFigure(ref bool directionIsNotValid, ref char direction, ref int room)
{
    Console.WriteLine("\nYou see a dark shadowy figure appear in the distance. You are crept out.");
    do
    {
        direction = GetDirectionOfPlayer(ref directionIsNotValid, ref direction, "north/", "east/", "south", "");
        switch (direction)
        {
            case 'n':
                room = CAMERA_SCENE;
                break;
            case 'e':
                room = INTRO_SCENE;
                break;
            case 's':
                Console.WriteLine(DEAD_END);
                break;
            case 'w':
                directionIsNotValid = true;
                break;
        }
    } while (directionIsNotValid);
}

void CameraScene(ref bool directionIsNotValid, ref char direction, ref int room)
{
    Console.WriteLine("\nYou see a camera that has been dropped on the ground. Someone has been here recently.");
    do
    {
        direction = GetDirectionOfPlayer(ref directionIsNotValid, ref direction, "north/", "south", "", "");
        switch (direction)
        {
            case 'n':
                room = EXIT;
                break;
            case 'e':
                directionIsNotValid = true;
                break;
            case 's':
                room = SHOW_SHADOW_FIGURE;
                break;
            case 'w':
                directionIsNotValid = true;
                break;
        }
    } while (directionIsNotValid);
}

void HauntedRoom(ref bool directionIsNotValid, ref char direction, ref int room)
{
    Console.WriteLine("\nYou hear strange voices. You think you have awoken some of the dead.");
    do
    {
        direction = GetDirectionOfPlayer(ref directionIsNotValid, ref direction, "north/", "east/", "west", "");
        switch (direction)
        {
            case 'n':
                room = INTRO_SCENE;
                break;
            case 'e':
                room = EXIT;
                break;
            case 's':
                directionIsNotValid = true;
                break;
            case 'w':
                room = DEAD_ROOM;
                break;
        }
    } while (directionIsNotValid);
}

void ShowSkeletons(ref bool directionIsNotValid, ref char direction, ref int room, ref bool weaponFound)
{
    Console.WriteLine("\nYou see a wall of skeletons as you walk into the room. Someone is watching you.");
    do
    {
        direction = GetDirectionOfPlayer(ref directionIsNotValid, ref direction, "north/", "east/", "west", "");
        switch (direction)
        {
            case 'n':
                Console.WriteLine(DEAD_END);
                if (!weaponFound)
                {
                    Console.WriteLine("You open some of the drywall to discover a knife.");
                }
                weaponFound = true;
                break;
            case 'e':
                room = STRANGE_CREATURE;
                break;
            case 's':
                directionIsNotValid = true;
                break;
            case 'w':
                room = INTRO_SCENE;
                break;
        }
    } while (directionIsNotValid);
}

void StrangeCreature(ref bool directionIsNotValid, ref char direction, ref int room, ref bool ghoulIsAlive, ref bool weaponFound)
{
    string decision;
    Console.WriteLine("\nA strange Ghoul-like creature has appeared. You can either run or fight it. What would you like to do?");
    do
    {
        Console.Write("Your options are to flee (like little people) or fight (like a true warrior): ");
        decision = Console.ReadLine()!.ToLower();
    } while (decision is not "flee" and not "fight");

    switch (decision)
    {
        case "flee":
            room = SHOW_SKELETONS;
            break;
        case "fight":
            if (weaponFound == true)
            {
                Console.WriteLine("\nYou kill the Ghoul with the knife you found earlier.");
                ghoulIsAlive = false;
                room = STRANGE_CREATURE;
            }
            else
            {
                Console.WriteLine("\nThe Ghoul-like creature has killed you.");
                room = 99;
            }
            break;
    }
}

void StrangeCreatureIsDead(ref bool directionIsNotValid, ref char direction, ref int room, ref int count)
{
    count++;
    if (count > 1) { Console.WriteLine("\nYou see the Ghoul-like creature that you killed earlier. What a relief!"); }


    direction = GetDirectionOfPlayer(ref directionIsNotValid, ref direction, "south/", "west", "", "");
    do
    {
        switch (direction)
        {
            case 'n':
                directionIsNotValid = true;
                break;
            case 'e':
                directionIsNotValid = true;
                break;
            case 's':
                room = EXIT;
                break;
            case 'w':
                room = SHOW_SKELETONS;
                break;
        }
    } while (directionIsNotValid);
}
