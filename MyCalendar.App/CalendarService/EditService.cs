﻿using System;
using System.Linq;
using MyCalendar.App.Helpers;
using MyCalendar.App.MainMenuService;
using MyCalendar.App.Models;

namespace MyCalendar.App.CalendarService
{
    public class EditService : BaseService
    {
        public static void EditMenu()
        {
            var actionService = new MenuActionService();
            MenuActionService.Initialize(actionService);

            var loop = true;
            while (loop)
            {
                Console.Clear();
                Console.WriteLine("Select element that you want edit:");
                var editMenu = actionService.GetMenuActionsByMenuName("EditMenu");
                foreach (var line in editMenu)
                    Console.WriteLine($"{line.Id}. {line.Name}");
                Console.Write("> ");

                var operation = Console.ReadKey();
                switch (operation.KeyChar)
                {
                    case '1':
                        EditCalendar();
                        break;

                    case '2':
                        EditEvent();
                        break;

                    case '3':
                        EditTask();
                        break;

                    case '4':
                        loop = false;
                        break;

                    default:
                        Console.Write("\nThere is no such option. Choose a different key.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static void EditCalendar()
        {
            Console.Clear();

            var calendarList = FileHelperEvent.DeserializeFromFile();
            var count = 1;
            foreach (var calendar in calendarList)
            {
                Console.ForegroundColor = calendar.Color;
                Console.WriteLine($"{count}. {calendar.Name}");
                count++;
            }
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.Write("Select calendar that you want edit: ");
            var enteredKeyOption = CheckValid.IsInputNumber(calendarList.Count);
            var selectedCalendar = calendarList.ElementAt(enteredKeyOption - 1);

            var newCalendar = new Calendar();
            Console.Clear();
            Console.WriteLine("--- EDITED CALENDAR ---");
            Console.ForegroundColor = selectedCalendar.Color;
            Console.WriteLine(selectedCalendar.Name);
            Console.WriteLine(selectedCalendar.Color);
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.Write("\nEnter calendar name: ");
            newCalendar.Name = Console.ReadLine();
            Console.WriteLine("Enter calendar color by number: ");
            var values = Enum.GetValues(typeof(ConsoleColor));
            var colorCount = 1;
            foreach (var item in values)
            {
                Console.WriteLine($"{colorCount}. {item}");
                colorCount++;
            }
            newCalendar.Color = CheckValid.IsValidColor();
            newCalendar.EventList = selectedCalendar.EventList;

            Console.Write("Press 'Y' if you are sure to save changes: ");
            var enteredKey = Console.ReadKey();
            if (enteredKey.Key == ConsoleKey.Y)
            {
                calendarList.RemoveAt(enteredKeyOption - 1);
                calendarList.Add(newCalendar);
                FileHelperEvent.SerializeToFile(calendarList);
                Console.WriteLine("\n\nEditing done! Click any key to continue...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("\n\nOperation stopped. Click any key to continue...");
                Console.ReadKey();
            }
        }
        private static void EditEvent()
        {
            Console.Clear();
            var calendarList = FileHelperEvent.DeserializeFromFile();

            var calendarCount = 1;
            foreach (var calendar in calendarList)
            {
                Console.ForegroundColor = calendar.Color;
                Console.WriteLine($"{calendarCount}. {calendar.Name}");
                calendarCount++;
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Select calendar where event you want to edit is: ");
            var enteredCalendarOption = CheckValid.IsInputNumber(calendarList.Count);
            var selectedCalendar = calendarList.ElementAt(enteredCalendarOption - 1);

            Console.WriteLine();
            var eventCount = 1;
            foreach (var item in selectedCalendar.EventList)
            {
                Console.WriteLine($"{eventCount}. {item.Name} {item.DateOfStart:dd MMMM yyyy HH:mm} - {item.DateOfEnd:dd MMMM yyyy HH:mm}");
                eventCount++;
            }
            Console.Write("Select event that you want edit: ");
            var enteredEventOption = CheckValid.IsInputNumber(selectedCalendar.EventList.Count);
            var selectedEvent = selectedCalendar.EventList.ElementAt(enteredEventOption - 1);

            var newEvent = new Event();
            Console.Clear();
            Console.WriteLine("--- EDITED EVENT ---");
            Console.ForegroundColor = selectedCalendar.Color;
            Console.WriteLine(selectedEvent.Name);
            Console.WriteLine($"{selectedEvent.DateOfStart:dd MMMM yyyy HH:mm}");
            Console.WriteLine($"{selectedEvent.DateOfEnd:dd MMMM yyyy HH:mm}");
            Console.WriteLine(selectedEvent.Description);
            Console.WriteLine(selectedEvent.IsBusy ? "Busy: YES" : "Busy: NO");
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.Write("\nEnter event name: ");
            newEvent.Name = Console.ReadLine();
            Console.Write("Enter event start date in correct format - DD-MM-YYYY hh:mm: ");
            newEvent.DateOfStart = CheckValid.IsValidDateExtended();
            Console.Write("Enter event end date in correct format - DD-MM-YYYY hh:mm: ");
            newEvent.DateOfEnd = CheckValid.IsValidDateExtended();
            Console.Write("Enter event description: ");
            newEvent.Description = Console.ReadLine();
            Console.Write("Status (FREE/BUSY): ");
            newEvent.IsBusy = CheckValid.IsBusy();

            Console.Write("Press 'Y' if you are sure to edit that event: ");
            var enteredKey = Console.ReadKey();
            if (enteredKey.Key == ConsoleKey.Y)
            {
                calendarList.ElementAt(enteredCalendarOption - 1).EventList.RemoveAt(enteredEventOption - 1);
                calendarList.ElementAt(enteredCalendarOption - 1).EventList.Add(newEvent);
                FileHelperEvent.SerializeToFile(calendarList);
                Console.WriteLine("\nEditing done! Click any key to continue...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("\n\nOperation stopped. Click any key to continue...");
                Console.ReadKey();
            }
        }

        private static void EditTask()
        {
            Console.Clear();

            var tasksList = FileHelperTask.DeserializeFromFile();
            var count = 1;
            foreach (var task in tasksList)
            {
                Console.WriteLine($"{count}. {task.Name}");
                count++;
            }
            Console.Write("Select task that you want edit: ");
            var enteredKeyOption = CheckValid.IsInputNumber(tasksList.Count);
            var selectedTask = tasksList.ElementAt(enteredKeyOption - 1);

            var newTask = new Task();
            Console.Clear();
            Console.WriteLine("--- EDITED TASK ---");
            Console.WriteLine(selectedTask.Name);
            Console.WriteLine($"{selectedTask.DayOfTask:dd MMMM yyyy}");
            Console.Write("\nEnter task name: ");
            newTask.Name = Console.ReadLine();
            Console.Write("Enter task date in correct format - DD-MM-YYYY: ");
            newTask.DayOfTask = CheckValid.IsValidDateNormal();

            Console.Write("Press 'Y' if you are sure to save changes: ");
            var enteredKey = Console.ReadKey();
            if (enteredKey.Key == ConsoleKey.Y)
            {
                tasksList.RemoveAt(enteredKeyOption - 1);
                tasksList.Add(newTask);
                FileHelperTask.SerializeToFile(tasksList);
                Console.WriteLine("\n\nEditing done! Click any key to continue...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("\n\nOperation stopped. Click any key to continue...");
                Console.ReadKey();
            }
        }
    }
}