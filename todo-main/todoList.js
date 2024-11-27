import task from "./task.js";
import taskInput from "./taskInput.js";
import { runToDoList } from "./toDoListRunner.js";
import { loadTasksFromFile, saveTasksToFile } from "./fileHandler.js";


const print = console.log;
export default class todoList {
  taskList;
  logs;

  constructor() {
    print("Welcome to your To-Do List. Let's add some tasks!");
    this.taskList = new task("");
    this.logs = [];
    this.runToDoList();
  }

  loadTasksFromFile() {
    loadTasksFromFile(this.taskList);
  }

  saveTasksToFile() {
    saveTasksToFile(this.taskList);
  }

  

  runToDoList() {
    this.loadTasksFromFile(); 
    runToDoList(this);
  }
  

  addTask() {
    let taskContent = new taskInput("Add a task: ").answer;
    this.taskList.addTask(taskContent);
    print("Task added!");
    this.saveTasksToFile();
  }

  listTasks() {
    print("Your current tasks:");
    this.taskList.printTasks();
    this.saveTasksToFile();
  }

  removeFromBottom() {
    this.taskList.removeFromBottom();
    print("Last task removed!");
  }

  
  removeFromTop() {
    this.taskList.removeFromTop();
  }
  removeByIndex() {
    let indexToRemove = new taskInput("Enter the index of the task to remove: ").answer;
    indexToRemove = parseInt(indexToRemove);
    this.taskList.removeByIndex(indexToRemove);
  }
  // TodoList class
  moveToTopByIndex() {
    let indexToMove = new taskInput("Enter the index of the task to move to the top: ").answer;
    indexToMove = parseInt(indexToMove);
    this.taskList.moveToTopByIndex(indexToMove);
  }

  moveToBottomByIndex() {
    let indexToMove = new taskInput("Enter the index of the task to move to the bottom: ").answer;
    indexToMove = parseInt(indexToMove);
    this.taskList.moveToBottomByIndex(indexToMove);
    console.log("Task moved to the bottom!");
  }


  removeByContent() {
    let taskContent = new taskInput("Enter the content of the task to remove: ").answer;
    this.taskList.removeByContent(taskContent);
  }

  moveDownByIndex() {
    let indexToMove = new taskInput("Enter the index of the task to move down: ").answer;
    indexToMove = parseInt(indexToMove);
    this.taskList.moveDownByIndex(indexToMove);
  }

  moveUpByIndex() {
    let indexToMove = new taskInput("Enter the index of the task to move up: ").answer;
    indexToMove = parseInt(indexToMove);
    this.taskList.moveUpByIndex(indexToMove);
  }
  
  moveToDoneByIndex() {
    let indexToMove = new taskInput("Enter the index of the task to move to the 'done' list: ").answer;
    indexToMove = parseInt(indexToMove);
    this.taskList.moveToDoneByIndex(indexToMove);
  }
  
  

}
