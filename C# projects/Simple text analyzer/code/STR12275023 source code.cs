using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace stringcatcher
{
    class Text_Analyser
    {
        static void Main()
        {
            int userChose = -1;
            // this loops the program will 0 is entered
            do
            { // disply choice to user
                Console.WriteLine("Enter the Number 1 for self inputting the words, Number 2 for from a file.\nTo quit the program enter the number 0.");
                //test users input
                try
                {
                    // stores users choice
                    userChose = Convert.ToInt16(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("An error was caught and thrown. this proves that try and catch is working");
                }

                switch (userChose)
                { // uses user choice to determine what to go to.
                    case 1:
                        userChose = -1;
                        Keyboard();
                        break;


                    case 2:
                        userChose = -1;
                        fromfile();
                        break;

                    // this will quit the program
                    case 0:
                        break;

                    default:
                        Console.WriteLine("You have entered an invalid input");
                        break;
                }
            } while (userChose != 0);


        }
        public static void fromfile()
        {
            //text input via a user selected txt file
            string file;
            string userans = "y";
            //loops until user does not want to re-input or enters correct file path
            do
            {
                Console.WriteLine("Enter the loction of the txt file that you want to read from:");
                file = Console.ReadLine();
                //check if the file exists
                if (File.Exists(file) == false)
                {
                    // gives the user the option to re-enter
                    Console.WriteLine("No file found\nEnter \"Y\" for re-try or press enter to continue");
                    userans = Console.ReadLine();
                }
                else
                {
                    //checks it the last 3 character are txt
                    if (file.Substring(file.Length - 3) != "txt")
                    {
                        Console.WriteLine("File must be a txt file i.e  myfile.txt");
                    }
                    else
                    {
                        //read text from file
                        string text = File.ReadAllText(file);
                        txtanalyse(text);
                        userans = "n";
                    }
                }
            } while (userans == "y" || userans == "Y");
        }

        public static void Keyboard()
        {
            //text input via the keyboard
            Console.WriteLine("Enter sentences, press enter to submit the text to be analyzed, use \"full stop\" to end a sentence\n you can enter multiple sentences ");
            string userinput = Console.ReadLine();
            txtanalyse(userinput);
        }

        public static void txtanalyse(string txt)
        {
            int wordsinputed, sentanceinput = 0, charainputed = 0, largewordcount = 0;
            decimal averagewordsize;
            string largewords = "";

            //cleans the window so user has a better view of analzyed text
            Console.Clear();

            //Remove white space at front and end of string
            string userinput = txt.Trim();

            //counts sentance and characters.
            foreach (char c in userinput)
            {
                if (c == '.')
                    sentanceinput = sentanceinput + 1;
                if (c == ' ')
                    continue;
                charainputed = charainputed + 1;
            }
            //find large words
            string[] words = userinput.Split(' ');
            wordsinputed = words.Length;
            for (int i = 0; i < wordsinputed; i++)
                if (words[i].Length > 7)
                {
                    //check to see if last character is a symbol
                    string last = words[i].Substring(words[i].Length - 1);
                    //if it is a symbol remove it
                    if (last == ".")
                        words[i] = words[i].Remove(words[i].Length - 1);

                    //keeps a list of large words
                    largewords = words[i] + "\n" + largewords;

                    //counts the number or large words
                    largewordcount++;
                }

            //finds average
            averagewordsize = charainputed / wordsinputed;

            //outputs stats
            Console.WriteLine("The Text that was analysed:\n{0}\n", userinput);
            Console.WriteLine("Number of characters entered (including spaces): {0}", userinput.Length);
            Console.WriteLine("Number of characters entered(not including spaces): " + charainputed);
            Console.WriteLine("Number of Words entered: " + wordsinputed);
            Console.WriteLine("Number of sentences entered: " + sentanceinput);
            Console.WriteLine("Number of large words entered: " + largewordcount);
            Console.WriteLine("Average size of words entered: " + averagewordsize);
            //waits
            Console.ReadLine();


            userWriteChoice(largewordcount, largewords, userinput);

        }

        private static void userWriteChoice(int largewordcount, string largewords, string userinput)
        {

            string userChoice, fileName = "";
            //check if there is large words
            if (largewordcount > 0)
            {
                //inquire if the user wants to save large-words to a text file
                Console.WriteLine("would you like to save the larage words to a text file on your documents?\nEnter Y to save it or press enter to continue");
                userChoice = Console.ReadLine();

                //checks users choice 
                if (userChoice == "y" || userChoice == "Y")
                {
                    writefile(largewords, "largewords.txt");
                }
            }
            //inquire if the user wants to save analysed text to a text file
            Console.WriteLine("Do you want to save the analysed text to a text file?\nEnter Y to save it or press enter to continue");
            userChoice = Console.ReadLine();

            //checks users choice 
            if (userChoice == "y" || userChoice == "Y")
            {
                //Makes sure the name is not null or blank space
                while (fileName == "")
                {
                    Console.WriteLine("What do you want the file to be called?");
                    fileName = Console.ReadLine();

                    //Remove white space at front and end of string
                    fileName = fileName.Trim();
                    if (fileName == "")
                        Console.WriteLine("Name cannot be blank");
                }
                //Add file type to name
                fileName = fileName + ".txt";


                writefile(userinput, fileName);

            }
        }
        public static void writefile(string txt, string filename)
        {
            //get the loction and the name of the file from prior methord
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string fullFilePath = Path.Combine(folder, filename);

            //disply where file will be located
            Console.WriteLine("The file will be saved at: " + fullFilePath + "\n");

            //check if file exists
            if (File.Exists(fullFilePath) == true)
            {
                Console.WriteLine("The file Exists\n Do you want to overwrite? enter Y to overwrite or press enter to continue");
                string userChoice = Console.ReadLine();

                //Check if the user want to save of existing file
                if (userChoice == "y" || userChoice == "Y")
                {
                    File.WriteAllText(fullFilePath, txt);
                }
                else
                    Console.WriteLine("File was not saved");
            }
            else
                //writes texts to file
                File.WriteAllText(fullFilePath, txt);
        }

    }
}





