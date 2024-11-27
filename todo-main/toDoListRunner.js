// ToDoListHandler.js
import taskInput from "./taskInput.js";

export function runToDoList(todoList) {
  let continueEditing = true;

  while (continueEditing) {
    console.log("-----------------");
    console.log('1. Add a task');
    console.log('2. Remove the last task');
    console.log('3. Remove the first task');
    console.log('4. Remove task from list by index');
    console.log('5. Move task to top of the list by index');
    console.log('6. Move task to bottom of the list by index');
    console.log('7. Remove task by content');
    console.log('8. Move a task down by index');
    console.log('9. Move a task up by index');
    console.log('10. Move to done by index');
    console.log('11. Exit');

    let option = new taskInput("Choose an option (1-11)").answer.toLowerCase();

    switch (option) {
      case '1':
        todoList.addTask();
        break;
      case '2':
        todoList.removeFromBottom();
        break;
      case '3':
        todoList.removeFromTop();
        break;
      case '4':
        todoList.removeByIndex();
        break;
      case '5':
        todoList.moveToTopByIndex();
        break;
      case '6':
        todoList.moveToBottomByIndex();
        break;
      case '7':
        todoList.removeByContent();
        break;
      case '8':
        todoList.moveDownByIndex();
        break;
      case '9':
        todoList.moveUpByIndex();
        break;
      case '10':
        todoList.moveToDoneByIndex();
        break;
      case '11':
        continueEditing = false; // Set continueEditing to false to exit the loop
        break;

      default:
        console.log("Invalid option. Please choose a valid option (1-11).");
        break;
    }
    console.log("YOUR TO-DO LIST:");
    todoList.taskList.printTasks();

    if (option === '10' && todoList.taskList.doneTasks.length > 0) {
      console.log("DONE TASKS:");
      todoList.taskList.printDoneTasks();

    }
  }
}
