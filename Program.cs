//######################################################################################################################
//Assignment
//
//Design a class called Stopwatch. The job of this class is to simulate a stopwatch. It should
//provide two methods: Start and Stop.We call the start method first, and the stop method next.
//Then we ask the stopwatch about the duration between start and stop.Duration should be a
//value in TimeSpan.Display the duration on the console.
//We should also be able to use a stopwatch multiple times.So we may start and stop it and then
//start and stop it again. Make sure the duration value each time is calculated properly.
//We should not be able to start a stopwatch twice in a row (because that may overwrite the initial
//start time). So the class should throw an InvalidOperationException if its started twice
//
//
//Notes:
//-I'll need to make use of TimeSpan and deltaTime i think
//-display a menu of what each character command does (ie s for start, e for end, r for reset)
//-display a "running..." tag and adjust a state tracker based on s or e input
//-Using while loop for act as listener basis for simulation
//-display the total elapsed time. reseting the base timer upon reset command
//-For exception hadnling don't want to be in a state where s is entered twice
//  -having a state tracker (checking if new state is same as previous state) could be used
//-Need to subtract paused time from endtime - really just need to += to elapsed time calculation
//-Remove unused code
//-May need to account for pause time behavior if display time is selected twice in a row
//     -don't run calcualtion if display was just executed (need extra display flag)
//-Move the call to set first capture to false to a separate method
//-Review set methods for properties - am I doing it right??? - yes I am
//-Bug - Prevent extra switch loops - this is being caused by newline character when i enter value for input variable
//     -fix is to either scan a character via a different function or to isolate the 'n\' character somehow from scan
//Did not implement a try catch block, instead just used conditions - if time can find a way to convert to try catch later if interested
//#######################################################################################################################


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OOP_Stopwatch_Exercise
{
    class Program
    {
        public class Stopwatch
        {
            //Properties
            private bool _isRunning { get; set; }
            private bool _firstCapture { get; set; }
            private bool _showingTime { get; set; }
            private DateTime _timeSpan;

            //Properties Management
            public void SetStart()
            {
                _isRunning = true;
                _firstCapture = false;
            }
            public void SetStop()
            {
                _isRunning = false;
            }
            public void SetDisplay(bool showingTime)
            {
                _showingTime = showingTime;
            }
            public bool GetState
            {
                get
                {
                    var state = _isRunning;
                    return state;
                }
            }
            public bool GetFirstTime
            {
                get
                {
                    var firstTime = _firstCapture;
                    return firstTime;
                }
            }
            public bool GetIsDisplayed
            {
                get
                {
                    var isItShowing = _showingTime;
                    return isItShowing;
                }
            }

            //Ctor
            public Stopwatch()
            {
                SetStop(); //<-- stopwatch not running by default
                _firstCapture = true;
                _showingTime = false;
            }

            //Menu
            public void Controls()
            {
                Console.WriteLine("---Stopwatch controls---\n");
                Console.WriteLine("s - starts/continues the timer\n");
                Console.WriteLine("e - stops the timer\n");
                Console.WriteLine("d - displays elapsed time\n");
                Console.WriteLine("r - resets the timer\n");
                Console.WriteLine("q - exit the simulation\n\n");
                Console.WriteLine("Please enter a valid command\n");
            } 

            //Clock the time
            public DateTime Capture()
            {
                 _firstCapture = false;
                _timeSpan = DateTime.Now;
                return _timeSpan;
            }

            //Reset to 0
            public void Reset(ref double timeStorage)
            {
                _firstCapture = true;
                _showingTime = false;
                _timeSpan = DateTime.MinValue;
                timeStorage = 0.0;
            }

            //Calculate time
            public double ElapsedTime(DateTime end, DateTime start)
            {
                var activeTime = new TimeSpan(); 
                activeTime = end - start;
                return activeTime.TotalSeconds;
            }            
        }
        
        static void Main(string[] args)
        {
            char input;
            double elapsedTime = 0.0;
            DateTime startTime = new DateTime(), endTime = new DateTime();
            Stopwatch Timer = new Stopwatch();
            
            Timer.Controls();
            input = Console.ReadKey().KeyChar;
            Console.WriteLine("\n");

            do
            {                               
                switch (input)
                {
                    case 's': //start/resume the clock
                        if (Timer.GetState == false && Timer.GetFirstTime == true)
                        {
                            Timer.SetStart();
                            Timer.SetDisplay(false);
                            startTime = Timer.Capture();
                            Console.WriteLine("Clock has started\n");
                        }
                        else if (Timer.GetState == false && Timer.GetFirstTime == false)
                        {
                            Timer.SetStart();
                            Timer.SetDisplay(false);
                            startTime = Timer.Capture();
                            Console.WriteLine("Clock has resumed\n");
                        }
                        else {
                            //convert to proper exception handling
                            Console.WriteLine("Clock is already running\n\n");
                        }
                        break;

                    case 'e': //stop the clock
                        if (Timer.GetState == true)
                        {
                            Timer.SetStop(); 
                            endTime = Timer.Capture(); 
                            Console.WriteLine("Time captured\n");
                        }
                        else {
                            //convert to proper exception handling
                            Console.WriteLine("Clock must be started again\n\n");
                        }                       
                        break;

                    case 'd': //display elapsed time
                        if (Timer.GetState == false && Timer.GetIsDisplayed == false)
                        {
                            elapsedTime += Timer.ElapsedTime(endTime, startTime);
                            Console.WriteLine("Elapsed time is " + elapsedTime.ToString("#.00") + " seconds\n");
                            Timer.SetDisplay(true);
                        } else if (Timer.GetState == false && Timer.GetIsDisplayed == true) {
                            Console.WriteLine("Again.....the elapsed time is " + elapsedTime.ToString("#.00") + " seconds\n");
                        } 
                        else {
                            //convert to proper exception handling
                            Console.WriteLine("Clock is still running. Please stop the clock and try again\n");
                        }
                        break;

                    case 'r': //reset the clock
                        Timer.SetStop();
                        Timer.Reset(ref elapsedTime);
                        startTime = endTime = new DateTime();
                        Console.WriteLine("Clock has been reset to 0.00 seconds\n");
                        break;

                    case 'q': //end the simulation
                        break;

                    default: //wrong command
                        Console.WriteLine("Invalid Command\n");
                        break;
                }

                if (input != 'q')
                {
                    Timer.Controls();
                    input = Console.ReadKey().KeyChar;
                    Console.WriteLine("\n");
                }
            } while (input != 'q');

            Console.WriteLine("Ending Simulation");
        }
    }
}
